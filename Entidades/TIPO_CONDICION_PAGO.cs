using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class TIPO_CONDICION_PAGO
	{
		public string ACCION { get; set; }
		public string ID_TIPO_CONDICION_PAGO { get; set; }
		public string NOM_TIPO_CONDICION_PAGO { get; set; }
		public bool FLG_NO_EVALUA_CREDITO { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
	}
}
