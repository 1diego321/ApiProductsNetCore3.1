using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.ApplicationUser
{
    public class ApplicationUserLoginDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ApplicationUserStatusId { get; set; }
        public string ApplicationUserStatusDescription { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public string FullName => FirstName + " " + LastName;
    }
}
