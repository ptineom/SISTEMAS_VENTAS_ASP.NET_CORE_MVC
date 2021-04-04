using System;
using System.Collections.Generic;
using Entidades;
using System.Data.SqlClient;
using System.Data;
namespace CapaDao
{
    public class DaoCliente
    {
        public List<CLIENTE> listaClientes(SqlConnection con, string tipoFiltro, string filtro, bool flgConInactivos)
        {
            List<CLIENTE> lista = null;
            CLIENTE modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CLIENTE", con))
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
                        lista = new List<CLIENTE>();
                        while (reader.Read())
                        {
                            modelo = new CLIENTE();
                            modelo.ID_CLIENTE = reader.GetString(reader.GetOrdinal("ID_CLIENTE"));
                            modelo.NOM_CLIENTE = reader.IsDBNull(reader.GetOrdinal("NOM_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_CLIENTE"));
                            modelo.ABREVIATURA = reader.GetString(reader.GetOrdinal("ABREVIATURA"));
                            modelo.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.TEL_CLIENTE = reader.IsDBNull(reader.GetOrdinal("TEL_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("TEL_CLIENTE"));
                            modelo.EMAIL_CLIENTE = reader.IsDBNull(reader.GetOrdinal("EMAIL_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_CLIENTE"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.DIR_CLIENTE = reader.IsDBNull(reader.GetOrdinal("DIR_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_CLIENTE"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public CLIENTE clientePorCodigo(SqlConnection con, string idCliente)
        {
            CLIENTE modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CLIENTE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar, 8).Value = idCliente;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new CLIENTE();
                            modelo.ID_CLIENTE = reader.GetString(reader.GetOrdinal("ID_CLIENTE"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.NOM_CLIENTE = reader.IsDBNull(reader.GetOrdinal("NOM_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_CLIENTE"));
                            modelo.DIR_CLIENTE = reader.IsDBNull(reader.GetOrdinal("DIR_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_CLIENTE"));
                            modelo.TEL_CLIENTE = reader.IsDBNull(reader.GetOrdinal("TEL_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("TEL_CLIENTE"));
                            modelo.EMAIL_CLIENTE = reader.IsDBNull(reader.GetOrdinal("EMAIL_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL_CLIENTE"));
                            modelo.ID_UBIGEO = reader.IsDBNull(reader.GetOrdinal("ID_UBIGEO")) ? default(string) : reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            modelo.OBS_CLIENTE = reader.IsDBNull(reader.GetOrdinal("OBS_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("OBS_CLIENTE"));
                            modelo.FLG_PERSONA_NATURAL = reader.GetBoolean(reader.GetOrdinal("FLG_PERSONA_NATURAL"));
                            modelo.SEXO = reader.IsDBNull(reader.GetOrdinal("SEXO")) ? default(string) : reader.GetString(reader.GetOrdinal("SEXO"));
                            modelo.APELLIDO_PATERNO = reader.IsDBNull(reader.GetOrdinal("APELLIDO_PATERNO")) ? default(string) : reader.GetString(reader.GetOrdinal("APELLIDO_PATERNO"));
                            modelo.APELLIDO_MATERNO = reader.IsDBNull(reader.GetOrdinal("APELLIDO_MATERNO")) ? default(string) : reader.GetString(reader.GetOrdinal("APELLIDO_MATERNO"));
                            modelo.NOMBRES = reader.IsDBNull(reader.GetOrdinal("NOMBRES")) ? default(string) : reader.GetString(reader.GetOrdinal("NOMBRES"));
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

        public bool grabarCliente(SqlConnection con, SqlTransaction trx, CLIENTE oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CLIENTE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;

                SqlParameter paramIdCliente = new SqlParameter("@ID_CLIENTE", SqlDbType.VarChar, 8);
                paramIdCliente.Direction = ParameterDirection.InputOutput;
                paramIdCliente.Value = oModelo.ID_CLIENTE == "" ? (object)DBNull.Value : oModelo.ID_CLIENTE;
                cmd.Parameters.Add(paramIdCliente);

                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = oModelo.ID_TIPO_DOCUMENTO == -1 ? (object)DBNull.Value : oModelo.ID_TIPO_DOCUMENTO;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.VarChar, 20).Value = oModelo.NRO_DOCUMENTO == "" ? (object)DBNull.Value : oModelo.NRO_DOCUMENTO;
                cmd.Parameters.Add("@DIR_CLIENTE", SqlDbType.VarChar, 100).Value = oModelo.DIR_CLIENTE == "" ? (object)DBNull.Value : oModelo.DIR_CLIENTE;
                cmd.Parameters.Add("@TEL_CLIENTE", SqlDbType.VarChar, 20).Value = oModelo.TEL_CLIENTE == "" ? (object)DBNull.Value : oModelo.TEL_CLIENTE;
                cmd.Parameters.Add("@EMAIL_CLIENTE", SqlDbType.VarChar, 100).Value = oModelo.EMAIL_CLIENTE == "" ? (object)DBNull.Value : oModelo.EMAIL_CLIENTE;
                cmd.Parameters.Add("@ID_UBIGEO", SqlDbType.VarChar, 6).Value = oModelo.ID_UBIGEO == "-1" ? (object)DBNull.Value : oModelo.ID_UBIGEO;
                cmd.Parameters.Add("@OBS_CLIENTE", SqlDbType.VarChar, 100).Value = oModelo.OBS_CLIENTE == "" ? (object)DBNull.Value : oModelo.OBS_CLIENTE;
                cmd.Parameters.Add("@FLG_PERSONA_NATURAL", SqlDbType.Bit).Value = oModelo.FLG_PERSONA_NATURAL;
                if (oModelo.FLG_PERSONA_NATURAL)
                {
                    cmd.Parameters.Add("@APELLIDO_PATERNO", SqlDbType.VarChar, 50).Value = oModelo.APELLIDO_PATERNO == "" ? (object)DBNull.Value : oModelo.APELLIDO_PATERNO;
                    cmd.Parameters.Add("@APELLIDO_MATERNO", SqlDbType.VarChar, 50).Value = oModelo.APELLIDO_MATERNO == "" ? (object)DBNull.Value : oModelo.APELLIDO_MATERNO;
                    cmd.Parameters.Add("@NOMBRES", SqlDbType.VarChar, 60).Value = oModelo.NOMBRES == "" ? (object)DBNull.Value : oModelo.NOMBRES;
                    cmd.Parameters.Add("@SEXO", SqlDbType.Char, 1).Value = oModelo.SEXO == "" ? (object)DBNull.Value : oModelo.SEXO;
                }
                else
                {
                    cmd.Parameters.Add("@NOM_CLIENTE", SqlDbType.VarChar, 160).Value = oModelo.NOM_CLIENTE == "" ? (object)DBNull.Value : oModelo.NOM_CLIENTE;
                    cmd.Parameters.Add("@CONTACTO", SqlDbType.VarChar, 100).Value = oModelo.CONTACTO == "" ? (object)DBNull.Value : oModelo.CONTACTO;
                }
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = oModelo.FLG_INACTIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;

                if (oModelo.ACCION == "INS")
                    oModelo.ID_CLIENTE = cmd.Parameters["@ID_CLIENTE"].Value.ToString();
            }
            return bExito;
        }

        public bool anularCliente(SqlConnection con, SqlTransaction trx, string idCliente, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CLIENTE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar, 8).Value = idCliente;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public CLIENTE clientePorDocumento(SqlConnection con, int idTipoDocumento, string nroDocumento)
        {
            CLIENTE modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CLIENTE", con))
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
                            modelo = new CLIENTE();
                            modelo.ID_CLIENTE = reader.GetString(reader.GetOrdinal("ID_CLIENTE"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NRO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.NOM_CLIENTE = reader.IsDBNull(reader.GetOrdinal("NOM_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_CLIENTE"));
                            modelo.DIR_CLIENTE = reader.IsDBNull(reader.GetOrdinal("DIR_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_CLIENTE"));
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
