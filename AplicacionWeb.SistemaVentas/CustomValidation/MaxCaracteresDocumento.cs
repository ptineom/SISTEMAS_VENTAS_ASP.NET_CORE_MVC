using AplicacionWeb.SistemaVentas.Extensiones;
using AplicacionWeb.SistemaVentas.Models.Request;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.CustomValidation
{
    public class MaxCaracteresDocumentoAttribute: ValidationAttribute
    {
        private string _propertyMaxCharacter = string.Empty;

        public MaxCaracteresDocumentoAttribute(string propertyMaxCharacter)
        {
            _propertyMaxCharacter = propertyMaxCharacter;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"El tamaño de caracteres del {name} es inválido.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            string documento = value.ToString();
            int maxCharacter = Convert.ToInt32(validationContext.ObjectInstance.GetValueProperty(this._propertyMaxCharacter));

            if (documento.Length != maxCharacter)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            return ValidationResult.Success;
        }
    }

    public class MaxCaracteresDocumentoAttributeAdapter : AttributeAdapterBase<MaxCaracteresDocumentoAttribute>
    {
        public MaxCaracteresDocumentoAttributeAdapter(MaxCaracteresDocumentoAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-max-caracteres-documento", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
