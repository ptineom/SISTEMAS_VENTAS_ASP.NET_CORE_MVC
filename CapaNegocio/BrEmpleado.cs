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
    public class BrEmpleado
    {
        DaoEmpleado dao = null;
        ResultadoOperacion oResultado = null;
        public BrEmpleado()
        {
            dao = new DaoEmpleado();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listaEmpleados()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaEmpleados(con);
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
        public ResultadoOperacion combosEmpleados()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    EMPLEADO modelo = dao.combosEmpleados(con);
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
        public ResultadoOperacion empleadoPorCodigo(string idEmpleado)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    EMPLEADO modelo = dao.empleadoPorCodigo(con, idEmpleado);
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
        public ResultadoOperacion grabarEmpleado(EMPLEADO oModelo, ref string idEmpleado, ref string xmlFotos)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarEmpleado(con, trx, oModelo, ref idEmpleado, ref xmlFotos);
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
        public ResultadoOperacion anularEmpleado(string idEmpleado, string idUsuario)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    string xmlFotos = string.Empty;
                    dao.anularEmpleado(con, trx, idEmpleado, idUsuario, ref xmlFotos);
                    oResultado.SetResultado(true, Helper.Constantes.sMensajeEliminadoOk);
                    oResultado.data = xmlFotos;
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
        public ResultadoOperacion listaEmpleadosGeneral()
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    var lista = dao.listaEmpleadosGeneral(con);
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
    }
}
