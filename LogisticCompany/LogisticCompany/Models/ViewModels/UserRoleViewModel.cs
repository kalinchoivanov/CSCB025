using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogisticCompany.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public ApplicationUser ApplicationUser { get; set; }

        public string Role { get; set; }
    }
}
