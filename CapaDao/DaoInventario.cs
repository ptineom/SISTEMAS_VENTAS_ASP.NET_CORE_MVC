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
    public class DaoInventario
    {
        public List<ARTICULO> listaArticulosInventario(SqlConnection con, string accion, string idSucursal, string nomArticulo, int idMarca,
            string procedencia, bool flgStockMinimo, bool flgSinStock, string xmlGrupos, string xmlFamilias, string idArticulo)
        {
            List<ARTICULO> lista = null;
            ARTICULO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = accion;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@NOM_ARTICULO", SqlDbType.VarChar, 150).Value = string.IsNullOrEmpty(nomArticulo) ? (object)DBNull.Value : nomArticulo;
                cmd.Parameters.Add("@ID_MARCA", SqlDbType.Int).Value = idMarca == -1 ? (object)DBNull.Value : idMarca;
                cmd.Parameters.Add("@PROCEDENCIA", SqlDbType.Char, 1).Value = procedencia;
                cmd.Parameters.Add("@FLG_STOCK_MINIMO", SqlDbType.Bit).Value = flgStockMinimo;
                cmd.Parameters.Add("@FLG_SIN_STOCK", SqlDbType.Bit).Value = flgSinStock;
                cmd.Parameters.Add("@XML_GRUPOS", SqlDbType.Xml).Value = string.IsNullOrEmpty(xmlGrupos) ? (object)DBNull.Value : xmlGrupos; ;
                cmd.Parameters.Add("@XML_FAMILIAS", SqlDbType.Xml).Value = string.IsNullOrEmpty(xmlFamilias) ? (object)DBNull.Value : xmlFamilias;
                cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(idArticulo) ? (object)DBNull.Value : idArticulo;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<ARTICULO>();
                        while (reader.Read())
                        {
                            modelo = new ARTICULO();
                            modelo.ID_ARTICULO = reader.GetString(reader.GetOrdinal("CODIGO_ARTICULO"));
                            modelo.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            modelo.NOM_MARCA = reader.IsDBNull(reader.GetOrdinal("NOM_MARCA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            modelo.NOM_GRUPO = reader.IsDBNull(reader.GetOrdinal("NOM_GRUPO")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_GRUPO"));
                            modelo.NOM_FAMILIA = reader.IsDBNull(reader.GetOrdinal("NOM_FAMILIA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_FAMILIA"));
                            modelo.NOM_UM = reader.IsDBNull(reader.GetOrdinal("NOM_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_UM"));
                            modelo.STOCK_ACTUAL = reader.GetDecimal(reader.GetOrdinal("STOCK_ACTUAL"));
                            modelo.STOCK_MINIMO = reader.GetDecimal(reader.GetOrdinal("STOCK_MINIMO"));
                            modelo.PRECIO_BASE = reader.GetDecimal(reader.GetOrdinal("PRECIO_BASE"));
                            modelo.DESCUENTO1 = reader.GetDecimal(reader.GetOrdinal("DESCUENTO1"));
                            modelo.PRECIO_VENTA_FINAL = reader.GetDecimal(reader.GetOrdinal("PRECIO_VENTA_FINAL"));
                            modelo.FLG_IMPORTADO = reader.GetBoolean(reader.GetOrdinal("FLG_IMPORTADO"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.PRECIO_COMPRA = reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public ARTICULO articuloXcodigoBarra(SqlConnection con, string accion, string idSucursal, string idArticulo, bool flgBuscarXcodBarra)
        {
            ARTICULO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = accion;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Value = idArticulo;
                cmd.Parameters.Add("@FLG_BUSCAR_X_COD_BARRA", SqlDbType.Bit).Value = flgBuscarXcodBarra;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new ARTICULO();
                            modelo.CODIGO_BARRA = reader.GetString(reader.GetOrdinal("CODIGO"));
                            modelo.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            modelo.NOM_MARCA = reader.IsDBNull(reader.GetOrdinal("NOM_MARCA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            modelo.STOCK_ACTUAL = reader.GetDecimal(reader.GetOrdinal("STOCK_ACTUAL"));
                            modelo.NOM_UM = reader.IsDBNull(reader.GetOrdinal("NOM_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_UM"));
                            modelo.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public List<INVENTARIO> listaInventario(SqlConnection con, string accion, string idSucursal, int idEstado, string fechaInicio, string fechaFinal,
            string idUsuarioInventario, string idTipoInventario)
        {
            List<INVENTARIO> lista = null;
            INVENTARIO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = accion;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@ID_ESTADO", SqlDbType.Int).Value = idEstado == -1 ? 0 : idEstado;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuarioInventario;
                cmd.Parameters.Add("@ID_TIPO_INVENTARIO", SqlDbType.VarChar, 2).Value = idTipoInventario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<INVENTARIO>();
                        while (reader.Read())
                        {
                            modelo = new INVENTARIO();
                            modelo.NRO_INVENTARIO = reader.GetInt32(reader.GetOrdinal("NRO_INVENTARIO"));
                            modelo.ID_USUARIO_INVENTARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO_INVENTARIO"));
                            modelo.FECHA_INVENTARIO = reader.GetString(reader.GetOrdinal("FECHA_INVENTARIO"));
                            modelo.ID_USUARIO_APROBACION = reader.IsDBNull(reader.GetOrdinal("ID_USUARIO_APROBACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("ID_USUARIO_APROBACION"));
                            modelo.FEC_APROBACION = reader.IsDBNull(reader.GetOrdinal("FEC_APROBACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("FEC_APROBACION"));
                            modelo.OBSERVACION = reader.IsDBNull(reader.GetOrdinal("OBSERVACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("OBSERVACION"));
                            modelo.NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"));
                            modelo.ID_ESTADO = reader.GetInt32(reader.GetOrdinal("ID_ESTADO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarInventario(SqlConnection con, SqlTransaction trx, INVENTARIO oModelo, ref int nroInventario,
            ref string idUsuarioInventario, ref string fechaInventario, ref int idEstado)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;

                SqlParameter paramNroInventario = new SqlParameter("@NRO_INVENTARIO", SqlDbType.Int);
                paramNroInventario.Direction = ParameterDirection.InputOutput;
                paramNroInventario.Value = oModelo.NRO_INVENTARIO == 0 ? (object)DBNull.Value : oModelo.NRO_INVENTARIO;
                cmd.Parameters.Add(paramNroInventario);

                SqlParameter paramIdUsuarioInventario = new SqlParameter("@ID_USUARIO_INVENTARIO", SqlDbType.VarChar, 20);
                paramIdUsuarioInventario.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramIdUsuarioInventario);

                SqlParameter paramFechaInventario = new SqlParameter("@FECHA_INVENTARIO", SqlDbType.DateTime);
                paramFechaInventario.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramFechaInventario);

                SqlParameter paramIdEstado = new SqlParameter("@ID_ESTADO", SqlDbType.Int);
                paramIdEstado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramIdEstado);

                cmd.Parameters.Add("@OBSERVACION", SqlDbType.VarChar, 500).Value = string.IsNullOrEmpty(oModelo.OBSERVACION) ? (object)DBNull.Value : oModelo.OBSERVACION;
                cmd.Parameters.Add("@XML_ARTICULOS", SqlDbType.Xml).Value = oModelo.CADENA_ARTICULOS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ID_TIPO_INVENTARIO", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_INVENTARIO;

                cmd.ExecuteNonQuery();
                bExito = true;

                nroInventario = Convert.ToInt32(cmd.Parameters["@NRO_INVENTARIO"].Value.ToString());
                idUsuarioInventario = cmd.Parameters["@ID_USUARIO_INVENTARIO"].Value.ToString();
                fechaInventario = Convert.ToDateTime(cmd.Parameters["@FECHA_INVENTARIO"].Value).ToShortDateString();
                idEstado = Convert.ToInt32(cmd.Parameters["@ID_ESTADO"].Value);
            }
            return bExito;
        }

        public bool eliminarInventario(SqlConnection con, SqlTransaction trx, string idSucursal, int nroInventario, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "IDE";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@NRO_INVENTARIO", SqlDbType.Int).Value = nroInventario;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public INVENTARIO inventarioXcodigo(SqlConnection con, string idSucursal, int nroInventario)
        {
            INVENTARIO modelo = null;
            List<INVENTARIO_DETALLE> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "IXC";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@NRO_INVENTARIO", SqlDbType.Int).Value = nroInventario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new INVENTARIO();
                            modelo.NRO_INVENTARIO = reader.GetInt32(reader.GetOrdinal("NRO_INVENTARIO"));
                            modelo.ID_USUARIO_INVENTARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO_INVENTARIO"));
                            modelo.FECHA_INVENTARIO = reader.GetString(reader.GetOrdinal("FECHA_INVENTARIO"));
                            modelo.ID_USUARIO_APROBACION = reader.IsDBNull(reader.GetOrdinal("ID_USUARIO_APROBACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("ID_USUARIO_APROBACION"));
                            modelo.FEC_APROBACION = reader.IsDBNull(reader.GetOrdinal("FEC_APROBACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("FEC_APROBACION"));
                            modelo.NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"));
                            modelo.OBSERVACION = reader.IsDBNull(reader.GetOrdinal("OBSERVACION")) ? string.Empty : reader.GetString(reader.GetOrdinal("OBSERVACION"));
                            modelo.ID_ESTADO = reader.GetInt32(reader.GetOrdinal("ID_ESTADO"));
                        }
                    }
                    if (reader.NextResult())
                    {
                        lista = new List<INVENTARIO_DETALLE>();
                        while (reader.Read())
                        {
                            lista.Add(new INVENTARIO_DETALLE()
                            {
                                CODIGO = reader.GetString(reader.GetOrdinal("CODIGO")),
                                NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO")),
                                NOM_MARCA = reader.GetString(reader.GetOrdinal("NOM_MARCA")),
                                NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM")),
                                STOCK_VIRTUAL = reader.GetDecimal(reader.GetOrdinal("STOCK_VIRTUAL")),
                                STOCK_FISICO = reader.GetDecimal(reader.GetOrdinal("STOCK_FISICO")),
                                DIFERENCIA = reader.GetDecimal(reader.GetOrdinal("DIFERENCIA")),
                                ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"))
                            });
                        }
                        modelo.listaDetalle = lista;
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public void combosInventario(SqlConnection con, ref List<GRUPO> listaGrupos, ref List<ESTADO> listaEstados)
        {
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaGrupos = new List<GRUPO>();
                        while (reader.Read())
                        {
                            listaGrupos.Add(new GRUPO()
                            {
                                ID_GRUPO = reader.GetString(reader.GetOrdinal("ID_GRUPO")),
                                NOM_GRUPO = reader.GetString(reader.GetOrdinal("NOM_GRUPO"))
                            });
                        }
                    }
                    if (reader.NextResult())
                    {
                        listaEstados = new List<ESTADO>();
                        while (reader.Read())
                        {
                            listaEstados.Add(new ESTADO()
                            {
                                ID_ESTADO = reader.GetInt32(reader.GetOrdinal("ID_ESTADO")),
                                NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"))
                            });
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
        }

        public bool aprobarInventario(SqlConnection con, SqlTransaction trx, string idSucursal, int nroInventario, string idUsuario,
            ref string idUsuarioAprobacion, ref string fechaAprobacion, ref int idEstado)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "IAP";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@NRO_INVENTARIO", SqlDbType.Int).Value = nroInventario;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;

                SqlParameter paramIdUsuarioInventario = new SqlParameter("@ID_USUARIO_APROBACION", SqlDbType.VarChar, 20);
                paramIdUsuarioInventario.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramIdUsuarioInventario);

                SqlParameter paramFechaInventario = new SqlParameter("@FECHA_APROBACION", SqlDbType.DateTime);
                paramFechaInventario.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramFechaInventario);

                SqlParameter paramIdEstado = new SqlParameter("@ID_ESTADO", SqlDbType.Int);
                paramIdEstado.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(paramIdEstado);

                cmd.ExecuteNonQuery();
                bExito = true;

                idUsuarioAprobacion = cmd.Parameters["@ID_USUARIO_APROBACION"].Value.ToString();
                fechaAprobacion = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(cmd.Parameters["@FECHA_APROBACION"].Value));
                idEstado = Convert.ToInt32(cmd.Parameters["@ID_ESTADO"].Value);
            }
            return bExito;
        }

        public List<ARTICULO> listaInventarioManual(SqlConnection con, string accion, string idSucursal,
            string xmlGrupos, string xmlFamilias, bool flgImprimirCodBarra)
        {
            List<ARTICULO> lista = null;
            ARTICULO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = accion;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@XML_GRUPOS", SqlDbType.Xml).Value = string.IsNullOrEmpty(xmlGrupos) ? (object)DBNull.Value : xmlGrupos; ;
                cmd.Parameters.Add("@XML_FAMILIAS", SqlDbType.Xml).Value = string.IsNullOrEmpty(xmlFamilias) ? (object)DBNull.Value : xmlFamilias;
                cmd.Parameters.Add("@FLG_IMPRIMIR_COD_BARRA", SqlDbType.Bit).Value = flgImprimirCodBarra;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<ARTICULO>();
                        while (reader.Read())
                        {
                            modelo = new ARTICULO();
                            modelo.CODIGO_BARRA = reader.IsDBNull(reader.GetOrdinal("CODIGO_BARRA")) ? string.Empty : reader.GetString(reader.GetOrdinal("CODIGO_BARRA"));
                            modelo.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                            modelo.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            modelo.NOM_MARCA = reader.IsDBNull(reader.GetOrdinal("NOM_MARCA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_MARCA"));
                            modelo.NOM_GRUPO = reader.IsDBNull(reader.GetOrdinal("NOM_GRUPO")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_GRUPO"));
                            modelo.NOM_FAMILIA = reader.IsDBNull(reader.GetOrdinal("NOM_FAMILIA")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_FAMILIA"));
                            modelo.NOM_UM = reader.IsDBNull(reader.GetOrdinal("NOM_UM")) ? string.Empty : reader.GetString(reader.GetOrdinal("NOM_UM"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<KARDEX> kardex(SqlConnection con, string idSucursal, string fechaInicio, string fechaFinal,
    string xmlGrupos, string xmlFamilias, string idArticulo)
        {
            List<KARDEX> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_INVENTARIO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 4).Value = idSucursal;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                cmd.Parameters.Add("@XML_GRUPOS", SqlDbType.Xml).Value = string.IsNullOrEmpty(xmlGrupos) ? (object)DBNull.Value : xmlGrupos; ;
                cmd.Parameters.Add("@XML_FAMILIAS", SqlDbType.Xml).Value = string.IsNullOrEmpty(xmlFamilias) ? (object)DBNull.Value : xmlFamilias;
                cmd.Parameters.Add("@ID_ARTICULO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(idArticulo) ? (object)DBNull.Value : idArticulo;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<KARDEX>();
                        while (reader.Read())
                        {
                            lista.Add(new KARDEX() { 
                            ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO")),
                            NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO")),
                            NOM_CPTO_MOVIMIENTO = reader.GetString(reader.GetOrdinal("NOM_CPTO_MOVIMIENTO")),
                            DOCUMENTO = reader.IsDBNull(reader.GetOrdinal("DOCUMENTO")) ? string.Empty : reader.GetString(reader.GetOrdinal("DOCUMENTO")),
                            FECHA_MOVIMIENTO = reader.GetDateTime(reader.GetOrdinal("FECHA_MOVIMIENTO")),
                            CAN_SALDO_INICIAL = reader.IsDBNull(reader.GetOrdinal("CAN_SALDO_INICIAL")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("CAN_SALDO_INICIAL")),
                            CAN_INGRESO = reader.IsDBNull(reader.GetOrdinal("CAN_INGRESO")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("CAN_INGRESO")),
                            CAN_SALIDA = reader.IsDBNull(reader.GetOrdinal("CAN_SALIDA")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("CAN_SALIDA")),
                            CAN_SALDO_FINAL = reader.IsDBNull(reader.GetOrdinal("CAN_SALDO_FINAL")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("CAN_SALDO_FINAL")),
                            NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM")),
                            STOCK_ACTUAL = reader.IsDBNull(reader.GetOrdinal("STOCK_ACTUAL")) ? default(decimal) : reader.GetDecimal(reader.GetOrdinal("STOCK_ACTUAL")),
                            });
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
