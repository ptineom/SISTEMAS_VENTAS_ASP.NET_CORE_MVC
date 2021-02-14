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
    public class BrConceptoIECaja
    {
        DaoConceptoIECaja dao = null;
        ResultadoOperacion resultado = null;
        public BrConceptoIECaja()
        {
            dao = new DaoConceptoIECaja();
            resultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listaConceptoIECaja()
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaConceptoIECaja(con);
                    if (lista != null)
                    {
                        oResultado.data = lista; //lista.ToList<Object>();
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
        public ResultadoOperacion grabarConceptoIECaja(CONCEPTO_I_E_CAJA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarConceptoIECaja(con, trx, oModelo);
                    resultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    resultado.SetResultado(false, ex.Message.ToString());
                    trx.Rollback();
                    Elog.save(this, ex);
                }
            }
            return resultado;
        }
        public ResultadoOperacion anularConceptoIECaja(string idConceptoIE, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularConceptoIECaja(con, trx, idConceptoIE, idUsuario);
                    resultado.SetResultado(true, Helper.Constantes.sMensajeEliminadoOk);
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    resultado.SetResultado(false, ex.Message.ToString());
                    trx.Rollback();
                    Elog.save(this, ex);
                }
            }
            return resultado;
        }
    }
}
