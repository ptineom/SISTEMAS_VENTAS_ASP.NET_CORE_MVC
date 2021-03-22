using CapaDao;
using Entidades;
using Helper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace CapaNegocio
{
    public class BrAplicacion
    {
        private DaoAplicacion _dao = null;
        private ResultadoOperacion _resultado = null;
        private IConfiguration _configuration;
        private Conexion _conexion = null;

        public BrAplicacion(IConfiguration configuration)
        {
            _dao = new DaoAplicacion();
            _resultado = new ResultadoOperacion();
            _configuration = configuration;
            //Conexion BD.
            _conexion = new Conexion(_configuration);
        }

        public ResultadoOperacion GetMenuByUserId(string idUsuario)
        {
            List<APLICACION> lista = null;
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    lista = _dao.GetMenuByUserId(con, idUsuario);
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
                    _resultado.SetResultado(true, "", lista);
                }
                catch (Exception ex)
                {
                    _resultado.SetResultado(false, ex.Message);
                    Elog.save(this, ex);
                }
            }
            return _resultado;
        }
    }
}
