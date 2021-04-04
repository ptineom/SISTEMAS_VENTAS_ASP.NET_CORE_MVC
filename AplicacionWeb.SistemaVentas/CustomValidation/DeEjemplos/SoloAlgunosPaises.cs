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
    public class SoloAlgunosPaisesAttribute: ValidationAttribute
    {
        public override string FormatErrorMessage(string name)
        {
            return "País inválido. Los valores válidos son Peru, cUBA y chile.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));

            string pais = value.ToString().ToUpper();

            if (pais == "PERU" || pais == "CHILE" || pais == "CUBA")
                return ValidationResult.Success;

            return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }
    }

    public class SoloAlgunosPaisesAttributeAdapter : AttributeAdapterBase<SoloAlgunosPaisesAttribute>
    {
        public SoloAlgunosPaisesAttributeAdapter(SoloAlgunosPaisesAttribute attribute, IStringLocalizer stringLocalizer)
            :base(attribute, stringLocalizer)
        {
        
        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-solo-algunos-paises", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }

}
