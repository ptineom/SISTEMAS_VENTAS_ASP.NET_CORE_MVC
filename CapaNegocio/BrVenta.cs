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
    public class BrVenta
    {
        DaoVenta dao = null;
        ResultadoOperacion oResultado = null;
        public BrVenta()
        {
            dao = new DaoVenta();
            oResultado = new ResultadoOperacion();
        }
        #region Proceso ventas
        public ResultadoOperacion combosVentas(string idSucursal, string idUsuario, ref string tipoNCAnular)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    DOC_VENTA modelo = dao.combosVentas(con, idSucursal, idUsuario, ref tipoNCAnular);
                    oResultado.SetResultado(true, modelo);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    oResultado.SetResultado(false, ex.Message);
                }
            }
            return oResultado;
        }

        public ResultadoOperacion grabarVenta(DOC_VENTA oModelo, ref string nroSerie, ref string nroComprobante)
        {
            SqlTransaction trx = null;
            string numero = string.Empty;

            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();

                    dao.grabarVenta(con, trx, oModelo, ref nroSerie, ref numero);

                    nroComprobante = ViewHelper.GetNroComprobante(numero);
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

        public ResultadoOperacion listaVentas(string idSucursal, string idCliente, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal, int idEstado)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    List<DOC_VENTA> lista = dao.listaVentas(con, idSucursal, idCliente, idTipoComprobante, nroSerie, nroDocumento, fechaInicio, fechaFinal, idEstado);

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

        public ResultadoOperacion ventaPorCodigo(string idSucursal, string idTipoComprobante, string nroSerie, int nroDocumento)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    DOC_VENTA modelo = dao.ventaPorCodigo(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento);
                    oResultado.SetResultado(true, "", modelo);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    oResultado.SetResultado(false, ex.Message);
                }
            }
            return oResultado;
        }

        public ResultadoOperacion anularVenta(string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string idUsuario, ref string nroSerieNC, ref int nroDocumentoNC)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();

                    dao.anularVenta(con, trx, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idUsuario, ref nroSerieNC, ref nroDocumentoNC);

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
        #endregion

        #region Consultas y reportes
        public ResultadoOperacion combosReportesVentas(string idSucursal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    DOC_VENTA modelo = dao.combosReportesVentas(con, idSucursal);
                
                    oResultado.SetResultado(true, modelo);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    oResultado.SetResultado(false, ex.Message);
                }
            }
            return oResultado;
        }

        public ResultadoOperacion consultaVentas(string idSucursal, string idCliente, string idTipoComprobante,
            string fechaInicio, string fechaFinal, string idTipoCondicionPago, string idUsuarioCaja)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.consultaVentas(con, idSucursal, idCliente, idTipoComprobante, fechaInicio, fechaFinal, idTipoCondicionPago, idUsuarioCaja);
    
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

        public ResultadoOperacion consultaVentasDetalle(string idSucursal, string idTipoComprobante, string nroSerie,
            int nroDocumento)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.consultaVentasDetalle(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento);
              
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

        public ResultadoOperacion consultaVentasPorUsuario(string idSucursal, string fechaInicio, string fechaFinal, string tipo)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.consultaVentasPorUsuario(con, idSucursal, fechaInicio, fechaFinal, tipo);
                 
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

        public ResultadoOperacion consultaVentasPorMes(string idSucursal, int anio)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.consultaVentasPorMes(con, idSucursal, anio);

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
        #endregion
    }
}
