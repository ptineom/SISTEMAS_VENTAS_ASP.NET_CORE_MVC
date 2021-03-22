using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using System.Data.SqlClient;
using System.Data;
namespace CapaDao
{
    public class DaoProveedor
    {
        public List<PROVEEDOR> GetAllByFilters(SqlConnection con, string tipoFiltro, string filtro, bool flgConInactivos )
        {
            List<PROVEEDOR> lista = null;
            PROVEEDOR modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PROVEEDOR", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@TIPO_FILTRO", SqlDbType.VarChar, 20).Value = tipoFiltro;
                cmd.Parameters.Add("@FILTRO", SqlDbType.VarChar, 160).Value = filtro;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = flgConInactivos;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<PROVEEDOR>();
                        while (reader.Read())
                        {
                            modelo = new PROVEEDOR();
                            modelo.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            modelo.NOM_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("NOM_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.ABREVIATURA = reader.GetString(reader.GetOrdinal("ABREVIATURA"));
                            modelo.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.TEL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("TEL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("TEL_PROVEEDOR"));
                            modelo.EMAIL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("EMAIL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_PROVEEDOR"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.ID_TIPO_DOCUMENTO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_DOCUMENTO")) ? default(int) : reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
                            modelo.FLG_MISMA_EMPRESA = reader.GetBoolean(reader.GetOrdinal("FLG_MISMA_EMPRESA")); 
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public PROVEEDOR GetById(SqlConnection con, string idProveedor)
        {
            PROVEEDOR modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PROVEEDOR", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = idProveedor;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new PROVEEDOR();
                            modelo.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.NOM_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("NOM_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
                            modelo.TEL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("TEL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("TEL_PROVEEDOR"));
                            modelo.EMAIL_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("EMAIL_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_PROVEEDOR"));
                            modelo.ID_UBIGEO = reader.IsDBNull(reader.GetOrdinal("ID_UBIGEO")) ? default(string) : reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            modelo.OBS_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("OBS_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("OBS_PROVEEDOR"));
                            modelo.CONTACTO = reader.IsDBNull(reader.GetOrdinal("CONTACTO")) ? default(string) : reader.GetString(reader.GetOrdinal("CONTACTO"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public bool Register(SqlConnection con, SqlTransaction trx, PROVEEDOR oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PROVEEDOR", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = oModelo.ID_PROVEEDOR == "" ? (object)DBNull.Value: oModelo.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = oModelo.ID_TIPO_DOCUMENTO == -1 ? (object)DBNull.Value : oModelo.ID_TIPO_DOCUMENTO;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 20).Value = oModelo.NRO_DOCUMENTO == "" ? (object)DBNull.Value : oModelo.NRO_DOCUMENTO;
                cmd.Parameters.Add("@DIR_PROVEEDOR", SqlDbType.VarChar, 100).Value = oModelo.DIR_PROVEEDOR == "" ? (object)DBNull.Value : oModelo.DIR_PROVEEDOR;
                cmd.Parameters.Add("@TEL_PROVEEDOR", SqlDbType.VarChar, 20).Value = oModelo.TEL_PROVEEDOR == "" ? (object)DBNull.Value : oModelo.TEL_PROVEEDOR;
                cmd.Parameters.Add("@EMAIL_PROVEEDOR", SqlDbType.VarChar, 100).Value = oModelo.EMAIL_PROVEEDOR == "" ? (object)DBNull.Value : oModelo.EMAIL_PROVEEDOR;
                cmd.Parameters.Add("@ID_UBIGEO", SqlDbType.VarChar, 6).Value = oModelo.ID_UBIGEO == "-1" ? (object)DBNull.Value : oModelo.ID_UBIGEO;
                cmd.Parameters.Add("@OBS_PROVEEDOR", SqlDbType.VarChar, 100).Value = oModelo.OBS_PROVEEDOR == "" ? (object)DBNull.Value : oModelo.OBS_PROVEEDOR;
                cmd.Parameters.Add("@NOM_PROVEEDOR", SqlDbType.VarChar, 160).Value = oModelo.NOM_PROVEEDOR == "" ? (object)DBNull.Value : oModelo.NOM_PROVEEDOR;
                cmd.Parameters.Add("@CONTACTO", SqlDbType.VarChar, 100).Value = oModelo.CONTACTO == "" ? (object)DBNull.Value : oModelo.CONTACTO;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = oModelo.FLG_INACTIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool Delete(SqlConnection con, SqlTransaction trx, string idProveedor, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PROVEEDOR", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = idProveedor;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public PROVEEDOR GetByDocument(SqlConnection con, int idTipoDocumento, string nroDocumento)
        {
            PROVEEDOR modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PROVEEDOR", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "NRO";
                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = idTipoDocumento;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 20).Value = nroDocumento;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new PROVEEDOR();
                            modelo.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.NOM_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("NOM_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
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
