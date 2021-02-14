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
    public class DaoEmpresa
    {
        public bool grabarEmpresa(SqlConnection con, SqlTransaction trx, EMPRESA oModelo, ref string idEmpresa, ref string xmlFotos)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPRESA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                if (oModelo.ACCION == "INS")
                {
                    cmd.Parameters.Add("@ID_EMPRESA", SqlDbType.VarChar, 2).Direction = ParameterDirection.InputOutput;
                }
                else
                {
                    cmd.Parameters.Add("@ID_EMPRESA", SqlDbType.VarChar, 2).Value = oModelo.ID_EMPRESA;
                }
                cmd.Parameters.Add("@NOM_EMPRESA", SqlDbType.VarChar, 150).Value = oModelo.NOM_EMPRESA;
                cmd.Parameters.Add("@NUMERO_RUC", SqlDbType.VarChar, 12).Value = oModelo.NUMERO_RUC == "" ? (object)DBNull.Value : oModelo.NUMERO_RUC;
                cmd.Parameters.Add("@IGV", SqlDbType.Decimal).Value = oModelo.IGV == 0 ? (object)DBNull.Value : oModelo.IGV;
                cmd.Parameters.Add("@STOCK_MINIMO", SqlDbType.Decimal).Value = oModelo.STOCK_MINIMO == 0 ? (object)DBNull.Value : oModelo.STOCK_MINIMO;
                cmd.Parameters.Add("@MONTO_BOLETA_OBLIGATORIO_CLIENTE", SqlDbType.Decimal).Value = oModelo.MONTO_BOLETA_OBLIGATORIO_CLIENTE == 0 ? (object)DBNull.Value : oModelo.MONTO_BOLETA_OBLIGATORIO_CLIENTE;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@XML_FOTOS", SqlDbType.Xml).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                bExito = true;
                if (oModelo.ACCION == "INS")
                {
                    idEmpresa = cmd.Parameters["@ID_EMPRESA"].Value.ToString();
                }
                else if (oModelo.ACCION == "UPD")
                {
                    xmlFotos = cmd.Parameters["@XML_FOTOS"].Value.ToString();
                }
            }
            return bExito;
        }

        public EMPRESA obtenerEmpresa(SqlConnection con, string idSucursal, bool flgMostrarSucursales = false)
        {
            EMPRESA modelo = null;
            List<SUCURSAL> sucursales = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_EMPRESA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idSucursal) ? (object)DBNull.Value: idSucursal;
                cmd.Parameters.Add("@FLG_MOSTRAR_SUCURSALES", SqlDbType.Bit).Value = flgMostrarSucursales;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new EMPRESA();
                            modelo.ID_EMPRESA = reader.GetString(reader.GetOrdinal("ID_EMPRESA"));
                            modelo.NOM_EMPRESA = reader.GetString(reader.GetOrdinal("NOM_EMPRESA"));
                            modelo.NUMERO_RUC = reader.GetString(reader.GetOrdinal("NUMERO_RUC"));
                            modelo.IGV = reader.IsDBNull(reader.GetOrdinal("IGV")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("IGV"));
                            modelo.FOTO1 = reader.IsDBNull(reader.GetOrdinal("FOTO1")) ? string.Empty : reader.GetString(reader.GetOrdinal("FOTO1"));
                            modelo.FOTO2 = reader.IsDBNull(reader.GetOrdinal("FOTO2")) ? string.Empty : reader.GetString(reader.GetOrdinal("FOTO2"));
                            modelo.STOCK_MINIMO = reader.IsDBNull(reader.GetOrdinal("STOCK_MINIMO")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("STOCK_MINIMO"));
                            modelo.MONTO_BOLETA_OBLIGATORIO_CLIENTE = reader.IsDBNull(reader.GetOrdinal("MONTO_BOLETA_OBLIGATORIO_CLIENTE")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("MONTO_BOLETA_OBLIGATORIO_CLIENTE"));
                        }
                    };
                    if (flgMostrarSucursales && reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            sucursales = new List<SUCURSAL>();
                            while (reader.Read())
                            {
                                sucursales.Add(new SUCURSAL()
                                {
                                    ID_SUCURSAL = reader.GetString(reader.GetOrdinal("ID_SUCURSAL")),
                                    NOM_SUCURSAL = reader.GetString(reader.GetOrdinal("NOM_SUCURSAL")),
                                    DIRECCION = reader.GetString(reader.GetOrdinal("DIRECCION_SUCURSAL")),
                                    NOM_UBIGEO = reader.GetString(reader.GetOrdinal("UBIGEO_SUCURSAL")),
                                    JSON_TELEFONOS = reader.IsDBNull(reader.GetOrdinal("JSON_TELEFONOS")) ? string.Empty : reader.GetString(reader.GetOrdinal("JSON_TELEFONOS")),
                                    EMAIL = reader.GetString(reader.GetOrdinal("EMAIL")),
                                    FLG_PRINCIPAL = reader.GetBoolean(reader.GetOrdinal("FLG_PRINCIPAL")),
                                    FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO")),
                                    FLG_INICIAR_FACTURACION_ELECTRONICA = reader.GetBoolean(reader.GetOrdinal("FLG_INICIAR_FACTURACION_ELECTRONICA")),
                                    NOM_ALMACEN = reader.GetString(reader.GetOrdinal("NOM_ALMACEN")),
                                    
                                });
                            }
                            modelo.sucursales = sucursales;
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
