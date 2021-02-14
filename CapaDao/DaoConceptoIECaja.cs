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
    public class DaoConceptoIECaja
    {

        public List<CONCEPTO_I_E_CAJA> listaConceptoIECaja(SqlConnection con)
        {
            List<CONCEPTO_I_E_CAJA> lista = null;
            CONCEPTO_I_E_CAJA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CONCEPTO_I_E_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CONCEPTO_I_E_CAJA>();
                        while (reader.Read())
                        {
                            modelo = new CONCEPTO_I_E_CAJA();
                            modelo.ID_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("ID_CONCEPTO_I_E"));
                            modelo.NOM_CONCEPTO_I_E = reader.GetString(reader.GetOrdinal("NOM_CONCEPTO_I_E"));
                            modelo.ID_TIPO_MOVIMIENTO = reader.GetString(reader.GetOrdinal("ID_TIPO_MOVIMIENTO"));
                            modelo.NOM_TIPO_MOVIMIENTO = reader.GetString(reader.GetOrdinal("NOM_TIPO_MOVIMIENTO"));
                            modelo.FLG_PAGO_PROVEEDORES = reader.GetBoolean(reader.GetOrdinal("FLG_PAGO_PROVEEDORES"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarConceptoIECaja(SqlConnection con, SqlTransaction trx, CONCEPTO_I_E_CAJA oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CONCEPTO_I_E_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;

                SqlParameter paramIdConceptoIE = new SqlParameter("@ID_CONCEPTO_I_E", SqlDbType.VarChar, 2);
                paramIdConceptoIE.Direction = ParameterDirection.InputOutput;
                paramIdConceptoIE.Value = oModelo.ID_CONCEPTO_I_E == "" ? (object)DBNull.Value : oModelo.ID_CONCEPTO_I_E;
                cmd.Parameters.Add(paramIdConceptoIE);

                cmd.Parameters.Add("@NOM_CONCEPTO_I_E", SqlDbType.VarChar, 90).Value = oModelo.NOM_CONCEPTO_I_E;
                cmd.Parameters.Add("@ID_TIPO_MOVIMIENTO", SqlDbType.VarChar, 2).Value = oModelo.ID_TIPO_MOVIMIENTO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
                if (oModelo.ACCION == "INS")
                {
                    oModelo.ID_CONCEPTO_I_E = cmd.Parameters["@ID_CONCEPTO_I_E"].Value.ToString();
                }
            }
            return bExito;
        }

        public bool anularConceptoIECaja(SqlConnection con, SqlTransaction trx, string idConceptoIE, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_CONCEPTO_I_E_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_CONCEPTO_I_E", SqlDbType.VarChar, 2).Value = idConceptoIE;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        
    }
}
