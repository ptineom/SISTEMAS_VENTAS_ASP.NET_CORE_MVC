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
    public class CajaRepository : ICajaRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_CAJA";
        public CajaRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<bool> DeleteAsync(CAJA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<List<CAJA>> GetAllAsync(CAJA obj)
        {
            List<CAJA> list = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<CAJA>();
                        while (reader.Read())
                        {
                            list.Add(new CAJA()
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
            return list;
        }

        public async Task<CAJA> GetByIdAsync(string id)
        {
            CAJA model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = id;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new CAJA();
                            model.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            model.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }

        public async Task<bool> RegisterAsync(CAJA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "INS";
                cmd.Parameters.Add("@NOM_CAJA", SqlDbType.VarChar, 90).Value = obj.NOM_CAJA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<bool> UpdateAsync(CAJA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "UPD";
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_CAJA) ? (object)DBNull.Value : obj.ID_CAJA;
                cmd.Parameters.Add("@NOM_CAJA", SqlDbType.VarChar, 90).Value = obj.NOM_CAJA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }
    }
}
