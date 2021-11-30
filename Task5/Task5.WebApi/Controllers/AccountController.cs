using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.Infrastructure.Interfaces;
using Task5.WebApi.ViewModels.User;

namespace Task5.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : ControllerBase
    {

        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IJwtGenerator jwtGenerator;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtGenerator = jwtGenerator;
        }

        [HttpPost]
        public async Task<ActionResult<string>> Login(LoginViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByNameAsync(request.UserName);
            if (user == null)
                return BadRequest();

            await signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            var token = await jwtGenerator.CreateToken(user);

            return token;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IEnumerable<string> RoleList() => roleManager.Roles.Select(x => x.Name);

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IEnumerable<UserViewModel>> UserList()
        {
            var users = userManager.Users.ToList();
            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                result.Add(new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = (await userManager.GetRolesAsync(user)).FirstOrDefault()
                });
            }
            return result;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserViewModel>> AddUser(RegisterViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByEmailAsync(request.Email);
            var userName = await userManager.FindByNameAsync(request.UserName);
            var roles = roleManager.Roles.FirstOrDefault(x => x.Name.ToLower() == request.Role.ToLower());
            var s = userManager.Users.Select(x => x.UserName);
            if (user != null || userName != null || roles == null)
                return BadRequest();

            user = new IdentityUser { UserName = request.UserName, Email = request.Email };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest();

            await userManager.AddToRoleAsync(user, roles.Name);

            var response = new UserViewModel { Email = user.Email, UserName = user.UserName, Role = (await userManager.GetRolesAsync(user)).FirstOrDefault() };

            return response;
        }



        public async Task<ActionResult<UserViewModel>> AddRole(ChangeRoleViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByNameAsync(request.UserName);
            var roles = roleManager.Roles;

            if (user == null || !roles.Any(x => x.Name == request.Role))
                return BadRequest();

            await userManager.AddToRoleAsync(user, request.Role);

            var response = new UserViewModel { Email = user.Email, UserName = user.UserName, Role = (await userManager.GetRolesAsync(user)).FirstOrDefault() };

            return response;
        }

        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserViewModel>> RemoveRole(ChangeRoleViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByNameAsync(request.UserName);
            var roles = roleManager.Roles;

            if (user == null || !roles.Any(x => x.Name == request.Role))
                return NotFound();

            await userManager.RemoveFromRoleAsync(user, request.Role);

            var response = new UserViewModel { Email = user.Email, UserName = user.UserName, Role = (await userManager.GetRolesAsync(user)).FirstOrDefault() };

            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserViewModel>> DeleteUser(BaseUserViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByNameAsync(request.UserName);

            if (user == null)
                return BadRequest();

            await userManager.DeleteAsync(user);

            return Ok();
        }
    }
}
