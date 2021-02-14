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
    public class DaoSucursalCaja
    {
        public List<SUCURSAL_CAJA> listaSucursalCaja(SqlConnection con, string idSucursal)
        {
            List<SUCURSAL_CAJA> lista = null;
            SUCURSAL_CAJA modelo = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "SEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<SUCURSAL_CAJA>();
                        while (reader.Read())
                        {
                            modelo = new SUCURSAL_CAJA();
                            modelo.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            lista.Add(modelo);
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public List<CAJA> listaCajas(SqlConnection con, string idSucursal,ref List<SUCURSAL_CAJA> listaSucCaj)
        {
            List<CAJA> lista = null;
            CAJA modelo = null;
            List<SUCURSAL_CAJA> lSucursalCaja = null;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "CBO";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        lista = new List<CAJA>();
                        while (reader.Read())
                        {
                            modelo = new CAJA();
                            modelo.ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA"));
                            modelo.NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"));
                            lista.Add(modelo);
                        }
                    }
                    if (reader.NextResult())
                    {
                        if (reader.HasRows)
                        {
                            lSucursalCaja = new List<SUCURSAL_CAJA>();
                            while (reader.Read())
                            {
                                lSucursalCaja.Add(new SUCURSAL_CAJA()
                                {
                                    ID_CAJA = reader.GetString(reader.GetOrdinal("ID_CAJA")),
                                    NOM_CAJA = reader.GetString(reader.GetOrdinal("NOM_CAJA"))
                                });
                            }
                            listaSucCaj = lSucursalCaja;
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return lista;
        }

        public bool grabarSucursalCaja(SqlConnection con, SqlTransaction trx, SUCURSAL_CAJA oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = oModelo.ID_SUCURSAL;
                cmd.Parameters.Add("@XML_CAJAS", SqlDbType.Xml).Value = oModelo.XML_CAJAS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
        public bool anularSucursalCaja(SqlConnection con, SqlTransaction trx, string idSucursal, string idCaja, string idUsuario)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_SUCURSAL_CAJA", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = "DEL";
                cmd.Parameters.Add("@ID_SUCURSAL", SqlDbType.VarChar, 2).Value = idSucursal;
                cmd.Parameters.Add("@ID_CAJA", SqlDbType.VarChar, 2).Value = idCaja;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = idUsuario;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
