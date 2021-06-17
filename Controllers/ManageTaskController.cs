using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SmartSTask.Models;
using SmartSTask.Models.ViewModels;
using SmartSTask.Services;

namespace SmartSTask.Controllers
{
    //[Authorize]
    public class ManageTaskController : Controller
    {
        private readonly ManageTaskContext _context;

        public ManageTaskController(ManageTaskContext context)
        {
            _context = context;
        }

        // GET: ManageTask
        public async Task<IActionResult> Index()
        {
            var task = await _context.ManageTasks.ToListAsync();
            var user = await _context.Users.ToListAsync();
            List<ManageTaskViewModel> list = new List<ManageTaskViewModel>();
            for (int i = 0; i < task.Count; i++)
            {
                ManageTaskViewModel viewModel = new ManageTaskViewModel();
                viewModel.Description = task[i].Description;
                viewModel.Title = task[i].Title;
                viewModel.StatusName = Enum.GetName(typeof(Status), task[i].Status);
                viewModel.Deadline = task[i].Deadline;
                viewModel.AssignedUsers = user.Where(x => x.TaskId == task[i].TaskId).ToList() ;
                viewModel.TaskId = task[i].TaskId;
                list.Add(viewModel);
            }
            return View(list);
        }

        [HttpGet]
        public IActionResult AddOrEdit(int id = 0)
        {
            //ViewData["users"] = _context.Users.ToList();
            var users = _context.Users.Where(x => !x.IsAdmin).ToList();
            var task = _context.ManageTasks.Find(id);

            if (task != null)
            {
                var taskView = new ManageTaskViewModel
                {
                    TaskId = id,
                    Deadline = task.Deadline,
                    Description = task.Description,
                    Status = task.Status,
                    Title = task.Title,
                    AssignedUsers = users
                };

                return View(taskView);
            }

            return View(new ManageTaskViewModel() { AssignedUsers = users });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(ManageTaskViewModel manageTask)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(manageTask.TaskUserId);

                if (manageTask.TaskId == 0)
                {
                    var task = new ManageTask
                    {
                        Deadline = manageTask.Deadline,
                        Description = manageTask.Description,
                        Status = manageTask.Status,
                        Title = manageTask.Title
                    };
                    if (user != null)
                    {
                        task.AssignedUsers = new[] { user };
                    }
                    _context.Add(task);
                    await _context.SaveChangesAsync();

                    if (!string.IsNullOrEmpty(user.Email))
                    {
                        await Notification.SendMail(user.Email, task.Title, task.Description);
                    }
                }
                else
                {
                    var task = await _context.ManageTasks.FindAsync(manageTask.TaskId);

                    task.Deadline = manageTask.Deadline;
                    task.Description = manageTask.Description;
                    task.Status = manageTask.Status;
                    task.Title = manageTask.Title;

                    if (user != null)
                    {
                        task.AssignedUsers = new[] { user };
                    }
                    _context.Update(task);

                    await _context.SaveChangesAsync();
                }


                return RedirectToAction(nameof(Index));
            }
            return View(manageTask);
        }

        // GET: ManageTask/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var task = await _context.ManageTasks.FindAsync(id);
            _context.ManageTasks.Remove(task);
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
