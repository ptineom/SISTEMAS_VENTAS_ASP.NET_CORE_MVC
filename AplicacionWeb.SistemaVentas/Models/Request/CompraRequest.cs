using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Request
{
    public class CompraRequest
    {
        public string IdTipoComprobante { get; set; }
        public string NroSerie { get; set; }
        public int NroDocumento { get; set; }
        public string IdProveedor { get; set; }
        public string IdMoneda { get; set; }
        public string FecCompra { get; set; }
        public string FecVencimiento { get; set; }
        public string IdTipoPago { get; set; }
        public string IdTipoCondicionPago { get; set; }
        public decimal TotBruto { get; set; }
        public decimal TotDescuento { get; set; }
        public decimal TasDescuento { get; set; }
        public decimal TasIgv { get; set; }
        public decimal TotImpuesto { get; set; }
        public decimal TotCompra { get; set; }
        public string JsonArticulos { get; set; }
        public decimal Abono { get; set; }
        public decimal Saldo { get; set; }
        public string Observacion { get; set; }


    }
}
