using CapaDao;
using Entidades;
using Helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CapaNegocio
{
    public class BrAplicacion
    {
        DaoAplicacion dao = null;
        ResultadoOperacion oResultado = null;
        public BrAplicacion()
        {
            dao = new DaoAplicacion();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion listarMenuUsuario(string idUsuario)
        {
            List<APLICACION> lista = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    lista = dao.listarMenuUsuario(con, idUsuario);
                    if (lista != null)
                    {
                        //Agregado el menú home
                        lista.Insert(1, new APLICACION()
                        {
                            ID_APLICACION_PADRE = 1,
                            ID_APLICACION = lista.Select(x => x.ID_APLICACION).Max(),
                            NOM_APLICACION = "Home",
                            FLG_FORMULARIO = true,
                            ICON_SPA = "mdi-home",
                            ROUTE_SPA = "Home",
                            FLG_RAIZ = false,
                            BREADCRUMS = "Home",
                            FLG_HOME = true
                        });
                    }
                    oResultado.SetResultado(true, "", lista);
                }
                catch (Exception ex)
                {
                    oResultado.SetResultado(false, ex.Message);
                    Elog.save(this, ex);
                }
            }
            return oResultado;
        }
    }
}
