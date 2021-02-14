using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class APLICACION
	{
		public int ID_APLICACION { get; set; }
		public string NOM_APLICACION { get; set; }
		public int ID_APLICACION_PADRE { get; set; }
		public bool FLG_FORMULARIO { get; set; }
		public string ICON_SPA { get; set; }
        public string ROUTE_SPA { get; set; }
        public bool FLG_RAIZ { get; set; }
        public string BREADCRUMS { get; set; }
        public bool FLG_HOME { get; set; }
    }
}
