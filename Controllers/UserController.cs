using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSTask.Models;
using SmartSTask.Models.ViewModels;

namespace SmartSTask.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly ManageTaskContext _context;

        public readonly UserManager<User> _userManager;

        public UserController(ManageTaskContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {

            var users = await _context.Users.ToListAsync();
            var tasks = await _context.ManageTasks.ToListAsync();
          
            List<UserViewModel> list = new List<UserViewModel>();
            for (int i = 0; i < users.Count; i++)
            {
                UserViewModel viewModel = new UserViewModel();
                viewModel.FirstName = users[i].FirstName;
                viewModel.LastName = users[i].LastName;
                viewModel.Email = users[i].Email;
                viewModel.Organization = users[i].Organization;
                viewModel.Task = tasks.Where(x => x.TaskId == users[i].TaskId).FirstOrDefault();
                viewModel.Id = users[i].Id;
                list.Add(viewModel);
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult AddOrEdit(int Id = 0)
        {
            if (Id == 0)
            {
                return View(new User());
            }
            else
            {
                return View(_context.Users.Find(Id));
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                if (model.Id == 0)
                {
                    var user = new User
                    {
                        UserName = model.Email,
                        Email = model.Email,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        Organization = _context.Set<Organization>().Find(currentUser.OrganizationId),
                    };

                    var result = await _userManager.CreateAsync(user, model.Password ?? "Kamal123@");
                }
                else
                {
                    var user = _context.Users.Find(model.Id);
                    user.Email = user.UserName = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    _context.Update(user);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            var task = await _context.Users.FindAsync(id);
            _context.Users.Remove(task);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}