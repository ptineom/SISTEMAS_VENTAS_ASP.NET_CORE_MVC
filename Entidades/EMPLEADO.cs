using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class EMPLEADO
	{
		public string ACCION { get; set; }
		public string ID_EMPLEADO { get; set; }
		public int ID_CARGO { get; set; }
		public int ID_TIPO_DOCUMENTO { get; set; }
		public string NUM_DOCUMENTO { get; set; }
		public string APELLIDO_PATERNO { get; set; }
		public string APELLIDO_MATERNO { get; set; }
		public string NOMBRES { get; set; }
		public string DIRECCION { get; set; }
		public string EMAIL { get; set; }
        public string FEC_ENTRANTE { get; set; }
        public string FEC_CESE { get; set; }
		public bool FLG_INACTIVO { get; set; }
		public string ID_UBIGEO { get; set; }
		public string SEXO { get; set; }
		public string FOTO { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
        public string TELEFONO { get; set; }
        //
        public string NOM_CARGO { get; set; }
        public string DOCUMENTO { get; set; }
        public string NOM_EMPLEADO { get; set; }
        public string FOTO_B64 { get; set; }
        public List<CARGO> LISTA_CARGO { get; set; }
        public List<TIPO_DOCUMENTO> LISTA_TIPO_DOCUMENTO { get; set; }
        public bool FLG_SIN_FOTO { get; set; }
	}
}
