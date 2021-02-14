using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class SUCURSAL_TIPO_COMPROBANTE
	{
		public string ACCION { get; set; }
		public string ID_SUCURSAL { get; set; }
		public string ID_TIPO_COMPROBANTE { get; set; }
		public string NRO_SERIE { get; set; }
		public int CORRELATIVO_INICIAL { get; set; }
		public int CORRELATIVO_FINAL { get; set; }
		public int CORRELATIVO_ACTUAL { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
        //
        public string NOM_TIPO_COMPROBANTE { get; set; }

        public List<TIPO_COMPROBANTE> listaTipCom { get; set; }
        public bool FLG_RENDIR_SUNAT { get; set; }
        public bool FLG_EN_CURSO { get; set; }
    }
}
