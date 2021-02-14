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
    public class BrPagos
    {
        DaoPagos dao = null;
        ResultadoOperacion oResultado = null;
        public BrPagos()
        {
            dao = new DaoPagos();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion combosPagos(string idSucursal, string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    PAGOS modelo = dao.combosPagos(con, idSucursal, idUsuario);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;
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
        public ResultadoOperacion listaCtaCtePagos(string idSucursal, string estadoPago, string idProveedor, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaCtaCtePagos(con, idSucursal, estadoPago, idProveedor, idTipoComprobante, nroSerie, nroDocumento, fechaInicio, fechaFinal);
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
        
        public ResultadoOperacion listaPagos(string idSucursal, string idTipoComprobante, string nroSerie, int nroDocumento, string idProveedor)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaPagos(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor);
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
        
        public ResultadoOperacion grabarPago(PAGOS oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarPago(con, trx, oModelo);
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

        public ResultadoOperacion anularPago(string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string idProveedor, int correlativo, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularPago(con, trx, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor, correlativo, idUsuario);
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

        public ResultadoOperacion combosReportePagos(string idSucursal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    PAGOS modelo = dao.combosReportePagos(con, idSucursal);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;
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

        public ResultadoOperacion reporteCtaCtePagos(string idSucursal, string idTipoComprobante,
            string fechaInicio, string fechaFinal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.reporteCtaCtePagos(con, idSucursal, idTipoComprobante, fechaInicio, fechaFinal);
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
