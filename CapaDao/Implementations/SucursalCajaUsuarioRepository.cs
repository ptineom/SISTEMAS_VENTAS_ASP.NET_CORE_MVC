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
    public class SucursalCajaUsuarioRepository : ISucursalCajaUsuarioRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_SUCURSAL_CAJA_USUARIO";
        public SucursalCajaUsuarioRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<List<CAJA>> GetAllBoxes(SUCURSAL_CAJA_USUARIO obj)
        {
            List<CAJA> list = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CPU";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(obj.ID_USUARIO) ? (object)DBNull.Value : obj.ID_USUARIO;
                
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

        public async Task<List<USUARIO>> GetAllUsersByCampusId(string campusId)
        {
            List<USUARIO> list = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "UPS";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = campusId;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<USUARIO>();
                        while (reader.Read())
                        {
                            list.Add(new USUARIO()
                            {
                                ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
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
