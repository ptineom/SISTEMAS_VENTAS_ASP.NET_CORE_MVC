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
    public class DaoSucursal
    {
        public List<SUCURSAL> listaSucursales(SqlConnection con)
        {
            List<SUCURSAL> lista = null;
            SUCURSAL modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SEDE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<SUCURSAL>();
                        while (reader.Read())
                        {
                            modelo = new SUCURSAL();
                            modelo.ID_SUCURSAL = reader.GetString(reader.GetOrdinal("ID_SUCURSAL"));
                            modelo.NOM_SUCURSAL = reader.GetString(reader.GetOrdinal("NOM_SUCURSAL"));
                            modelo.DIRECCION = reader.GetString(reader.GetOrdinal("DIRECCION"));
                            modelo.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            modelo.NOM_UBIGEO = reader.GetString(reader.GetOrdinal("NOM_UBIGEO"));
                            modelo.FLG_PRINCIPAL = reader.GetBoolean(reader.GetOrdinal("FLG_PRINCIPAL"));
                            //modelo.TELEFONO_CELULAR1 = reader.IsDBNull(reader.GetOrdinal("TELEFONO_CELULAR1")) ? default(string) : reader.GetString(reader.GetOrdinal("TELEFONO_CELULAR1"));
                            //modelo.TELEFONO_CASA1 = reader.IsDBNull(reader.GetOrdinal("TELEFONO_CASA1")) ? default(string) : reader.GetString(reader.GetOrdinal("TELEFONO_CASA1"));
                            modelo.EMAIL = reader.GetString(reader.GetOrdinal("EMAIL"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.FLG_INICIAR_FACTURACION_ELECTRONICA = reader.GetBoolean(reader.GetOrdinal("FLG_INICIAR_FACTURACION_ELECTRONICA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarSucursal(SqlConnection con, SqlTransaction trx, SUCURSAL oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SEDE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL == "" ? (object)DBNull.Value : oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_EMPRESA", SqlDbType.VarChar, 2).Value =  oModelo.ID_EMPRESA;
                cmd.Parameters.Add("@NOM_SUCURSAL", SqlDbType.VarChar, 150).Value = oModelo.NOM_SUCURSAL;
                cmd.Parameters.Add("@DIRECCION", SqlDbType.VarChar, 200).Value = oModelo.DIRECCION == "" ? (object)DBNull.Value : oModelo.DIRECCION;
                cmd.Parameters.Add("@ID_UBIGEO", SqlDbType.VarChar, 6).Value = oModelo.ID_UBIGEO == "-1" ? (object)DBNull.Value : oModelo.ID_UBIGEO;
                cmd.Parameters.Add("@EMAIL", SqlDbType.VarChar, 100).Value = oModelo.EMAIL == "" ? (object)DBNull.Value : oModelo.EMAIL;
                //cmd.Parameters.Add("@XML_TELEFONOS", SqlDbType.Xml).Value = oModelo.XML_TELEFONOS == "" ? (object)DBNull.Value : oModelo.XML_TELEFONOS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@FLG_PRINCIPAL", SqlDbType.Bit).Value = oModelo.FLG_PRINCIPAL;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = oModelo.FLG_INACTIVO;
                cmd.Parameters.Add("@FLG_INICIAR_FACTURACION_ELECTRONICA", SqlDbType.Bit).Value = oModelo.FLG_INICIAR_FACTURACION_ELECTRONICA;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public void obtenerSucursalPorCodigo(SqlConnection con,string idSucursal, ref List<UBIGEO> listaProvincia, ref List<UBIGEO> listaDistrito)
        {
            UBIGEO oProvincia = null;
            UBIGEO oDistrito = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SEDE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
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
                    if (reader.NextResult())
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
                }
                reader.Close();
                reader.Dispose();
            }
        }
        public bool anularSucursal(SqlConnection con, SqlTransaction trx, string idSucursal, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SEDE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        
    }
}
