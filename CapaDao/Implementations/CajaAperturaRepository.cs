using CapaDao.Contracts;
using Entidades;
using Helper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace CapaDao.Implementations
{
    public class CajaAperturaRepository : ICajaAperturaRepository
    {
        private readonly IConnection _sqlConnection;
        private readonly string _storeProcedure = "PA_MANT_CAJA_APERTURA";
        public CajaAperturaRepository(IConnection sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        public async Task<List<CAJA_APERTURA>> GetAllAsync(CAJA_APERTURA obj)
        {
            List<CAJA_APERTURA> list = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CON";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_CAJA) ? (object)DBNull.Value : obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(obj.ID_USUARIO) ? (object)DBNull.Value : obj.ID_USUARIO;
                cmd.Parameters.Add("@FEC_INI", SqlDbType.DateTime).Value = DataUtility.ObjectToDateTimeNull(obj.FECHA_INICIAL);
                cmd.Parameters.Add("@FEC_FIN", SqlDbType.DateTime).Value = DataUtility.ObjectToDateTimeNull(obj.FECHA_FINAL);

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<CAJA_APERTURA>();
                        while (reader.Read())
                        {
                            list.Add(new CAJA_APERTURA()
                            {
                                NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO")),
                                NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA")),
                                FECHA_APERTURA = reader.GetDateTime(reader.GetOrdinal("FECHA_APERTURA")),
                                FECHA_CIERRE = DataUtility.ObjectToDateTimeNull(reader["FECHA_CIERRE"]),
                                SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA")),
                                MONTO_COBRADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO")),
                                MONTO_APERTURA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA")),
                                FLG_CIERRE = reader.GetBoolean(reader.GetOrdinal("FLG_CIERRE")),
                                ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
                                ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO")),
                                FLG_REAPERTURADO = reader.GetBoolean(reader.GetOrdinal("FLG_REAPERTURADO"))
                            });
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return list;
        }

        /// Nos devuelve la caja abierta actualmente, si no hay ninguna devuelve null.
        public async Task<CAJA_APERTURA> GetBoxStateAsync(CAJA_APERTURA obj)
        {
            CAJA_APERTURA model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "ABI";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_CAJA) ? (object)DBNull.Value : obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = obj.CORRELATIVO == 0 ? (object)DBNull.Value : obj.CORRELATIVO;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new CAJA_APERTURA();
                            model.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            model.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            model.CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_CA"));
                            model.FECHA_APERTURA = reader.GetDateTime(reader.GetOrdinal("FECHA_APERTURA"));
                            model.MONTO_APERTURA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA"));
                            model.FECHA_CIERRE = DataUtility.ObjectToDateTimeNull(reader["FECHA_CIERRE"]);
                            model.HORA_CIERRE = DataUtility.ObjectToString(reader["HORA_CIERRE"]);
                            model.MONTO_COBRADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO"));
                            model.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            model.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            model.ITEM = reader.GetInt32(reader.GetOrdinal("ITEM"));
                            model.FLG_REAPERTURADO = reader.GetBoolean(reader.GetOrdinal("FLG_REAPERTURADO"));
                            model.NOM_MONEDA = reader.GetString(reader.GetOrdinal("NOM_MONEDA"));
                            model.FLG_CIERRE_DIFERIDO = reader.GetBoolean(reader.GetOrdinal("FLG_CIERRE_DIFERIDO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }

        public async Task<DINERO_EN_CAJA> GetTotalsByUserIdAsync(CAJA_APERTURA obj)
        {
            DINERO_EN_CAJA model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "TOT";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(obj.ID_CAJA) ? (object)DBNull.Value : obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = obj.CORRELATIVO == 0 ? (object)DBNull.Value : obj.CORRELATIVO;

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new DINERO_EN_CAJA();
                            model.MONTO_APERTURA_CAJA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA_CAJA"));
                            model.MONTO_COBRADO_CONTADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CONTADO"));
                            model.MONTO_COBRADO_CREDITO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CREDITO"));
                            model.MONTO_CAJA_OTROS_INGRESO = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_OTROS_INGRESO"));
                            model.MONTO_CAJA_SALIDA = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_SALIDA"));
                            model.MONTO_TOTAL = reader.GetDecimal(reader.GetOrdinal("MONTO_TOTAL"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }

        public async Task<CAJA_APERTURA> RegisterAsync(CAJA_APERTURA obj, SqlTransaction transaction = null)
        {
            CAJA_APERTURA model = null;
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = obj.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = obj.CORRELATIVO == 0 ? (object)DBNull.Value : obj.CORRELATIVO;
                cmd.Parameters.Add("@MONTO_APERTURA", SqlDbType.Decimal).Value = obj.MONTO_APERTURA == 0 ? (object)DBNull.Value : obj.MONTO_APERTURA;
                cmd.Parameters.Add("@MONTO_COBRADO", SqlDbType.Decimal).Value = obj.MONTO_COBRADO == 0 ? (object)DBNull.Value : obj.MONTO_COBRADO;
                cmd.Parameters.Add("@ID_MONEDA", SqlDbType.VarChar, 3).Value = string.IsNullOrEmpty(obj.ID_MONEDA) ? (object)DBNull.Value : obj.ID_MONEDA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ITEM", SqlDbType.Int).Value = obj.ITEM == 0 ? (object)DBNull.Value : obj.ITEM;
                cmd.Parameters.Add("@FECHA_CIERRE", SqlDbType.DateTime).Value = obj.FECHA_CIERRE == null ? (object)DBNull.Value : obj.FECHA_CIERRE;
                cmd.Parameters.Add("@FLG_REAPERTURADO", SqlDbType.Bit).Value = obj.FLG_REAPERTURADO;
                cmd.Parameters.Add("@FLG_CIERRE_DIFERIDO", SqlDbType.Bit).Value = obj.FLG_CIERRE_DIFERIDO;
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        if (reader.Read())
                        {
                            model = new CAJA_APERTURA();
                            model.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            model.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            model.CORRELATIVO = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_CA"));
                            model.FECHA_APERTURA = reader.GetDateTime(reader.GetOrdinal("FECHA_APERTURA"));
                            model.MONTO_APERTURA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA"));
                            model.FECHA_CIERRE = DataUtility.ObjectToDateTimeNull(reader["FECHA_CIERRE"]);
                            model.MONTO_COBRADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO"));
                            model.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            model.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            model.ITEM = reader.GetInt32(reader.GetOrdinal("ITEM"));
                            model.FLG_REAPERTURADO = reader.GetBoolean(reader.GetOrdinal("FLG_REAPERTURADO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }

        public async Task<bool> ReopenBoxAsync(CAJA_APERTURA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA_APERTURA", _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "REA";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = obj.CORRELATIVO == 0 ? (object)DBNull.Value : obj.CORRELATIVO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO_REGISTRO;

                await cmd.ExecuteNonQueryAsync();

            }
            return true;
        }

        public async Task<bool> ValidateBoxAsync(CAJA_APERTURA obj, SqlTransaction transaction = null)
        {
            using (SqlCommand cmd = new SqlCommand(_storeProcedure, _sqlConnection.DbConnection, transaction))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "VAL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = obj.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = obj.ID_CAJA;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = obj.ID_USUARIO;
                cmd.Parameters.Add("@CORRELATIVO_CA", SqlDbType.Int).Value = obj.CORRELATIVO;

                await cmd.ExecuteNonQueryAsync();
            }

            return true;
        }


        //#region Consultas y reportes
        //public COMBOS_REPORTE_CAJA_ARQUEO combosReportesCajaArqueo(SqlConnection con, string idSucursal)
        //{
        //    COMBOS_REPORTE_CAJA_ARQUEO modelo = null;
        //    List<CAJA> listaCajas = null;
        //    List<USUARIO> listaUsuarios = null;
        //    using (SqlCommand cmd = new SqlCommand("PA_REPORTE_CAJA", con))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
        //        cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader != null)
        //        {
        //            if (reader.HasRows)
        //            {
        //                listaUsuarios = new List<USUARIO>();
        //                while (reader.Read())
        //                {
        //                    listaUsuarios.Add(new USUARIO()
        //                    {
        //                        ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO")),
        //                        NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"))
        //                    });
        //                }
        //            }
        //            if (reader.NextResult())
        //            {
        //                if (reader.HasRows)
        //                {
        //                    listaCajas = new List<CAJA>();
        //                    while (reader.Read())
        //                    {
        //                        listaCajas.Add(new CAJA()
        //                        {
        //                            ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
        //                            NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"))
        //                        });
        //                    }
        //                }
        //            }

        //            modelo = new COMBOS_REPORTE_CAJA_ARQUEO();
        //            modelo.listaUsuarios = listaUsuarios;
        //            modelo.listaCajas = listaCajas;
        //        }
        //        reader.Close();
        //        reader.Dispose();
        //    }
        //    return modelo;
        //}

        //public List<ARQUEO_CAJA> listaArqueoCaja(SqlConnection con, string idSucursal, string fecIni, string fecFin, string idUsuario, string idCaja)
        //{
        //    List<ARQUEO_CAJA> lista = null;
        //    using (SqlCommand cmd = new SqlCommand("PA_REPORTE_CAJA", con))
        //    {
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "ARQ";
        //        cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
        //        cmd.Parameters.Add("@FEC_INI", SqlDbType.DateTime).Value = fecIni;
        //        cmd.Parameters.Add("@FEC_FIN", SqlDbType.DateTime).Value = fecFin;
        //        cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 20).Value = string.IsNullOrEmpty(idUsuario) ? (object)DBNull.Value : idUsuario;
        //        cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idCaja) ? (object)DBNull.Value : idCaja;
        //        SqlDataReader reader = cmd.ExecuteReader();
        //        if (reader != null)
        //        {
        //            if (reader.HasRows)
        //            {
        //                lista = new List<ARQUEO_CAJA>();
        //                while (reader.Read())
        //                {
        //                    lista.Add(new ARQUEO_CAJA()
        //                    {
        //                        NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO")),
        //                        NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA")),
        //                        FECHA_APERTURA = reader.GetString(reader.GetOrdinal("FECHA_APERTURA")),
        //                        FECHA_CIERRE = reader.GetString(reader.GetOrdinal("FECHA_CIERRE")),
        //                        SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA")),
        //                        MONTO_APERTURA_CAJA = reader.GetDecimal(reader.GetOrdinal("MONTO_APERTURA_CAJA")),
        //                        MONTO_COBRADO_CONTADO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CONTADO")),
        //                        MONTO_COBRADO_CREDITO = reader.GetDecimal(reader.GetOrdinal("MONTO_COBRADO_CREDITO")),
        //                        MONTO_CAJA_OTROS_INGRESO = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_OTROS_INGRESO")),
        //                        MONTO_CAJA_SALIDA = reader.GetDecimal(reader.GetOrdinal("MONTO_CAJA_SALIDA")),
        //                        MONTO_TOTAL = reader.GetDecimal(reader.GetOrdinal("MONTO_TOTAL")),
        //                    }
        //                    );
        //                }
        //            }
        //        }
        //        reader.Close();
        //        reader.Dispose();
        //    }
        //    return lista;
        //}
        //#endregion
    }
}
