using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.DAL.Entities;
using Task5.WebApi.ViewModels.User;

namespace Task5.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class UserController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        public UserController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        public IEnumerable<string> RoleList() => roleManager.Roles.Select(x => x.Name);

        [HttpGet]
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
                    Role = await userManager.GetRolesAsync(user)
                });
            }
            return result;
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> AddUser(RegisterViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByEmailAsync(request.Email);
            var roles = roleManager.Roles.FirstOrDefault(x => x.Name.ToLower() == request.Role.ToLower());
            var s = userManager.Users.Select(x => x.UserName);
            if (user != null || roles == null)
                return BadRequest();

            user = new User { UserName = request.UserName, Email = request.Email };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await userManager.AddToRoleAsync(user, roles.Name);

            var response = new UserViewModel { Email = user.Email, UserName = user.UserName, Role = await userManager.GetRolesAsync(user) };

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<UserViewModel>> AddRole(ChangeRoleViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByEmailAsync(request.Email);
            var roles = roleManager.Roles;

            if (user == null || !roles.Any(x => x.Name == request.Role))
                return BadRequest();

            await userManager.AddToRoleAsync(user, request.Role);

            var response = new UserViewModel { Email = user.Email, UserName = user.UserName, Role = await userManager.GetRolesAsync(user) };

            return Ok(response);
        }

        [HttpPut]
        public async Task<ActionResult<UserViewModel>> RemoveRole(ChangeRoleViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByEmailAsync(request.Email);
            var roles = roleManager.Roles;

            if (user == null || !roles.Any(x => x.Name == request.Role))
                return NotFound();

            await userManager.RemoveFromRoleAsync(user, request.Role);

            var response = new UserViewModel { Email = user.Email, UserName = user.UserName, Role = await userManager.GetRolesAsync(user) };

            return Ok(response);
        }

        [HttpDelete]
        public async Task<ActionResult<UserViewModel>> DeleteUser(BaseUserViewModel request)
        {
            if (request == null)
                return BadRequest();

            var user = await userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return BadRequest();

            await userManager.DeleteAsync(user);

            return Ok();
        }
    }
}
