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
    public class DaoUsuario
    {
       
        public List<USUARIO> GetAll(SqlConnection con)
        {
            List<USUARIO> lista = null;
            USUARIO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<USUARIO>();
                        while (reader.Read())
                        {
                            modelo = new USUARIO();
                            modelo.ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO"));
                            modelo.NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"));
                            modelo.EMAIL_USUARIO = reader.IsDBNull(reader.GetOrdinal("EMAIL_USUARIO")) ? string.Empty : reader.GetString(reader.GetOrdinal("EMAIL_USUARIO"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.NOM_GRUPO_ACCESO = reader.GetString(reader.GetOrdinal("NOM_GRUPO_ACCESO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public USUARIO GetById(SqlConnection con, string idUsuario)
        {
            USUARIO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new USUARIO();
                            modelo.ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO"));
                            modelo.NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"));
                            modelo.ID_EMPLEADO = reader.GetString(reader.GetOrdinal("ID_EMPLEADO"));
                            modelo.ID_GRUPO_ACCESO = reader.GetInt32(reader.GetOrdinal("ID_GRUPO_ACCESO"));
                            modelo.EMAIL_USUARIO = reader.IsDBNull(reader.GetOrdinal("EMAIL_USUARIO")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_USUARIO"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public bool Register(SqlConnection con, SqlTransaction trx, USUARIO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO;
                cmd.Parameters.Add("@CLAVE", SqlDbType.VarChar, 64).Value = oModelo.CLAVE == "" ? (object)DBNull.Value : oModelo.CLAVE;
                cmd.Parameters.Add("@EMAIL_USUARIO", SqlDbType.VarChar, 150).Value = oModelo.EMAIL_USUARIO == "" ? (object)DBNull.Value : oModelo.EMAIL_USUARIO;
                cmd.Parameters.Add("@ID_EMPLEADO", SqlDbType.VarChar, 8).Value = oModelo.ID_EMPLEADO;
                cmd.Parameters.Add("@ID_GRUPO_ACCESO", SqlDbType.Int).Value = oModelo.ID_GRUPO_ACCESO;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = oModelo.FLG_INACTIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool Delete(SqlConnection con, SqlTransaction trx, string idUsuario, string IdUsuarioRegistro)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = IdUsuarioRegistro;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public List<USUARIO> GetUsersActivated(SqlConnection con)
        {
            USUARIO modelo = null;
            List<USUARIO> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CCO";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<USUARIO>();
                        while (reader.Read())
                        {
                            modelo = new USUARIO();
                            modelo.ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO"));
                            modelo.NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"));
                            modelo.EMAIL_USUARIO = reader.IsDBNull(reader.GetOrdinal("EMAIL_USUARIO")) ? string.Empty : reader.GetString(reader.GetOrdinal("EMAIL_USUARIO")); ;
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool ChangePassword(SqlConnection con, SqlTransaction trx, USUARIO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO;
                cmd.Parameters.Add("@CLAVE", SqlDbType.VarChar, 64).Value = oModelo.CLAVE;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool SaveTokenRecovererPassword(SqlConnection con, SqlTransaction trx, USUARIO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@TOKEN_RECUPERACION_PASSWORD", SqlDbType.VarChar, 64).Value = oModelo.TOKEN_RECUPERACION_PASSWORD;
                cmd.Parameters.Add("@EMAIL_USUARIO", SqlDbType.VarChar, 150).Value = oModelo.EMAIL_USUARIO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool RecoverPassowrd(SqlConnection con, SqlTransaction trx, USUARIO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_USUARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO;
                cmd.Parameters.Add("@CLAVE", SqlDbType.VarChar, 64).Value = oModelo.CLAVE;
                cmd.Parameters.Add("@TOKEN_RECUPERACION_PASSWORD", SqlDbType.VarChar, 64).Value = oModelo.TOKEN_RECUPERACION_PASSWORD;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

    }
}
