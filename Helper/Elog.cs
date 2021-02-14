using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Helper
{
    public static class Elog
    {
        public static void save(object obj, Exception exception)
        {
            string directorioBase = Directory.GetCurrentDirectory();
            string directorio = Path.Combine(directorioBase, "log");

            try
            {
                if (!Directory.Exists(directorio))
                {
                    Directory.CreateDirectory(directorio);
                }

                //Nombre del archivo.
                string fecha = System.DateTime.Now.ToString("yyyyMMdd");
                var path = System.IO.Path.Combine(directorio, fecha + ".txt");

                //Hora del error capturado.
                string hora = System.DateTime.Now.ToString("HH:mm:ss");
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    StackTrace stacktrace = new StackTrace();
                    sw.WriteLine(obj.GetType().FullName + " " + hora);
                    sw.WriteLine(stacktrace.GetFrame(1).GetMethod().Name + " - " + exception.Message);
                    sw.WriteLine("");
                    sw.Flush();
                }
            }
            catch (DirectoryNotFoundException ex)
            {
                throw new DirectoryNotFoundException(ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
