using AplicacionWeb.SistemaVentas.Models.Request;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.CustomValidation
{
    public class DocumentoSegunEdadAttribute : ValidationAttribute
    {
        private string _documentoEvaluar = "RUC";
        public int EdadEvaluar { get; set; }

        public DocumentoSegunEdadAttribute(int edadEvaluar)
        {
            EdadEvaluar = edadEvaluar;
        }
        public override string FormatErrorMessage(string name)
        {
            return $"El documento debe ser {_documentoEvaluar} si supera la edad de {EdadEvaluar} años";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            string documento = value.ToString().ToUpper();
            var registro = (RegistrationViewModel)validationContext.ObjectInstance;

            if (registro.Age >= EdadEvaluar && documento != _documentoEvaluar)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    public class DocumentoSegunEdadAttributeAdapter : AttributeAdapterBase<DocumentoSegunEdadAttribute>
    {
        public DocumentoSegunEdadAttributeAdapter(DocumentoSegunEdadAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-documento-segun-edad", GetErrorMessage(context));

            //Parámetro que se usará en el cliente, viene del tipo generic declarado en el nombre de la clase
            var edadEvaluar = Attribute.EdadEvaluar.ToString(CultureInfo.InvariantCulture);
            MergeAttribute(context.Attributes, "data-val-documento-segun-edad-edadevaluar", edadEvaluar);
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
