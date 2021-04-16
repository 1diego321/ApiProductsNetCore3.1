using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.ApplicationUser.Request
{
    public class ApplicationUserAuthRequest
    {
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //[Display(Name = "Nombre de Usuario o Correo Electronico")]
        public string UserNameOrEmail { get; set; }
        //[Required(ErrorMessage = "El campo {0} es obligatorio.")]
        //[Display(Name = "Contraseña")]
        public string Password { get; set; }
    }
}
