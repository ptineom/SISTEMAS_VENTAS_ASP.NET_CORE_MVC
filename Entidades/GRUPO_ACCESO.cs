using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class GRUPO_ACCESO
	{
		public string ACCION { get; set; }
		public int ID_GRUPO_ACCESO { get; set; }

        [Display(Name = "Nombre de grupo acceso")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
		public string NOM_GRUPO_ACCESO { get; set; }

		public string OBS_GRUPO_ACCESO { get; set; }
		public bool FLG_CTRL_TOTAL { get; set; }
		public string ID_USUARIO_REGISTRO { get; set; }
        public string XML_APLICACIONES { get; set; }
	}
}
