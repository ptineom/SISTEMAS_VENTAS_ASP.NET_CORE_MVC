using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class INGRESO_EGRESO_CAJA
	{
		public string ACCION { get; set; }
        public string ID_SUCURSAL { get; set; }
        public string ID_TIPO_MOVIMIENTO { get; set; }
		public string ID_I_E_CAJA { get; set; }
		public decimal IMPORTE { get; set; }
		public string ID_CONCEPTO_I_E { get; set; }
		public string DETALLE { get; set; }
		public DateTime FECHA_I_E { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
        public string ID_MONEDA { get; set; }
        public List<CONCEPTO_I_E_CAJA> listaConceptoIE { get; set; }
        public List<MONEDA> listaMonedas { get; set; }
        public string ID_CAJA_CA { get; set; }
        public string ID_USUARIO_CA { get; set; }
        public int CORRELATIVO_CA { get; set; }
        public string HOR_DOCUMENTO { get; set; }
        public bool FLG_AUTOMATICO { get; set; }
        //
        public string NOM_CONCEPTO_I_E { get; set; }
        public string SGN_MONEDA { get; set; }
	}
}
