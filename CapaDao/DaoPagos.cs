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
    public class DaoPagos
    {
        public PAGOS combosPagos(SqlConnection con, string idSucursal, string idUsuario)
        {
            PAGOS modelo = null;
            List<TIPO_DOCUMENTO> listaDocumentos = null;
            List<TIPO_COMPROBANTE> listaComprobantes = null;
            List<MONEDA> listaMonedas = null;
            List<TIPO_PAGO> listaTipPag = null;

            using (SqlCommand cmd = new SqlCommand("PA_MANT_PAGOS", con))
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
                                ABREVIATURA = reader.GetString(reader.GetOrdinal("ABREVIATURA"))
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
                    modelo = new PAGOS();
                    modelo.listaDocumentos = listaDocumentos;
                    modelo.listaComprobantes = listaComprobantes;
                    modelo.listaMonedas = listaMonedas;
                    modelo.listaTipPag = listaTipPag;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }
        public List<PAGOS> listaCtaCtePagos(SqlConnection con, string idSucursal, string estadoPago, string idProveedor, string idTipoComprobante,
            string nroSerie, int nroDocumento, string fechaInicio, string fechaFinal)
        {
            List<PAGOS> lista = null;
            PAGOS modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PAGOS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ESTADO_PAGO", SqlDbType.VarChar, 1).Value = estadoPago;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = string.IsNullOrEmpty(idProveedor) ? (object)DBNull.Value : idProveedor;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoComprobante) ? (object)DBNull.Value : idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,2).Value = string.IsNullOrEmpty(nroSerie ) ? (object)DBNull.Value : nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento == 0 ? (object)DBNull.Value : nroDocumento;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<PAGOS>();
                        while (reader.Read())
                        {
                            modelo = new PAGOS();
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.DOC_PROVEEDOR = reader.GetString(reader.GetOrdinal("DOC_PROVEEDOR"));
                            modelo.NOM_PROVEEDOR = reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.TOT_SALDO = reader.GetDecimal(reader.GetOrdinal("TOT_SALDO"));
                            modelo.TOT_ABONADO = reader.GetDecimal(reader.GetOrdinal("TOT_ABONADO"));
                            modelo.TOT_PAGO = reader.GetDecimal(reader.GetOrdinal("TOT_PAGO"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.FEC_VENCIMIENTO = reader.IsDBNull(reader.GetOrdinal("FEC_VENCIMIENTO")) ? default(string) : reader.GetString(reader.GetOrdinal("FEC_VENCIMIENTO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public List<PAGOS> listaPagos(SqlConnection con, string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string idProveedor)
        {
            List<PAGOS> lista = null;
            PAGOS modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PAGOS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "PAG";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,2).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = idProveedor;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<PAGOS>();
                        while (reader.Read())
                        {
                            modelo = new PAGOS();
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.DOC_PROVEEDOR = reader.GetString(reader.GetOrdinal("DOC_PROVEEDOR"));
                            modelo.NOM_PROVEEDOR = reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.TOT_PAGO = reader.GetDecimal(reader.GetOrdinal("TOT_PAGO"));
                            modelo.FEC_COBRANZA = reader.GetString(reader.GetOrdinal("FEC_COBRANZA"));
                            modelo.CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO"));
                            modelo.SECUENCIA = reader.GetInt32(reader.GetOrdinal("SECUENCIA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarPago(SqlConnection con, SqlTransaction trx, PAGOS oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PAGOS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.Int).Value = oModelo.NRO_SERIE;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = oModelo.NRO_DOCUMENTO;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = oModelo.ID_PROVEEDOR;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = oModelo.ID_MONEDA;
                cmd.Parameters.Add("@ID_TIPO_PAGO", SqlDbType.VarChar, 3).Value = oModelo.ID_TIPO_PAGO;
                cmd.Parameters.Add("@TOT_PAGO", SqlDbType.Decimal).Value = oModelo.TOT_PAGO;
                cmd.Parameters.Add("@FEC_PAGO", SqlDbType.DateTime).Value = oModelo.FEC_COBRANZA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                //
                cmd.Parameters.Add("@FLG_RETIRAR_CAJA", SqlDbType.Bit).Value = oModelo.FLG_RETIRAR_CAJA;
                cmd.Parameters.Add("@MONTO_RETIRA_CAJA", SqlDbType.Decimal).Value = oModelo.MONTO_RETIRA_CAJA == 0 ? (object)DBNull.Value : oModelo.MONTO_RETIRA_CAJA;
                cmd.Parameters.Add("@ID_CAJA_CA", SqlDbType.VarChar, 2).Value = oModelo.ID_CAJA_CA;
                cmd.Parameters.Add("@ID_USUARIO_CA", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_CA;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = oModelo.CORRELATIVO_CA;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool anularPago(SqlConnection con, SqlTransaction trx, string idSucursal, string idTipoComprobante,
            string nroSerie, int nroDocumento, string idProveedor, int correlativo, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_PAGOS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,2).Value = nroSerie;
                cmd.Parameters.Add("@NRO_DOCUMENTO", SqlDbType.Int).Value = nroDocumento;
                cmd.Parameters.Add("@ID_PROVEEDOR", SqlDbType.VarChar, 8).Value = idProveedor;
                cmd.Parameters.Add("@CORRELATIVO", SqlDbType.Int).Value = correlativo;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public PAGOS combosReportePagos(SqlConnection con, string idSucursal)
        {
            PAGOS modelo = null;
            List<TIPO_COMPROBANTE> listaComprobantes = null;

            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_PAGOS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
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

                    modelo = new PAGOS();
                    modelo.listaComprobantes = listaComprobantes;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public List<PAGOS> reporteCtaCtePagos(SqlConnection con, string idSucursal, string idTipoComprobante,
            string fechaInicio, string fechaFinal)
        {
            List<PAGOS> lista = null;
            PAGOS modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_PAGOS", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CXP";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoComprobante) ? (object)DBNull.Value : idTipoComprobante;
                cmd.Parameters.Add("@FECHA_INICIO", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaInicio) ? (object)DBNull.Value : fechaInicio;
                cmd.Parameters.Add("@FECHA_FINAL", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(fechaFinal) ? (object)DBNull.Value : fechaFinal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<PAGOS>();
                        while (reader.Read())
                        {
                            modelo = new PAGOS();
                            modelo.COMPROBANTE = reader.GetString(reader.GetOrdinal("COMPROBANTE"));
                            modelo.DOC_PROVEEDOR = reader.GetString(reader.GetOrdinal("DOC_PROVEEDOR"));
                            modelo.NOM_PROVEEDOR = reader.GetString(reader.GetOrdinal("NOM_PROVEEDOR"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.TOT_SALDO = reader.GetDecimal(reader.GetOrdinal("TOT_SALDO"));
                            modelo.TOT_ABONADO = reader.GetDecimal(reader.GetOrdinal("TOT_ABONADO"));
                            modelo.TOT_PAGO = reader.GetDecimal(reader.GetOrdinal("TOT_PAGO"));
                            modelo.FEC_DOCUMENTO = reader.GetString(reader.GetOrdinal("FEC_DOCUMENTO"));
                            modelo.FEC_VENCIMIENTO = reader.IsDBNull(reader.GetOrdinal("FEC_VENCIMIENTO")) ? default(string) : reader.GetString(reader.GetOrdinal("FEC_VENCIMIENTO"));
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
