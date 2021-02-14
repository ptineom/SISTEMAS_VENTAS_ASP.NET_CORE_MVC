using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class TIPO_DOCUMENTO
	{
		public string ACCION { get; set; }
		public int ID_TIPO_DOCUMENTO { get; set; }
		public string NOM_TIPO_DOCUMENTO { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
		public string ABREVIATURA { get; set; }
        public bool FLG_NO_NATURAL { get; set; }
        public int MAX_DIGITOS { get; set; }
    }
}
