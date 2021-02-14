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
    public class BrTipoMovimiento
    {
        DaoTipoMovimiento dao = null;
        ResultadoOperacion resultado = null;
        public BrTipoMovimiento()
        {
            dao = new DaoTipoMovimiento();
            resultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaTipoMovimiento()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaTipoMovimiento(con);
                    if (lista != null)
                    {
                        resultado.data = lista;// lista.ToList<Object>();
                    }
                    resultado.SetResultado(true, "");
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    resultado.SetResultado(false, ex.Message);
                }
            }
            return resultado;
        }
    }
}
