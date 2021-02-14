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
    public class DaoEmpleado
    {
        public List<EMPLEADO> listaEmpleados(SqlConnection con)
        {
            List<EMPLEADO> lista = null;
            EMPLEADO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPLEADO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<EMPLEADO>();
                        while (reader.Read())
                        {
                            modelo = new EMPLEADO();
                            modelo.ID_EMPLEADO = reader.GetString(reader.GetOrdinal("ID_EMPLEADO"));
                            modelo.NOM_CARGO = reader.IsDBNull(reader.GetOrdinal("NOM_CARGO")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_CARGO"));
                            modelo.DOCUMENTO = reader.GetString(reader.GetOrdinal("DOCUMENTO"));
                            modelo.NOM_EMPLEADO = reader.GetString(reader.GetOrdinal("NOM_EMPLEADO"));
                            modelo.SEXO = reader.IsDBNull(reader.GetOrdinal("SEXO")) ? default(string) : reader.GetString(reader.GetOrdinal("SEXO"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.TELEFONO = reader.IsDBNull(reader.GetOrdinal("TELEFONO")) ? default(string) : reader.GetString(reader.GetOrdinal("TELEFONO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public EMPLEADO combosEmpleados(SqlConnection con)
        {
            EMPLEADO modelo = null;
            List<CARGO> listaCargo = null;
            CARGO oCargo = null;
            List<TIPO_DOCUMENTO> listaTipoDocumento = null;
            TIPO_DOCUMENTO oTipoDocumento = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPLEADO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaCargo = new List<CARGO>();
                        while (reader.Read())
                        {
                            oCargo = new CARGO();
                            oCargo.ID_CARGO = reader.GetInt32(reader.GetOrdinal("ID_CARGO"));
                            oCargo.NOM_CARGO = reader.GetString(reader.GetOrdinal("NOM_CARGO"));
                            listaCargo.Add(oCargo);
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaTipoDocumento = new List<TIPO_DOCUMENTO>();
                            while (reader.Read())
                            {
                                oTipoDocumento = new TIPO_DOCUMENTO();
                                oTipoDocumento.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                                oTipoDocumento.NOM_TIPO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NOM_TIPO_DOCUMENTO"));
                                listaTipoDocumento.Add(oTipoDocumento);
                            }
                        }
                    }
                    modelo = new EMPLEADO();
                    modelo.LISTA_CARGO = listaCargo;
                    modelo.LISTA_TIPO_DOCUMENTO = listaTipoDocumento;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }
        public EMPLEADO empleadoPorCodigo(SqlConnection con, string idEmpleado)
        {
            EMPLEADO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPLEADO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_EMPLEADO", SqlDbType.VarChar, 8).Value = idEmpleado;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new EMPLEADO();
                            modelo.ID_EMPLEADO = reader.GetString(reader.GetOrdinal("ID_EMPLEADO"));
                            modelo.ID_CARGO = reader.IsDBNull(reader.GetOrdinal("ID_CARGO")) ? -1 : reader.GetInt32(reader.GetOrdinal("ID_CARGO"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NUM_DOCUMENTO = reader.GetString(reader.GetOrdinal("NUM_DOCUMENTO"));
                            modelo.APELLIDO_PATERNO = reader.GetString(reader.GetOrdinal("APELLIDO_PATERNO"));
                            modelo.APELLIDO_MATERNO = reader.GetString(reader.GetOrdinal("APELLIDO_MATERNO"));
                            modelo.NOMBRES = reader.GetString(reader.GetOrdinal("NOMBRES"));
                            modelo.SEXO = reader.GetString(reader.GetOrdinal("SEXO"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.DIRECCION = reader.IsDBNull(reader.GetOrdinal("DIRECCION")) ? default(string) : reader.GetString(reader.GetOrdinal("DIRECCION"));
                            modelo.EMAIL = reader.IsDBNull(reader.GetOrdinal("EMAIL")) ? default(string) : reader.GetString(reader.GetOrdinal("EMAIL"));
                            modelo.FEC_ENTRANTE = reader.IsDBNull(reader.GetOrdinal("FEC_ENTRANTE")) ? default(string) : reader.GetString(reader.GetOrdinal("FEC_ENTRANTE"));
                            modelo.FEC_CESE = reader.IsDBNull(reader.GetOrdinal("FEC_CESE")) ? default(string) : reader.GetString(reader.GetOrdinal("FEC_CESE"));
                            modelo.FOTO = reader.IsDBNull(reader.GetOrdinal("FOTO")) ? default(string) : reader.GetString(reader.GetOrdinal("FOTO"));
                            modelo.TELEFONO = reader.IsDBNull(reader.GetOrdinal("TELEFONO")) ? default(string) : reader.GetString(reader.GetOrdinal("TELEFONO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }
        public bool grabarEmpleado(SqlConnection con, SqlTransaction trx, EMPLEADO oModelo, ref string idEmpleado, ref string xmlFotos)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPLEADO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                if (oModelo.ACCION == "INS")
                {
                    cmd.Parameters.Add("@ID_EMPLEADO", SqlDbType.VarChar, 8).Direction = ParameterDirection.InputOutput;
                }
                else
                {
                    cmd.Parameters.Add("@ID_EMPLEADO", SqlDbType.VarChar, 8).Value = oModelo.ID_EMPLEADO;
                }
                cmd.Parameters.Add("@ID_CARGO", SqlDbType.Int).Value = oModelo.ID_CARGO == -1 ? (object)DBNull.Value : oModelo.ID_CARGO;
                cmd.Parameters.Add("@ID_TIPO_DOCUMENTO", SqlDbType.Int).Value = oModelo.ID_TIPO_DOCUMENTO == -1 ? (object)DBNull.Value : oModelo.ID_TIPO_DOCUMENTO;
                cmd.Parameters.Add("@NUM_DOCUMENTO", SqlDbType.VarChar, 12).Value = oModelo.NUM_DOCUMENTO == "" ? (object)DBNull.Value : oModelo.NUM_DOCUMENTO;
                cmd.Parameters.Add("@APELLIDO_PATERNO", SqlDbType.VarChar, 50).Value = oModelo.APELLIDO_PATERNO == "" ? (object)DBNull.Value : oModelo.APELLIDO_PATERNO;
                cmd.Parameters.Add("@APELLIDO_MATERNO", SqlDbType.VarChar, 50).Value = oModelo.APELLIDO_MATERNO == "" ? (object)DBNull.Value : oModelo.APELLIDO_MATERNO;
                cmd.Parameters.Add("@NOMBRES", SqlDbType.VarChar, 50).Value = oModelo.NOMBRES == "" ? (object)DBNull.Value : oModelo.NOMBRES;
                cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar, 150).Value = oModelo.DIRECCION == "" ? (object)DBNull.Value : oModelo.DIRECCION;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 150).Value = oModelo.EMAIL == "" ? (object)DBNull.Value : oModelo.EMAIL;
                cmd.Parameters.Add("@FEC_ENTRANTE", SqlDbType.DateTime).Value = oModelo.FEC_ENTRANTE == "" ? (object)DBNull.Value : oModelo.FEC_ENTRANTE;
                cmd.Parameters.Add("@FEC_CESE", SqlDbType.DateTime).Value = oModelo.FEC_CESE == "" ? (object)DBNull.Value : oModelo.FEC_CESE;
                cmd.Parameters.Add("@SEXO", SqlDbType.Char, 1).Value = oModelo.SEXO == "" ? (object)DBNull.Value : oModelo.SEXO;
                cmd.Parameters.Add("@TELEFONO", SqlDbType.VarChar, 20).Value = oModelo.TELEFONO == "" ? (object)DBNull.Value : oModelo.TELEFONO;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = oModelo.FLG_INACTIVO;
                cmd.Parameters.Add("@FLG_SIN_FOTO", SqlDbType.Bit).Value = oModelo.FLG_SIN_FOTO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@XML_FOTOS", SqlDbType.Xml).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                bExito = true;
                if (oModelo.ACCION == "INS")
                {
                    idEmpleado = cmd.Parameters["@ID_EMPLEADO"].Value.ToString();
                }
                else if (oModelo.ACCION == "UPD")
                {
                    if (oModelo.FLG_SIN_FOTO && cmd.Parameters["@XML_FOTOS"].Value != null)
                    {
                        xmlFotos = cmd.Parameters["@XML_FOTOS"].Value.ToString();
                    }
                }
            }
            return bExito;
        }
        public bool anularEmpleado(SqlConnection con, SqlTransaction trx, string idEmpleado, string idUsuario, ref string xmlFotos)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPLEADO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_EMPLEADO", SqlDbType.VarChar, 8).Value = idEmpleado;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@XML_FOTOS", SqlDbType.Xml).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                bExito = true;
                if (cmd.Parameters["@XML_FOTOS"].Value != null)
                {
                    xmlFotos = cmd.Parameters["@XML_FOTOS"].Value.ToString();
                }
            }
            return bExito;
        }
        public List<EMPLEADO> listaEmpleadosGeneral(SqlConnection con)
        {
            List<EMPLEADO> lista = null;
            EMPLEADO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPLEADO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "EMP";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<EMPLEADO>();
                        while (reader.Read())
                        {
                            modelo = new EMPLEADO();
                            modelo.ID_EMPLEADO = reader.GetString(reader.GetOrdinal("ID_EMPLEADO"));
                            modelo.NOM_EMPLEADO = reader.GetString(reader.GetOrdinal("NOM_EMPLEADO"));
                            modelo.DOCUMENTO = reader.GetString(reader.GetOrdinal("DOCUMENTO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
    }
}
