using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Extensiones
{
    public static class Helpers
    {
        public static object getValueProperty(this object objeto, string propiedad)
        {
            var propertyInfo = objeto.GetType().GetProperty(propiedad);
            return propertyInfo.GetValue(objeto, null);
        }
    }
}
