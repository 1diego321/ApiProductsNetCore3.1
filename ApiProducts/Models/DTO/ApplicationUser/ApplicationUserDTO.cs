using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.ApplicationUser
{
    public class ApplicationUserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ApplicationUserStatusId { get; set; }
        public string ApplicationUserStatusDescription { get; set; }

    }
}
