using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.CustomValidation
{
    public class AdapterProvider : IValidationAttributeAdapterProvider
    {
        private readonly IValidationAttributeAdapterProvider _baseProvider = new ValidationAttributeAdapterProvider();

        public IAttributeAdapter GetAttributeAdapter(ValidationAttribute attribute, IStringLocalizer stringLocalizer)
        {
            if (attribute is SoloAlgunosPaisesAttribute)
                return new SoloAlgunosPaisesAttributeAdapter(attribute as SoloAlgunosPaisesAttribute, stringLocalizer);
            else if (attribute is DebeSerMarcadoAttribute)
                return new DebeSerMarcadoAttributeAdapter(attribute as DebeSerMarcadoAttribute, stringLocalizer);
            else if (attribute is DocumentoSegunEdadAttribute)
                return new DocumentoSegunEdadAttributeAdapter(attribute as DocumentoSegunEdadAttribute, stringLocalizer);
            else if (attribute is RequiredProvinciaAttribute)
                return new RequiredProvinciaAttributeAdapter(attribute as RequiredProvinciaAttribute, stringLocalizer);
            else if (attribute is RequiredDistritoAttribute)
                return new RequiredDistritoAttributeAdapter(attribute as RequiredDistritoAttribute, stringLocalizer);
            else if (attribute is RequiredDireccionAttribute)
                return new RequiredDireccionAttributeAdapter(attribute as RequiredDireccionAttribute, stringLocalizer);
            else if (attribute is MaxCaracteresDocumentoAttribute)
                return new MaxCaracteresDocumentoAttributeAdapter(attribute as MaxCaracteresDocumentoAttribute, stringLocalizer);
            else
                return _baseProvider.GetAttributeAdapter(attribute, stringLocalizer);

        }
    }
}