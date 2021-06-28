using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using LogisticCompany.Models;
using LogisticCompany.Models.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace LogisticCompany.Controllers
{
    public class RoleController : Controller
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly INotyfService _notyf;

        public RoleController(UserManager<ApplicationUser> userManager, INotyfService notyf)
        {
            _userManager = userManager;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            var userRoleViewModels = new List<UserRoleViewModel>();

            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                var userRoleViewModel = new UserRoleViewModel();
                userRoleViewModel.ApplicationUser = user;
                var userRoles = await _userManager.GetRolesAsync(user);
                userRoleViewModel.Role = userRoles.FirstOrDefault();
                userRoleViewModels.Add(userRoleViewModel);
            }
            return View(userRoleViewModels.Where(m => m.Role != "Admin"));
        }

        public async Task<IActionResult> ChangeRoleToEmployee(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            try
            {
                await _userManager.RemoveFromRoleAsync(user, "Client");
                await _userManager.AddToRoleAsync(user, "Employee");
            }
            catch (Exception ex)
            {
                _notyf.Error("Something went wrong");
            }            
            _notyf.Success($"User - {user.UserName} is set to role Employee!");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeRoleToClient(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            try
            {
                await _userManager.RemoveFromRoleAsync(user, "Employee");
                await _userManager.AddToRoleAsync(user, "Client");
            }
            catch (Exception ex)
            {
               _notyf.Error("Something went wrong");
            }

            _notyf.Success($"User - {user.UserName} is set to role Client!");
            return RedirectToAction(nameof(Index));
        }
    }
}
