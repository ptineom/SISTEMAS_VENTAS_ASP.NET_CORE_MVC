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
    public class DaoTipoPago
    {
        public List<TIPO_PAGO> listaTipoPago(SqlConnection con)
        {
            List<TIPO_PAGO> lista = null;
            TIPO_PAGO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_PAGO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<TIPO_PAGO>();
                        while (reader.Read())
                        {
                            modelo = new TIPO_PAGO();
                            modelo.ID_TIPO_PAGO = reader.GetString(reader.GetOrdinal("ID_TIPO_PAGO"));
                            modelo.NOM_TIPO_PAGO = reader.GetString(reader.GetOrdinal("NOM_TIPO_PAGO"));
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
