using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
namespace Helper
{
    public class ViewHelper
    {
        public ViewHelper()
        {
            
        }

        public static string getValueConfiguration(string section)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var configBuilder = new ConfigurationBuilder();
            configBuilder.AddJsonFile(path, false);
            return configBuilder.Build().GetSection(section).Value;
        }
        public static string getNroComprobante(string numero)
        {
            int tamanioMax = 6;
            string cadena = $"00000{numero}";
            return cadena.Substring(cadena.Length - tamanioMax);
        }

        // Primera letra de la palabra en mayuscula
        public static string UCWords(string str)
        {
            if (str.Length > 2)
            {
                return str.Trim().Substring(0, 1).ToUpper() + str.Trim().Substring(1).ToLower();
            }
            else
            {
                return str;
            }
        }
        // fecha con hora y sin hora
        public static string getDate(bool time = false)
        {
            if (time) return System.DateTime.Now.ToString("dd/MM/yyyy H:mm:ss");
            else return System.DateTime.Now.ToString("dd/MM/yyyy");
        }
        // prefijo para la creacion de archivos para que no se repita
        public static string getNameForFiles()
        {
            return System.DateTime.Now.ToString("yyyyMMddHHmmssfff");
        }
        // convierte la fecha formato 2018/02/09 14:10:10 a 09/02/2018 14:10:10
        public static string ConvertToDate(string d)
        {
            if (d == null) return "";
            if (d.Length < 10) return "";

            var fecha1 = d.Substring(0, 10);
            var hora = "";

            string[] fecha = fecha1.Split('/');
            if (d.Length > 10)
            {
                hora = d.Substring(11, 8);
            }
            fecha1 = fecha[2] + "/" + fecha[1] + "/" + fecha[0] + " " + hora;
            return fecha1.Trim();
        }
        // obtiene el nombre corto del mes
        public static string getShortMonthName(int m)
        {
            string mes = "";
            switch ((m))
            {
                case 1:
                    mes = "Ene";
                    break;
                case 2:
                    mes = "Feb";
                    break;
                case 3:
                    mes = "Mar";
                    break;
                case 4:
                    mes = "Abr";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Jun";
                    break;
                case 7:
                    mes = "Jul";
                    break;
                case 8:
                    mes = "Ago";
                    break;
                case 9:
                    mes = "Sept";
                    break;
                case 10:
                    mes = "Oct";
                    break;
                case 11:
                    mes = "Nov";
                    break;
                case 12:
                    mes = "Dic";
                    break;
            }

            return mes;
        }
        // obtiene el nombre completo del mes
        public static string getMonthName(int m)
        {
            string mes = "";
            switch ((m))
            {
                case 1:
                    mes = "Enero";
                    break;
                case 2:
                    mes = "Febrero";
                    break;
                case 3:
                    mes = "Marzo";
                    break;
                case 4:
                    mes = "Abril";
                    break;
                case 5:
                    mes = "Mayo";
                    break;
                case 6:
                    mes = "Junio";
                    break;
                case 7:
                    mes = "Julio";
                    break;
                case 8:
                    mes = "Agosto";
                    break;
                case 9:
                    mes = "Setiembre";
                    break;
                case 10:
                    mes = "Octubre";
                    break;
                case 11:
                    mes = "Noviembre";
                    break;
                case 12:
                    mes = "Diciembre";
                    break;
            }

            return mes;
        }

        // convierte la cadena de numeros, los miles con comas y a 2 decimales
        public static string toMoneyFormat(string str)
        {
            decimal valor = default(decimal);
            if (decimal.TryParse(str, out valor))
            {
                return string.Format("{0:n}", valor);
            }
            else
            {
                return "0.00";
            }
        }
        // convierte la cadena de numeros, los miles con comas y los decimales segun lo ordene.
        public static string toMoneyFormat(string str, decimal decimals)
        {
            decimal valor = default(decimal);
            string zero = "";
            for (int i = 1; i <= decimals; i++)
            {
                zero += "0";
            }
            if (decimal.TryParse(str, out valor))
            {
                return string.Format("{0:0,0." + zero + "}", valor);
            }
            else
            {
                return "0.000";
            }
        }

