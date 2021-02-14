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
    public class BrTipoNotaCredito
    {
         DaoTipoNotaCredito dao = null;
        ResultadoOperacion oResultado = null;
        public BrTipoNotaCredito()
        {
            dao = new DaoTipoNotaCredito();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaTipoNC()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaTipoNC(con);
                    if (lista != null)
                    {
                        oResultado.data = lista;// lista.ToList<Object>();
                    }
                    oResultado.SetResultado(true, "");
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
