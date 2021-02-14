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
    public class DaoGrupoAcceso
    {
        public List<GRUPO_ACCESO> listaGrupoAcceso(SqlConnection con)
        {
            List<GRUPO_ACCESO> lista = null;
            GRUPO_ACCESO modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO_ACCESO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<GRUPO_ACCESO>();
                        while (reader.Read())
                        {
                            modelo = new GRUPO_ACCESO();
                            modelo.ID_GRUPO_ACCESO = reader.GetInt32(reader.GetOrdinal("ID_GRUPO_ACCESO"));
                            modelo.NOM_GRUPO_ACCESO = reader.GetString(reader.GetOrdinal("NOM_GRUPO_ACCESO"));
                            modelo.OBS_GRUPO_ACCESO = reader.IsDBNull(reader.GetOrdinal("OBS_GRUPO_ACCESO")) ? string.Empty : reader.GetString(reader.GetOrdinal("OBS_GRUPO_ACCESO"));
                            modelo.FLG_CTRL_TOTAL = reader.GetBoolean(reader.GetOrdinal("FLG_CTRL_TOTAL"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }
        public bool grabarGrupoAcceso(SqlConnection con, SqlTransaction trx, GRUPO_ACCESO oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO_ACCESO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_GRUPO_ACCESO", SqlDbType.Int).Value = oModelo.ID_GRUPO_ACCESO == 0 ? (object)DBNull.Value : oModelo.ID_GRUPO_ACCESO;
                cmd.Parameters.Add("@NOM_GRUPO_ACCESO", SqlDbType.VarChar, 50).Value = oModelo.NOM_GRUPO_ACCESO;
                cmd.Parameters.Add("@OBS_GRUPO_ACCESO", SqlDbType.VarChar, 300).Value = oModelo.OBS_GRUPO_ACCESO == "" ? (object)DBNull.Value : oModelo.OBS_GRUPO_ACCESO;
                cmd.Parameters.Add("@FLG_CTRL_TOTAL", SqlDbType.Bit).Value = oModelo.FLG_CTRL_TOTAL;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.Parameters.Add("@XML_APLICACIONES", SqlDbType.Xml).Value = oModelo.XML_APLICACIONES;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularGrupoAcceso(SqlConnection con, SqlTransaction trx, string idGrupoAcceso, string IdUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO_ACCESO", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_GRUPO_ACCESO", SqlDbType.Int).Value = idGrupoAcceso;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = IdUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public List<APLICACION> listarMenuUsuario(SqlConnection con, int idGrupoAcceso)
        {
            List<APLICACION> listaAplicacion = null;
            APLICACION oAplicacion = null;
            SqlDataReader reader = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_GRUPO_ACCESO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "MEN";
                cmd.Parameters.Add("@ID_GRUPO_ACCESO",SqlDbType.Int).Value = idGrupoAcceso;
                reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaAplicacion = new List<APLICACION>();
                        while (reader.Read())
                        {
                            oAplicacion = new APLICACION();
                            //oAplicacion.CHK = reader.GetBoolean(reader.GetOrdinal("CHK"));
                            oAplicacion.ID_APLICACION = reader.GetInt32(reader.GetOrdinal("ID_APLICACION"));
                            oAplicacion.NOM_APLICACION = reader.GetString(reader.GetOrdinal("NOM_APLICACION"));
                            oAplicacion.ID_APLICACION_PADRE = reader.IsDBNull(reader.GetOrdinal("ID_APLICACION_PADRE")) ? default(int) : reader.GetInt32(reader.GetOrdinal("ID_APLICACION_PADRE"));
                            oAplicacion.FLG_FORMULARIO = reader.GetBoolean(reader.GetOrdinal("FLG_FORMULARIO"));
                            //oAplicacion.NIVEL = reader.GetInt32(reader.GetOrdinal("NIVEL"));
                            listaAplicacion.Add(oAplicacion);
                        }
                    }
                }
            }
            return listaAplicacion;
        }
    }
}
