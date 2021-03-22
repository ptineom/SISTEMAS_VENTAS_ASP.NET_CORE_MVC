using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
//using System.Data;
//using System.Data.SqlClient;
using System.Text;

namespace CapaDao
{
    public class DaoAplicacion
    {
        public List<APLICACION> GetMenuByUserId(SqlConnection con, string sIdUsuario)
        {
            List<APLICACION> listaAplicacion = null;
            APLICACION oAplicacion = null;
            SqlDataReader reader = null;

            using (SqlCommand cmd = new SqlCommand("PA_LISTA_MENU_PROYECTO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 2).Value = sIdUsuario;
                reader = cmd.ExecuteReader(CommandBehavior.SingleResult);
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaAplicacion = new List<APLICACION>();
                        while (reader.Read())
                        {
                            oAplicacion = new APLICACION();
                            oAplicacion.ID_APLICACION = reader.GetInt32(reader.GetOrdinal("ID_APLICACION"));
                            oAplicacion.NOM_APLICACION = reader.GetString(reader.GetOrdinal("NOM_APLICACION"));
                            oAplicacion.ID_APLICACION_PADRE = reader.IsDBNull(reader.GetOrdinal("ID_APLICACION_PADRE")) ? default(int) : reader.GetInt32(reader.GetOrdinal("ID_APLICACION_PADRE"));
                            oAplicacion.FLG_FORMULARIO = reader.GetBoolean(reader.GetOrdinal("FLG_FORMULARIO"));
                            oAplicacion.NOM_FORMULARIO = reader.GetString(reader.GetOrdinal("NOM_FORMULARIO"));
                            oAplicacion.NOM_CONTROLADOR = reader.GetString(reader.GetOrdinal("NOM_CONTROLADOR"));
                            oAplicacion.FLG_RAIZ = reader.GetBoolean(reader.GetOrdinal("FLG_RAIZ"));
                            oAplicacion.ICON = reader.IsDBNull(reader.GetOrdinal("ICON")) ? string.Empty : reader.GetString(reader.GetOrdinal("ICON"));
                            oAplicacion.BREADCRUMS = reader.IsDBNull(reader.GetOrdinal("BREADCRUMS")) ? string.Empty : reader.GetString(reader.GetOrdinal("BREADCRUMS"));
                            oAplicacion.FLG_ACCESO_DIRECTO = reader.GetBoolean(reader.GetOrdinal("FLG_ACCESO_DIRECTO"));
                            oAplicacion.BG_COLOR = reader.IsDBNull(reader.GetOrdinal("BG_COLOR")) ? string.Empty : reader.GetString(reader.GetOrdinal("BG_COLOR"));
                            listaAplicacion.Add(oAplicacion);
                        }
                    }
                }
            }
            return listaAplicacion;
        }
    }
}
