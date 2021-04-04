using AplicacionWeb.SistemaVentas.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Request
{
    //public class ContactModel
    //{
    //    [Required]
    //    [StringLength(5, MinimumLength = 5)]
    //    public string CustomerID { get; set; }

    //    [Required]
    //    [StringLength(40)]
    //    public string CompanyName { get; set; }

    //    [Required]
    //    [StringLength(40)]
    //    public string ContactName { get; set; }

    //    [Required]
    //    [Country]
    //    public string Country { get; set; }
    //}

    public class RegistrationViewModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Username { get; set; }

        [DebeSerMarcado]
        [Display(Name = "Aceptar política de privacidad")]
        public bool AcceptedPrivacyPolicy { get; set; }

        [Required]
        [Display(Name = "Edad")]
        public int Age { get; set; }

        [DocumentoSegunEdad(22)]
        [Display(Name = "Documento")]
        public string Document { get; set; }

        public int Note { get; set; }

        [Display(Name = "Pais")]
        [SoloAlgunosPaises]
        public string Country { get; set; }


    }
}
