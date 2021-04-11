using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Request
{
    public class DeleteCompraRequest
    {
        public string IdTipoComprobante { get; set; }
        public string NroSerie { get; set; }
        public int NroDocumento { get; set; }
        public string IdProveedor { get; set; }
        
    }
}
