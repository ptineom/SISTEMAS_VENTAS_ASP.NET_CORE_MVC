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
    public class DaoUnidadMedida
    {
        public List<UNIDAD_MEDIDA> listaUm(SqlConnection con)
        {
            List<UNIDAD_MEDIDA> lista = null;
            UNIDAD_MEDIDA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_UNIDAD_MEDIDA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<UNIDAD_MEDIDA>();
                        while (reader.Read())
                        {
                            modelo = new UNIDAD_MEDIDA();
                            modelo.ID_UM = reader.GetString(reader.GetOrdinal("ID_UM"));
                            modelo.NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM"));
                            modelo.FLG_MEDIDA_PESO = reader.GetBoolean(reader.GetOrdinal("FLG_MEDIDA_PESO"));
                            modelo.ABREVIADO = reader.IsDBNull(reader.GetOrdinal("ABREVIADO")) ? string.Empty : reader.GetString(reader.GetOrdinal("ABREVIADO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<UNIDAD_MEDIDA> listaUmPorFamilia(SqlConnection con, string idGrupo, string idFamilia)
        {
            List<UNIDAD_MEDIDA> lista = null;
            UNIDAD_MEDIDA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_FAMILIA_UM", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "GET";
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = idGrupo;
                cmd.Parameters.Add("@ID_FAMILIA", SqlDbType.VarChar, 3).Value = idFamilia;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<UNIDAD_MEDIDA>();
                        while (reader.Read())
                        {
                            modelo = new UNIDAD_MEDIDA();
                            modelo.ID_UM = reader.GetString(reader.GetOrdinal("ID_UM"));
                            modelo.NOM_UM = reader.GetString(reader.GetOrdinal("NOM_UM"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarUm(SqlConnection con, SqlTransaction trx, UNIDAD_MEDIDA oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_UNIDAD_MEDIDA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_UM", SqlDbType.VarChar,3).Value = oModelo.ID_UM == "" ? (object)DBNull.Value : oModelo.ID_UM;
                cmd.Parameters.Add("@NOM_UM", SqlDbType.VarChar, 30).Value = oModelo.NOM_UM;
                cmd.Parameters.Add("@FLG_MEDIDA_PESO", SqlDbType.Bit).Value = oModelo.FLG_MEDIDA_PESO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@ABREVIADO", SqlDbType.VarChar, 3).Value = oModelo.ABREVIADO == "" ? (object)DBNull.Value : oModelo.ABREVIADO; 
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }

        public bool anularUm(SqlConnection con, SqlTransaction trx, string idUm, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_UNIDAD_MEDIDA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_UM", SqlDbType.VarChar,3).Value = idUm;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
