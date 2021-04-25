using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Request
{
    public class ReaperturarCajaRequest
    {
        [Required(ErrorMessage = "Ingrese el {0}")]
        [Display(Name = "Id caja")]
        public string IdCaja { get; set; }

        [Required(ErrorMessage = "Ingrese el {0}")]
        [Display(Name = "Usuario")]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "Ingrese el {0}")]
        public int Correlativo { get; set; }
    }
}
