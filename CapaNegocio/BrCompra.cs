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
    public class BrCompra
    {
        DaoCompra dao = null;
        ResultadoOperacion oResultado = null;
        public BrCompra()
        {
            dao = new DaoCompra();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion combosCompras( string idSucursal, string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    DOC_COMPRA modelo = dao.combosCompras(con, idSucursal, idUsuario);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;//(Object)modelo;
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

        public ResultadoOperacion grabarCompra(DOC_COMPRA oModelo, ref string nroSerie, ref string nroComprobante)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarCompra(con, trx, oModelo, ref nroSerie, ref nroComprobante);
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

        public ResultadoOperacion anularCompra(string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string idProveedor, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularCompra(con, trx, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor, idUsuario);
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

        public ResultadoOperacion listaCompras(string idSucursal, string idTipoComprobante, string nroSerie, int nroDocumento, string fechaInicio, 
            string fechaFinal, int idEstado)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaCompras(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento, fechaInicio, fechaFinal, idEstado);
                    if (lista != null)
                    {
                        oResultado.data = lista; // lista.ToList<Object>();
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

        public ResultadoOperacion compraPorCodigo(string idSucursal, string idTipoComprobante, string nroSerie,
            int nroDocumento, string idProveedor)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    DOC_COMPRA_INFORME modelo = dao.compraPorCodigo(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;//(Object)modelo;
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
