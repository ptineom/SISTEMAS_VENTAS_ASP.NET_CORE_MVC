using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Data.SqlClient;
using System.Data;
namespace CapaDao
{
    public class DaoSucursalCajaUsuario
    {
        public List<USUARIO> listaUsuarios(SqlConnection con, string idSucursal, string idCaja, ref List<SUCURSAL_CAJA_USUARIO> listaSucCajUsu)
        {
            List<USUARIO> lista = null;
            List<SUCURSAL_CAJA_USUARIO> lSucursalCajaUsuario = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = idCaja;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<USUARIO>();
                        while (reader.Read())
                        {
                            lista.Add(new USUARIO()
                            {
                                ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
                            });
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            lSucursalCajaUsuario = new List<SUCURSAL_CAJA_USUARIO>();
                            while (reader.Read())
                            {
                                lSucursalCajaUsuario.Add(new SUCURSAL_CAJA_USUARIO()
                                {
                                    ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                    NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
                                });
                            }
                            listaSucCajUsu = lSucursalCajaUsuario;
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public List<CAJA> listaCajas(SqlConnection con, string idSucursal)
        {
            List<CAJA> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CAJ";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CAJA>();
                        while (reader.Read())
                        {
                            lista.Add(new CAJA()
                            {
                                ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"))
                            });
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarSucursalCajaUsuario(SqlConnection con, SqlTransaction trx, SUCURSAL_CAJA_USUARIO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = oModelo.ID_CAJA;
                cmd.Parameters.Add("@XML_USUARIOS", SqlDbType.Xml).Value = oModelo.XML_USUARIOS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularSucursalCajaUsuario(SqlConnection con, SqlTransaction trx, string idSucursal, string idCaja, string idUsuario, string idUsuarioRegistro)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = idCaja;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuarioRegistro;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
