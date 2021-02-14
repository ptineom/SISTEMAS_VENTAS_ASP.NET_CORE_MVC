using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class CONCEPTO_I_E_CAJA
	{
		public string ACCION { get; set; }
		public string ID_CONCEPTO_I_E { get; set; }
		public string NOM_CONCEPTO_I_E { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
		public string ID_TIPO_MOVIMIENTO { get; set; }
        public bool FLG_PAGO_PROVEEDORES { get; set; }
        //
        public string NOM_TIPO_MOVIMIENTO { get; set; }
	}
}
