using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class ARTICULO_UM
	{
		public string ACCION { get; set; }
		public string ID_UM { get; set; }
		public string ID_ARTICULO { get; set; }
		public decimal NRO_FACTOR { get; set; }
		public int NRO_ORDEN { get; set; }
        public decimal DESCUENTO1 { get; set; }
        public decimal PRECIO_VENTA { get; set; }
        public decimal PRECIO_VENTA_FINAL { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        public bool FLG_PROMOCION { get; set; }
        public string FEC_INICIO_PROMOCION { get; set; }
        public string FEC_FINAL_PROMOCION { get; set; }
        //
        public string NOM_UM { get; set; }
	}
}
