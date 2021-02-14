using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDao;
using System.Data.SqlClient;
using Helper;
namespace CapaNegocio
{
    public class BrCobranza
    {
        DaoCobranza dao = null;
        ResultadoOperacion oResultado = null;
        public BrCobranza()
        {
            dao = new DaoCobranza();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion combosCobranza(string idSucursal, string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    COBRANZA modelo = dao.combosCobranza(con, idSucursal, idUsuario);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;// (Object)modelo;
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
        public ResultadoOperacion listaCtaCteCobranza(string idSucursal, string estadoPago, string idCliente, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaCtaCteCobranza(con, idSucursal, estadoPago, idCliente, idTipoComprobante, nroSerie, nroDocumento, fechaInicio, fechaFinal);
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

        public ResultadoOperacion listaCobranza(string idSucursal, string idTipoComprobante, string nroSerie, int nroDocumento)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaCobranza(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento);
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

        public ResultadoOperacion grabarCobranza(COBRANZA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarCobranza(con, trx, oModelo);
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

        public ResultadoOperacion anularCobranza(string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, int correlativo, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularCobranza(con, trx, idSucursal, idTipoComprobante, nroSerie, nroDocumento, correlativo, idUsuario);
                    oResultado.SetResultado(true, Helper.Constantes.sMensajeEliminadoOk);
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

        public ResultadoOperacion combosReporteCobranza(string idSucursal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    COBRANZA modelo = dao.combosReporteCobranza(con, idSucursal);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;// (Object)modelo;
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

        public ResultadoOperacion ReporteCtaCteCobranza(string idSucursal, string idTipoComprobante,
            string fechaInicio, string fechaFinal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.reporteCtaCteCobranza(con, idSucursal, idTipoComprobante, fechaInicio, fechaFinal);
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
