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
    public class DaoTipoDocumento
    {
        public List<TIPO_DOCUMENTO> listaTipoDocumento(SqlConnection con)
        {
            List<TIPO_DOCUMENTO> lista = null;
            TIPO_DOCUMENTO oModelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_DOCUMENTO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<TIPO_DOCUMENTO>();
                        while (reader.Read())
                        {
                            oModelo = new TIPO_DOCUMENTO();
                            oModelo.ID_TIPO_DOCUMENTO = reader.GetInt32(reader.GetOrdinal("ID_TIPO_DOCUMENTO"));
                            oModelo.NOM_TIPO_DOCUMENTO = reader.GetString(reader.GetOrdinal("NOM_TIPO_DOCUMENTO"));
                            oModelo.ABREVIATURA = reader.GetString(reader.GetOrdinal("ABREVIATURA"));
                            lista.Add(oModelo);
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
