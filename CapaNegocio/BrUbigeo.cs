using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidades;
using CapaDao;
using Helper;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CapaNegocio
{
    public class BrUbigeo
    {
        private DaoUbigeo _dao = null;
        private ResultadoOperacion _resultado = null;
        private IConfiguration _configuration = null;
        private Conexion _conexion = null;

        public BrUbigeo(IConfiguration configuration)
        {
            _dao = new DaoUbigeo();
            _resultado = new ResultadoOperacion();
            _configuration = configuration;
            _conexion = new Conexion(_configuration);
        }

        public ResultadoOperacion GetAllDepartaments()
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    List<UBIGEO> lista = _dao.GetAllDepartaments(con);

                    _resultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }
        public ResultadoOperacion GetAllProvinces(string idDepartamento)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    List<UBIGEO> lista = _dao.GetAllProvinces(con, idDepartamento);

                    _resultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }
        public ResultadoOperacion GetAllDistricts(string idProvincia)
        {
            using (SqlConnection con = new SqlConnection(_conexion.getConexion))
            {
                try
                {
                    con.Open();
                    List<UBIGEO> lista = _dao.GetAllDistricts(con, idProvincia);

                    _resultado.SetResultado(true, lista);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    _resultado.SetResultado(false, ex.Message);
                }
            }
            return _resultado;
        }
    }
}
