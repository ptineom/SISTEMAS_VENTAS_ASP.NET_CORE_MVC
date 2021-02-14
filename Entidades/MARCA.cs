using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
	public class MARCA
	{
		public string ACCION { get; set; }
		public int ID_MARCA { get; set; }

        [Display(Name = "Marca")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
		public string NOM_MARCA { get; set; }

		public string ID_USUARIO_REGISTRO { get; set; }
	}
}
