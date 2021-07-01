using LogisticCompany.Data;
using LogisticCompany.Models;
using LogisticCompany.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticCompany.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> GetReport(string reportName)
        {
            return RedirectToAction(reportName);
        }

        public async Task<IActionResult> AllClients()
        {
            var usersViewModel = new List<ApplicationUserViewModel>();
            var userRoles = _context.UserRoles.Where(c => c.RoleId == "2").ToList();

            foreach (var role in userRoles)
            {
               var user = _context.ApplicationUsers.Where(u => u.Id == role.UserId).FirstOrDefault();

                usersViewModel.Add(MapToUserViewModel(user, "Client"));
            }

            return PartialView("_AllClients", usersViewModel);
        }
        public async Task<IActionResult> AllEmployees()
        {
            var usersViewModel = new List<ApplicationUserViewModel>();
            var userRoles = _context.UserRoles.Where(c => c.RoleId == "1").ToList();

            foreach (var role in userRoles)
            {
                var user = _context.ApplicationUsers.Where(u => u.Id == role.UserId).FirstOrDefault();

                usersViewModel.Add(MapToUserViewModel(user, "Employee"));
            }

            return PartialView("_AllEmployees", usersViewModel);
        }

        public async Task<IActionResult> AllShipments()
        {
            var shipments = _context.Shipments.Include(s => s.Recipient).Include(s => s.Sender).ToList();

            return PartialView("_AllShipments", shipments);
        }
        public async Task<IActionResult> AllUndeliveredShipments()
        {
            var shipments = _context.Shipments.Include(s => s.Recipient).Include(s => s.Sender).Where(s => s.Status == Status.Sent).ToList();

            return PartialView("_AllUndeliveredShipments", shipments);
        }

        public async Task<IActionResult> OpenShipmentsSentBy()
        {
            return PartialView("_FindShipmentsSentBy");
        }

        public async Task<IActionResult> ShipmentsSentBy(string userName)
        {
            var shipments = _context.Shipments.Include(s => s.Sender).Include(s => s.Recipient).Where(s => s.Sender.UserName == userName).ToList();

            return PartialView("_ShipmentsSentBy", shipments);
        }

        public async Task<IActionResult> OpenShipmentsRecievedBy()
        {
            return PartialView("_FindShipmentsRecievedBy");
        }

        public async Task<IActionResult> ShipmentsRecievedBy(string userName)
        {
            var shipments = _context.Shipments.Include(s => s.Recipient).Include(s => s.Sender).Where(s => s.Recipient.UserName == userName).ToList();

            return PartialView("_ShipmentsRecievedBy", shipments);
        }

        public async Task<IActionResult> OpenShipmentsRegisteredBy()
        {
            return PartialView("_FindShipmentsRegisteredBy");
        }
        public async Task<IActionResult> ShipmentsRegisteredBy(string userName)
        {
            var shipments = _context.Shipments.Include(s => s.Recipient).Include(s => s.Sender).Include(s => s.Employee).Where(s => s.Employee.UserName == userName).ToList();

            return PartialView("_ShipmentsRegisteredBy", shipments);
        }

        public async Task<IActionResult> OpenIncomeForPeriod()
        {
            return PartialView("_SelectIncomePeriod");
        }

        public async Task<IActionResult> IncomeForPeriod(DatePickerVewModel datePickerVewModel)
        {
            PriceViewModel priceViewModel = new PriceViewModel();

            var shipments = _context.Shipments
                .Where(sd => sd.Date >= datePickerVewModel.DateTimeFrom && sd.Date < datePickerVewModel.DateTimeTo)
                .OrderByDescending(s => s.Date)
                .ToList();

            foreach (var shipment in shipments)
            {
                priceViewModel.Price += shipment.Price;
            }

            return PartialView("_IncomeForPeriod", priceViewModel);
        }

        private ApplicationUserViewModel MapToUserViewModel(ApplicationUser user, string role)
        {
            var userViewModel = new ApplicationUserViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = role
            };

            return userViewModel;
        }
    }
}
