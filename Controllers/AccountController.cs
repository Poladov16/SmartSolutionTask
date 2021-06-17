using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSTask.Models;
using SmartSTask.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartSTask.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly ManageTaskContext context;

        public AccountController(UserManager<User> userManager,
            SignInManager<User> signInManager,
            ManageTaskContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email,
                                                                     model.Password,
                                                                     false,
                                                                     false);

                if (result.Succeeded)
                {
                    var user = await context.Users.FirstAsync(x => x.Email == model.Email);

                    if (!user.IsAdmin)
                    {
                        return RedirectToAction(nameof(UserInterface), user);
                    }

                    return RedirectToAction(nameof(ManageTaskController.Index), "ManageTask");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    IsAdmin = true,
                    Organization = new Organization
                    {
                        OrganizationAddress = model.OrganizationAddress,
                        OrganizationName = model.OrganizationName,
                        OrganizationPhone = model.OrganizationPhone
                    }
                };

                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> UserInterface(User user)
        {
            var task = await context.ManageTasks.Where(x => x.TaskId == user.TaskId).ToListAsync();

            return View(task);
        }
    }
}
