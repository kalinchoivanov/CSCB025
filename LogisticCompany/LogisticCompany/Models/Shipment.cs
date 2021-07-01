using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticCompany.Models
{
    public class Shipment
    {
        [Key]
        public string Id { get; set; }

        public Guid BillOfLanding { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Description { get; set; }

        public virtual ApplicationUser Sender { get; set; }

        public string SenderId { get; set; }

        public virtual ApplicationUser Recipient { get; set; }

        public string RecipientId { get; set; }

        public virtual ApplicationUser Employee {get; set;}

        public string EmployeeId { get; set; }

        public Status Status { get; set; }

        public ShipmentType Type { get; set; }

        public decimal Weight { get; set; }

        public decimal Price { get; set; }

        public DateTime Date { get; set; }
    }

    public enum Status
    {
        [Display(Name = "Sent")]
        Sent,
        [Display(Name = "Delivered")]
        Delivered,
    }

    public enum ShipmentType
    {
        [Display(Name = "Normal")]
        Normal,
        [Display(Name = "Express")]
        Express
    }
}
