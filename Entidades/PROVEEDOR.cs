using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class PROVEEDOR
	{
		public string ACCION { get; set; }
		public string ID_PROVEEDOR { get; set; }
		public int ID_TIPO_DOCUMENTO { get; set; }
		public string NRO_DOCUMENTO { get; set; }
		public string NOM_PROVEEDOR { get; set; }
		public string DIR_PROVEEDOR { get; set; }
		public string TEL_PROVEEDOR { get; set; }
		public string EMAIL_PROVEEDOR { get; set; }
		public string ID_UBIGEO { get; set; }
		public string OBS_PROVEEDOR { get; set; }
		public string CONTACTO { get; set; }
        public bool FLG_INACTIVO { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        public bool FLG_MISMA_EMPRESA { get; set; }
        //
        public string ABREVIATURA { get; set; }
	}
}
