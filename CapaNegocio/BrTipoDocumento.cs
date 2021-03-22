using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDao;
using Helper;
using System.Data.SqlClient;
namespace CapaNegocio
{
    public class BrTipoDocumento
    {
        DaoTipoDocumento dao = null;
        ResultadoOperacion oResultado = null;
        public BrTipoDocumento()
        {
            dao = new DaoTipoDocumento();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaTipoDocumentos()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaTipoDocumento(con);
               
                    oResultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    oResultado.SetResultado(false, ex.Message);
                }
            }
            return oResultado;
        }
    }
}
