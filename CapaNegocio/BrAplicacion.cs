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
                        int idMax = (lista.Select(x => x.ID_APLICACION).Max() + 1);
                        lista.Insert(1, new APLICACION()
                        {
                            ID_APLICACION_PADRE = 1,
                            ID_APLICACION = idMax,
                            NOM_APLICACION = "Home",
                            FLG_FORMULARIO = true,
                            NOM_FORMULARIO = "Index",
                            ICON = "bi bi-house-fill",
                            NOM_CONTROLADOR = "Home",
                            FLG_RAIZ = false,
                            BREADCRUMS = $"{idMax}|Home"
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
