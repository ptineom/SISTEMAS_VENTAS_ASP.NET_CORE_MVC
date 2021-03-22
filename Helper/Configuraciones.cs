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
                return Convert.ToInt32(ViewHelper.getValueConfiguration("AppSettings:TamanioMaxArchiboMb"));
            }

        }

        public static string UPLOAD_ARTICULOS
        {
            get
            {
                return ViewHelper.getValueConfiguration("AppSettings:Upload:Articulos").Replace("/", @"\");
            }
        }

        public static string UPLOAD_EMPLEADOS
        {
            get
            {
                return ViewHelper.getValueConfiguration("AppSettings:Upload:Empleados").Replace("/", @"\");
            }
        }

        public static string UPLOAD_EMPRESA
        {
            get
            {
                return ViewHelper.getValueConfiguration("AppSettings:Upload:Empresa").Replace("/", @"\");
            }
        }
        public static int[] SCALES_IMAGES_ARTICULOS
        {
            get
            {
                string[] scales = ViewHelper.getValueConfiguration("AppSettings:ScalesImagesArticulos").Split(',');
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
