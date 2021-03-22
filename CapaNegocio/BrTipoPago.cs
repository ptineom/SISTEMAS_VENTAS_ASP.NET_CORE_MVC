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
    public class BrTipoPago
    {
        DaoTipoPago dao = null;
        ResultadoOperacion oResultado = null;
        public BrTipoPago()
        {
            dao = new DaoTipoPago();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaTipoPago()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaTipoPago(con);
               
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
