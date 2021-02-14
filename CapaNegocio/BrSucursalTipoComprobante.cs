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
    public class BrSucursalTipoComprobante
    {
        DaoSucursalTipoComprobante dao = null;
        ResultadoOperacion oResultado = null;
        public BrSucursalTipoComprobante()
        {
            dao = new DaoSucursalTipoComprobante();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaSucursalTipoComprobante(string idSucursal, string idTipoComprobante, bool flgEnUso)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaSucursalTipoComprobante(con, idSucursal, idTipoComprobante, flgEnUso);
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

        public ResultadoOperacion listaTipoComprobante()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaTipoComprobante(con);
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

        public ResultadoOperacion grabarSucursalTipoComprobante(SUCURSAL_TIPO_COMPROBANTE oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarSucursalTipoComprobante(con, trx, oModelo);
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
        public ResultadoOperacion anularSucursalTipoComprobante(string idSucursal, string idTipoComprobante, string nroSerie, string idUsuario)
        {

            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularSucursalTipoComprobante(con, trx, idSucursal, idTipoComprobante, nroSerie, idUsuario);
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
    }
}
