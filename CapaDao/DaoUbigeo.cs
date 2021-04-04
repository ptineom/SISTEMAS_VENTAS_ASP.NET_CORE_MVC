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
    public class DaoUbigeo
    {
        public List<UBIGEO> GetAllDepartaments(SqlConnection con)
        {
            List<UBIGEO> listaDepartamento = null;
            UBIGEO oDepartamento = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_UBIGEO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEP";
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaDepartamento = new List<UBIGEO>();
                        while (reader.Read())
                        {
                            oDepartamento = new UBIGEO();
                            oDepartamento.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            oDepartamento.UBIGEO_DEPARTAMENTO = reader.GetString(reader.GetOrdinal("UBIGEO_DEPARTAMENTO"));
                            listaDepartamento.Add(oDepartamento);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return listaDepartamento;
        }
        public List<UBIGEO> GetAllProvinces(SqlConnection con, string idDepartamento)
        {
            List<UBIGEO> listaProvincia = null;
            UBIGEO oProvincia = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_UBIGEO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "PRO";
                cmd.Parameters.Add("@ID_DEPARTAMENTO", SqlDbType.VarChar, 2).Value = idDepartamento;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaProvincia = new List<UBIGEO>();
                        while (reader.Read())
                        {
                            oProvincia = new UBIGEO();
                            oProvincia.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            oProvincia.UBIGEO_PROVINCIA = reader.GetString(reader.GetOrdinal("UBIGEO_PROVINCIA"));
                            listaProvincia.Add(oProvincia);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return listaProvincia;
        }
        public List<UBIGEO> GetAllDistricts(SqlConnection con, string idProvincia)
        {
            List<UBIGEO> listaDistrito = null;
            UBIGEO oDistrito = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_UBIGEO", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DIS";
                cmd.Parameters.Add("@ID_PROVINCIA", SqlDbType.VarChar, 4).Value = idProvincia;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        listaDistrito = new List<UBIGEO>();
                        while (reader.Read())
                        {
                            oDistrito = new UBIGEO();
                            oDistrito.ID_UBIGEO = reader.GetString(reader.GetOrdinal("ID_UBIGEO"));
                            oDistrito.UBIGEO_DISTRITO = reader.GetString(reader.GetOrdinal("UBIGEO_DISTRITO"));
                            listaDistrito.Add(oDistrito);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return listaDistrito;
        }
    }
}
