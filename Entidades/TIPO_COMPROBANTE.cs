using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class TIPO_COMPROBANTE
	{
		public string ACCION { get; set; }
		public string ID_TIPO_COMPROBANTE { get; set; }

        [Display(Name = "Tipo comprobante")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
		public string NOM_TIPO_COMPROBANTE { get; set; }

		public bool FLG_VENTA { get; set; }
		public bool FLG_COMPRA { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
        public bool FLG_RENDIR_SUNAT { get; set; }
        public bool FLG_NO_EDITABLE { get; set; }
        public string JSON_TIPO_NC { get; set; }
        public string LETRA_INICIAL_SERIE_ELECTRONICA { get; set; }

    }
}
