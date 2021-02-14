using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class SUCURSAL_USUARIO
	{
		public string ACCION { get; set; }
		public string ID_SUCURSAL { get; set; }
		public string ID_USUARIO { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        //
        public bool CHK { get; set; }
        public string NOM_SUCURSAL { get; set; }
        public string DIRECCION { get; set; }
        public bool FLG_PRINCIPAL { get; set; }
        public string CADENA_SUCURSALES { get; set; }
        public string NOM_USUARIO { get; set; }
        public string XML_USUARIOS { get; set; }
	}
}
