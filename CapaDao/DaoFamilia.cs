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
    public class DaoFamilia
    {
        public List<FAMILIA> listaFamilias(SqlConnection con,  string idGrupo)
        {
            List<FAMILIA> lista = null;
            FAMILIA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_FAMILIA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = idGrupo == "-1" ? (object)DBNull.Value : idGrupo; ;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<FAMILIA>();
                        while (reader.Read())
                        {
                            modelo = new FAMILIA();
                            modelo.ID_FAMILIA = reader.GetString(reader.GetOrdinal("ID_FAMILIA"));
                            modelo.NOM_FAMILIA = reader.GetString(reader.GetOrdinal("NOM_FAMILIA"));
                            modelo.ID_GRUPO = reader.GetString(reader.GetOrdinal("ID_GRUPO"));
                            modelo.NOM_GRUPO = reader.GetString(reader.GetOrdinal("NOM_GRUPO"));
                            modelo.CADENA_UM = reader.IsDBNull(reader.GetOrdinal("CADENA_UM")) ? string.Empty: reader.GetString(reader.GetOrdinal("CADENA_UM"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public List<FAMILIA> cboFamilia(SqlConnection con, string idGrupo)
        {
            List<FAMILIA> lista = null;
            FAMILIA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_FAMILIA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = idGrupo;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<FAMILIA>();
                        while (reader.Read())
                        {
                            modelo = new FAMILIA();
                            modelo.ID_FAMILIA = reader.GetString(reader.GetOrdinal("ID_FAMILIA"));
                            modelo.NOM_FAMILIA = reader.GetString(reader.GetOrdinal("NOM_FAMILIA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarFamilia(SqlConnection con, SqlTransaction trx, FAMILIA oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_FAMILIA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_FAMILIA", SqlDbType.VarChar, 3).Value = oModelo.ID_FAMILIA;
                cmd.Parameters.Add("@NOM_FAMILIA", SqlDbType.VarChar, 60).Value = oModelo.NOM_FAMILIA;
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = oModelo.ID_GRUPO;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@XML_UM", SqlDbType.Xml).Value = oModelo.CADENA_UM;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularFamilia(SqlConnection con, SqlTransaction trx, string idGrupo, string idFamilia, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_FAMILIA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_GRUPO", SqlDbType.VarChar, 2).Value = idGrupo;
                cmd.Parameters.Add("@ID_FAMILIA", SqlDbType.VarChar, 3).Value = idFamilia;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
