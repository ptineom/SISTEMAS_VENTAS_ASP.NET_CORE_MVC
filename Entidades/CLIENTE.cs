using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class CLIENTE
	{
		public string ACCION { get; set; }
		public string ID_CLIENTE { get; set; }
		public int ID_TIPO_DOCUMENTO { get; set; }
		public string NRO_DOCUMENTO { get; set; }
		public string NOM_CLIENTE { get; set; }
		public string DIR_CLIENTE { get; set; }
		public string TEL_CLIENTE { get; set; }
		public string EMAIL_CLIENTE { get; set; }
		public string ID_UBIGEO { get; set; }
		public string OBS_CLIENTE { get; set; }
		public bool FLG_PERSONA_NATURAL { get; set; }
		public string SEXO { get; set; }
		public string APELLIDO_PATERNO { get; set; }
		public string APELLIDO_MATERNO { get; set; }
		public string NOMBRES { get; set; }
		public string CONTACTO { get; set; }
		public bool FLG_INACTIVO { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
        //
        public string ABREVIATURA { get; set; }
	}
}
