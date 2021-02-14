using System;
using System.Collections.Generic;
using System.Text;

namespace Helper
{
    public static class Extensiones
    {
        public static object getValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName).GetValue(obj, null);
        }
    }
}
