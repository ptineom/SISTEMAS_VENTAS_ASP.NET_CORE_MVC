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
    public class DaoTipoNotaCredito
    {
        public List<TIPO_NOTA_CREDITO> listaTipoNC(SqlConnection con)
        {
            List<TIPO_NOTA_CREDITO> lista = null;
            TIPO_NOTA_CREDITO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_NOTA_CREDITO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<TIPO_NOTA_CREDITO>();
                        while (reader.Read())
                        {
                            modelo = new TIPO_NOTA_CREDITO();
                            modelo.ID_TIPO_NC = reader.GetString(reader.GetOrdinal("ID_TIPO_NC"));
                            modelo.NOM_TIPO_NC = reader.GetString(reader.GetOrdinal("NOM_TIPO_NC"));
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
