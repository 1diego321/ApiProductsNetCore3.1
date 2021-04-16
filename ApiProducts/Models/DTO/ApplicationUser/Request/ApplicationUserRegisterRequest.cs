using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Models.DTO.ApplicationUser.Request
{
    public class ApplicationUserRegisterRequest
    {
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener un mínimo de 5 y un máximo de 30 caracteres.")]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener un mínimo de 5 y un máximo de 30 caracteres.")]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener un máximo de 50 caracteres.")]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener un máximo de 50 caracteres.")]
        [Display(Name = "Apellidos")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(50, ErrorMessage = "El campo {0} debe tener un máximo de 50 caracteres.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo electrónico valido para el campo {0}.")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }
    }
}