        // inserta una lista de string y te retornara una sola palabra separadas por guiones.
        // para ser usada como una url amigable
        public static string ConvertNameToUrl(params string[] words)
        {
            string str = string.Join("-", words);

            if (str == null) return "";

            str = str.ToLower();
            str = str.Trim();

            str = str.Replace(",", "");
            str = str.Replace("&", "");
            str = str.Replace(" ", "-");
            str = str.Replace("--", "-");

            // Tildes, ñ, caracteres raros
            str = str.Replace("ñ", "n");
            str = str.Replace("á", "a");
            str = str.Replace("é", "e");
            str = str.Replace("í", "i");
            str = str.Replace("ó", "o");
            str = str.Replace("ú", "u");

            return str;
        }
        // Obtiene solo la hora de la (fecha y hora) que se ingresa. en formato tal
        public static string TimeFormat(string d)
        {
            if (d == "") return "";
            else return Convert.ToDateTime(d).ToString("hh:mmtt", new System.Globalization.CultureInfo("en-US"));
        }
        // resta de horas
        public static string TimeDiff(string hinicio, string hfin)
        {
            if (hinicio == "") return "0";
            if (hfin == "") return "0";

            DateTime a = ViewHelper.DateTimeFromTime(hinicio);
            DateTime b = ViewHelper.DateTimeFromTime(hfin);
            TimeSpan c = b - a;

            return c.ToString(@"hh\:mm");
        }
        // convierte la hora en fecha completa(fecha del dia con la hora insertada)
        public static DateTime DateTimeFromTime(string d)
        {
            return Convert.ToDateTime(d);
        }
        // corta la frase segun en e indice que se le indique, y sera continuado con puntos suspensivos.
        public static string CutPhrases(string frase, int lenght)
        {
            if (frase == null) return "";
            if (frase.Length < lenght) return frase;
            return frase.Substring(0, lenght) + " ...";
        }
        //  de cada palabra de la frase la priemra letra lo convierte en mayuscula.
        public static string CapitalizeAll(string frase)
        {
            if (frase == null)
            {
                return "";
            }
            else
            {
                String resultado = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(frase.ToLower());
                //String resultado2 = new CultureInfo("en-US", false).TextInfo.ToTitleCase(frase);
                frase = resultado;
            }
            return frase;
        }
        // obtengo hace que tiempo fue ingresado algo(fecha y hora)
        public static string TimeAgo(string d)
        {
            if (d == "") return "";

            bool time = true;

            if (d.Length == 10) time = false;

            DateTime dt = Convert.ToDateTime(d);

            TimeSpan timeSince = DateTime.Now.Subtract(dt);
            if (timeSince.TotalMilliseconds < 1)
                return "Todavía";
            if (timeSince.TotalMinutes < 1)
            {
                if (time)
                {
                    int s = Convert.ToInt32(timeSince.TotalSeconds);
                    return "Hace " + (s >= 0 && s <= 15 ? "un instante" : s + " segundos");
                }
                else
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalMinutes < 2)
            {
                if (time)
                {
                    return "Hace un minuto";
                }
                else
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalMinutes < 60)
            {
                if (time)
                {
                    return string.Format("Hace {0} minutos", timeSince.Minutes);
                }
                else
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalMinutes < 120)
            {
                if (time)
                {
                    return "Hace una hora";
                }
                else
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalHours < 24)
            {
                if (time)
                {
                    return string.Format("Hace {0} horas", timeSince.Hours);
                }
                else
                {
                    return "Hoy";
                }
            }
            if (timeSince.TotalDays < 2)
            {
                if (time)
                {
                    return "Ayer a la(s) " + dt.ToString("hh:mmtt").Replace(".", "");
                }
                else
                {
                    return "Ayer";
                }
            }

            if (timeSince.TotalDays < 365)
            {
                if (time)
                {
                    return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower() + " a la(s) " + dt.ToString("hh:mmtt").Replace(".", "");
                }
                else
                {
                    return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower();
                }
            }

            if (time)
            {
                return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower() + " del " + dt.Year + " a la(s) " + dt.ToString("hh:mmtt").Replace(".", "");
            }
            else
            {
                return dt.Day + " de " + getMonthName(Convert.ToInt32(dt.ToString("MM"))).ToLower() + " del " + dt.Year;
            }
        }
        // obtengo segun el numero de dia, me retorna el nombre del dia
        public static string GetDayName(int id, bool _short = false)
        {
            switch (id)
            {
                case 1:
                    return _short ? "Lun" : "Lunes";
                case 2:
                    return _short ? "Mar" : "Martes";
                case 3:
                    return _short ? "Mie" : "Miercoles";
                case 4:
                    return _short ? "Jue" : "Jueves";
                case 5:
                    return _short ? "Vie" : "Viernes";
                case 6:
                    return _short ? "Sab" : "Sábado"; ;
                case 0:
                    return _short ? "Dom" : "Domingo";
                default:
                    return "";
            };
        }
        // retorna el codigo de busqueda dela url de youtube
        public static string GetYouTubeID(string YoutubeUrl = null)
        {
            if (YoutubeUrl != null)
            {
                //Setup the RegEx Match and give it 
                Match regexMatch = Regex.Match(YoutubeUrl, "^[^v]+v=(.{11}).*", RegexOptions.IgnoreCase);
                if (regexMatch.Success) return regexMatch.Groups[1].Value;
            }

            return null;
        }
        // del url amistoso verifica si la ultima palabra concatenada es enter caso sea que si, devuelve valor como si fuera su codigo
        public static int GetIdFromUrl(string url)
        {
            if (url == null) return 0;
            int id = 0;
            if (int.TryParse(url.Split('-')[url.Split('-').Count() - 1], out id))
            {
                return id;
            }
            else
            {
                return 0;
            }

        }
        // obtenemos la edad, segun la fecha de nacimiento
        public static int GetAge(string d)
        {
            if (String.IsNullOrEmpty(d)) return 0;

            DateTime dtFecNac = Convert.ToDateTime(d);

            DateTime today = DateTime.Today;
            int age = today.Year - dtFecNac.Year;
            if (dtFecNac > today.AddYears(-age))
            {
                age--;
            }

            return age;
        }
        // obtener la direccion ip
        public static string GetIpHelper()
        {
            IHttpContextAccessor accesor = new HttpContextAccessor();
            return accesor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        public static string ReplaceHttpInWord(string word)
        {
            if (word == null) return "";

            if (!word.Contains("http")) return "http://" + word;
            return word;
        }
        public static string RemoveHTMLFromString(string html)
        {
            return Regex.Replace(html, "<.+?>", string.Empty);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void ListaATexto<T>(List<T> lista, string archivo, char separador)
        {
            if (File.Exists(archivo)) File.Delete(archivo);
            using (FileStream fs = new FileStream(archivo, FileMode.Create, FileAccess.Write, FileShare.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs, Encoding.GetEncoding(1252)))
                {
                    PropertyInfo[] propiedades = null;// lista[0].GetType().GetProperties();
                    //Recorre la lista de objetos
                    for (int j = 0; j < lista.Count; j++)
                    {
                        propiedades = lista[j].GetType().GetProperties();//Recupera las propiedades de cada objeto(clase) 
                        for (int i = 0; i < propiedades.Length; i++)//Recorre el array de propiedades
                        {
                            if (propiedades[i].GetValue(lista[j]) == null)
                            {
                                sw.Write(string.Empty);
                            }
                            else
                            {
                                sw.Write(propiedades[i].GetValue(lista[j]).ToString());
                            }
                            if (i < propiedades.Length - 1) sw.Write(separador);
                        }
                        sw.WriteLine();
                    }
                }
            }
        }

        public static string formatoIgv(decimal igv)
        {
            int parteEntera = (int)igv; // sin redondeo;
            decimal decimales = (igv - parteEntera);
            return (decimales > 0 ? string.Format("{0:0,0.00}", igv) : parteEntera.ToString());
        }

        public static string redondearSiNoHayDecimales(decimal numero)
        {
            int parteEntera = (int)numero; // sin redondeo;
            decimal decimales = (numero - parteEntera);
            return (decimales > 0 ? string.Format("{0:#,#0.00}", numero) : parteEntera.ToString());
        }
        public static string redondearSiNoHayDecimales(string numero)
        {
            decimal dNumero = Convert.ToDecimal(numero);
            int parteEntera = (int)dNumero; // sin redondeo;
            decimal decimales = (dNumero - parteEntera);
            return (decimales > 0 ? string.Format("{0:#,#0.00}", dNumero) : parteEntera.ToString());
        }
        private static string repetirCaracter(char caracater, int nroVeces)
        {
            return string.Concat(Enumerable.Repeat(caracater.ToString(), nroVeces));
        }

        public static string formatoComprobante(string tipoComprobante, int serie, int documento, bool abreviado = false)
        {
            //FACTURA: 003-000015
            int maxLSerie = 3, maxLDocumento = 6;
            string resultado = string.Empty;
            if (serie.ToString().Length > maxLSerie)
            {
                resultado = "La longitud del nro de serie no debe de sobrepasar de los " + maxLSerie + " caracteres.";
                return resultado;
            }
            if (documento.ToString().Length > maxLDocumento)
            {
                resultado = "La longitud del nro del comprobante no debe de sobrepasar de los " + maxLDocumento + " caracteres.";
                return resultado;
            }
            string nroSerie = (repetirCaracter('0', maxLSerie) + serie);
            nroSerie = nroSerie.Substring((nroSerie.Length - maxLSerie), maxLSerie);// sustituye a la funcion right de vb
            string nroComprobante = (repetirCaracter('0', maxLDocumento) + documento);
            nroComprobante = nroComprobante.Substring((nroComprobante.Length - maxLDocumento), maxLDocumento);

            resultado = (abreviado ? tipoComprobante.Substring(0, 3) : tipoComprobante) + ": " + nroSerie + "-" + nroComprobante;
            return resultado;
        }

        public static bool validarEmail(string email)
        {
            String sFormato;
            sFormato = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, sFormato))
            {
                if (Regex.Replace(email, sFormato, String.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }

}
