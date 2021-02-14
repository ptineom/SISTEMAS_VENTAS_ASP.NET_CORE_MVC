using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Helper;
namespace Entidades
{
    public class USUARIO
    {
        public string ACCION { get; set; }

        [Required(ErrorMessage = Constantes.RequiredMensaje)]
        [StringLength(20,ErrorMessage = Constantes.MaxLengthMensaje)]
        [Display(Name = "Usuario")]
        public string ID_USUARIO { get; set; }

        public string NOM_USUARIO { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
        public string CLAVE { get; set; }

        [EmailAddress(ErrorMessage = Constantes.EmailAddressMensaje )]
        public string EMAIL_USUARIO { get; set; }

        [Required(ErrorMessage = "Debe de seleccionar el empleado")]
        public string ID_EMPLEADO { get; set; }

        [Required(ErrorMessage = "Debe de seleccionar el grupo de acceso")]
        public int ID_GRUPO_ACCESO { get; set; }

        public string ID_USUARIO_REGISTRO { get; set; }
        public bool FLG_INACTIVO { get; set; }
        //
        public string NOM_ROL { get; set; }
        public string FOTO { get; set; }
        public string NOM_GRUPO_ACCESO { get; set; }
        public int COUNT_SEDES { get; set; }
        public string ID_SUCURSAL { get; set; }
        public string NOM_SUCURSAL { get; set; }
        public bool FLG_CTRL_TOTAL { get; set; }
        public bool FLG_ENVIAR_CORREO { get; set; }
        public string CONTRASENIA_ACTUAL { get; set; }
        public string TOKEN_RECUPERACION_PASSWORD { get; set; }
    }
}
