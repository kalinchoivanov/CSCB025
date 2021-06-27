using System.ComponentModel.DataAnnotations;

namespace LogisticCompany.Models
{
    public class Office
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}
