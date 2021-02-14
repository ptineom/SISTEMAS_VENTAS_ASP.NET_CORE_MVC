using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Entidades;
namespace CapaDao
{
    public class DaoRefreshToken
    {
        public bool grabarRefreshToken(SqlConnection con, SqlTransaction trx, REFRESH_TOKEN oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_REFRESH_TOKEN", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "INS";
                cmd.Parameters.Add("@ID_REFRESH_TOKEN", SqlDbType.VarChar, 300).Value = oModelo.ID_REFRESH_TOKEN;
                cmd.Parameters.Add("@TIEMPO_EXPIRACION_MINUTOS", SqlDbType.Int).Value = oModelo.TIEMPO_EXPIRACION_MINUTOS;
                cmd.Parameters.Add("@ID_USUARIO_TOKEN", SqlDbType.VarChar, 90).Value = oModelo.ID_USUARIO_TOKEN;
                cmd.Parameters.Add("@IP_ADDRESS", SqlDbType.VarChar, 90).Value = oModelo.IP_ADDRESS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@FEC_CREACION_UTC", SqlDbType.DateTime).Value = oModelo.FEC_CREACION_UTC;
                cmd.Parameters.Add("@XML_CLAIMS", SqlDbType.Xml).Value = oModelo.XML_CLAIMS;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public REFRESH_TOKEN refreshTokenPorCodigo(SqlConnection con, string idRefreshToken)
        {
            REFRESH_TOKEN modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_REFRESH_TOKEN", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_REFRESH_TOKEN", SqlDbType.VarChar, 300).Value = idRefreshToken;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new REFRESH_TOKEN();
                            modelo.ID_REFRESH_TOKEN = reader.GetString(reader.GetOrdinal("ID_REFRESH_TOKEN"));
                            modelo.FEC_CREACION_UTC = reader.GetDateTime(reader.GetOrdinal("FEC_CREACION_UTC"));
                            modelo.FEC_EXPIRACION_UTC = reader.GetDateTime(reader.GetOrdinal("FEC_EXPIRACION_UTC"));
                            modelo.ID_USUARIO_TOKEN = reader.GetString(reader.GetOrdinal("ID_USUARIO_TOKEN"));
                            modelo.XML_CLAIMS = reader.GetString(reader.GetOrdinal("XML_CLAIMS"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }
    }
}
