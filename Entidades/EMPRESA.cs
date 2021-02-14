using Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Entidades
{
    public class EMPRESA
    {
        public string ACCION { get; set; }

        public string ID_EMPRESA { get; set; }

        [Display(Name = "Razón social")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
        public string NOM_EMPRESA { get; set; }

        public string ID_USUARIO_REGISTRO { get; set; }

        public string LOGO_EMPRESA { get; set; }

        [Required(ErrorMessage = Constantes.RequiredMensaje)]
        [Range(1, 100, ErrorMessage = Constantes.RangeMensaje)]
        public decimal IGV { get; set; }

        [Display(Name = "RUC")]
        [Required(ErrorMessage = Constantes.RequiredMensaje)]
        [StringLength(11, MinimumLength = 11, ErrorMessage = Constantes.StringLengthMensaje)]
        public string NUMERO_RUC { get; set; }

        [Display(Name ="Stock mínimo")]
        public decimal STOCK_MINIMO { get; set; }

        [Required(ErrorMessage = "Debe de ingresar el monto excedente de la boleta de venta")]
        [Display(Name = "Monto excedente")]
        public decimal MONTO_BOLETA_OBLIGATORIO_CLIENTE { get; set; }

        public SUCURSAL SUCURSAL_EN_SESION { get; set; }

        public string FOTO1 { get; set; }
        public string FOTO2 { get; set; }
        public string FOTO_B64 { get; set; }

        public List<SUCURSAL>sucursales{ get; set; }

    }
}
