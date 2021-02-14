using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Seguridad
{
    public class ReqLoginViewModel
    {
        [Required(ErrorMessage = "Debe de ingresar el {0}")]
        [Display(Name = "Usuario")]
        public string idUsuario { get; set; }

        [Required(ErrorMessage = "Debe de ingresar la {0}")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }
    }

    public class ReqSeleccionSucursalViewModel : ReqLoginViewModel
    {
        [Required(ErrorMessage = "Debe de ingresar el {0}")]
        public string idSucursal { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el {0}")]
        public string nomSucursal { get; set; }
    }

}
