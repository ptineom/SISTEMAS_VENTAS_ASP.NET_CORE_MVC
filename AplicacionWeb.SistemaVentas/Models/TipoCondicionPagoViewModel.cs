using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models
{
    public class TipoCondicionPagoViewModel
    {
        public string IdTipoCondicionPago { get; set; }
        public string NomTipoCondicionPago { get; set; }
        public bool FlgEvaluaCredito { get; set; }
    }
}
