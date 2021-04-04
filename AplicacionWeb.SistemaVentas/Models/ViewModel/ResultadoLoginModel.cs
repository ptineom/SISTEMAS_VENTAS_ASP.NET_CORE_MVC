using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.ViewModel
{
    public class ResultadoLoginModel
    {
        public bool MasDeUnaSucursal { get; set; }
        public string ReturnUrl { get; set; }
        public List<SucursalModel> Sucursales { get; set; }
    }
}
