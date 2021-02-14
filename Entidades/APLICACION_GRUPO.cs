using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class APLICACION_GRUPO
	{
		public string ACCION { get; set; }
		public int ID_GRUPO_ACCESO { get; set; }
		public int ID_APLICACION { get; set; }
		public bool FLG_NUEVO { get; set; }
		public bool FLG_MODIFICACION { get; set; }
		public bool FLG_ANULACION { get; set; }
		public bool FLG_REPORTE { get; set; }
		public bool FLG_APROBACION { get; set; }
		public bool FLG_CIERRE { get; set; }
		public bool FLG_REAPERTURA { get; set; }
		public bool FLG_EXPORTAR { get; set; }
		public bool FLG_IMPORTAR { get; set; }
	}
}
