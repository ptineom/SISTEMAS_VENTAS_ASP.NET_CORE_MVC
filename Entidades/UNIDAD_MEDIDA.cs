using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UNIDAD_MEDIDA
    {
        public string ACCION { get; set; }
        public string ID_UM { get; set; }

        [Display(Name = "Unidad medida")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
        public string NOM_UM { get; set; }

        public bool FLG_MEDIDA_PESO { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        public string ABREVIADO { get; set; }
    }
}
