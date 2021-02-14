using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class SUCURSAL_CAJA_USUARIO
	{
		public string ACCION { get; set; }
		public string ID_USUARIO { get; set; }
		public string ID_SUCURSAL { get; set; }
		public string ID_CAJA { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
		public bool FLG_INACTIVO { get; set; }
        //
        public string NOM_USUARIO { get; set; }
        public string XML_USUARIOS { get; set; }
	}
}
