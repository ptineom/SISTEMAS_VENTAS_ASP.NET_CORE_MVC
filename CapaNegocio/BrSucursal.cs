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
    public class BrSucursal
    {
        DaoSucursal dao = null;
        ResultadoOperacion oResultado = null;
        public BrSucursal()
        {
            dao = new DaoSucursal();
            oResultado = new ResultadoOperacion();
        }
        public ResultadoOperacion listaSucursales()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaSucursales(con);
                 
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
        public ResultadoOperacion grabarSucursal(SUCURSAL oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarSucursal(con, trx, oModelo);
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
        public void obtenerSucursalPorCodigo(string idSucursal, ref List<UBIGEO> listaProvincia, ref List<UBIGEO> listaDistrito)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    DaoSucursal dao = new DaoSucursal();
                    dao.obtenerSucursalPorCodigo(con, idSucursal, ref listaProvincia, ref listaDistrito);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                }
            }
        }
        public ResultadoOperacion anularSucursal(string idSucursal, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.anularSucursal(con, trx, idSucursal, idUsuario);
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
