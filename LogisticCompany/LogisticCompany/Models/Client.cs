using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticCompany.Models
{
    public class Client : ApplicationUser
    {
        public virtual ICollection<Shipment> Shipments { get; set; }
    }
}
