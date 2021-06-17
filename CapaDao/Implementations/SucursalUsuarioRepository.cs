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
    public class SucursalUsuarioRepository : ISucursalUsuarioRepository
    {
        private readonly IConnection _sqlConnection;
        public SucursalUsuarioRepository(IConnection db)
        {
            _sqlConnection = db;
        }
        public async Task<List<SUCURSAL>> GetAllByCampusId(string userId)
        {
            List<SUCURSAL> list = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_USUARIO", _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SPU";
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = userId;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<SUCURSAL>();
                        while (reader.Read())
                        {
                            list.Add(new SUCURSAL()
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
            return list;
        }
    }
}
