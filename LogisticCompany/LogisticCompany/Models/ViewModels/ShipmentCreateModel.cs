using System;
using System.ComponentModel.DataAnnotations;

namespace LogisticCompany.Models.ViewModels
{
    public class ShipmentCreateModel
    {
        public string Id { get; set; }

        public Guid BillOfLanding { get; set; }
        
        [Required]
        public string Origin { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string SenderUserName { get; set; }

        [Required]
        public string RecipientUserName { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public ShipmentType Type { get; set; }

        [Required]
        public decimal Weight { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}

