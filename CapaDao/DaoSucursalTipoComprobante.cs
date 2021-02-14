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
    public class DaoSucursalTipoComprobante
    {
        public List<SUCURSAL_TIPO_COMPROBANTE> listaSucursalTipoComprobante(SqlConnection con, 
            string idSucursal, string idTipoComprobante, bool flgEnUso)
        {
            List<SUCURSAL_TIPO_COMPROBANTE> lista = null;
            SUCURSAL_TIPO_COMPROBANTE modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_TIPO_COMPROBANTE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(idTipoComprobante)? (object)DBNull.Value: idTipoComprobante;
                cmd.Parameters.Add("@FLG_EN_CURSO", SqlDbType.Bit).Value = flgEnUso;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<SUCURSAL_TIPO_COMPROBANTE>();
                        while (reader.Read())
                        {
                            modelo = new SUCURSAL_TIPO_COMPROBANTE();
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NOM_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("NOM_TIPO_COMPROBANTE"));
                            modelo.NRO_SERIE = reader.GetString(reader.GetOrdinal("NRO_SERIE"));
                            modelo.CORRELATIVO_INICIAL = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_INICIAL"));
                            modelo.CORRELATIVO_FINAL = reader.GetInt32(reader.GetOrdinal("CORRELATIVO_FINAL"));
                            modelo.CORRELATIVO_ACTUAL = reader.IsDBNull(reader.GetOrdinal("CORRELATIVO_ACTUAL")) ? default(int) : reader.GetInt32(reader.GetOrdinal("CORRELATIVO_ACTUAL"));
                            modelo.FLG_RENDIR_SUNAT = reader.GetBoolean(reader.GetOrdinal("FLG_RENDIR_SUNAT"));
                            modelo.FLG_EN_CURSO = reader.GetBoolean(reader.GetOrdinal("FLG_EN_CURSO"));
                            lista.Add(modelo);
                        }
                    }
                    
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<TIPO_COMPROBANTE> listaTipoComprobante(SqlConnection con)
        {
            List<TIPO_COMPROBANTE> lista = null;
            TIPO_COMPROBANTE modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_TIPO_COMPROBANTE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "LTC";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<TIPO_COMPROBANTE>();
                        while (reader.Read())
                        {
                            modelo = new TIPO_COMPROBANTE();
                            modelo.ID_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("ID_TIPO_COMPROBANTE"));
                            modelo.NOM_TIPO_COMPROBANTE = reader.GetString(reader.GetOrdinal("NOM_TIPO_COMPROBANTE"));
                            modelo.FLG_VENTA = reader.GetBoolean(reader.GetOrdinal("FLG_VENTA"));
                            modelo.FLG_RENDIR_SUNAT = reader.GetBoolean(reader.GetOrdinal("FLG_RENDIR_SUNAT"));
                            modelo.FLG_NO_EDITABLE = reader.GetBoolean(reader.GetOrdinal("FLG_NO_EDITABLE"));
                            modelo.LETRA_INICIAL_SERIE_ELECTRONICA = reader.GetString(reader.GetOrdinal("PREFIJO_SERIE"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarSucursalTipoComprobante(SqlConnection con, SqlTransaction trx, SUCURSAL_TIPO_COMPROBANTE oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_TIPO_COMPROBANTE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar,6).Value = oModelo.NRO_SERIE;
                cmd.Parameters.Add("@CORRELATIVO_INICIAL", SqlDbType.Int).Value = oModelo.CORRELATIVO_INICIAL;
                cmd.Parameters.Add("@CORRELATIVO_FINAL", SqlDbType.Int).Value = oModelo.CORRELATIVO_FINAL;
                cmd.Parameters.Add("@CORRELATIVO_ACTUAL", SqlDbType.Int).Value = oModelo.CORRELATIVO_ACTUAL;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@FLG_EN_CURSO", SqlDbType.Bit).Value = oModelo.FLG_EN_CURSO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularSucursalTipoComprobante(SqlConnection con, SqlTransaction trx, 
            string idSucursal, string idTipoComprobante, string nroSerie,string idUsuario )
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_TIPO_COMPROBANTE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@NRO_SERIE", SqlDbType.VarChar, 6).Value = nroSerie;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
