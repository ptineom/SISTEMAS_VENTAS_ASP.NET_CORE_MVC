using AplicacionWeb.SistemaVentas.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Request
{
    public class ProveedorRequest
    {
        public string IdProveedor { get; set; }

        [Required(ErrorMessage = "Seleccione el {0}")]
        [Display(Name = "Tipo documento")]
        public int? IdTipoDocumento { get; set; }

        [Required(ErrorMessage = "Ingrese el {0}")]
        [MaxCaracteresDocumento("MaxDigitosDocumento")]
        [Display(Name = "Número documento")]
        public string NroDocumento { get; set; }

        [Required(ErrorMessage = "Ingrese la {0}")]
        [Display(Name = "Razón social")]
        public string RazonSocial { get; set; }

        public string Contacto { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        [Display(Name = "Departamento")]
        public string IdDepartamento { get; set; }

        [RequiredProvincia("IdDepartamento")]
        [Display(Name = "Provincia")]
        public string IdProvincia { get; set; }

        [RequiredDistrito("IdDepartamento")]
        [Display(Name = "Distrito")]
        public string IdDistrito { get; set; }

        [RequiredDireccion("IdDepartamento")]
        [Display(Name = "Direccion")]
        public string Direccion { get; set; }
        public string Observacion { get; set; }
        public bool FlgInactivo { get; set; }
        public int MaxDigitosDocumento { get; set; }
    }
}
