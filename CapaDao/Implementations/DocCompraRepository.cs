using CapaDao.Contracts;
using Entidades;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Implementations
{
    public class DocCompraRepository : IDocCompraRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_COMPRAS";
        public DocCompraRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<bool> DeleteAsync(DOC_COMPRA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = obj.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar, 6).Value = obj.NRO_SERIE;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = obj.NRO_DOCUMENTO;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = obj.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }

        public async Task<List<DOC_COMPRA_LISTADO>> GetAllAsync(DOC_COMPRA obj)
        {
            List<DOC_COMPRA_LISTADO> lista = null;
            DOC_COMPRA_LISTADO modelo = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_TIPO_COMPROBANTE) ? (object)DBNull.Value : obj.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar, 6).Value = string.IsNullOrEmpty(obj.NRO_SERIE) ? (object)DBNull.Value : obj.NRO_SERIE;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = obj.NRO_DOCUMENTO == 0 ? (object)DBNull.Value : obj.NRO_DOCUMENTO;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_PROVEEDOR) ? (object)DBNull.Value : obj.ID_PROVEEDOR;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.DateTime).Value = DataUtility.ObjectToDateTimeNull(obj.FECHA_INICIAL); ;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.DateTime).Value = DataUtility.ObjectToDateTimeNull(obj.FECHA_FINAL); ;
                cmd.Parameters.Add("@ID_ESTADO", SqlDbType.Int).Value = obj.EST_DOCUMENTO == 0 ? (object)DBNull.Value : obj.EST_DOCUMENTO;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
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
                            modelo.FEC_DOCUMENTO = reader.GetDateTime(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            modelo.NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO"));
                            modelo.EST_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("EST_DOCUMENTO"));
                            modelo.NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"));
                            modelo.FLG_EVALUA_CREDITO = reader.GetBoolean(reader.GetOrdinal("FLG_EVALUA_CREDITO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public async Task<DOC_COMPRA> GetByIdAsync(DOC_COMPRA obj)
        {
            DOC_COMPRA docCompra = null;
            List<DOC_COMPRA_DETALLE> listDocCompraDetalle = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = obj.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar, 6).Value = obj.NRO_SERIE;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = obj.NRO_DOCUMENTO;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = obj.ID_PROVEEDOR;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            docCompra = new DOC_COMPRA();
                            docCompra.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            docCompra.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            docCompra.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            docCompra.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            docCompra.ID_PROVEEDOR = reader.GetString(reader.GetOrdinal("ID_PROVEEDOR"));
                            docCompra.NOM_PROVEEDOR = reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            docCompra.DIR_PROVEEDOR = reader.IsDBNull(reader.GetOrdinal("DIR_PROVEEDOR")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_PROVEEDOR"));
                            docCompra.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            docCompra.NRO_DOCUMENTO_PROVEEDOR = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO_PROVEEDOR"));
                            docCompra.FEC_DOCUMENTO = reader.GetDateTime(reader.GetOrdinal("FEC_DOCUMENTO"));
                            docCompra.FEC_VENCIMIENTO = reader.GetDateTime(reader.GetOrdinal("FEC_VENCIMIENTO"));
                            docCompra.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            docCompra.ID_TIPO_PAGO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_PAGO")) ? "" : reader.GetString(reader.GetOrdinal("ID_TIPO_PAGO"));
                            docCompra.ID_TIPO_CONDICION_PAGO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO")) ? "" : reader.GetString(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO"));
                            docCompra.OBS_COMPRA = reader.IsDBNull(reader.GetOrdinal("OBS_COMPRA")) ? default(string) : reader.GetString(reader.GetOrdinal("OBS_COMPRA"));
                            docCompra.TOT_BRUTO = reader.GetDecimal(reader.GetOrdinal("TOT_BRUTO"));
                            docCompra.TOT_IMPUESTO = reader.GetDecimal(reader.GetOrdinal("TOT_IMPUESTO"));
                            docCompra.TOT_COMPRA = reader.GetDecimal(reader.GetOrdinal("TOT_COMPRA"));
                            docCompra.TOT_COMPRA_REDONDEO = reader.GetDecimal(reader.GetOrdinal("TOT_COMPRA_REDONDEO"));
                            docCompra.TOT_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TOT_DESCUENTO"));
                            docCompra.TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO"));
                            docCompra.TAS_IGV = reader.GetDecimal(reader.GetOrdinal("TAS_IGV"));
                            docCompra.EST_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("EST_DOCUMENTO"));
                            docCompra.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            docCompra.NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"));
                        }
                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                listDocCompraDetalle = new List<DOC_COMPRA_DETALLE>();
                                while (reader.Read())
                                {
                                    listDocCompraDetalle.Add(new DOC_COMPRA_DETALLE() {
                                        ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO")),
                                    NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO")),
                                    PRECIO_ARTICULO = reader.GetDecimal(reader.GetOrdinal("PRECIO_ARTICULO")),
                                    ID_UM = reader.GetString(reader.GetOrdinal("ID_UM")),
                                    CANTIDAD = reader.GetDecimal(reader.GetOrdinal("CANTIDAD")),
                                    TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO")),
                                    IMPORTE = reader.GetDecimal(reader.GetOrdinal("IMPORTE")),
                                    NRO_FACTOR = reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR")),
                                    CODIGO_BARRA = reader.IsDBNull(reader.GetOrdinal("CODIGO_BARRA")) ? string.Empty : reader.GetString(reader.GetOrdinal("CODIGO_BARRA")),
                                    JSON_UM = reader.GetString(reader.GetOrdinal("JSON_UM"))
                                });
                                }
                                docCompra.detalle = listDocCompraDetalle;
                            }
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }

            return docCompra;
        }

        public async Task<OBJETOS_DOC_COMPRA> GetLoadObjects(string campusId, string userId)
        {
            OBJETOS_DOC_COMPRA modelo = null;
            List<TIPO_DOCUMENTO> listTipoDocumento = null;
            List<TIPO_COMPROBANTE> listTipoComprobante = null;
            List<MONEDA> listMoneda = null;
            List<TIPO_PAGO> listTipoPago = null;
            List<TIPO_CONDICION_PAGO> listTipoCondicionPago = null;
            List<ESTADO> listEstado = null;
            List<UBIGEO> listDepartamento = null;
            decimal tasIgv = 0;

            using (SqlCommand cmd = new SqlCommand("PA_MANT_COMPRAS", _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = campusId;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = userId;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listTipoDocumento = new List<TIPO_DOCUMENTO>();
                        while (reader.Read())
                        {
                            listTipoDocumento.Add(new TIPO_DOCUMENTO()
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
                            listTipoComprobante = new List<TIPO_COMPROBANTE>();
                            while (reader.Read())
                            {
                                listTipoComprobante.Add(new TIPO_COMPROBANTE()
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
                            listMoneda = new List<MONEDA>();
                            while (reader.Read())
                            {
                                listMoneda.Add(new MONEDA()
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
                            listTipoPago = new List<TIPO_PAGO>();
                            while (reader.Read())
                            {
                                listTipoPago.Add(new TIPO_PAGO()
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
                            listTipoCondicionPago = new List<TIPO_CONDICION_PAGO>();
                            while (reader.Read())
                            {
                                listTipoCondicionPago.Add(new TIPO_CONDICION_PAGO()
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
                            listEstado = new List<ESTADO>();
                            while (reader.Read())
                            {
                                listEstado.Add(new ESTADO()
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
                            listDepartamento = new List<UBIGEO>();
                            while (reader.Read())
                            {
                                listDepartamento.Add(new UBIGEO()
                                {
                                    ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO")),
                                    UBIGEO_DEPARTAMENTO = reader.GetString(reader.GetOrdinal("UBIGEO_DEPARTAMENTO"))
                                });
                            }
                        }
                    }

                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                tasIgv = reader.GetDecimal(reader.GetOrdinal("IGV"));
                            }
                        }
                    }

                    modelo = new OBJETOS_DOC_COMPRA();
                    modelo.ListTipoDocumento = listTipoDocumento;
                    modelo.ListTipoComprobante = listTipoComprobante;
                    modelo.ListMoneda = listMoneda;
                    modelo.ListTipoPago = listTipoPago;
                    modelo.ListTipoCondicionPago = listTipoCondicionPago;
                    modelo.ListEstado = listEstado;
                    modelo.ListDepartamento = listDepartamento;
                    modelo.TasIgv = tasIgv;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public async Task<bool> RegisterAsync(DOC_COMPRA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "INS";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = obj.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar, 6).Value = obj.NRO_SERIE;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = obj.NRO_DOCUMENTO;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = obj.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = obj.ID_MONEDA;
                cmd.Parameters.Add("@FEC_DOCUMENTO", SqlDbType.DateTime).Value = obj.FEC_DOCUMENTO;
                cmd.Parameters.Add("@FEC_VENCIMIENTO", SqlDbType.DateTime).Value = obj.FEC_VENCIMIENTO;
                cmd.Parameters.Add("@OBS_COMPRA", SqlDbType.VarChar, 200).Value = string.IsNullOrEmpty(obj.OBS_COMPRA) ? (object)DBNull.Value : obj.OBS_COMPRA;
                cmd.Parameters.Add("@TOT_BRUTO", SqlDbType.Decimal).Value = obj.TOT_BRUTO == 0 ? (object)DBNull.Value : obj.TOT_BRUTO;
                cmd.Parameters.Add("@TOT_DESCUENTO", SqlDbType.Decimal).Value = obj.TOT_DESCUENTO == 0 ? (object)DBNull.Value : obj.TOT_DESCUENTO;
                cmd.Parameters.Add("@TAS_DESCUENTO", SqlDbType.Decimal).Value = obj.TAS_DESCUENTO == 0 ? (object)DBNull.Value : obj.TAS_DESCUENTO;
                cmd.Parameters.Add("@TAS_IGV", SqlDbType.Decimal).Value = obj.TAS_IGV == 0 ? (object)DBNull.Value : obj.TAS_IGV;
                cmd.Parameters.Add("@TOT_IMPUESTO", SqlDbType.Decimal).Value = obj.TOT_IMPUESTO == 0 ? (object)DBNull.Value : obj.TOT_IMPUESTO;
                cmd.Parameters.Add("@TOT_COMPRA", SqlDbType.Decimal).Value = obj.TOT_COMPRA == 0 ? (object)DBNull.Value : obj.TOT_COMPRA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ID_TIPO_PAGO", SqlDbType.VarChar, 3).Value = obj.ID_TIPO_PAGO == "-1" ? (object)DBNull.Value : obj.ID_TIPO_PAGO;
                cmd.Parameters.Add("@ID_TIPO_CONDICION_PAGO", SqlDbType.VarChar, 2).Value = obj.ID_TIPO_CONDICION_PAGO == "-1" ? (object)DBNull.Value : obj.ID_TIPO_CONDICION_PAGO;
                cmd.Parameters.Add("@JSON_ARTICULOS", SqlDbType.VarChar, -1).Value = obj.JSON_ARTICULOS;
                cmd.Parameters.Add("@ABONO", SqlDbType.Decimal).Value = obj.ABONO == 0 ? (object)DBNull.Value : obj.ABONO;
                cmd.Parameters.Add("@SALDO", SqlDbType.Decimal).Value = obj.SALDO == 0 ? (object)DBNull.Value : obj.SALDO;
                cmd.Parameters.Add("@FEC_CANCELACION", SqlDbType.DateTime).Value = obj.FEC_CANCELACION == "" ? (object)DBNull.Value : obj.FEC_CANCELACION;
                cmd.Parameters.Add("@FLG_RETIRAR_CAJA", SqlDbType.Bit).Value = obj.FLG_RETIRAR_CAJA;
                cmd.Parameters.Add("@MONTO_RETIRA_CAJA", SqlDbType.Decimal).Value = obj.MONTO_RETIRA_CAJA == 0 ? (object)DBNull.Value : obj.MONTO_RETIRA_CAJA;
                cmd.Parameters.Add("@ID_CAJA_CA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_CAJA_CA) ? (object)DBNull.Value : obj.ID_CAJA_CA;
                cmd.Parameters.Add("@ID_USUARIO_CA", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(obj.ID_USUARIO_CA) ? (object)DBNull.Value : obj.ID_USUARIO_CA;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = obj.CORRELATIVO_CA == 0 ? (object)DBNull.Value : obj.CORRELATIVO_CA;

                await cmd.ExecuteNonQueryAsync();
            }
            return true;
        }
    }
}
