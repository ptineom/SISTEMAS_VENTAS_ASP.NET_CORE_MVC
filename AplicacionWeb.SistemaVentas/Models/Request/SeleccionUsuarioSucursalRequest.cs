using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Request
{
    public class SeleccionUsuarioSucursalRequest
    {
        [Required(ErrorMessage = "Ingrese el {0}")]
        [Display(Name = "Usuario")]
        public string idUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese la {0}")]
        [Display(Name = "Contraseña")]
        public string password { get; set; }

        [Required(ErrorMessage = "Ingrese el {0}")]
        [Display(Name = "Id sucursal")]
        public string idSucursal { get; set; }

        [Required(ErrorMessage = "Ingrese el {0}")]
        [Display(Name = "Nombre sucursal")]
        public string nomSucursal { get; set; }
    }
}
