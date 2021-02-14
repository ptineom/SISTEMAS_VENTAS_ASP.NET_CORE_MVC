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
    public class BrInventario
    {
        DaoInventario dao = null;
        ResultadoOperacion oResultado = null;
        public BrInventario()
        {
            dao = new DaoInventario();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listaArticulosInventario(string accion, string idSucursal, string nomArticulo, int idMarca,
            string procedencia, bool flgStockMinimo, bool flgSinStock, string xmlGrupos, string xmlFamilias, string idArticulo)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaArticulosInventario(con, accion, idSucursal, nomArticulo, idMarca, procedencia,
                        flgStockMinimo, flgSinStock, xmlGrupos, xmlFamilias, idArticulo);
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

        public ResultadoOperacion articuloXcodigoBarra(string accion, string idSucursal, string idArticulo, bool flgBuscarXcodBarra)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    ARTICULO modelo = dao.articuloXcodigoBarra(con, accion, idSucursal, idArticulo, flgBuscarXcodBarra);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;
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

        public ResultadoOperacion listaInventario(string accion, string idSucursal, int idEstado, string fechaInicio, string fechaFinal,
            string idUsuarioInventario, string idTipoInventario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaInventario(con, accion, idSucursal, idEstado, fechaInicio, fechaFinal, idUsuarioInventario, idTipoInventario);
                    if (lista != null)
                    {
                        oResultado.data = lista;
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

        public ResultadoOperacion grabarInventario(INVENTARIO oModelo, ref int nroInventario,
            ref string idUsuarioInventario, ref string fechaInventario, ref int idEstado)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarInventario(con, trx, oModelo, ref nroInventario, ref idUsuarioInventario, ref fechaInventario, ref idEstado);
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

        public ResultadoOperacion eliminarInventario(string idSucursal, int nroInventario, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.eliminarInventario(con, trx, idSucursal, nroInventario, idUsuario);
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

        public ResultadoOperacion inventarioXcodigo(string idSucursal, int nroInventario)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    INVENTARIO modelo = dao.inventarioXcodigo(con, idSucursal, nroInventario);
                    if (modelo != null)
                    {
                        oResultado.data = modelo;
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

        public ResultadoOperacion combosInventario(ref List<GRUPO> listaGrupos, ref List<ESTADO> listaEstados)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    dao.combosInventario(con, ref listaGrupos, ref listaEstados);

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

        public ResultadoOperacion aprobarInventario(string idSucursal, int nroInventario, string idUsuario,
            ref string idUsuarioAprobacion, ref string fechaAprobacion, ref int idEstado)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.aprobarInventario(con, trx, idSucursal, nroInventario,idUsuario, ref idUsuarioAprobacion, ref fechaAprobacion, ref idEstado);
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

        public ResultadoOperacion listaInventarioManual(string accion, string idSucursal, string xmlGrupos, string xmlFamilias, bool flgImprimirCodBarra)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaInventarioManual(con, accion, idSucursal, xmlGrupos, xmlFamilias, flgImprimirCodBarra);
                    if (lista != null)
                    {
                        oResultado.data = lista;//lista.ToList<Object>();
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

        public ResultadoOperacion kardex(string idSucursal, string fechaInicio, string fechaFinal,
    string xmlGrupos, string xmlFamilias, string idArticulo)
        {
            ResultadoOperacion oResultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.kardex(con, idSucursal, fechaInicio, fechaFinal, xmlGrupos,
                        xmlFamilias, idArticulo);
                    if (lista != null)
                    {
                        oResultado.data = lista;
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
