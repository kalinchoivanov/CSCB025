using System.ComponentModel.DataAnnotations;

namespace LogisticCompany.Models
{
    public class Office
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }
    }
}
