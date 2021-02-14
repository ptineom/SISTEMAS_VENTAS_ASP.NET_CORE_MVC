using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDao;
using Entidades;
using System.Data.SqlClient;
using Helper;
using System.Xml;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace CapaNegocio
{
    public class BrFotos
    {
        DaoFotos dao = null;
        ResultadoOperacion oResultado = null;
        public BrFotos()
        {
            dao = new DaoFotos();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion grabarFotos(FOTOS oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();

                    dao.grabarFotos(con, trx, oModelo);

                    oResultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    oResultado.SetResultado(false, ex.Message.ToString());
                    trx.Rollback();
                    Elog.save(this, ex);
                }
            }
            return oResultado;
        }
        public ResultadoOperacion procesoGrabadoFotos(string uri, int[] medidas, FOTOS fotos, string formato)
        {
            List<string> listaFotos = null;
            string directory = Path.GetDirectoryName(uri);
            string nameForFile = ViewHelper.getNameForFiles() + formato;
            try
            {
                ImageHelper imghelper = new ImageHelper(uri);
                imghelper.Img = nameForFile;
                imghelper.scales = medidas;
                imghelper.jpg = true;

                //Creación de imágenes según las medidas solicitadas.
                imghelper.resizes();

                //Obtenemos las imágenes creadas
                listaFotos = imghelper.getNewImages();

                if (listaFotos.Count > 0)
                {
                    int num = 0;
                    string file = "";

                    listaFotos.ForEach(el =>
                    {
                        num += 1;
                        file += ",\"FOTO" + num.ToString() + "\":\"" + el + "\"";
                    });
                    
                    string jsonFotos = "{" + file.Substring(1) + "}";

                    fotos.ACCION = "INS";
                    fotos.JSON_FOTOS = jsonFotos;

                    BrFotos brFoto = new BrFotos();
                    oResultado = brFoto.grabarFotos(fotos);
                }
            }
            catch (Exception ex)
            {
                oResultado.SetResultado(false, ex.Message);
                Elog.save(this, ex);
            }
            return oResultado;
        }
     
        public static void deleteFotosDirectory(string jsonFotos, string directory)
        {
            if (string.IsNullOrEmpty(jsonFotos))
                return;

            //object fotos = JsonConvert.DeserializeObject<object>(jsonFotos);
            FOTOS fotos =  JsonSerializer.Deserialize<FOTOS>(jsonFotos);

            if (fotos.getValue("FOTO1") != null)
                 ImageHelper.deleteFile(Path.Combine(directory, fotos.getValue("FOTO1").ToString()));

            if (fotos.getValue("FOTO2") != null)
                ImageHelper.deleteFile(Path.Combine(directory, fotos.getValue("FOTO2").ToString()));

            if (fotos.getValue("FOTO3") != null)
                ImageHelper.deleteFile(Path.Combine(directory, fotos.getValue("FOTO3").ToString()));
        }
    }
}
