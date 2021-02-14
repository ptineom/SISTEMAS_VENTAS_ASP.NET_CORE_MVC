using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models
{
    public class ResultadoLoginViewModel
    {
        public bool masDeUnaSucursal { get; set; }
        public string returnUrl { get; set; }
        public List<SucursalViewModel> sucursales { get; set; }
    }
}
