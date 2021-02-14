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
    public class DaoGrupo
    {
        public List<GRUPO> listaGrupos(SqlConnection con)
        {
            List<GRUPO> lista = null;
            GRUPO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<GRUPO>();
                        while (reader.Read())
                        {
                            modelo = new GRUPO();
                            modelo.ID_GRUPO = reader.GetString(reader.GetOrdinal("ID_GRUPO"));
                            modelo.NOM_GRUPO = reader.GetString(reader.GetOrdinal("NOM_GRUPO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public List<GRUPO> cboGrupos(SqlConnection con)
        {
            List<GRUPO> lista = null;
            GRUPO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<GRUPO>();
                        while (reader.Read())
                        {
                            modelo = new GRUPO();
                            modelo.ID_GRUPO = reader.GetString(reader.GetOrdinal("ID_GRUPO"));
                            modelo.NOM_GRUPO = reader.GetString(reader.GetOrdinal("NOM_GRUPO"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarGrupo(SqlConnection con, SqlTransaction trx, GRUPO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = string.IsNullOrEmpty(oModelo.ID_GRUPO) ? (object)DBNull.Value : oModelo.ID_GRUPO;
                cmd.Parameters.Add("@NOM_GRUPO", SqlDbType.VarChar, 60).Value = oModelo.NOM_GRUPO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularGrupo(SqlConnection con, SqlTransaction trx,string idGrupo, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = idGrupo;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
