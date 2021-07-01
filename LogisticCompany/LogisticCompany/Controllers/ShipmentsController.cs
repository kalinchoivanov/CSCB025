using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogisticCompany.Data;
using LogisticCompany.Models;
using LogisticCompany.Models.ViewModels;
using System.Security.Claims;
using AspNetCoreHero.ToastNotification.Abstractions;
using Newtonsoft.Json;

namespace LogisticCompany.Controllers
{
    public class ShipmentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly INotyfService _notyf;

        public ShipmentsController(ApplicationDbContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        // GET: Shipments
        public async Task<IActionResult> Index()
        {
            var shipments = _context.Shipments.Include(s => s.Recipient).Include(s => s.Sender).ToList();

            var isClient = HttpContext.User.IsInRole("Client");


            if (isClient)
            {
                var clientId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                shipments = shipments.Where(s => s.RecipientId == clientId || s.SenderId == clientId).ToList();
            }

            return View(shipments);
        }

        // GET: Shipments/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Recipient)
                .Include(s => s.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }

        // GET: Shipments/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            ViewData["SenderId"] = new SelectList(_context.ApplicationUsers, "Id", "Id");
            
            return View();    
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShipmentCreateModel shipmentCreateModel)
        {
            var shipment = new Shipment();

            if (ModelState.IsValid)
            {
                shipment.Id = Guid.NewGuid().ToString();
                shipment.BillOfLanding = Guid.NewGuid();
                shipment.Status = Status.Sent;
                
                var recipient = _context.ApplicationUsers.FirstOrDefault(c => c.UserName == shipmentCreateModel.RecipientUserName);
                var sender = _context.ApplicationUsers.FirstOrDefault(c => c.UserName == shipmentCreateModel.SenderUserName);
                var employee = _context.ApplicationUsers.FirstOrDefault(c => c.UserName == HttpContext.User.Identity.Name);
                    
                if (recipient == null)
                {
                    _notyf.Error("Recipient with this user name was not found!");
                    return View(shipmentCreateModel);
                }
                else if (sender == null)
                {
                    _notyf.Error("Sender with this user name was not found!");
                    return View(shipmentCreateModel);
                }


                shipment.Sender = sender;
                shipment.Recipient = recipient;
                shipment.Employee = employee;
                shipment.SenderId = sender.Id;
                shipment.RecipientId = recipient.Id;
                shipment.Origin = shipmentCreateModel.Origin;
                shipment.Destination = shipmentCreateModel.Destination;
                shipment.Description = shipmentCreateModel.Description;
                shipment.Weight = shipmentCreateModel.Weight;
                shipment.Price = shipmentCreateModel.Price;
                shipment.Date = DateTime.Now;

                _context.Add(shipment);
                await _context.SaveChangesAsync();

                _notyf.Success("Shipment created");
                return RedirectToAction(nameof(Index));
            }

            return View(shipment);
        }

        // GET: Shipments/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments.FindAsync(id);

            if (shipment == null)
            {
                return NotFound();
            }
            return View(MapToShipmentCreateModel(shipment));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, ShipmentCreateModel shipmentCreateModel)
        {
            var shipment = new Shipment();

            if (id != shipmentCreateModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    shipment = MapToShipment(shipmentCreateModel);
                    _context.Update(shipment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShipmentExists(shipment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(shipment);
        }

        // GET: Shipments/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shipment = await _context.Shipments
                .Include(s => s.Recipient)
                .Include(s => s.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shipment == null)
            {
                return NotFound();
            }

            return View(shipment);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            _context.Shipments.Remove(shipment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Deliver(string id)
        {
            var shipment = await _context.Shipments.FindAsync(id);
            shipment.Status = Status.Delivered;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //public task<iactionresult> calculateprice(double weight, shipmenttype type)
        //{
        //    double price = type == shipmenttype.normal ? 4.8 : 8;

        //    if (weight > 5 && weight < 20)
        //    {
        //        price += 10;
        //    }
        //    else if (weight > 20)
        //    {
        //        price += 25;
        //    }


        //    return view();
        //}

        private bool ShipmentExists(string id)
        {
            return _context.Shipments.Any(e => e.Id == id);
        }

        private Shipment MapToShipment(ShipmentCreateModel shipmentCreateModel)
        {
            var shipment = new Shipment();

            shipment.Id = shipmentCreateModel.Id;
            shipment.BillOfLanding = Guid.NewGuid();
            shipment.Status = Status.Sent;

            var recipient = _context.ApplicationUsers.First(c => c.UserName == shipmentCreateModel.RecipientUserName);
            var sender = _context.ApplicationUsers.First(c => c.UserName == shipmentCreateModel.SenderUserName);

            shipment.Sender = sender;
            shipment.Recipient = recipient;
            shipment.SenderId = sender.Id;
            shipment.RecipientId = recipient.Id;
            shipment.Origin = shipmentCreateModel.Origin;
            shipment.Destination = shipmentCreateModel.Destination;
            shipment.Description = shipmentCreateModel.Description;
            shipment.Weight = shipmentCreateModel.Weight;
            shipment.Price = shipmentCreateModel.Price;

            return shipment;
        }

        private ShipmentCreateModel MapToShipmentCreateModel(Shipment shipment)
        {
            var shipmentCreateModel = new ShipmentCreateModel();

            shipmentCreateModel.Id = shipment.Id;
            shipmentCreateModel.BillOfLanding = shipment.BillOfLanding;
            shipmentCreateModel.Origin = shipment.Origin;
            shipmentCreateModel.Destination = shipment.Destination;
            shipmentCreateModel.Description = shipment.Description;

            var recipient = _context.ApplicationUsers.First(c => c.Id == shipment.RecipientId);
            var sender = _context.ApplicationUsers.First(c => c.Id == shipment.SenderId);

            shipmentCreateModel.SenderUserName = sender.UserName;
            shipmentCreateModel.RecipientUserName = recipient.UserName;
            shipmentCreateModel.Status = shipment.Status;
            shipmentCreateModel.Type = shipment.Type;
            shipment.Price = shipmentCreateModel.Price;

            return shipmentCreateModel;
        }
    }
}
