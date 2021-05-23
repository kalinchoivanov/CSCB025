using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LogisticCompany.Models;
using Microsoft.AspNetCore.Identity;
using LogisticCompany.Models.ViewModels;

namespace LogisticCompany.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserManager<ApplicationUser> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            if (user == null)
            {
                return View("NotAuthorized");
            }

            var appUserViewModel = await MapToApplicationUserViewModel(user);
            return View("Views/Shipments/Create");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task<ApplicationUserViewModel> MapToApplicationUserViewModel(ApplicationUser applicationUser)
        {
            var viewModel = new ApplicationUserViewModel();
            var roles = await _userManager.GetRolesAsync(applicationUser);
            viewModel.Email = applicationUser.Email;
            viewModel.UserName = applicationUser.UserName;
            viewModel.Role = roles.FirstOrDefault() != null ? roles.First() : "NoRole";

            return viewModel;
        }
    }
}
