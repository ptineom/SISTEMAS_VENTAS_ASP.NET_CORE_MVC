using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models
{
    public class ResultadoLoginViewModel
    {
        public bool MasDeUnaSucursal { get; set; }
        public string ReturnUrl { get; set; }
        public List<SucursalViewModel> Sucursales { get; set; }
    }
}
