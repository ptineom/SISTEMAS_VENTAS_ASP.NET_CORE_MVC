using CapaDao;
using Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class BrMoneda
    {
        DaoMoneda dao = null;
        ResultadoOperacion oResultado = null;
        public BrMoneda()
        {
            dao = new DaoMoneda();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listaMonedas()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaMonedas(con);
                    if (lista != null)
                    {
                        oResultado.data = lista;
                    }
                    oResultado.SetResultado(true, "", lista);
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
