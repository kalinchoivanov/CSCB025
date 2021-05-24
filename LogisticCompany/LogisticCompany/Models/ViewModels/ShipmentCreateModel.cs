using System;

namespace LogisticCompany.Models.ViewModels
{
    public class ShipmentCreateModel
    {
        public string Id { get; set; }

        public Guid BillOfLanding { get; set; }

        public string Origin { get; set; }

        public string Destination { get; set; }

        public string Description { get; set; }

        public string SenderUserName { get; set; }

        public string RecipientUserName { get; set; }

        public Status Status { get; set; }

        public ShipmentType Type { get; set; }

        public decimal Weight { get; set; }

        public decimal Price { get; set; }
    }
}

