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
    public class BrGrupoAcceso
    {
        DaoGrupoAcceso dao = null;
        ResultadoOperacion oResultado = null;
        public BrGrupoAcceso()
        {
            dao = new DaoGrupoAcceso();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listaGrupoAcceso()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaGrupoAcceso(con);
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
        public ResultadoOperacion grabarGrupoAcceso(GRUPO_ACCESO oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarGrupoAcceso(con, trx, oModelo);
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
        public ResultadoOperacion anularGrupoAcceso(string idGrupoAcceso, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularGrupoAcceso(con, trx, idGrupoAcceso, idUsuario);
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
        public ResultadoOperacion listarMenuUsuario(int idGrupoAcceso)
        {
            List<APLICACION> lista = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    lista = dao.listarMenuUsuario(con, idGrupoAcceso);
                    if (lista != null)
                    {
                        oResultado.data = lista;// lista.ToList<Object>();
                    }
                    oResultado.SetResultado(true, "");
                }
                catch (Exception ex)
                {
                    oResultado.SetResultado(false, ex.Message);
                    Elog.save(this, ex);
                }
            }
            return oResultado;
        }
    }
}
