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
    public class DaoVenta
    {
        #region proceso facturacion
        public DOC_VENTA combosVentas(SqlConnection con, string idSucursal, string idUsuario, ref string tipoNCAnular)
        {
            DOC_VENTA modelo = null;
            List<TIPO_DOCUMENTO> listaDocumentos = null;
            List<TIPO_COMPROBANTE> listaComprobantes = null;
            List<MONEDA> listaMonedas = null;
            List<TIPO_PAGO> listaTipPag = null;
            List<TIPO_CONDICION_PAGO> listaTipCon = null;
            List<ESTADO> listaEstados = null;
            List<UBIGEO> listaDepartamentos = null;

            using (SqlCommand cmd = new SqlCommand("PA_MANT_VENTAS", con))
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
                                    FLG_RENDIR_SUNAT = reader.GetBoolean(reader.GetOrdinal("FLG_RENDIR_SUNAT"))
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
                                    FLG_EVALUA_CREDITO = reader.GetBoolean(reader.GetOrdinal("FLG_EVALUA_CREDITO")),
                                });
                            }
                        }
                    }

                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read()) tipoNCAnular = reader.GetString(reader.GetOrdinal("TIPO_NC_ANULAR"));
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
                            listaDepartamentos = new List<UBIGEO>();
                            while (reader.Read())
                            {
                                listaDepartamentos.Add(new UBIGEO()
                                {
                                    ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO")),
                                    UBIGEO_DEPARTAMENTO = reader.GetString(reader.GetOrdinal("UBIGEO_DEPARTAMENTO"))
                                });
                            }
                        }
                    }
                    modelo = new DOC_VENTA();
                    modelo.listaDocumentos = listaDocumentos;
                    modelo.listaComprobantes = listaComprobantes;
                    modelo.listaMonedas = listaMonedas;
                    modelo.listaTipPag = listaTipPag;
                    modelo.listaTipCon = listaTipCon;
                    modelo.listaEstados = listaEstados;
                    modelo.listaDepartamentos = listaDepartamentos;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public bool grabarVenta(SqlConnection con, SqlTransaction trx, DOC_VENTA oModelo, ref string serie, ref string nroComprobante)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_VENTAS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_COMPROBANTE;
                //
                SqlParameter paramNroSerie = new SqlParameter("@NRO_SERIE", SqlDbType.VarChar,6);
                paramNroSerie.Direction = ParameterDirection.InputOutput;
                paramNroSerie.Value =  string.IsNullOrEmpty(oModelo.NRO_SERIE) ? (object)DBNull.Value : oModelo.NRO_SERIE;
                cmd.Parameters.Add(paramNroSerie);
                SqlParameter paramNroDocumento = new SqlParameter("@NRO_DOCUMENTO", SqlDbType.Int);
                paramNroDocumento.Direction = ParameterDirection.InputOutput;
                paramNroDocumento.Value = oModelo.NRO_DOCUMENTO == 0 ? (object)DBNull.Value : oModelo.NRO_DOCUMENTO;
                cmd.Parameters.Add(paramNroDocumento);
                //
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar, 8).Value = oModelo.ID_CLIENTE;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = oModelo.ID_MONEDA;
                cmd.Parameters.Add("@FEC_DOCUMENTO", SqlDbType.DateTime).Value = oModelo.FEC_DOCUMENTO;
                cmd.Parameters.Add("@OBS_VENTA", SqlDbType.VarChar, 200).Value = string.IsNullOrEmpty(oModelo.OBS_VENTA) ? (object)DBNull.Value : oModelo.OBS_VENTA;
                cmd.Parameters.Add("@TOT_BRUTO", SqlDbType.Decimal).Value = oModelo.TOT_BRUTO == 0 ? (object)DBNull.Value : oModelo.TOT_BRUTO;
                cmd.Parameters.Add("@TOT_DESCUENTO", SqlDbType.Decimal).Value = oModelo.TOT_DESCUENTO == 0 ? (object)DBNull.Value : oModelo.TOT_DESCUENTO;
                cmd.Parameters.Add("@TAS_DESCUENTO", SqlDbType.Decimal).Value = oModelo.TAS_DESCUENTO == 0 ? (object)DBNull.Value : oModelo.TAS_DESCUENTO;
                cmd.Parameters.Add("@TOT_IMPUESTO", SqlDbType.Decimal).Value = oModelo.TOT_IMPUESTO == 0 ? (object)DBNull.Value : oModelo.TOT_IMPUESTO;
                cmd.Parameters.Add("@TOT_VENTA", SqlDbType.Decimal).Value = oModelo.TOT_VENTA == 0 ? (object)DBNull.Value : oModelo.TOT_VENTA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ID_TIPO_PAGO", SqlDbType.VarChar, 3).Value = oModelo.ID_TIPO_PAGO == "-1" ? (object)DBNull.Value : oModelo.ID_TIPO_PAGO;
                cmd.Parameters.Add("@ID_TIPO_CONDICION_PAGO", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_CONDICION_PAGO == "-1" ? (object)DBNull.Value : oModelo.ID_TIPO_CONDICION_PAGO;
                cmd.Parameters.Add("@JSON_ARTICULOS", SqlDbType.VarChar, -1).Value = oModelo.JSON_ARTICULOS;
                //
                cmd.Parameters.Add("@ABONO", SqlDbType.Decimal).Value = oModelo.ABONO == 0 ? (object)DBNull.Value : oModelo.ABONO;
                cmd.Parameters.Add("@SALDO", SqlDbType.Decimal).Value = oModelo.SALDO == 0 ? (object)DBNull.Value : oModelo.SALDO;
                cmd.Parameters.Add("@FECHA_VENCIMIENTO", SqlDbType.DateTime).Value = string.IsNullOrEmpty(oModelo.FEC_VENCIMIENTO) ? (object)DBNull.Value : oModelo.FEC_VENCIMIENTO;
                cmd.Parameters.Add("@HOR_DOCUMENTO", SqlDbType.Time).Value = string.IsNullOrEmpty(oModelo.HOR_DOCUMENTO) ? (object)DBNull.Value : DateTime.Parse(oModelo.HOR_DOCUMENTO).ToShortTimeString(); ;
                //
                cmd.Parameters.Add("@ID_CAJA_CA", SqlDbType.VarChar, 2).Value = oModelo.ID_CAJA_CA;
                cmd.Parameters.Add("@ID_USUARIO_CA", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_CA;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = oModelo.CORRELATIVO_CA;
                cmd.ExecuteNonQuery();
                bExito = true;
                serie = cmd.Parameters["@NRO_SERIE"].Value.ToString();
                nroComprobante = cmd.Parameters["@NRO_DOCUMENTO"].Value.ToString();
            }
            return bExito;
        }

        public List<DOC_VENTA> listaVentas(SqlConnection con, string idSucursal, string idCliente, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal, int idEstado)
        {
            List<DOC_VENTA> lista = null;
            DOC_VENTA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(idCliente) ? (object)DBNull.Value : idCliente;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoComprobante) ? (object)DBNull.Value : idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = string.IsNullOrEmpty( nroSerie) ? (object)DBNull.Value : nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento == 0 ? (object)DBNull.Value : nroDocumento;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                cmd.Parameters.Add("@ID_ESTADO", SqlDbType.Int).Value = idEstado;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<DOC_VENTA>();
                        while (reader.Read())
                        {
                            modelo = new DOC_VENTA();
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.DOC_CLIENTE = reader.GetString(reader.GetOrdinal("DOC_CLIENTE"));
                            modelo.NOM_CLIENTE = reader.GetString(reader.GetOrdinal("NOM_CLIENTE"));
                            modelo.TOT_VENTA = reader.GetDecimal(reader.GetOrdinal("TOT_VENTA"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.EST_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("EST_DOCUMENTO"));
                            modelo.NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO"));
                            modelo.FLG_NO_EVALUA_CREDITO = reader.GetBoolean(reader.GetOrdinal("FLG_NO_EVALUA_CREDITO"));
                            modelo.NOM_ESTADO = reader.GetString(reader.GetOrdinal("NOM_ESTADO"));
                            modelo.EMAIL_CLIENTE = reader.GetString(reader.GetOrdinal("EMAIL_CLIENTE"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public DOC_VENTA ventaPorCodigo(SqlConnection con, string idSucursal, string idTipoComprobante, string nroSerie,
            int nroDocumento)
        {
            DOC_VENTA modelo = null;
            List<DOC_VENTA_DETALLE> lista = null;
            DOC_VENTA_DETALLE detalle = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new DOC_VENTA();
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.ID_CLIENTE = reader.GetString(reader.GetOrdinal("ID_CLIENTE"));
                            modelo.NOM_CLIENTE = reader.GetString(reader.GetOrdinal("NOM_CLIENTE"));
                            modelo.DIR_CLIENTE = reader.IsDBNull(reader.GetOrdinal("DIR_CLIENTE")) ? default(string) : reader.GetString(reader.GetOrdinal("DIR_CLIENTE"));
                            modelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            modelo.NRO_DOCUMENTO_CLIENTE = reader.GetString(reader.GetOrdinal("NRO_DOCUMENTO_CLIENTE"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.FEC_VENCIMIENTO = reader.GetString(reader.GetOrdinal("FEC_VENCIMIENTO"));
                            modelo.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            modelo.ID_TIPO_PAGO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_PAGO")) ? "-1" : reader.GetString(reader.GetOrdinal("ID_TIPO_PAGO"));
                            modelo.ID_TIPO_CONDICION_PAGO = reader.IsDBNull(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO")) ? "-1" : reader.GetString(reader.GetOrdinal("ID_TIPO_CONDICION_PAGO"));
                            modelo.OBS_VENTA = reader.IsDBNull(reader.GetOrdinal("OBS_VENTA")) ? default(string) : reader.GetString(reader.GetOrdinal("OBS_VENTA"));
                            modelo.TOT_BRUTO = reader.GetDecimal(reader.GetOrdinal("TOT_BRUTO"));
                            modelo.TOT_IMPUESTO = reader.GetDecimal(reader.GetOrdinal("TOT_IMPUESTO"));
                            modelo.TOT_VENTA = reader.GetDecimal(reader.GetOrdinal("TOT_VENTA"));
                            modelo.TOT_VENTA_REDONDEO = reader.GetDecimal(reader.GetOrdinal("TOT_VENTA_REDONDEO"));
                            modelo.TOT_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TOT_DESCUENTO"));
                            modelo.TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO"));
                            modelo.EST_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("EST_DOCUMENTO"));
                            modelo.TOT_VENTA_EN_LETRAS = reader.GetString(reader.GetOrdinal("TOT_VENTA_EN_LETRAS"));
                            modelo.ID_CAJA_CA = reader.GetString(reader.GetOrdinal("ID_CAJA_CA"));
                            modelo.ID_USUARIO_CA = reader.GetString(reader.GetOrdinal("ID_USUARIO_CA"));
                            modelo.CORRELATIVO_CA = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_CA"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.NOM_USUARIO_CAJA = reader.GetString(reader.GetOrdinal("NOM_USUARIO_CAJA"));
                            modelo.HOR_DOCUMENTO = reader.GetString(reader.GetOrdinal("HOR_DOCUMENTO"));
                        }
                        if (reader.NextResult())
                        {
                            if (reader.HasRows)
                            {
                                lista = new List<DOC_VENTA_DETALLE>();
                                while (reader.Read())
                                {
                                    detalle = new DOC_VENTA_DETALLE();
                                    detalle.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                                    detalle.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                                    detalle.ID_UM = reader.GetString(reader.GetOrdinal("ID_UM"));
                                    detalle.NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM"));
                                    detalle.CANTIDAD = reader.GetDecimal(reader.GetOrdinal("CANTIDAD"));
                                    detalle.TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO"));
                                    detalle.NRO_FACTOR = reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR"));
                                    detalle.PRECIO_ARTICULO = reader.GetDecimal(reader.GetOrdinal("PRECIO_BASE"));
                                    detalle.PRECIO_UNITARIO = reader.GetDecimal(reader.GetOrdinal("PRECIO_UNITARIO"));
                                    detalle.IMPORTE = reader.GetDecimal(reader.GetOrdinal("IMPORTE"));
                                    detalle.ABREVIADO = reader.GetString(reader.GetOrdinal("ABREVIADO"));
                                    lista.Add(detalle);
                                }
                                modelo.listaDetalle = lista;
                            }
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public bool anularVenta(SqlConnection con, SqlTransaction trx, string idSucursal, string idTipoComprobante,
    string nroSerie, int nroDocumento, string idUsuario, ref string nroSerieNC, ref int nroDocumentoNC)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_VENTAS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar, 6).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;

                SqlParameter paramNroSerie = new SqlParameter("@NRO_SERIE_NC", SqlDbType.VarChar,2);
                paramNroSerie.Direction = ParameterDirection.InputOutput;
                //paramNroSerie.Value = nroSerie == 0 ? (object)DBNull.Value : nroSerie;
                cmd.Parameters.Add(paramNroSerie);
                SqlParameter paramNroDocumento = new SqlParameter("@NRO_DOCUMENTO_NC", SqlDbType.Int);
                paramNroDocumento.Direction = ParameterDirection.InputOutput;
                //paramNroDocumento.Value = nroDocumento == 0 ? (object)DBNull.Value : nroDocumento;
                cmd.Parameters.Add(paramNroDocumento);

                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
                if (cmd.Parameters["@NRO_DOCUMENTO_NC"].Value != DBNull.Value)
                {
                    nroSerieNC = cmd.Parameters["@NRO_SERIE_NC"].Value.ToString();
                    nroDocumentoNC = Convert.ToInt32(cmd.Parameters["@NRO_DOCUMENTO_NC"].Value);
                }
            }
            return bExito;
        }
        #endregion

        #region Consultas y reportes
        public DOC_VENTA combosReportesVentas(SqlConnection con, string idSucursal)
        {
            DOC_VENTA modelo = null;
            List<TIPO_DOCUMENTO> listaDocumentos = null;
            List<TIPO_COMPROBANTE> listaComprobantes = null;
            List<TIPO_CONDICION_PAGO> listaTipCon = null;
            List<USUARIO> listaUsuarios = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
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
                                NOM_TIPO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NOM_TIPO_DOCUMENTO"))
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
                                    NOM_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("NOM_TIPO_COMPROBANTE"))
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
                                    NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO"))
                                });
                            }
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaUsuarios = new List<USUARIO>();
                            while (reader.Read())
                            {
                                listaUsuarios.Add(new USUARIO()
                                {
                                    ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                    NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
                                });
                            }
                        }
                    }
                    modelo = new DOC_VENTA();
                    modelo.listaDocumentos = listaDocumentos;
                    modelo.listaComprobantes = listaComprobantes;
                    modelo.listaTipCon = listaTipCon;
                    modelo.listaUsuarios = listaUsuarios;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public List<DOC_VENTA> consultaVentas(SqlConnection con, string idSucursal, string idCliente, string idTipoComprobante,
            string fechaInicio, string fechaFinal, string idTipoCondicionPago, string id_usuario_caja)
        {
            List<DOC_VENTA> lista = null;
            DOC_VENTA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CAB";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CLIENTE", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(idCliente) ? (object)DBNull.Value : idCliente;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoComprobante) ? (object)DBNull.Value : idTipoComprobante;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                cmd.Parameters.Add("@ID_TIPO_CONDICION_PAGO", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoCondicionPago) ? (object)DBNull.Value : idTipoCondicionPago;
                cmd.Parameters.Add("@ID_USUARIO_CAJA", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(id_usuario_caja) ? (object)DBNull.Value : id_usuario_caja;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<DOC_VENTA>();
                        while (reader.Read())
                        {
                            modelo = new DOC_VENTA();
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.NRO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("NRO_DOCUMENTO"));
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.DOC_CLIENTE = reader.GetString(reader.GetOrdinal("DOC_CLIENTE"));
                            modelo.NOM_CLIENTE = reader.GetString(reader.GetOrdinal("NOM_CLIENTE"));
                            modelo.TOT_VENTA = reader.GetDecimal(reader.GetOrdinal("TOT_VENTA"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.NOM_TIPO_CONDICION_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_CONDICION_PAGO"));
                            modelo.NOM_USUARIO_CAJA = reader.GetString(reader.GetOrdinal("NOM_USUARIO_CAJA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
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

        public List<DOC_VENTA_DETALLE> consultaVentasDetalle(SqlConnection con, string idSucursal, string idTipoComprobante, string nroSerie,
            int nroDocumento)
        {
            List<DOC_VENTA_DETALLE> lista = null;
            DOC_VENTA_DETALLE detalle = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DET";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<DOC_VENTA_DETALLE>();
                        while (reader.Read())
                        {
                            detalle = new DOC_VENTA_DETALLE();
                            detalle.ID_ARTICULO = reader.GetString(reader.GetOrdinal("ID_ARTICULO"));
                            detalle.NOM_ARTICULO = reader.GetString(reader.GetOrdinal("NOM_ARTICULO"));
                            detalle.NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM"));
                            detalle.ABREVIADO = reader.GetString(reader.GetOrdinal("ABREVIADO"));
                            detalle.NRO_FACTOR = reader.GetDecimal(reader.GetOrdinal("NRO_FACTOR"));
                            detalle.CANTIDAD = reader.GetDecimal(reader.GetOrdinal("CANTIDAD"));
                            detalle.PRECIO_UNITARIO = reader.GetDecimal(reader.GetOrdinal("PRECIO_UNITARIO"));
                            detalle.TAS_DESCUENTO = reader.GetDecimal(reader.GetOrdinal("TAS_DESCUENTO"));
                            detalle.IMPORTE = reader.GetDecimal(reader.GetOrdinal("IMPORTE"));
                            lista.Add(detalle);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<DOC_VENTA> consultaVentasPorUsuario(SqlConnection con, string idSucursal, string fechaInicio, string fechaFinal,string tipo)
        {
            List<DOC_VENTA> lista = null;
            DOC_VENTA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "VXU";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                cmd.Parameters.Add("@TIPO_FECHA_X_USUARIO", SqlDbType.Char, 1).Value = tipo;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<DOC_VENTA>();
                        while (reader.Read())
                        {
                            modelo = new DOC_VENTA();
                            modelo.ID_USUARIO_CA = reader.GetString(reader.GetOrdinal("ID_USUARIO_CA"));
                            modelo.NOM_USUARIO_CAJA = reader.GetString(reader.GetOrdinal("NOM_USUARIO_CAJA"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.TOT_VENTA = reader.GetDecimal(reader.GetOrdinal("TOT_VENTA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<CONSULTA_X_MES> consultaVentasPorMes(SqlConnection con, string idSucursal, int anio)
        {
            List<CONSULTA_X_MES> lista = null;
            CONSULTA_X_MES modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_VENTAS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "VXM";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ANIO", SqlDbType.Int).Value = anio;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CONSULTA_X_MES>();
                        while (reader.Read())
                        {
                            modelo = new CONSULTA_X_MES();
                            modelo.NOMBRE_MES = reader.GetString(reader.GetOrdinal("NOMBRE_MES"));
                            modelo.MONTO_TOTAL = reader.GetDecimal(reader.GetOrdinal("MONTO_TOTAL"));
                            modelo.PORCENTAJE = reader.GetDecimal(reader.GetOrdinal("PORCENTAJE"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        #endregion

    }
}
