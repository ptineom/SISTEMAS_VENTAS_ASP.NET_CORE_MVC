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
    public class MonedaRepository : IMonedaRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_MONEDA";
        public MonedaRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<List<MONEDA>> GetAllAsync()
        {
            List<MONEDA> list = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<MONEDA>();
                        while (reader.Read())
                        {
                            list.Add(new MONEDA()
                            {
                                ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA")),
                                NOM_MONEDA = reader.GetString(reader.GetOrdinal("NOM_MONEDA")),
                                SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA")),
                                FLG_LOCAL = reader.GetBoolean(reader.GetOrdinal("FLG_LOCAL"))
                            });
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return list;
        }
    }
}
