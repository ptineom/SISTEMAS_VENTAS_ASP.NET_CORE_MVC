using AplicacionWeb.SistemaVentas.Extensiones;
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
    public class RequiredDireccionAttribute : ValidationAttribute
    {
        public string _otherProperty { get; }

        public RequiredDireccionAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }

        public override string FormatErrorMessage(string name)
        {
            return $"Ingrese la {name}";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(_otherProperty))
                throw new ArgumentNullException(nameof(_otherProperty));

            var valueIdDepartamento = validationContext.ObjectInstance.GetValueProperty(this._otherProperty);
            if (valueIdDepartamento == null)
                throw new ArgumentNullException(nameof(_otherProperty));

            if (!string.IsNullOrEmpty(valueIdDepartamento.ToString()))
            {
                //Validamos que la propiedad a validar no sea nulo.
                if (string.IsNullOrEmpty(value.ToString()))
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
            }

            return ValidationResult.Success;
        }
    }

    public class RequiredDireccionAttributeAdapter : AttributeAdapterBase<RequiredDireccionAttribute>
    {
        public RequiredDireccionAttributeAdapter(RequiredDireccionAttribute attribute, IStringLocalizer stringLocalizer)
            : base(attribute, stringLocalizer)
        {

        }

        public override void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            MergeAttribute(context.Attributes, "data-val", "true");
            MergeAttribute(context.Attributes, "data-val-required-direccion", GetErrorMessage(context));
        }

        public override string GetErrorMessage(ModelValidationContextBase validationContext)
        {
            if (validationContext == null)
                throw new ArgumentNullException(nameof(validationContext));

            return GetErrorMessage(validationContext.ModelMetadata, validationContext.ModelMetadata.GetDisplayName());
        }
    }
}
