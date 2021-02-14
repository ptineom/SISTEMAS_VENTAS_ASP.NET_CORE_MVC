using System;
using System.Collections.Generic;
using Entidades;
using System.Data.SqlClient;
using System.Data;
namespace CapaDao
{
    public class DaoCargo
    {
        public List<CARGO> listaCargos(SqlConnection con)
        {
            List<CARGO> lista = null;
            CARGO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CARGO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CARGO>();
                        while (reader.Read())
                        {
                            modelo = new CARGO();
                            modelo.ID_CARGO = reader.GetInt32(reader.GetOrdinal("ID_CARGO"));
                            modelo.NOM_CARGO = reader.GetString(reader.GetOrdinal("NOM_CARGO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarCargo(SqlConnection con, SqlTransaction trx, CARGO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CARGO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_CARGO", SqlDbType.Int).Value = oModelo.ID_CARGO == 0 ? (object)DBNull.Value : oModelo.ID_CARGO;
                cmd.Parameters.Add("@NOM_CARGO", SqlDbType.VarChar, 50).Value = oModelo.NOM_CARGO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularCargo(SqlConnection con, SqlTransaction trx, int idCargo, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CARGO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_CARGO", SqlDbType.Int).Value = idCargo;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
