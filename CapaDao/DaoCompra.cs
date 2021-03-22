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
    public class DaoCompra
    {
        public DOC_COMPRA GetData(SqlConnection con, string idSucursal, string idUsuario)
        {
            DOC_COMPRA modelo = null;
            List<TIPO_DOCUMENTO> listaDocumentos = null;
            List<TIPO_COMPROBANTE> listaComprobantes = null;
            List<MONEDA> listaMonedas = null;
            List<TIPO_PAGO> listaTipPag = null;
            List<TIPO_CONDICION_PAGO> listaTipCon = null;
            List<ESTADO> listaEstados = null;
            decimal tasIgv = 0;

            using (SqlCommand cmd = new SqlCommand("PA_MANT_COMPRAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaDocumentos = new List<TIPO_DOCUMENTO>();
                        while (reader.Read())
                        {
                            listaDocumentos.Add(new TIPO_DOCUMENTO()
                            {
                                ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO")),
                                NOM_TIPO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NOM_TIPO_DOCUMENTO")),
                                ABREVIATURA = reader.GetString(reader.GetOrdinal("ABREVIATURA")),
                                FLG_RUC = reader.GetBoolean(reader.GetOrdinal("FLG_RUC")),
                                MAX_DIGITOS = reader.GetInt32(reader.GetOrdinal("MAX_DIGITOS")),
                            });
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaComprobantes = new List<TIPO_COMPROBANTE>();
                            while (reader.Read())
                            {
                                listaComprobantes.Add(new TIPO_COMPROBANTE()
                                {
                                    ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE")),
                                    NOM_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("NOM_TIPO_COMPROBANTE")),
                                    LETRA_INICIAL_SERIE_ELECTRONICA = reader.GetString(reader.GetOrdinal("LETRA_INICIAL_SERIE_ELECTRONICA")),
                                });
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaMonedas = new List<MONEDA>();
                            while (reader.Read())
                            {
                                listaMonedas.Add(new MONEDA()
                                {
                                    ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA")),
                                    NOM_MONEDA = reader.GetString(reader.GetOrdinal("NOM_MONEDA")),
                                    FLG_LOCAL = reader.GetBoolean(reader.GetOrdinal("FLG_LOCAL")),
                                    SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"))
                                });
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaTipPag = new List<TIPO_PAGO>();
                            while (reader.Read())
                            {
                                listaTipPag.Add(new TIPO_PAGO()
                                {
                                    ID_TIPO_PAGO = reader.GetString(reader.GetOrdinal("ID_TIPO_PAGO")),
                                    NOM_TIPO_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_PAGO"))
                                });
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaTipCon = new List<TIPO_CONDICION_PAGO>();
                            while (reader.Read())
                            {
                                listaTipCon.Add(new TIPO_CONDICION_PAGO()
                                {
                                    ID_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO")),
                                    NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO")),
                                    FLG_EVALUA_CREDITO = reader.GetBoolean(reader.GetOrdinal("FLG_EVALUA_CREDITO"))
                                });
                            }
                        }
                    }

                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
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

                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read()){
                                tasIgv = reader.GetDecimal(reader.GetOrdinal("IGV"));
                            }
                        }
                    }

                    modelo = new DOC_COMPRA();
                    modelo.listaDocumentos = listaDocumentos;
                    modelo.listaComprobantes = listaComprobantes;
                    modelo.listaMonedas = listaMonedas;
                    modelo.listaTipPag = listaTipPag;
                    modelo.listaTipCon = listaTipCon;
                    modelo.listaEstados = listaEstados;
                    modelo.TAS_IGV = tasIgv;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public bool Register(SqlConnection con, SqlTransaction trx, DOC_COMPRA oModelo, ref string nroSerie, ref string nroComprobante)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_COMPRAS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_COMPROBANTE;
                //
                SqlParameter paramNroSerie = new SqlParameter("@NRO_SERIE", SqlDbType.VarChar,6);
                paramNroSerie.Direction = ParameterDirection.InputOutput;
                paramNroSerie.Value = string.IsNullOrEmpty(oModelo.NRO_SERIE) ? (object)DBNull.Value : oModelo.NRO_SERIE;
                cmd.Parameters.Add(paramNroSerie);
                SqlParameter paramNroDocumento = new SqlParameter("@NRO_DOCUMENTO", SqlDbType.Int);
                paramNroDocumento.Direction = ParameterDirection.InputOutput;
                paramNroDocumento.Value = oModelo.NRO_DOCUMENTO == 0 ? (object)DBNull.Value : oModelo.NRO_DOCUMENTO;
                cmd.Parameters.Add(paramNroDocumento);
                //
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = oModelo.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = oModelo.ID_MONEDA;
                cmd.Parameters.Add("@FEC_DOCUMENTO", SqlDbType.DateTime).Value = oModelo.FEC_DOCUMENTO;
                cmd.Parameters.Add("@OBS_COMPRA", SqlDbType.VarChar, 200).Value = string.IsNullOrEmpty(oModelo.OBS_COMPRA) ? (object)DBNull.Value : oModelo.OBS_COMPRA;
                cmd.Parameters.Add("@TOT_BRUTO", SqlDbType.Decimal).Value = oModelo.TOT_BRUTO == 0 ? (object)DBNull.Value : oModelo.TOT_BRUTO;
                cmd.Parameters.Add("@TOT_DESCUENTO", SqlDbType.Decimal).Value = oModelo.TOT_DESCUENTO == 0 ? (object)DBNull.Value : oModelo.TOT_DESCUENTO;
                cmd.Parameters.Add("@TAS_DESCUENTO", SqlDbType.Decimal).Value = oModelo.TAS_DESCUENTO == 0 ? (object)DBNull.Value : oModelo.TAS_DESCUENTO;
                cmd.Parameters.Add("@TAS_IGV", SqlDbType.Decimal).Value = oModelo.TAS_IGV == 0 ? (object)DBNull.Value : oModelo.TAS_IGV;
                cmd.Parameters.Add("@TOT_IMPUESTO", SqlDbType.Decimal).Value = oModelo.TOT_IMPUESTO == 0 ? (object)DBNull.Value : oModelo.TOT_IMPUESTO;
                cmd.Parameters.Add("@TOT_COMPRA", SqlDbType.Decimal).Value = oModelo.TOT_COMPRA == 0 ? (object)DBNull.Value : oModelo.TOT_COMPRA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ID_TIPO_PAGO", SqlDbType.VarChar, 3).Value = oModelo.ID_TIPO_PAGO == "-1" ? (object)DBNull.Value : oModelo.ID_TIPO_PAGO;
                cmd.Parameters.Add("@ID_TIPO_CONDICION_PAGO", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_CONDICION_PAGO == "-1" ? (object)DBNull.Value : oModelo.ID_TIPO_CONDICION_PAGO;
                cmd.Parameters.Add("@XML_ARTICULOS", SqlDbType.Xml).Value = oModelo.CADENA_ARTICULOS;
                cmd.Parameters.Add("@FLG_SIN_COSTO", SqlDbType.Bit).Value = oModelo.FLG_SIN_COSTO;
                //
                cmd.Parameters.Add("@ABONO", SqlDbType.Decimal).Value = oModelo.ABONO == 0 ? (object)DBNull.Value : oModelo.ABONO;
                cmd.Parameters.Add("@SALDO", SqlDbType.Decimal).Value = oModelo.SALDO == 0 ? (object)DBNull.Value : oModelo.SALDO;
                cmd.Parameters.Add("@FECHA_CANCELACION", SqlDbType.DateTime).Value = oModelo.FECHA_CANCELACION == "" ? (object)DBNull.Value : oModelo.FECHA_CANCELACION;

                cmd.Parameters.Add("@FLG_RETIRAR_CAJA", SqlDbType.Bit).Value = oModelo.FLG_RETIRAR_CAJA;
                cmd.Parameters.Add("@MONTO_RETIRA_CAJA", SqlDbType.Decimal).Value = oModelo.MONTO_RETIRA_CAJA == 0 ? (object)DBNull.Value : oModelo.MONTO_RETIRA_CAJA;
                cmd.Parameters.Add("@ID_CAJA_CA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(oModelo.ID_CAJA_CA) ? (object)DBNull.Value : oModelo.ID_CAJA_CA;
                cmd.Parameters.Add("@ID_USUARIO_CA", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(oModelo.ID_USUARIO_CA) ? (object)DBNull.Value : oModelo.ID_USUARIO_CA;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = oModelo.CORRELATIVO_CA == 0 ? (object)DBNull.Value : oModelo.CORRELATIVO_CA;
                cmd.Parameters.Add("@FLG_SIN_COMPROBANTE", SqlDbType.Bit).Value = oModelo.FLG_SIN_COMPROBANTE;
                
                cmd.ExecuteNonQuery();
                bExito = true;
                if (oModelo.ACCION == "INS" && (oModelo.FLG_SIN_COMPROBANTE))
                {
                    nroSerie = cmd.Parameters["@NRO_SERIE"].Value.ToString();
                    nroComprobante = cmd.Parameters["@NRO_DOCUMENTO"].Value.ToString();
                }
            }
            return bExito;
        }

        public bool Delete(SqlConnection con, SqlTransaction trx, string idSucursal, string idTipoComprobante,
    string nroSerie, int nroDocumento, string idProveedor, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_COMPRAS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = idProveedor;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public List<DOC_COMPRA_LISTADO> GetAllByFilters(SqlConnection con, string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal, int idEstado)
        {
            List<DOC_COMPRA_LISTADO> lista = null;
            DOC_COMPRA_LISTADO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_COMPRAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoComprobante) ? (object)DBNull.Value : idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = string.IsNullOrEmpty(nroSerie ) ? (object)DBNull.Value : nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento == 0 ? (object)DBNull.Value : nroDocumento;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                cmd.Parameters.Add("@ID_ESTADO", SqlDbType.Int).Value = idEstado;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<DOC_COMPRA_LISTADO>();
                        while (reader.Read())
                        {
                            modelo = new DOC_COMPRA_LISTADO();
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.DOC_PROVEEDOR = reader.GetString(reader.GetOrdinal("DOC_PROVEEDOR"));
                            modelo.NOM_PROVEEDOR = reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.TOT_COMPRA = reader.GetDecimal(reader.GetOrdinal("TOT_COMPRA"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.FLG_SIN_COSTO = reader.GetBoolean(reader.GetOrdinal("FLG_SIN_COSTO"));
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            modelo.NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO"));
                            modelo.EST_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("EST_DOCUMENTO"));
                            modelo.NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"));
                            modelo.FLG_NO_EVALUA_CREDITO = reader.GetBoolean(reader.GetOrdinal("FLG_NO_EVALUA_CREDITO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public DOC_COMPRA_INFORME GetById(SqlConnection con, string idSucursal, string idTipoComprobante, string nroSerie,
            int nroDocumento, string idProveedor)
        {
            DOC_COMPRA_INFORME modelo = null;
            List<DOC_COMPRA_DETALLE> listaDetalle = null;
            DOC_COMPRA_DETALLE detalle = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_COMPRAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = idProveedor;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new DOC_COMPRA_INFORME();
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            modelo.NOM_PROVEEDOR = reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NRO_DOCUMENTO_PROVEEDOR = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO_PROVEEDOR"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            modelo.FLG_SIN_COSTO = reader.GetBoolean(reader.GetOrdinal("FLG_SIN_COSTO"));
                            modelo.ID_TIPO_PAGO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_PAGO")) ? "-1" : reader.GetString(reader.GetOrdinal("ID_TIPO_PAGO"));
                            modelo.ID_TIPO_CONDICION_PAGO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO")) ? "-1" : reader.GetString(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO"));
                            modelo.OBS_COMPRA = reader.IsDBNull(reader.GetOrdinal("OBS_COMPRA")) ? default(string) : reader.GetString(reader.GetOrdinal("OBS_COMPRA"));
                            modelo.TOT_BRUTO = reader.GetDecimal(reader.GetOrdinal("TOT_BRUTO"));
                            modelo.TOT_IMPUESTO = reader.GetDecimal(reader.GetOrdinal("TOT_IMPUESTO"));
                            modelo.TOT_COMPRA = reader.GetDecimal(reader.GetOrdinal("TOT_COMPRA"));
                            modelo.TOT_COMPRA_REDONDEO = reader.GetDecimal(reader.GetOrdinal("TOT_COMPRA_REDONDEO"));
                            modelo.TOT_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TOT_DESCUENTO"));
                            modelo.TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO"));
                            modelo.TAS_IGV = reader.GetDecimal(reader.GetOrdinal("TAS_IGV"));
                            modelo.EST_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("EST_DOCUMENTO"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.NOM_TIPO_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_PAGO"));
                            modelo.NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO"));
                            modelo.TEL_PROVEEDOR = reader.GetString(reader.GetOrdinal("TEL_PROVEEDOR"));
                        }
                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                listaDetalle = new List<DOC_COMPRA_DETALLE>();
                                while (reader.Read())
                                {
                                    detalle = new DOC_COMPRA_DETALLE();
                                    detalle.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                                    detalle.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                                    detalle.PRECIO_ARTICULO = reader.GetDecimal(reader.GetOrdinal("PRECIO_ARTICULO"));
                                    detalle.PRECIO_COMPRA = reader.GetDecimal(reader.GetOrdinal("PRECIO_COMPRA"));
                                    detalle.ID_UM = reader.GetString(reader.GetOrdinal("ID_UM"));
                                    detalle.NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM"));
                                    detalle.CANTIDAD = reader.GetDecimal(reader.GetOrdinal("CANTIDAD"));
                                    detalle.TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO"));
                                    detalle.IMPORTE = reader.GetDecimal(reader.GetOrdinal("IMPORTE"));
                                    detalle.NRO_FACTOR = reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR"));
                                    listaDetalle.Add(detalle);
                                }
                                modelo.detalle = listaDetalle;
                            }
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
