using System;
using System.Collections.Generic;
using Entidades;
using System.Data.SqlClient;
using System.Data;
namespace CapaDao
{
    public class DaoMarca
    {
        public List<MARCA> listaMarcas(SqlConnection con, string nomMarca)
        {
            List<MARCA> lista = null;
            MARCA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_MARCA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@NOM_MARCA", SqlDbType.VarChar, 90).Value = nomMarca;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<MARCA>();
                        while (reader.Read())
                        {
                            modelo = new MARCA();
                            modelo.ID_MARCA = reader.GetInt32(reader.GetOrdinal("ID_MARCA"));
                            modelo.NOM_MARCA = reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarMarca(SqlConnection con, SqlTransaction trx, MARCA oModelo, ref int idMarca)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_MARCA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                if (oModelo.ACCION == "INS")
                    cmd.Parameters.Add("@ID_MARCA", SqlDbType.Int).Direction = ParameterDirection.InputOutput;
                else
                    cmd.Parameters.Add("@ID_MARCA", SqlDbType.Int).Value = oModelo.ID_MARCA;
                cmd.Parameters.Add("@NOM_MARCA", SqlDbType.VarChar, 90).Value = oModelo.NOM_MARCA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;

                cmd.ExecuteNonQuery();
                bExito = true;

                if (oModelo.ACCION == "INS")
                    idMarca = Convert.ToInt32(cmd.Parameters["@ID_MARCA"].Value);
            }
            return bExito;
        }
        public bool anularMarca(SqlConnection con, SqlTransaction trx, int idMarca, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_MARCA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_MARCA", SqlDbType.Int).Value = idMarca;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
