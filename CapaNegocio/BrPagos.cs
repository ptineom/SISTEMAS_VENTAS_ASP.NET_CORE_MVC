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
        DaoPagos _dao = null;
        ResultadoOperacion _resultado = null;
        public BrPagos()
        {
            _dao = new DaoPagos();
            _resultado = new ResultadoOperacion();
        }
        public ResultadoOperacion combosPagos(string idSucursal, string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    PAGOS modelo = _dao.combosPagos(con, idSucursal, idUsuario);
                   
                    _resultado.SetResultado(true, modelo);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }
        public ResultadoOperacion listaCtaCtePagos(string idSucursal, string estadoPago, string idProveedor, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.listaCtaCtePagos(con, idSucursal, estadoPago, idProveedor, idTipoComprobante, nroSerie, nroDocumento, fechaInicio, fechaFinal);
                 
                    _resultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }
        
        public ResultadoOperacion listaPagos(string idSucursal, string idTipoComprobante, string nroSerie, int nroDocumento, string idProveedor)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.listaPagos(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor);
                  
                    _resultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
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
                    _dao.grabarPago(con, trx, oModelo);
                    _resultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    _resultado.SetResultado(false, ex.Message.ToString());
                    trx.Rollback();
                    Elog.save(this, ex);
                }
            }
            return _resultado;
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
                    _dao.anularPago(con, trx, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor, correlativo, idUsuario);
                    _resultado.SetResultado(true, Helper.Constantes.sMensajeEliminadoOk);
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    _resultado.SetResultado(false, ex.Message.ToString());
                    trx.Rollback();
                    Elog.save(this, ex);
                }
            }
            return _resultado;
        }

        public ResultadoOperacion combosReportePagos(string idSucursal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    PAGOS modelo = _dao.combosReportePagos(con, idSucursal);

                    _resultado.SetResultado(true, modelo);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }

        public ResultadoOperacion reporteCtaCtePagos(string idSucursal, string idTipoComprobante,
            string fechaInicio, string fechaFinal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.reporteCtaCtePagos(con, idSucursal, idTipoComprobante, fechaInicio, fechaFinal);
                  
                    _resultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }

    }
}
