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
    public class DaoCaja
    {
        public List<CAJA> listaCajas(SqlConnection con)
        {
            List<CAJA> lista = null;
            CAJA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CAJA>();
                        while (reader.Read())
                        {
                            modelo = new CAJA();
                            modelo.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarCaja(SqlConnection con, SqlTransaction trx, CAJA oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar,2).Value = string.IsNullOrEmpty(oModelo.ID_CAJA) ? (object)DBNull.Value : oModelo.ID_CAJA;
                cmd.Parameters.Add("@NOM_CAJA", SqlDbType.VarChar, 90).Value = oModelo.NOM_CAJA;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularCaja(SqlConnection con, SqlTransaction trx, int idCaja, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = idCaja;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
