using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDao;
using Entidades;
using System.Data.SqlClient;
using Helper;
namespace CapaNegocio
{
    public class BrSucursalUsuario
    {
        DaoSucursalUsuario dao = null;
        ResultadoOperacion oResultado = null;
        public BrSucursalUsuario()
        {
            dao = new DaoSucursalUsuario();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaUsuarios(string idSucursal, ref List<SUCURSAL_USUARIO> listaSucUsu)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaUsuarios(con, idSucursal, ref listaSucUsu);
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

        public ResultadoOperacion grabarSucursalUsuario(SUCURSAL_USUARIO oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarSucursalUsuario(con, trx, oModelo);
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

        public ResultadoOperacion anularSucursalUsuario(string idSucursal, string idUsuario, string idUsuarioRegistro)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularSucursalUsuario(con, trx, idSucursal, idUsuario, idUsuarioRegistro);
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

        public ResultadoOperacion listaSucursalPorUsuario(string idUsuario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaSucursalPorUsuario(con, idUsuario);
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
    }
}
