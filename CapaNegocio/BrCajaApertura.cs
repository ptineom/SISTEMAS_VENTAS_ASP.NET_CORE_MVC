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
    public class BrCajaApertura
    {
        DaoCajaApertura dao = null;
        ResultadoOperacion oResultado = null;
        public BrCajaApertura()
        {
            dao = new DaoCajaApertura();
            oResultado = new ResultadoOperacion();
        }
        #region Proceso CajaApertura
        public ResultadoOperacion cajaAbiertaXusuario(string idSucursal, string idCaja, string idUsuario, int correlativo)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var modelo = dao.cajaAbiertaXusuario(con, idSucursal,idCaja, idUsuario, correlativo);
                
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
        public ResultadoOperacion totalCobranzaXcaja(string idSucursal, string idCaja, string idUsuario, int correlativo)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var modelo = dao.totalCobranzaXcaja(con, idSucursal, idCaja, idUsuario, correlativo);
               
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
        public ResultadoOperacion grabarCajaApertura(CAJA_APERTURA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    var modelo = dao.grabarCajaApertura(con, trx, oModelo);
               
                    oResultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk, modelo);
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

        public ResultadoOperacion validarCaja(string idSucursal, string idCaja, string idUsuario, int correlativoCa)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    bool value = dao.validarCaja(con, idSucursal, idCaja, idUsuario, correlativoCa);
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

        public ResultadoOperacion cajaXusuario(string idSucursal, string idUsuario)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var modelo = dao.cajaXusuario(con, idSucursal, idUsuario);
                 
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

        public ResultadoOperacion combosCajaApertura(string idSucursal, string idUsuario, 
            ref List<MONEDA> listaMonedas, ref List<CAJA> listaCajas)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    dao.combosCajaApertura(con, idSucursal, idUsuario, ref listaMonedas,ref listaCajas);
                    oResultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                }
                catch (Exception ex)
                {
                    oResultado.SetResultado(false, ex.Message.ToString());
                    Elog.save(this, ex);
                }
            }
            return oResultado;
        }

        public ResultadoOperacion listaAperturasCajasXusuario(string idSucursal, string idUsuario, string fecIni, string fecFin)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaAperturasCajasXusuario(con, idSucursal, idUsuario, fecIni, fecFin);
                  
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

        #region Consultas y reportes
        public ResultadoOperacion combosReportesCajaArqueo(string idSucursal)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    COMBOS_REPORTE_CAJA_ARQUEO modelo = dao.combosReportesCajaArqueo(con, idSucursal);
                
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

        public ResultadoOperacion listaArqueoCaja(string idSucursal, string fecIni, string fecFin, string idUsuario, string idCaja)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaArqueoCaja(con, idSucursal, fecIni, fecFin, idUsuario, idCaja);
                  
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
