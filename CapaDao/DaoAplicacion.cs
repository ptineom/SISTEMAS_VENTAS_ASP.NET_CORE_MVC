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
        public List<APLICACION> listarMenuUsuario(SqlConnection con, string sIdUsuario)
        {
            List<APLICACION> listaAplicacion = null;
            APLICACION oAplicacion = null;
            SqlDataReader reader = null;
            using (SqlCommand cmd = new SqlCommand("PA_LISTA_MENU_PROYECTO_SPA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ID_USUARIO", sIdUsuario);
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
                            oAplicacion.FLG_RAIZ = reader.GetBoolean(reader.GetOrdinal("FLG_RAIZ"));
                            oAplicacion.ROUTE_SPA = reader.IsDBNull(reader.GetOrdinal("ROUTE_SPA")) ? default(string) : reader.GetString(reader.GetOrdinal("ROUTE_SPA"));
                            oAplicacion.ICON_SPA = reader.IsDBNull(reader.GetOrdinal("ICON_SPA")) ? default(string) : reader.GetString(reader.GetOrdinal("ICON_SPA"));
                            oAplicacion.BREADCRUMS = reader.IsDBNull(reader.GetOrdinal("BREADCRUMS")) ? default(string) : reader.GetString(reader.GetOrdinal("BREADCRUMS"));
                            listaAplicacion.Add(oAplicacion);
                        }
                    }
                }
            }
            return listaAplicacion;
        }
    }
}
