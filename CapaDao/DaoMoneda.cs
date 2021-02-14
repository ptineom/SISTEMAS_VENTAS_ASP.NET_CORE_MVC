using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao
{
    public class DaoMoneda
    {
        public List<MONEDA> listaMonedas(SqlConnection con  )
        {
            List<MONEDA> lista = null;
            MONEDA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_MONEDA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<MONEDA>();
                        while (reader.Read())
                        {
                            modelo = new MONEDA();
                            modelo.ID_MONEDA = reader.GetString(reader.GetOrdinal("ID_MONEDA"));
                            modelo.NOM_MONEDA = reader.GetString(reader.GetOrdinal("NOM_MONEDA"));
                            modelo.SGN_MONEDA = reader.GetString(reader.GetOrdinal("SGN_MONEDA"));
                            modelo.FLG_LOCAL = reader.GetBoolean(reader.GetOrdinal("FLG_LOCAL"));
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
