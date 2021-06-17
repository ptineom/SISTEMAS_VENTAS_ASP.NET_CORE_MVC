using CapaDao.Contracts;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Implementations
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_REFRESH_TOKEN";
        public RefreshTokenRepository(IConnection db)
        {
            _sqlConnection = db;
        }
        public async Task<REFRESH_TOKEN> GetByIdAsync(string idRefreshToken)
        {
            REFRESH_TOKEN model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_REFRESH_TOKEN", SqlDbType.VarChar, 300).Value = idRefreshToken;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new REFRESH_TOKEN();
                            model.ID_REFRESH_TOKEN = reader.GetString(reader.GetOrdinal("ID_REFRESH_TOKEN"));
                            model.FEC_CREACION_UTC = reader.GetDateTime(reader.GetOrdinal("FEC_CREACION_UTC"));
                            model.FEC_EXPIRACION_UTC = reader.GetDateTime(reader.GetOrdinal("FEC_EXPIRACION_UTC"));
                            model.ID_USUARIO_TOKEN = reader.GetString(reader.GetOrdinal("ID_USUARIO_TOKEN"));
                            model.JSON_CLAIMS = reader.GetString(reader.GetOrdinal("JSON_CLAIMS"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }

        public async Task<bool> RegisterAsync(REFRESH_TOKEN obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "INS";
                cmd.Parameters.Add("@ID_REFRESH_TOKEN", SqlDbType.VarChar, 300).Value = obj.ID_REFRESH_TOKEN;
                cmd.Parameters.Add("@TIEMPO_EXPIRACION_MINUTOS", SqlDbType.Int).Value = obj.TIEMPO_EXPIRACION_MINUTOS;
                cmd.Parameters.Add("@ID_USUARIO_TOKEN", SqlDbType.VarChar, 90).Value = obj.ID_USUARIO_TOKEN;
                cmd.Parameters.Add("@IP_ADDRESS", SqlDbType.VarChar, 90).Value = obj.IP_ADDRESS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@FEC_CREACION_UTC", SqlDbType.DateTime).Value = obj.FEC_CREACION_UTC;
                cmd.Parameters.Add("@JSON_CLAIMS", SqlDbType.Xml).Value = obj.JSON_CLAIMS;

                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }
    }
}
