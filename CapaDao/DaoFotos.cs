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
    public class DaoFotos
    {
        public bool grabarFotos(SqlConnection con, SqlTransaction trx, FOTOS oModelo)
        {
            bool bExito;
            using (SqlCommand cmd = new SqlCommand("PA_MANT_FOTOS", con, trx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@ACCION", SqlDbType.VarChar, 3).Value = oModelo.ACCION;
                cmd.Parameters.Add("@ID_ELEMENTO", SqlDbType.VarChar, 30).Value = oModelo.ID_ELEMENTO;
                cmd.Parameters.Add("@ID_TIPO_FOTO", SqlDbType.VarChar, 30).Value = oModelo.ID_TIPO_FOTO;
                cmd.Parameters.Add("@JSON_FOTOS", SqlDbType.VarChar, -1).Value = string.IsNullOrEmpty(oModelo.JSON_FOTOS) ? (object)DBNull.Value: oModelo.JSON_FOTOS;
                cmd.Parameters.Add("@ID_USUARIO_REGISTRO", SqlDbType.VarChar, 20).Value = oModelo.ID_USUARIO_REGISTRO;
                cmd.ExecuteNonQuery();
                bExito = true;
            }
            return bExito;
        }
    }
}
