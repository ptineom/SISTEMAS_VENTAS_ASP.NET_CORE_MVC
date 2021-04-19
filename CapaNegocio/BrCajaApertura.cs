using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDao;
using Helper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CapaNegocio
{
    public class BrCajaApertura
    {
        private DaoCajaApertura _dao = null;
        private ResultadoOperacion _resultado = null;
        private IConfiguration _configuration = null;
        private Conexion _conexion = null;
        public BrCajaApertura(IConfiguration configuration)
        {
            _dao = new DaoCajaApertura();
            _resultado = new ResultadoOperacion();
            _configuration = configuration;
            _conexion = new Conexion(_configuration);
        }

        #region Proceso CajaApertura
        public ResultadoOperacion GetStateBox(string idSucursal, string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    var modelo = _dao.GetStateBox(con, idSucursal, "", idUsuario, 0);

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
        public ResultadoOperacion GetStateBox(string idSucursal, string idCaja, string idUsuario, int correlativo)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    var modelo = _dao.GetStateBox(con, idSucursal, idCaja, idUsuario, correlativo);

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
        public ResultadoOperacion GetTotalsByUserId(string idSucursal, string idCaja, string idUsuario, int correlativo)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    var modelo = _dao.GetTotalsByUserId(con, idSucursal, idCaja, idUsuario, correlativo);

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
        public ResultadoOperacion Register(CAJA_APERTURA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    var modelo = _dao.Register(con, trx, oModelo);

                    _resultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk, modelo);
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

        public ResultadoOperacion ReopenBox(CAJA_APERTURA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    _dao.ReopenBox(con, trx, oModelo);

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

        public ResultadoOperacion ValidateBox(string idSucursal, string idCaja, string idUsuario, int correlativoCa)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    bool value = _dao.ValidateBox(con, idSucursal, idCaja, idUsuario, correlativoCa);
                    oResultado.SetResultado(value, "OK");
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    oResultado.SetResultado(false, ex.Message);
                }
            }
            return oResultado;
        }

        public ResultadoOperacion GetData(string idSucursal, string idUsuario,
            ref List<MONEDA> listaMonedas, ref List<CAJA> listaCajas)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    _dao.GetData(con, idSucursal, idUsuario, ref listaMonedas, ref listaCajas);

                    _resultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                }
                catch (Exception ex)
                {
                    _resultado.SetResultado(false, ex.Message.ToString());
                    Elog.save(this, ex);
                }
            }
            return _resultado;
        }

        public ResultadoOperacion GetAllByFilters(string idSucursal, string idCaja, string idUsuario, string fecIni, string fecFin)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.GetAllByFilters(con, idSucursal, idCaja, idUsuario, fecIni, fecFin);

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

        public ResultadoOperacion GetDataQuerys(string idSucursal, ref List<USUARIO> listaUsuario, ref List<CAJA> listaCaja)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    _dao.GetDataQuerys(con, idSucursal, ref listaUsuario, ref listaCaja);

                    _resultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                }
                catch (Exception ex)
                {
                    _resultado.SetResultado(false, ex.Message.ToString());
                    Elog.save(this, ex);
                }
            }
            return _resultado;
        }

        #endregion

        #region Consultas y reportes
        //public ResultadoOperacion combosReportesCajaArqueo(string idSucursal)
        //{
        //    using (SqlConnection con = new SqlConnection(Conexion.sConexion))
        //    {
        //        try
        //        {
        //            con.Open();
        //            COMBOS_REPORTE_CAJA_ARQUEO modelo = _dao.combosReportesCajaArqueo(con, idSucursal);
        //            _resultado.SetResultado(true, modelo);
        //        }
        //        catch (Exception ex)
        //        {
        //            Elog.save(this, ex);
        //            _resultado.SetResultado(false, ex.Message);
        //        }
        //    }
        //    return _resultado;
        //}

        //public ResultadoOperacion listaArqueoCaja(string idSucursal, string fecIni, string fecFin, string idUsuario, string idCaja)
        //{
        //    using (SqlConnection con = new SqlConnection(Conexion.sConexion))
        //    {
        //        try
        //        {
        //            con.Open();
        //            var lista = _dao.listaArqueoCaja(con, idSucursal, fecIni, fecFin, idUsuario, idCaja);
        //            _resultado.SetResultado(true, lista);
        //        }
        //        catch (Exception ex)
        //        {
        //            Elog.save(this, ex);
        //            _resultado.SetResultado(false, ex.Message);
        //        }
        //    }
        //    return _resultado;
        //}
        #endregion
    }
}
