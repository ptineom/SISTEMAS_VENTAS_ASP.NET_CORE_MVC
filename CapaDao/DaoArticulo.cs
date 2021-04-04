using System.Collections.Generic;
using Entidades;
using System.Data;
using System;
using System.Data.SqlClient;

namespace CapaDao
{
    public class DaoArticulo
    {
        public List<ARTICULO> GetAllByFilters(SqlConnection con, string accion, string idSucursal, string tipoFiltro, string filtro)
        {
            List<ARTICULO> lista = null;
            ARTICULO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_ARTICULO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = accion;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@TIPO_FILTRO", SqlDbType.VarChar, 20).Value = tipoFiltro;
                cmd.Parameters.Add("@FILTRO", SqlDbType.VarChar, 160).Value = filtro;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<ARTICULO>();
                        while (reader.Read())
                        {
                            modelo = new ARTICULO();
                            modelo.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                            modelo.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            modelo.NOM_MARCA = reader.IsDBNull(reader.GetOrdinal("NOM_MARCA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            modelo.NOM_GRUPO = reader.IsDBNull(reader.GetOrdinal("NOM_GRUPO")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_GRUPO"));
                            modelo.NOM_FAMILIA = reader.IsDBNull(reader.GetOrdinal("NOM_FAMILIA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_FAMILIA"));
                            modelo.CODIGO_BARRA = reader.IsDBNull(reader.GetOrdinal("CODIGO_BARRA")) ? string.Empty : reader.GetString(reader.GetOrdinal("CODIGO_BARRA"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.PRECIO_VENTA_FINAL = reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA_FINAL"));
                            modelo.PRECIO_COMPRA = reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA"));
                            modelo.STOCK_ACTUAL = reader.GetDecimal(reader.GetOrdinal("STOCK_ACTUAL"));
                            modelo.STOCK_MINIMO = reader.GetDecimal(reader.GetOrdinal("STOCK_MINIMO"));
                            modelo.ID_UM = reader.IsDBNull(reader.GetOrdinal("ID_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("ID_UM"));
                            modelo.NOM_UM = reader.IsDBNull(reader.GetOrdinal("NOM_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_UM"));
                            modelo.NRO_FACTOR = reader.IsDBNull(reader.GetOrdinal("NRO_FACTOR")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR"));
                            modelo.FOTO1 = reader.IsDBNull(reader.GetOrdinal("FOTO1")) ? string.Empty : reader.GetString(reader.GetOrdinal("FOTO1"));
                            modelo.FOTO2 = reader.IsDBNull(reader.GetOrdinal("FOTO2")) ? string.Empty : reader.GetString(reader.GetOrdinal("FOTO2"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<ARTICULO> GetAllByFiltersHelper(SqlConnection con, string accion, string idSucursal, string tipoFiltro, string filtro, bool flgCompra)
        {
            List<ARTICULO> lista = null;
            ARTICULO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_ARTICULO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = accion;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@TIPO_FILTRO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(tipoFiltro) ? (object)DBNull.Value : tipoFiltro;
                cmd.Parameters.Add("@FILTRO", SqlDbType.VarChar, 160).Value = filtro;
                cmd.Parameters.Add("@FLG_COMPRA", SqlDbType.Bit).Value = flgCompra;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<ARTICULO>();
                        while (reader.Read())
                        {
                            modelo = new ARTICULO();
                            modelo.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                            modelo.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            modelo.CODIGO_BARRA = reader.IsDBNull(reader.GetOrdinal("CODIGO_BARRA")) ? string.Empty : reader.GetString(reader.GetOrdinal("CODIGO_BARRA"));
                            //Precio de venta final aplicado igv y dscto.
                            modelo.PRECIO_VENTA_FINAL = reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA_FINAL"));
                            //Precio de venta final aplicado solo el igv sin dscto
                            modelo.PRECIO_VENTA = reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA"));
                            //Precio base.
                            modelo.PRECIO_BASE = reader.GetDecimal(reader.GetOrdinal("PRECIO_BASE"));
                            modelo.PRECIO_COMPRA = reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA"));
                            modelo.STOCK_ACTUAL = reader.GetDecimal(reader.GetOrdinal("STOCK_ACTUAL"));
                            modelo.STOCK_MINIMO = reader.GetDecimal(reader.GetOrdinal("STOCK_MINIMO"));
                            modelo.DESCUENTO1 = reader.IsDBNull(reader.GetOrdinal("DESCUENTO1")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("DESCUENTO1"));
                            modelo.ID_UM = reader.IsDBNull(reader.GetOrdinal("ID_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("ID_UM"));
                            modelo.NOM_UM = reader.IsDBNull(reader.GetOrdinal("NOM_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_UM"));
                            modelo.NRO_FACTOR = reader.IsDBNull(reader.GetOrdinal("NRO_FACTOR")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR"));
                            modelo.JSON_UM = reader.IsDBNull(reader.GetOrdinal("JSON_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("JSON_UM"));
                            modelo.NOM_MARCA = reader.IsDBNull(reader.GetOrdinal("NOM_MARCA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool Register(SqlConnection con, SqlTransaction trx, ARTICULO oModelo, ref string idArticulo, ref string jsonFotos,
            ref bool flgMismaFoto)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_ARTICULO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                if (oModelo.ACCION == "INS")
                {
                    cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Direction = ParameterDirection.InputOutput;
                }
                else
                {
                    cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Value = oModelo.ID_ARTICULO;
                }
                cmd.Parameters.Add("@NOM_ARTICULO", SqlDbType.VarChar, 150).Value = oModelo.NOM_ARTICULO;
                cmd.Parameters.Add("@NOM_VENTA", SqlDbType.VarChar, 150).Value = string.IsNullOrEmpty(oModelo.NOM_VENTA) ? (object)DBNull.Value : oModelo.NOM_VENTA;
                cmd.Parameters.Add("@FLG_IMPORTADO", SqlDbType.Bit).Value = oModelo.FLG_IMPORTADO;
                cmd.Parameters.Add("@PRECIO_COMPRA", SqlDbType.Decimal).Value = oModelo.PRECIO_COMPRA == 0 ? (object)DBNull.Value : oModelo.PRECIO_COMPRA;
                cmd.Parameters.Add("@PRECIO_VENTA", SqlDbType.Decimal).Value = oModelo.PRECIO_VENTA == 0 ? (object)DBNull.Value : oModelo.PRECIO_VENTA;
                cmd.Parameters.Add("@FLG_INACTIVO", SqlDbType.Bit).Value = oModelo.FLG_INACTIVO;
                cmd.Parameters.Add("@ID_MARCA", SqlDbType.Int).Value = oModelo.ID_MARCA == -1 ? (object)DBNull.Value : oModelo.ID_MARCA;
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = oModelo.ID_GRUPO == "-1" ? (object)DBNull.Value : oModelo.ID_GRUPO;
                cmd.Parameters.Add("@ID_FAMILIA", SqlDbType.VarChar, 3).Value = oModelo.ID_FAMILIA == "-1" ? (object)DBNull.Value : oModelo.ID_FAMILIA;
                cmd.Parameters.Add("@CODIGO_BARRA", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(oModelo.CODIGO_BARRA) ? (object)DBNull.Value : oModelo.CODIGO_BARRA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@STOCK_MINIMO", SqlDbType.Decimal).Value = oModelo.STOCK_MINIMO == 0 ? (object)DBNull.Value : oModelo.STOCK_MINIMO;
                cmd.Parameters.Add("@FLG_MISMA_FOTO", SqlDbType.Bit).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@JSON_UM", SqlDbType.VarChar, -1).Value = string.IsNullOrEmpty(oModelo.JSON_UM) ? (object)DBNull.Value : oModelo.JSON_UM;
                cmd.Parameters.Add("@JSON_FOTOS", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@JSON_SUCURSAL", SqlDbType.VarChar, -1).Value = string.IsNullOrEmpty(oModelo.JSON_SUCURSAL) ? (object)DBNull.Value : oModelo.JSON_SUCURSAL;
                cmd.Parameters.Add("@NOM_FOTO", SqlDbType.VarChar, 30).Value = string.IsNullOrEmpty(oModelo.NOM_FOTO) ? (object)DBNull.Value : oModelo.NOM_FOTO;
                cmd.ExecuteNonQuery();
                bExito = true;

                if (oModelo.ACCION == "INS")
                {
                    idArticulo = cmd.Parameters["@ID_ARTICULO"].Value.ToString();
                }
                else if (oModelo.ACCION == "UPD")
                {
                    jsonFotos = cmd.Parameters["@JSON_FOTOS"].Value.ToString();
                    flgMismaFoto = Convert.ToBoolean(cmd.Parameters["@FLG_MISMA_FOTO"].Value);
                }
            }
            return bExito;
        }

        public bool Delete(SqlConnection con, SqlTransaction trx, string idArticulo, string idUsuario, ref string xmlFotos)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_ARTICULO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Value = idArticulo;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@JSON_FOTOS", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                bExito = true;

                if (cmd.Parameters["@JSON_FOTOS"].Value != null)
                    xmlFotos = cmd.Parameters["@JSON_FOTOS"].Value.ToString();
            }
            return bExito;
        }

        public ARTICULO GetById(SqlConnection con, string idSucursal, string idArticulo)
        {
            ARTICULO modelo = null;
            List<ARTICULO_UM> listaArticuloUm = null;
            List<SUCURSAL> listaSucursal = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_ARTICULO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Value = idArticulo;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new ARTICULO();
                            modelo.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                            modelo.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            modelo.NOM_VENTA = reader.IsDBNull(reader.GetOrdinal("NOM_VENTA")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_VENTA"));
                            modelo.CODIGO_BARRA = reader.IsDBNull(reader.GetOrdinal("CODIGO_BARRA")) ? default(string) : reader.GetString(reader.GetOrdinal("CODIGO_BARRA"));
                            modelo.ID_MARCA = reader.IsDBNull(reader.GetOrdinal("ID_MARCA")) ? -1 : reader.GetInt32(reader.GetOrdinal("ID_MARCA"));
                            modelo.NOM_MARCA = reader.IsDBNull(reader.GetOrdinal("NOM_MARCA")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            modelo.ID_GRUPO = reader.IsDBNull(reader.GetOrdinal("ID_GRUPO")) ? "-1" : reader.GetString(reader.GetOrdinal("ID_GRUPO"));
                            modelo.ID_FAMILIA = reader.IsDBNull(reader.GetOrdinal("ID_FAMILIA")) ? "-1" : reader.GetString(reader.GetOrdinal("ID_FAMILIA"));
                            modelo.PRECIO_COMPRA = reader.IsDBNull(reader.GetOrdinal("PRECIO_COMPRA")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA"));
                            modelo.PRECIO_VENTA = reader.IsDBNull(reader.GetOrdinal("PRECIO_VENTA")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA"));
                            modelo.STOCK_MINIMO = reader.IsDBNull(reader.GetOrdinal("STOCK_MINIMO")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("STOCK_MINIMO"));
                            modelo.FLG_IMPORTADO = reader.GetBoolean(reader.GetOrdinal("FLG_IMPORTADO"));
                            modelo.FLG_INACTIVO = reader.GetBoolean(reader.GetOrdinal("FLG_INACTIVO"));
                            modelo.FOTO1 = reader.IsDBNull(reader.GetOrdinal("FOTO1")) ? string.Empty : reader.GetString(reader.GetOrdinal("FOTO1"));
                            modelo.FOTO2 = reader.IsDBNull(reader.GetOrdinal("FOTO2")) ? string.Empty : reader.GetString(reader.GetOrdinal("FOTO2"));
                        }

                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                listaArticuloUm = new List<ARTICULO_UM>();
                                while (reader.Read())
                                {
                                    listaArticuloUm.Add(new ARTICULO_UM()
                                    {
                                        ID_UM = reader.GetString(reader.GetOrdinal("ID_UM")),
                                        NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM")),
                                        NRO_FACTOR = reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR")),
                                        NRO_ORDEN = reader.GetInt32(reader.GetOrdinal("NRO_ORDEN")),
                                        FLG_PROMOCION = reader.GetBoolean(reader.GetOrdinal("FLG_PROMOCION")),
                                        DESCUENTO1 = reader.IsDBNull(reader.GetOrdinal("DESCUENTO1")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("DESCUENTO1")),
                                        FEC_INICIO_PROMOCION = reader.IsDBNull(reader.GetOrdinal("FEC_INICIO_PROMOCION")) ? string.Empty : reader.GetString(reader.GetOrdinal("FEC_INICIO_PROMOCION")),
                                        FEC_FINAL_PROMOCION = reader.IsDBNull(reader.GetOrdinal("FEC_FINAL_PROMOCION")) ? string.Empty : reader.GetString(reader.GetOrdinal("FEC_FINAL_PROMOCION")),
                                        PRECIO_VENTA = reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA")),
                                        PRECIO_VENTA_FINAL = reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA_FINAL"))
                                    });
                                };
                                modelo.listaArticuloUm = listaArticuloUm;
                            };
                        };
                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                listaSucursal = new List<SUCURSAL>();
                                while (reader.Read())
                                {
                                    listaSucursal.Add(new SUCURSAL()
                                    {
                                        FLG_EN_USO = reader.GetBoolean(reader.GetOrdinal("FLG_EN_USO")),
                                        NOM_ALMACEN = reader.GetString(reader.GetOrdinal("NOM_ALMACEN")),
                                        STOCK_ACTUAL = reader.GetDecimal(reader.GetOrdinal("STOCK_ACTUAL")),
                                        ID_SUCURSAL = reader.GetString(reader.GetOrdinal("ID_SUCURSAL")),
                                        NOM_SUCURSAL = reader.GetString(reader.GetOrdinal("NOM_SUCURSAL")),
                                        DIRECCION = reader.GetString(reader.GetOrdinal("DIRECCION_SUCURSAL")),
                                        NOM_UBIGEO = reader.GetString(reader.GetOrdinal("UBIGEO_SUCURSAL")),
                                    });
                                };
                                modelo.listaSucursal = listaSucursal;
                            };
                        };
                    };
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }
    }
}
