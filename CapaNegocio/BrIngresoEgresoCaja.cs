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
    public class BrIngresoEgresoCaja
    {
        DaoIngresoEgresoCaja dao = null;
        ResultadoOperacion resultado = null;
        public BrIngresoEgresoCaja()
        {
            dao = new DaoIngresoEgresoCaja();
            resultado = new ResultadoOperacion();
        }
        public ResultadoOperacion cboIngresoEgresoCaja(string movimiento, string idSucursal, string idUsuario)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var modelo = dao.cboIngresoEgresoCaja(con, movimiento, idSucursal, idUsuario);
                    if (modelo != null)
                    {
                        oResultado.data = (Object)modelo;
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
        public ResultadoOperacion listaIngresoEgresoCaja(string idSucursal, string idTipoMovimiento, string fecIni, string fecFin)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaIngresoEgresoCaja(con, idSucursal, idTipoMovimiento, fecIni, fecFin);
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

        public ResultadoOperacion grabarIngresoEgresoCaja(INGRESO_EGRESO_CAJA oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarIngresoEgresoCaja(con, trx, oModelo);
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

        public ResultadoOperacion anularIngresoEgresoCaja(string idSucursal, string idTipoMovimiento, string idIECaja, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularIngresoEgresoCaja(con, trx, idSucursal, idTipoMovimiento, idIECaja, idUsuario);
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

        public ResultadoOperacion cboReporteIngresoEgresoCaja(string movimiento, string idSucursal)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var modelo = dao.cboReporteIngresoEgresoCaja(con, movimiento, idSucursal);
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

        public ResultadoOperacion reporteIngresoEgresoCaja(string idSucursal, string movimiento, string cadenaIdConceptoIE, string fecIni, string fecFin)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.reporteIngresoEgresoCaja(con, idSucursal,movimiento, cadenaIdConceptoIE, fecIni, fecFin);
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
