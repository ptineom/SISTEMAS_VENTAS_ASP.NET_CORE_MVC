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
    public class UbigeoRepository : IUbigeoRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_UBIGEO";
        public UbigeoRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<List<UBIGEO>> GetAllDepartamentsAsync()
        {
            List<UBIGEO> listaDepartamento = null;
            UBIGEO oDepartamento = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEP";
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaDepartamento = new List<UBIGEO>();
                        while (reader.Read())
                        {
                            oDepartamento = new UBIGEO();
                            oDepartamento.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            oDepartamento.UBIGEO_DEPARTAMENTO = reader.GetString(reader.GetOrdinal("UBIGEO_DEPARTAMENTO"));
                            listaDepartamento.Add(oDepartamento);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return listaDepartamento;
        }

        public async Task<List<UBIGEO>> GetAllDistrictsAsync(string provinceId)
        {
            List<UBIGEO> listaDistrito = null;
            UBIGEO oDistrito = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DIS";
                cmd.Parameters.Add("@ID_PROVINCIA", SqlDbType.VarChar, 4).Value = provinceId;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaDistrito = new List<UBIGEO>();
                        while (reader.Read())
                        {
                            oDistrito = new UBIGEO();
                            oDistrito.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            oDistrito.UBIGEO_DISTRITO = reader.GetString(reader.GetOrdinal("UBIGEO_DISTRITO"));
                            listaDistrito.Add(oDistrito);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return listaDistrito;
        }

        public async Task<List<UBIGEO>> GetAllProvincesAsync(string departamentId)
        {
            List<UBIGEO> listaProvincia = null;
            UBIGEO oProvincia = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "PRO";
                cmd.Parameters.Add("@ID_DEPARTAMENTO", SqlDbType.VarChar, 2).Value = departamentId;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaProvincia = new List<UBIGEO>();
                        while (reader.Read())
                        {
                            oProvincia = new UBIGEO();
                            oProvincia.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            oProvincia.UBIGEO_PROVINCIA = reader.GetString(reader.GetOrdinal("UBIGEO_PROVINCIA"));
                            listaProvincia.Add(oProvincia);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return listaProvincia;
        }
    }
}
