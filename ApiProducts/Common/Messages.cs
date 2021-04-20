using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProducts.Common
{
    public static class Messages
    {
        public const string InternalServerError = "Ha ocurrido un error interno en el servidor.";
        public const string ResourceNotFound = "El recurso solicitado no se ha encontrado o no existe.";
        public const string ResourceDisabled = "El recurso solicitado está desactivado.";
        public const string AccountDisabled = "La cuenta de usuario está desactivada.";
        public const string ValidationsFailed = "Se han incumplido una o más validaciones.";
        public const string ResourceNameAlreadyExists = "El nombre del recurso ingresado ya existe.";
        public const string ResourceCodeAlreadyExists = "El código del recurso ingresado ya existe.";
        public const string InvalidUserNamePassword = "Nombre de usuario y/o contraseña incorrectos.";
        public const string SubCategoryIdDontExists = "El identificador de sub categoria no existe.";
    }
}
