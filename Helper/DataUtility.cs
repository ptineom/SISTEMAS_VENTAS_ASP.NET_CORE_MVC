using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Helper
{
    public static class DataUtility
    {
        public static DateTime? ObjectToDateTime(object obj)
        {
            IFormatProvider culture = new CultureInfo("es-PE", true);
            return ((obj == null) || (obj == DBNull.Value)) ? DateTime.MinValue : Convert.ToDateTime(obj, culture);
        }
        public static DateTime? ObjectToDateTimeNull(object obj)
        {
            IFormatProvider culture = new CultureInfo("es-PE", true);
            DateTime? value = null;
            if ((obj == null) || (obj == DBNull.Value))
            {
                return value;
            }
            else
            {
                value = Convert.ToDateTime(obj, culture);
            }
            return value;
        }
        public static decimal ObjectToDecimal(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? 0.00M : Convert.ToDecimal(obj);
        }
        public static string ObjectToString(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value)) ? "" : Convert.ToString(obj).Trim();
        }
        public static Int32 ObjectToInt32(object obj)
        {
            return ((obj == null) || (obj == DBNull.Value) || string.IsNullOrEmpty(ObjectToString(obj))) ? 0 : Convert.ToInt32(obj);
        }
    }
}
