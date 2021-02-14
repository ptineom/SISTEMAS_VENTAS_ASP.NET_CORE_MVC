using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Helper
{
    public class Configuraciones
    {

        public static int TAMANIO_MAX_ARCHIVO_MB
        {
            get
            {
                return Convert.ToInt32(ViewHelper.getValueConfiguration("APP_SETTINGS:TAMANIO_MAX_ARCHIVO_MB"));
            }

        }

        public static string DOMINIO_WEB_API
        {
            get
            {
                return ViewHelper.getValueConfiguration("APP_SETTINGS:DOMINIO_WEB_API").Replace("/", @"\");
            }
        }

        public static string UPLOAD_ARTICULOS
        {
            get
            {
                return ViewHelper.getValueConfiguration("APP_SETTINGS:UPLOAD:ARTICULOS").Replace("/", @"\");
            }
        }

        public static string UPLOAD_EMPLEADOS
        {
            get
            {
                return ViewHelper.getValueConfiguration("APP_SETTINGS:UPLOAD:EMPLEADOS").Replace("/", @"\");
            }
        }

        public static string UPLOAD_EMPRESA
        {
            get
            {
                return ViewHelper.getValueConfiguration("APP_SETTINGS:UPLOAD:EMPRESA").Replace("/", @"\");
            }
        }
        public static int[] SCALES_IMAGES_ARTICULOS
        {
            get
            {
                string[] scales = ViewHelper.getValueConfiguration("APP_SETTINGS:SCALES_IMAGES_ARTICULOS").Split(',');
                return scales.Select(int.Parse).ToArray().OrderByDescending(x => x).ToArray();
            }
        }
        public static int ID_TIPO_FOTO_ARTICULO
        {
            get
            {
                return 3;
            }
        }

        public static int ID_TIPO_FOTO_EMPRESA
        {
            get
            {
                return 2;
            }
        }

        public static int ID_TIPO_FOTO_EMPLEADO
        {
            get
            {
                return 1;
            }
        }
    }
}
