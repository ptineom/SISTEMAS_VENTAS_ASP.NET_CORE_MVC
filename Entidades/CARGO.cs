using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class CARGO
    {
        public string ACCION { get; set; }
        [Display(Name = "IdCargo")]
        [Required(ErrorMessage = "Debe de ingresar el Id del cargo")]
        public int ID_CARGO { get; set; }
        [Required(ErrorMessage = "Debe de ingresar el nombre del cargo")]
        [Display(Name = "Cargo")]
        [StringLength(50, MinimumLength = 1, ErrorMessage = Constantes.StringLengthMensaje)]
        public string NOM_CARGO { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        [Display(Name = "Abreviatura")]
        public string ABREVIATURA { get; set; }
    }
}
