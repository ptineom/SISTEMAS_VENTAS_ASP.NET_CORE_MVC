using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class OBJETOS_DOC_COMPRA
    {
        public List<TIPO_DOCUMENTO> ListTipoDocumento { get; set; }
        public List<TIPO_COMPROBANTE> ListTipoComprobante { get; set; }
        public List<MONEDA> ListMoneda { get; set; }
        public List<TIPO_PAGO> ListTipoPago { get; set; }
        public List<TIPO_CONDICION_PAGO> ListTipoCondicionPago { get; set; }
        public List<ESTADO> ListEstado { get; set; }
        public List<UBIGEO> ListDepartamento { get; set; }
        public decimal TasIgv { get; set; }
    }
}
