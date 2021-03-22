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
        private DaoInventario _dao = null;
        private ResultadoOperacion _resultado = null;
        public BrInventario()
        {
            _dao = new DaoInventario();
            _resultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listaArticulosInventario(string accion, string idSucursal, string nomArticulo, int idMarca,
            string procedencia, bool flgStockMinimo, bool flgSinStock, string xmlGrupos, string xmlFamilias, string idArticulo)
        {
            ResultadoOperacion _resultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.listaArticulosInventario(con, accion, idSucursal, nomArticulo, idMarca, procedencia,
                        flgStockMinimo, flgSinStock, xmlGrupos, xmlFamilias, idArticulo);
                 
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

        public ResultadoOperacion articuloXcodigoBarra(string accion, string idSucursal, string idArticulo, bool flgBuscarXcodBarra)
        {
            ResultadoOperacion _resultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    ARTICULO modelo = _dao.articuloXcodigoBarra(con, accion, idSucursal, idArticulo, flgBuscarXcodBarra);
                  
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

        public ResultadoOperacion listaInventario(string accion, string idSucursal, int idEstado, string fechaInicio, string fechaFinal,
            string idUsuarioInventario, string idTipoInventario)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.listaInventario(con, accion, idSucursal, idEstado, fechaInicio, fechaFinal, idUsuarioInventario, idTipoInventario);
                  
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
                    _dao.grabarInventario(con, trx, oModelo, ref nroInventario, ref idUsuarioInventario, ref fechaInventario, ref idEstado);
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

        public ResultadoOperacion eliminarInventario(string idSucursal, int nroInventario, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    _dao.eliminarInventario(con, trx, idSucursal, nroInventario, idUsuario);
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

        public ResultadoOperacion inventarioXcodigo(string idSucursal, int nroInventario)
        {
            ResultadoOperacion _resultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    INVENTARIO modelo = _dao.inventarioXcodigo(con, idSucursal, nroInventario);
                  
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

        public ResultadoOperacion combosInventario(ref List<GRUPO> listaGrupos, ref List<ESTADO> listaEstados)
        {
            ResultadoOperacion _resultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    _dao.combosInventario(con, ref listaGrupos, ref listaEstados);

                    _resultado.SetResultado(true, "");
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
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
                    _dao.aprobarInventario(con, trx, idSucursal, nroInventario,idUsuario, ref idUsuarioAprobacion, ref fechaAprobacion, ref idEstado);
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

        public ResultadoOperacion listaInventarioManual(string accion, string idSucursal, string xmlGrupos, string xmlFamilias, bool flgImprimirCodBarra)
        {
            ResultadoOperacion _resultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.listaInventarioManual(con, accion, idSucursal, xmlGrupos, xmlFamilias, flgImprimirCodBarra);
                 
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

        public ResultadoOperacion kardex(string idSucursal, string fechaInicio, string fechaFinal,
    string xmlGrupos, string xmlFamilias, string idArticulo)
        {
            ResultadoOperacion _resultado = new ResultadoOperacion();
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = _dao.kardex(con, idSucursal, fechaInicio, fechaFinal, xmlGrupos,
                        xmlFamilias, idArticulo);
                 
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
    }
}
