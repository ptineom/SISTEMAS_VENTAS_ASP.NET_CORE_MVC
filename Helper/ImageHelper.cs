using ImageMagick;// Nugget magick.net -- escoger 64 o 32 tambien de 8 o 16(16 es para fotos enormes)
using Microsoft.AspNetCore.Http;//Nuget Microsoft.AspNetCore.Http
using System;
using System.Collections.Generic;
//using System.Drawing; //Nugget System.Drawing.Common ///   objImg = Image.FromFile(this.Ruta + this.Img);
using System.IO;
using System.Linq;
using System.Text;

namespace Helper
{
    public class ImageHelper
    {
        private string _uriOrigen;
        private List<string> _images;

        public ImageHelper(string uriOrigen)
        {
            _uriOrigen = uriOrigen;
            _images = new List<string>();
        }

        public int[] scales { get; set; }// escalas(anchos): medidas que deseamos convertir las imágenes.
        public string Img { get; set; }// Nombre del archivo.
        public bool jpg { get; set; }//True indica que el archivo será jpg.

        /// <summary>
        /// b = bytes, kb = kilobytes, mb = megabytes, gb = gigabytes
        /// </summary>
        public enum TipoTamanio
        {
            bytes = 0,
            kb = 1,
            mb = 2,
            gb = 3
        }

        public List<string> getNewImages()
        {
            return this._images;
        }

        public void resizes()
        {
            string name = "";
            int correlativo = 0;
            int width = 0;
            int height = 0;

            using (MagickImage mi = new MagickImage(_uriOrigen))
            {
                int imgWidth = mi.Width;
                int imgHeight = mi.Height;

                if (this.jpg)
                    mi.Format = MagickFormat.Jpg;

                foreach (int scale in this.scales)
                {
                    correlativo += 1;
                    name = $"{correlativo.ToString()}-{this.Img}";

                    //Cuando el ancho es mas grande que la escala, y el alto es más chico que la escala.
                    if (imgWidth > scale & imgHeight < scale)
                    {
                        width = scale;
                        height = Convert.ToInt32((imgHeight) * ((decimal)scale / imgWidth));
                    }
                    //Cuando el ancho es mas chico que la escala y el alto es mas grande que la escala.
                    else if (imgWidth < scale & imgHeight > scale)
                    {
                        width = Convert.ToInt32(imgWidth * ((decimal)scale / imgHeight));
                        height = scale;
                    }
                    //Cuando el ancho y el alto son mas grandes que la escala.
                    else if (imgWidth > scale & imgHeight > scale)
                    {
                        if (imgWidth > imgHeight)
                        {
                            width = scale;
                            height = Convert.ToInt32((imgHeight) * ((decimal)scale / imgWidth));
                        }
                        else
                        {
                            width = Convert.ToInt32(imgWidth * ((decimal)scale / imgHeight));
                            height = scale;
                        }
                    }
                    //Caso que la anchura y el alto es menor o igual a la escala.
                    else
                    {
                        width = imgWidth;
                        height = imgHeight;
                    }

                    //generamos el tamaño deseado.
                    mi.Resize(width, height);
                    //uri destino.
                    string directory = Path.GetDirectoryName(_uriOrigen) + @"\";
                    mi.Write(Path.Combine(directory, name));

                    _images.Add(name);
                };
            };
        }

        /// <summary>
        ///  Validará el tamaño y formato permitido del objeto file.
        /// </summary>
        /// <param name="file">Objeto File</param>
        /// <param name="kb">Tamaño de la imagen en kilobytes, si coloca 0, no validará el tamaño.</param>
        /// <returns>Retorna una cadena vacía si es todo satisfactorio</returns>
        public static string TryParse(IFormFile file, int kb = 0)
        {
            if (file == null) return "Debe seleccionar un archivo";

            string rpta = "";

            if (file.ContentType.ToLower() == "image/jpg"
                || file.ContentType.ToLower() == "image/jpeg"
                || file.ContentType.ToLower() == "image/pjpeg"
                || file.ContentType.ToLower() == "image/gif"
                || file.ContentType.ToLower() == "image/x-png"
                || file.ContentType.ToLower() == "image/png")
            {
                if (kb > 0)
                {
                    if (Convert.ToInt32(file.Length / 1024) > kb)
                    {
                        rpta = "La imagen no puede ser mayor a los " + kb + "kb";
                    }
                }
            }
            else
            {
                rpta = "Extensión de archivo no válida";
            }

            return rpta;
        }

        /// <summary>
        /// Validará el tamaño y formato permitido del objeto file.
        /// </summary>
        /// <param name="file">
        /// Objeto File
        /// </param>
        /// <param name="tipo">
        /// Tipo del tamaño de la imagen a validar.
        /// </param>
        /// <param name="size">
        /// Tamaño máximo de la imagen permitido.
        /// </param>
        /// <param name="arrExtensiones">
        /// Formatos aceptados ejemplo: ".png", ".jpg"
        /// </param>
        /// <returns>Retorna una cadena vacía si es todo satisfactorio</returns>
        public static string TryParse(IFormFile file, TipoTamanio tipo, int size = 0, params string[] arrExtensiones)
        {
            if (file == null)
            {
                throw new ArgumentNullException("No existe un archivo a evaluar");
            };
            if (size == 0)
            {
                throw new ArgumentNullException("No se ingreso el parámetro del tamaño máximo permitido");
            }
            if (arrExtensiones.Length == 0)
            {
                throw new ArgumentNullException("No se ingreso el parámetro de las extensiones permitidas");
            }

            string rpta = "";

            string extension = Path.GetExtension(file.FileName);
            if (!arrExtensiones.Contains(extension, StringComparer.OrdinalIgnoreCase))
            {
                rpta = string.Format("Extensión de archivo no válida");
            }
            else
            {
                long lengthMax = 0;
                switch (tipo)
                {
                    case TipoTamanio.bytes:
                        lengthMax = size;
                        break;
                    case TipoTamanio.kb:
                        lengthMax = (size * 1024);
                        break;
                    case TipoTamanio.mb:
                        lengthMax = ((size * 1024) * 1024);
                        break;
                    case TipoTamanio.gb:
                        lengthMax = (((size * 1024) * 1024) * 2014);
                        break;
                    default:
                        break;
                }

                if (file.Length > lengthMax)
                {
                    rpta = string.Format("El archivo no puede superar los {0}{1}", size.ToString(), tipo.ToString());
                }
            }

            return rpta;
        }

        public static string TryParseFileDirectory(string sServidor)
        {
            string rpta = "";
            // verificar si existe el directorio
            if (!System.IO.Directory.Exists(sServidor))
            {
                rpta = "Directorio de archivos no existe...";
            }
            else
            {
                // verificar si tenemos acceso a crear el archivo
                try
                {
                    using (FileStream fs = System.IO.File.Create(Path.Combine(sServidor, "AccessTemp.txt"), 1, FileOptions.DeleteOnClose))
                    {
                        fs.Close();
                    }
                }
                catch (Exception ex)
                {
                    rpta = ex.Message.ToString();
                }
            }
            return rpta;
        }


        public static void deleteFile(string uri)
        {
            if (System.IO.File.Exists(uri))
                System.IO.File.Delete(uri);
        }

    }
}
