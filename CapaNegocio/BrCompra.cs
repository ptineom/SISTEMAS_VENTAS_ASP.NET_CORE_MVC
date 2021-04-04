using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDao;
using System.Data.SqlClient;
using Helper;
using Microsoft.Extensions.Configuration;

namespace CapaNegocio
{
    public class BrCompra
    {
        private DaoCompra _dao = null;
        private ResultadoOperacion _resultado = null;
        private IConfiguration _configuration = null;
        private Conexion _conexion = null;
        public BrCompra(IConfiguration configuration)
        {
            _dao = new DaoCompra();
            _resultado = new ResultadoOperacion();
            _configuration = configuration;
            _conexion = new Conexion(_configuration);
        }

        public ResultadoOperacion GetData( string idSucursal, string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    DOC_COMPRA modelo = _dao.GetData(con, idSucursal, idUsuario);
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

        public ResultadoOperacion Register(DOC_COMPRA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    _dao.Register(con, trx, oModelo);
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

        public ResultadoOperacion Delete(string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string idProveedor, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    _dao.Delete(con, trx, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor, idUsuario);
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

        public ResultadoOperacion GetAllByFilters(string idSucursal, string idTipoComprobante, string nroSerie, int nroDocumento, string fechaInicio, 
            string fechaFinal, int idEstado)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.GetAllByFilters(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento, fechaInicio, fechaFinal, idEstado);
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

        public ResultadoOperacion GetById(string idSucursal, string idTipoComprobante, string nroSerie,
            int nroDocumento, string idProveedor)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    DOC_COMPRA_INFORME modelo = _dao.GetById(con, idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor);
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
    }
}
