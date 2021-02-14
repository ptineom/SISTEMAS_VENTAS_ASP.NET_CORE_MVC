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
    public class DaoIngresoEgresoCaja
    {

        public INGRESO_EGRESO_CAJA cboIngresoEgresoCaja(SqlConnection con, string movimiento, string idSucursal, string idUsuario)
        {
            INGRESO_EGRESO_CAJA oIngresoEgresoCaja = null;
            List<CONCEPTO_I_E_CAJA> listaConceptos = null;
            List<MONEDA> listaMonedas = null;
            string idTipoMovimiento = string.Empty;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INGRESO_EGRESO_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@MOVIMIENTO", SqlDbType.VarChar, 1).Value = movimiento;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaConceptos = new List<CONCEPTO_I_E_CAJA>();
                        while (reader.Read())
                        {
                            listaConceptos.Add(new CONCEPTO_I_E_CAJA()
                            {
                                ID_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("ID_CONCEPTO_I_E")),
                                NOM_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("NOM_CONCEPTO_I_E"))
                            });
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                idTipoMovimiento = reader.GetString(reader.GetOrdinal("ID_TIPO_MOVIMIENTO"));
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
                                    SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA")),
                                    FLG_LOCAL = reader.GetBoolean(reader.GetOrdinal("FLG_LOCAL"))
                                });
                            }
                        }
                    }
                }
                oIngresoEgresoCaja = new INGRESO_EGRESO_CAJA();
                oIngresoEgresoCaja.listaConceptoIE = listaConceptos;
                oIngresoEgresoCaja.ID_TIPO_MOVIMIENTO = idTipoMovimiento;
                oIngresoEgresoCaja.listaMonedas = listaMonedas;
                reader.Close();
                reader.Dispose();
            }
            return oIngresoEgresoCaja;
        }

        public List<INGRESO_EGRESO_CAJA> listaIngresoEgresoCaja(SqlConnection con, string idSucursal, string idTipoMovimiento, string fecIni, string fecFin)
        {
            List<INGRESO_EGRESO_CAJA> lista = null;
            INGRESO_EGRESO_CAJA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INGRESO_EGRESO_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_MOVIMIENTO", SqlDbType.VarChar, 2).Value = idTipoMovimiento;
                cmd.Parameters.Add("@FECHA_I_E", SqlDbType.DateTime).Value = fecIni;
                cmd.Parameters.Add("@FECHA_I_E_FINAL", SqlDbType.DateTime).Value = fecFin;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<INGRESO_EGRESO_CAJA>();
                        while (reader.Read())
                        {
                            modelo = new INGRESO_EGRESO_CAJA();
                            modelo.ID_I_E_CAJA = reader.GetString(reader.GetOrdinal("ID_I_E_CAJA"));
                            modelo.IMPORTE = reader.GetDecimal(reader.GetOrdinal("IMPORTE"));
                            modelo.ID_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("ID_CONCEPTO_I_E"));
                            modelo.DETALLE = reader.GetString(reader.GetOrdinal("DETALLE"));
                            modelo.FECHA_I_E = reader.GetDateTime(reader.GetOrdinal("FECHA_I_E"));
                            modelo.NOM_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("NOM_CONCEPTO_I_E"));
                            modelo.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.FLG_AUTOMATICO = reader.GetBoolean(reader.GetOrdinal("FLG_AUTOMATICO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarIngresoEgresoCaja(SqlConnection con, SqlTransaction trx, INGRESO_EGRESO_CAJA oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INGRESO_EGRESO_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_MOVIMIENTO", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_MOVIMIENTO;
                cmd.Parameters.Add("@ID_I_E_CAJA", SqlDbType.VarChar, 10).Value = string.IsNullOrEmpty(oModelo.ID_I_E_CAJA) ? (object)DBNull.Value : oModelo.ID_I_E_CAJA;
                cmd.Parameters.Add("@IMPORTE", SqlDbType.Decimal).Value = oModelo.IMPORTE;
                cmd.Parameters.Add("@ID_CONCEPTO_I_E", SqlDbType.VarChar, 2).Value = oModelo.ID_CONCEPTO_I_E;
                cmd.Parameters.Add("@DETALLE", SqlDbType.VarChar, 600).Value = oModelo.DETALLE;
                cmd.Parameters.Add("@FECHA_I_E", SqlDbType.DateTime).Value = oModelo.FECHA_I_E;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = oModelo.ID_MONEDA;
                cmd.Parameters.Add("@ID_CAJA_CA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(oModelo.ID_CAJA_CA) ? (object)DBNull.Value : oModelo.ID_CAJA_CA;
                cmd.Parameters.Add("@ID_USUARIO_CA", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(oModelo.ID_USUARIO_CA) ? (object)DBNull.Value : oModelo.ID_USUARIO_CA;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = oModelo.CORRELATIVO_CA == 0 ? (object)DBNull.Value : oModelo.CORRELATIVO_CA;
                cmd.Parameters.Add("@HOR_DOCUMENTO", SqlDbType.Time).Value = string.IsNullOrEmpty(oModelo.HOR_DOCUMENTO) ? (object)DBNull.Value : DateTime.Parse(oModelo.HOR_DOCUMENTO).ToShortTimeString(); ;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool anularIngresoEgresoCaja(SqlConnection con, SqlTransaction trx,
            string idSucursal, string idTipoMovimiento, string idIECaja, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_INGRESO_EGRESO_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_MOVIMIENTO", SqlDbType.VarChar, 2).Value = idTipoMovimiento;
                cmd.Parameters.Add("@ID_I_E_CAJA", SqlDbType.VarChar, 10).Value = idIECaja;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public INGRESO_EGRESO_CAJA cboReporteIngresoEgresoCaja(SqlConnection con, string movimiento, string idSucursal)
        {
            INGRESO_EGRESO_CAJA oIngresoEgresoCaja = null;
            List<CONCEPTO_I_E_CAJA> listaConceptos = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_INGRESO_EGRESO_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@MOVIMIENTO", SqlDbType.VarChar, 1).Value = movimiento;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaConceptos = new List<CONCEPTO_I_E_CAJA>();
                        while (reader.Read())
                        {
                            listaConceptos.Add(new CONCEPTO_I_E_CAJA()
                            {
                                ID_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("ID_CONCEPTO_I_E")),
                                NOM_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("NOM_CONCEPTO_I_E"))
                            });
                        }
                    }
                }
                oIngresoEgresoCaja = new INGRESO_EGRESO_CAJA();
                oIngresoEgresoCaja.listaConceptoIE = listaConceptos;
                reader.Close();
                reader.Dispose();
            }
            return oIngresoEgresoCaja;
        }

        public List<INGRESO_EGRESO_CAJA> reporteIngresoEgresoCaja(SqlConnection con, string idSucursal, string movimiento, string cadenaIdConceptoIE, string fecIni, string fecFin)
        {
            List<INGRESO_EGRESO_CAJA> lista = null;
            INGRESO_EGRESO_CAJA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_INGRESO_EGRESO_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "RIE";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@MOVIMIENTO", SqlDbType.Char, 1).Value = movimiento;
                cmd.Parameters.Add("@CADENA_ID_CONCEPTO_I_E", SqlDbType.VarChar, 300).Value = string.IsNullOrEmpty(cadenaIdConceptoIE)? (object)DBNull.Value: cadenaIdConceptoIE;
                cmd.Parameters.Add("@FECHA_I_E_INICIAL", SqlDbType.DateTime).Value = fecIni;
                cmd.Parameters.Add("@FECHA_I_E_FINAL", SqlDbType.DateTime).Value = fecFin;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<INGRESO_EGRESO_CAJA>();
                        while (reader.Read())
                        {
                            modelo = new INGRESO_EGRESO_CAJA();
                            modelo.NOM_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("NOM_CONCEPTO_I_E"));
                            modelo.DETALLE = reader.GetString(reader.GetOrdinal("DETALLE"));
                            modelo.IMPORTE = reader.GetDecimal(reader.GetOrdinal("IMPORTE"));
                            modelo.FECHA_I_E = reader.GetDateTime(reader.GetOrdinal("FECHA_I_E"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.FLG_AUTOMATICO = reader.GetBoolean(reader.GetOrdinal("FLG_AUTOMATICO"));
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
