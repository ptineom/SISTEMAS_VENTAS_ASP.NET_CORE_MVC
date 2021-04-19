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
    public class DaoCajaApertura
    {
        /// <summary>
        /// Nos devuelve la caja abierta actualmente, si no hay ninguna devuelve null.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idSucursal"></param>
        /// <param name="idCaja"></param>
        /// <param name="idUsuario"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public CAJA_APERTURA GetStateBox(SqlConnection con, string idSucursal, string idCaja, string idUsuario, int correlativo)
        {
            CAJA_APERTURA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "ABI";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idCaja) ? (object)DBNull.Value : idCaja;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = correlativo == 0 ? (object)DBNull.Value : correlativo;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new CAJA_APERTURA();
                            modelo.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            modelo.CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_CA"));
                            modelo.FECHA_APERTURA = reader.GetString(reader.GetOrdinal("FECHA_APERTURA"));
                            modelo.MONTO_APERTURA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA"));
                            modelo.FECHA_CIERRE = reader.GetString(reader.GetOrdinal("FECHA_CIERRE"));
                            modelo.HORA_CIERRE = reader.GetString(reader.GetOrdinal("HORA_CIERRE"));
                            modelo.MONTO_COBRADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO"));
                            modelo.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.ITEM = reader.GetInt32(reader.GetOrdinal("ITEM"));
                            modelo.FLG_REAPERTURADO = reader.GetBoolean(reader.GetOrdinal("FLG_REAPERTURADO"));
                            modelo.NOM_MONEDA = reader.GetString(reader.GetOrdinal("NOM_MONEDA"));
                            modelo.FLG_CIERRE_DIFERIDO = reader.GetBoolean(reader.GetOrdinal("FLG_CIERRE_DIFERIDO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        /// <summary>
        /// Nos devuelve los montos totales ingresados en la caja actual.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idSucursal"></param>
        /// <param name="idCaja"></param>
        /// <param name="idUsuario"></param>
        /// <param name="correlativo"></param>
        /// <returns></returns>
        public DINERO_EN_CAJA GetTotalsByUserId(SqlConnection con, string idSucursal, string idCaja, string idUsuario, int correlativo)
        {
            DINERO_EN_CAJA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "TOT";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idCaja) ? (object)DBNull.Value : idCaja;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = correlativo == 0 ? (object)DBNull.Value : correlativo;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new DINERO_EN_CAJA();
                            modelo.MONTO_APERTURA_CAJA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA_CAJA"));
                            modelo.MONTO_COBRADO_CONTADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CONTADO"));
                            modelo.MONTO_COBRADO_CREDITO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CREDITO"));
                            modelo.MONTO_CAJA_OTROS_INGRESO = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_OTROS_INGRESO"));
                            modelo.MONTO_CAJA_SALIDA = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_SALIDA"));
                            modelo.MONTO_TOTAL = reader.GetDecimal(reader.GetOrdinal("MONTO_TOTAL"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        /// <summary>
        /// Aperturar la caja seleccionada.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="trx"></param>
        /// <param name="oModelo"></param>
        /// <returns></returns>
        public CAJA_APERTURA Register(SqlConnection con, SqlTransaction trx, CAJA_APERTURA oModelo)
        {
            CAJA_APERTURA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = oModelo.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = oModelo.CORRELATIVO == 0 ? (object)DBNull.Value : oModelo.CORRELATIVO;
                cmd.Parameters.Add("@MONTO_APERTURA", SqlDbType.Decimal).Value = oModelo.MONTO_APERTURA == 0 ? (object)DBNull.Value : oModelo.MONTO_APERTURA;
                cmd.Parameters.Add("@MONTO_COBRADO", SqlDbType.Decimal).Value = oModelo.MONTO_COBRADO == 0 ? (object)DBNull.Value : oModelo.MONTO_COBRADO;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(oModelo.ID_MONEDA) ? (object)DBNull.Value : oModelo.ID_MONEDA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ITEM", SqlDbType.Int).Value = oModelo.ITEM == 0 ? (object)DBNull.Value : oModelo.ITEM;
                cmd.Parameters.Add("@FECHA_CIERRE", SqlDbType.DateTime).Value = string.IsNullOrEmpty(oModelo.FECHA_CIERRE) ? (object)DBNull.Value : oModelo.FECHA_CIERRE;
                cmd.Parameters.Add("@FLG_REAPERTURADO", SqlDbType.Bit).Value = oModelo.FLG_REAPERTURADO;
                cmd.Parameters.Add("@FLG_CIERRE_DIFERIDO", SqlDbType.Bit).Value = oModelo.FLG_CIERRE_DIFERIDO;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            modelo = new CAJA_APERTURA();
                            modelo.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            modelo.CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_CA"));
                            modelo.FECHA_APERTURA = reader.GetString(reader.GetOrdinal("FECHA_APERTURA"));
                            modelo.MONTO_APERTURA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA"));
                            modelo.FECHA_CIERRE = reader.GetString(reader.GetOrdinal("FECHA_CIERRE"));
                            modelo.MONTO_COBRADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO"));
                            modelo.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.ITEM = reader.GetInt32(reader.GetOrdinal("ITEM"));
                            modelo.FLG_REAPERTURADO = reader.GetBoolean(reader.GetOrdinal("FLG_REAPERTURADO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        /// <summary>
        /// Validamos el estado de la caja, si esta cerrado nos devuelve un mensaje de error mostrando la caja.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idSucursal"></param>
        /// <param name="idCaja"></param>
        /// <param name="idUsuario"></param>
        /// <param name="correlativoCa"></param>
        /// <returns></returns>
        public bool ValidateBox(SqlConnection con, string idSucursal, string idCaja, string idUsuario, int correlativoCa)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "VAL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = idCaja;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = correlativoCa;

                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        /// <summary>
        /// Nos devuelve las listas a utilizar para llenar los combos para la apertura de caja.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idSucursal"></param>
        /// <param name="idUsuario"></param>
        /// <param name="listaMonedas"></param>
        /// <param name="listaCajas"></param>
        public void GetData(SqlConnection con, string idSucursal, string idUsuario,
            ref List<MONEDA> listaMonedas, ref List<CAJA> listaCajas)
        {
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = idUsuario;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaCajas = new List<CAJA>();
                        while (reader.Read())
                        {
                            listaCajas.Add(new CAJA()
                            {
                                ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"))
                            });
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
                }
                reader.Close();
                reader.Dispose();
            }
        }

        /// <summary>
        /// Nos devuelve una lista de las cajas aperturadas, según los filtros ingresados.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idSucursal"></param>
        /// <param name="idCaja"></param>
        /// <param name="idUsuario"></param>
        /// <param name="fecIni"></param>
        /// <param name="fecFin"></param>
        /// <returns></returns>
        public List<CAJA_APERTURA> GetAllByFilters(SqlConnection con, string idSucursal, string idCaja, string idUsuario, string fecIni, string fecFin)
        {
            List<CAJA_APERTURA> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CON";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idCaja) ? (object)DBNull.Value : idCaja;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(idUsuario) ? (object)DBNull.Value : idUsuario;
                cmd.Parameters.Add("@FEC_INI", SqlDbType.DateTime).Value = fecIni;
                cmd.Parameters.Add("@FEC_FIN", SqlDbType.DateTime).Value = fecFin;

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CAJA_APERTURA>();
                        while (reader.Read())
                        {
                            lista.Add(new CAJA_APERTURA()
                            {
                                NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO")),
                                NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA")),
                                FECHA_APERTURA = reader.GetString(reader.GetOrdinal("FECHA_APERTURA")),
                                FECHA_CIERRE = reader.GetString(reader.GetOrdinal("FECHA_CIERRE")),
                                SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA")),
                                MONTO_COBRADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO")),
                                MONTO_APERTURA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA")),
                                FLG_CIERRE = reader.GetBoolean(reader.GetOrdinal("FLG_CIERRE")),
                                ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO")),
                                FLG_REAPERTURADO = reader.GetBoolean(reader.GetOrdinal("FLG_REAPERTURADO"))
                            }
                            );
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        /// <summary>
        /// Nos devuelve las listas a utilizar para la cosnulta de cajas aperturadas.
        /// </summary>
        /// <param name="con"></param>
        /// <param name="idSucursal"></param>
        /// <param name="listaUsuario"></param>
        /// <param name="listaCaja"></param>
        public void GetDataQuerys(SqlConnection con, string idSucursal, ref List<USUARIO> listaUsuario, ref List<CAJA> listaCaja)
        {
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "IPC";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaUsuario = new List<USUARIO>();
                        while (reader.Read())
                        {
                            listaUsuario.Add(new USUARIO()
                            {
                                ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
                            });
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaCaja = new List<CAJA>();
                            while (reader.Read())
                            {
                                listaCaja.Add(new CAJA()
                                {
                                    ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                    NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"))
                                });
                            }
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
        }

        public bool ReopenBox(SqlConnection con, SqlTransaction trx, CAJA_APERTURA oModelo)
        {
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = oModelo.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = oModelo.CORRELATIVO == 0 ? (object)DBNull.Value : oModelo.CORRELATIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;

                cmd.ExecuteNonQuery();

            }
            return true;
        }

        #region Consultas y reportes
        public COMBOS_REPORTE_CAJA_ARQUEO combosReportesCajaArqueo(SqlConnection con, string idSucursal)
        {
            COMBOS_REPORTE_CAJA_ARQUEO modelo = null;
            List<CAJA> listaCajas = null;
            List<USUARIO> listaUsuarios = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
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
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            listaCajas = new List<CAJA>();
                            while (reader.Read())
                            {
                                listaCajas.Add(new CAJA()
                                {
                                    ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                    NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"))
                                });
                            }
                        }
                    }

                    modelo = new COMBOS_REPORTE_CAJA_ARQUEO();
                    modelo.listaUsuarios = listaUsuarios;
                    modelo.listaCajas = listaCajas;
                }
                reader.Close();
                reader.Dispose();
            }
            return modelo;
        }

        public List<ARQUEO_CAJA> listaArqueoCaja(SqlConnection con, string idSucursal, string fecIni, string fecFin, string idUsuario, string idCaja)
        {
            List<ARQUEO_CAJA> lista = null;
            using (SqlCommand cmd = new SqlCommand("PA_REPORTE_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "ARQ";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@FEC_INI", SqlDbType.DateTime).Value = fecIni;
                cmd.Parameters.Add("@FEC_FIN", SqlDbType.DateTime).Value = fecFin;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(idUsuario) ? (object)DBNull.Value : idUsuario;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idCaja) ? (object)DBNull.Value : idCaja;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<ARQUEO_CAJA>();
                        while (reader.Read())
                        {
                            lista.Add(new ARQUEO_CAJA()
                            {
                                NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO")),
                                NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA")),
                                FECHA_APERTURA = reader.GetString(reader.GetOrdinal("FECHA_APERTURA")),
                                FECHA_CIERRE = reader.GetString(reader.GetOrdinal("FECHA_CIERRE")),
                                SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA")),
                                MONTO_APERTURA_CAJA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA_CAJA")),
                                MONTO_COBRADO_CONTADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CONTADO")),
                                MONTO_COBRADO_CREDITO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CREDITO")),
                                MONTO_CAJA_OTROS_INGRESO = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_OTROS_INGRESO")),
                                MONTO_CAJA_SALIDA = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_SALIDA")),
                                MONTO_TOTAL = reader.GetDecimal(reader.GetOrdinal("MONTO_TOTAL")),
                            }
                            );
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
