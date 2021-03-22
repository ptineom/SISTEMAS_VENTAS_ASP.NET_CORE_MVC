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
    public class DaoTipoComprobante
    {
        public List<TIPO_COMPROBANTE> listaTipoComprobante(SqlConnection con)
        {
            List<TIPO_COMPROBANTE> lista = null;
            TIPO_COMPROBANTE modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_COMPROBANTE", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
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
                            modelo.FLG_COMPRA = reader.GetBoolean(reader.GetOrdinal("FLG_COMPRA"));
                            modelo.FLG_RENDIR_SUNAT = reader.GetBoolean(reader.GetOrdinal("FLG_RENDIR_SUNAT"));
                            modelo.FLG_NO_EDITABLE = reader.GetBoolean(reader.GetOrdinal("FLG_NO_EDITABLE"));
                            modelo.JSON_TIPO_NC = reader.GetString(reader.GetOrdinal("JSON_TIPO_NC"));
                            modelo.LETRA_INICIAL_SERIE_ELECTRONICA = reader.GetString(reader.GetOrdinal("LETRA_INICIAL_SERIE_ELECTRONICA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarTipoComprobante(SqlConnection con, SqlTransaction trx, TIPO_COMPROBANTE oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_COMPROBANTE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(oModelo.ID_TIPO_COMPROBANTE) ? (object)DBNull.Value : oModelo.ID_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@NOM_TIPO_COMPROBANTE", SqlDbType.VarChar, 30).Value = oModelo.NOM_TIPO_COMPROBANTE;
                cmd.Parameters.Add("@FLG_VENTA", SqlDbType.Bit).Value = oModelo.FLG_VENTA;
                cmd.Parameters.Add("@FLG_COMPRA", SqlDbType.Bit).Value = oModelo.FLG_COMPRA;
                cmd.Parameters.Add("@FLG_RENDIR_SUNAT", SqlDbType.Bit).Value = oModelo.FLG_RENDIR_SUNAT;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@JSON_TIPO_NC", SqlDbType.VarChar,-1).Value = string.IsNullOrEmpty(oModelo.JSON_TIPO_NC) ? (object)DBNull.Value : oModelo.JSON_TIPO_NC;
                cmd.Parameters.Add("@LETRA_INICIAL_SERIE_ELECTRONICA", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(oModelo.LETRA_INICIAL_SERIE_ELECTRONICA) ? (object)DBNull.Value : oModelo.LETRA_INICIAL_SERIE_ELECTRONICA;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularTipoComprobante(SqlConnection con, SqlTransaction trx, string idTipoComprobante, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_COMPROBANTE", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_TIPO_COMPROBANTE", SqlDbType.VarChar, 2).Value = idTipoComprobante;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
