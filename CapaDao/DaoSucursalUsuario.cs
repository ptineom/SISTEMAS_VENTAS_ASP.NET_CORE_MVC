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
    public class DaoSucursalUsuario
    {
        public bool grabarSucursalUsuario(SqlConnection con, SqlTransaction trx, SUCURSAL_USUARIO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@XML_USUARIOS", SqlDbType.Xml).Value = oModelo.XML_USUARIOS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public List<USUARIO> listaUsuarios(SqlConnection con, string idSucursal, ref List<SUCURSAL_USUARIO> listaSucUsu)
        {
            List<USUARIO> lista = null;
            List<SUCURSAL_USUARIO> lSucursalUsuario = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
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
                            lSucursalUsuario = new List<SUCURSAL_USUARIO>();
                            while (reader.Read())
                            {
                                lSucursalUsuario.Add(new SUCURSAL_USUARIO()
                                {
                                    ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                    NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
                                });
                            }
                            listaSucUsu = lSucursalUsuario;
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool anularSucursalUsuario(SqlConnection con, SqlTransaction trx, string idSucursal, string idUsuario, string idUsuarioRegistro)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuarioRegistro;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public List<SUCURSAL> listaSucursalPorUsuario(SqlConnection con, string idUsuario)
        {
            List<SUCURSAL> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SPU";
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<SUCURSAL>();
                        while (reader.Read())
                        {
                            lista.Add(new SUCURSAL()
                            {
                                ID_SUCURSAL = reader.GetString(reader.GetOrdinal("ID_SUCURSAL")),
                                NOM_SUCURSAL = reader.GetString(reader.GetOrdinal("NOM_SUCURSAL"))
                            });
                        }
                    }

                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
    }
}
