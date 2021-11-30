using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task5.Infrastructure.Interfaces;
using Task7.WebApplication.Models.User;

namespace Task7.WebApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IJwtGenerator jwtGenerator;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, IJwtGenerator jwtGenerator)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.jwtGenerator = jwtGenerator;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, false);
                    if (result.Succeeded)
                    {
                        var token = await jwtGenerator.CreateToken(user);
                        HttpContext.Session.SetString("auth", token);

                        return Redirect("/");
                    }
                }
                ModelState.AddModelError(nameof(LoginViewModel), "Invalid username or password");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = userManager.Users;
            var result = new List<UserViewModel>();
            foreach (var user in users)
            {
                result.Add(new UserViewModel
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    Role = (await userManager.GetRolesAsync(user)).OrderBy(x => x).FirstOrDefault()
                });
            }

            return View(result.OrderBy(x => x.Role));
        }

        public IActionResult Create()
        {
            ViewBag.Roles = new SelectList(roleManager.Roles.OrderBy(x => x.Name));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = await userManager.FindByNameAsync(model.UserName);
                var userEmail = await userManager.FindByEmailAsync(model.Email);
                if (userName == null && userEmail == null)
                {
                    var role = roleManager.Roles.FirstOrDefault(x => x.Name.ToLower() == model.Role.ToLower());
                    if (role != null)
                    {
                        var user = new IdentityUser { UserName = model.UserName, Email = model.Email };
                        await userManager.CreateAsync(user, model.Password);
                        var result = await userManager.AddToRoleAsync(user, role.Name);

                        if (result.Succeeded)
                            return RedirectToAction("Users");
                    }
                    ModelState.AddModelError(nameof(RegisterViewModel), "Invalid role");
                }
                ModelState.AddModelError(nameof(RegisterViewModel), "Username or email already exists");
            }
            ViewBag.Roles = new SelectList(roleManager.Roles.OrderBy(x => x.Name));
            return View(model);
        }

        public async Task<IActionResult> Edit(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            ViewBag.Roles = new SelectList(roleManager.Roles, (await userManager.GetRolesAsync(user)).FirstOrDefault());
            var result = new UserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = (await userManager.GetRolesAsync(user)).OrderBy(x => x).FirstOrDefault()
            };
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    var role = roleManager.Roles.FirstOrDefault(x => x.Name.ToLower() == model.Role.ToLower()).Name;
                    if (role != null)
                    {
                        var currentRoles = (await userManager.GetRolesAsync(user)).FirstOrDefault();
                        if (role != currentRoles)
                        {
                            await userManager.RemoveFromRoleAsync(user, currentRoles);
                            await userManager.AddToRoleAsync(user, role);
                        }

                        var result = await userManager.UpdateAsync(user);

                        if (result.Succeeded)
                            return RedirectToAction("Users");
                    }
                    ModelState.AddModelError(nameof(RegisterViewModel), "Invalid role");
                }
                ModelState.AddModelError(nameof(RegisterViewModel), "Username or email already exists");
            }
            ViewBag.Roles = new SelectList(roleManager.Roles.OrderBy(x => x.Name));
            return View(model);
        }

        public async Task<IActionResult> Delete(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
                return NotFound();

            await userManager.DeleteAsync(user);
            return RedirectToAction("Users");
        }
    }
}
