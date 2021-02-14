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
    public class DaoTipoMovimiento
    {
        public List<TIPO_MOVIMIENTO> listaTipoMovimiento(SqlConnection con)
        {
            List<TIPO_MOVIMIENTO> lista = null;
            TIPO_MOVIMIENTO oModelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_TIPO_MOVIMIENTO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CAJ";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<TIPO_MOVIMIENTO>();
                        while (reader.Read())
                        {
                            oModelo = new TIPO_MOVIMIENTO();
                            oModelo.ID_TIPO_MOVIMIENTO = reader.GetString(reader.GetOrdinal("ID_TIPO_MOVIMIENTO"));
                            oModelo.NOM_TIPO_MOVIMIENTO = reader.GetString(reader.GetOrdinal("NOM_TIPO_MOVIMIENTO"));
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
