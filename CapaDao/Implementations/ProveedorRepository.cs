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
    public class ProveedorRepository : IProveedorRepository, IProveedorExtention<PROVEEDOR>
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_PROVEEDOR";
        public ProveedorRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<bool> DeleteAsync(PROVEEDOR obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = obj.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<List<PROVEEDOR>> GetAllAsync(PROVEEDOR obj)
        {
            List<PROVEEDOR> list = null;

            string tipoFiltro = "descripcion";
            string filtro = "";

            if (!string.IsNullOrEmpty(obj.ID_PROVEEDOR) && string.IsNullOrEmpty(obj.NOM_PROVEEDOR) && string.IsNullOrEmpty(obj.NRO_DOCUMENTO))
            {
                tipoFiltro = "codigo";
                filtro = obj.ID_PROVEEDOR;
            }
            else if (!string.IsNullOrEmpty(obj.NOM_PROVEEDOR) && string.IsNullOrEmpty(obj.ID_PROVEEDOR) && string.IsNullOrEmpty(obj.NRO_DOCUMENTO))
            {
                tipoFiltro = "descripcion";
                filtro = obj.NOM_PROVEEDOR;
            }
            else if (!string.IsNullOrEmpty(obj.NRO_DOCUMENTO) && string.IsNullOrEmpty(obj.ID_PROVEEDOR) && string.IsNullOrEmpty(obj.NOM_PROVEEDOR))
            {
                tipoFiltro = "numDoc";
                filtro = obj.NOM_PROVEEDOR;
            }

            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@TIPO_FILTRO", SqlDbType.VarChar, 20).Value = tipoFiltro;
                cmd.Parameters.Add("@FILTRO", SqlDbType.VarChar, 160).Value = filtro;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = obj.FLG_INACTIVO;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<PROVEEDOR>();
                        while (reader.Read())
                        {
                            list.Add(new PROVEEDOR()
                            {
                                ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR")),
                                NOM_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("NOM_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR")),
                                ABREVIATURA = reader.GetString(reader.GetOrdinal("ABREVIATURA")),
                                NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO")),
                                TEL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("TEL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("TEL_PROVEEDOR")),
                                EMAIL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("EMAIL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_PROVEEDOR")),
                                FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO")),
                                ID_TIPO_DOCUMENTO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_DOCUMENTO")) ? default(int) : reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO")),
                                DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR")),
                                FLG_MISMA_EMPRESA = reader.GetBoolean(reader.GetOrdinal("FLG_MISMA_EMPRESA")),
                            });
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return list;
        }

        public async Task<PROVEEDOR> GetByIdAsync(string id)
        {
            PROVEEDOR model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = id;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new PROVEEDOR();
                            model.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            model.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            model.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            model.NOM_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("NOM_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            model.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
                            model.TEL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("TEL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("TEL_PROVEEDOR"));
                            model.EMAIL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("EMAIL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_PROVEEDOR"));
                            model.ID_UBIGEO = reader.IsDBNull(reader.GetOrdinal("ID_UBIGEO")) ? default(string) : reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            model.OBS_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("OBS_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("OBS_PROVEEDOR"));
                            model.CONTACTO = reader.IsDBNull(reader.GetOrdinal("CONTACTO")) ? default(string) : reader.GetString(reader.GetOrdinal("CONTACTO"));
                            model.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }
        public async Task<bool> RegisterAsync(PROVEEDOR obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "INS";

                SqlParameter paramIdProveedor = new SqlParameter("@ID_PROVEEDOR", SqlDbType.VarChar, 8);
                paramIdProveedor.Direction = ParameterDirection.InputOutput;
                paramIdProveedor.Value = (object)DBNull.Value;
                cmd.Parameters.Add(paramIdProveedor);

                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = obj.ID_TIPO_DOCUMENTO == null ? (object)DBNull.Value : obj.ID_TIPO_DOCUMENTO;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 20).Value = obj.NRO_DOCUMENTO == "" ? (object)DBNull.Value : obj.NRO_DOCUMENTO;
                cmd.Parameters.Add("@DIR_PROVEEDOR", SqlDbType.VarChar, 100).Value = obj.DIR_PROVEEDOR == "" ? (object)DBNull.Value : obj.DIR_PROVEEDOR;
                cmd.Parameters.Add("@TEL_PROVEEDOR", SqlDbType.VarChar, 20).Value = obj.TEL_PROVEEDOR == "" ? (object)DBNull.Value : obj.TEL_PROVEEDOR;
                cmd.Parameters.Add("@EMAIL_PROVEEDOR", SqlDbType.VarChar, 100).Value = obj.EMAIL_PROVEEDOR == "" ? (object)DBNull.Value : obj.EMAIL_PROVEEDOR;
                cmd.Parameters.Add("@ID_UBIGEO", SqlDbType.VarChar, 6).Value = string.IsNullOrEmpty(obj.ID_UBIGEO) ? (object)DBNull.Value : obj.ID_UBIGEO;
                cmd.Parameters.Add("@OBS_PROVEEDOR", SqlDbType.VarChar, 100).Value = obj.OBS_PROVEEDOR == "" ? (object)DBNull.Value : obj.OBS_PROVEEDOR;
                cmd.Parameters.Add("@NOM_PROVEEDOR", SqlDbType.VarChar, 160).Value = obj.NOM_PROVEEDOR == "" ? (object)DBNull.Value : obj.NOM_PROVEEDOR;
                cmd.Parameters.Add("@CONTACTO", SqlDbType.VarChar, 100).Value = obj.CONTACTO == "" ? (object)DBNull.Value : obj.CONTACTO;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = obj.FLG_INACTIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();

                if (obj.ACCION == "INS")
                    obj.ID_PROVEEDOR = cmd.Parameters["@ID_PROVEEDOR"].Value.ToString();
            }

            return true;
        }
        public async Task<bool> UpdateAsync(PROVEEDOR obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "UPD";
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = obj.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = obj.ID_TIPO_DOCUMENTO == null ? (object)DBNull.Value : obj.ID_TIPO_DOCUMENTO;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 20).Value = obj.NRO_DOCUMENTO == "" ? (object)DBNull.Value : obj.NRO_DOCUMENTO;
                cmd.Parameters.Add("@DIR_PROVEEDOR", SqlDbType.VarChar, 100).Value = obj.DIR_PROVEEDOR == "" ? (object)DBNull.Value : obj.DIR_PROVEEDOR;
                cmd.Parameters.Add("@TEL_PROVEEDOR", SqlDbType.VarChar, 20).Value = obj.TEL_PROVEEDOR == "" ? (object)DBNull.Value : obj.TEL_PROVEEDOR;
                cmd.Parameters.Add("@EMAIL_PROVEEDOR", SqlDbType.VarChar, 100).Value = obj.EMAIL_PROVEEDOR == "" ? (object)DBNull.Value : obj.EMAIL_PROVEEDOR;
                cmd.Parameters.Add("@ID_UBIGEO", SqlDbType.VarChar, 6).Value = obj.ID_UBIGEO == "-1" ? (object)DBNull.Value : obj.ID_UBIGEO;
                cmd.Parameters.Add("@OBS_PROVEEDOR", SqlDbType.VarChar, 100).Value = obj.OBS_PROVEEDOR == "" ? (object)DBNull.Value : obj.OBS_PROVEEDOR;
                cmd.Parameters.Add("@NOM_PROVEEDOR", SqlDbType.VarChar, 160).Value = obj.NOM_PROVEEDOR == "" ? (object)DBNull.Value : obj.NOM_PROVEEDOR;
                cmd.Parameters.Add("@CONTACTO", SqlDbType.VarChar, 100).Value = obj.CONTACTO == "" ? (object)DBNull.Value : obj.CONTACTO;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = obj.FLG_INACTIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();

            }

            return true;
        }
        public async Task<PROVEEDOR> GetByDocument(PROVEEDOR obj)
        {
            PROVEEDOR model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "NRO";
                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = obj.ID_TIPO_DOCUMENTO;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 20).Value = obj.NRO_DOCUMENTO;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new PROVEEDOR();
                            model.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            model.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            model.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            model.NOM_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("NOM_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            model.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }
    }
}
