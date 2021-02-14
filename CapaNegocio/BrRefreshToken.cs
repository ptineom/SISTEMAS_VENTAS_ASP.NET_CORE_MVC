using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using CapaDao;
using Entidades;
using Helper;

namespace CapaNegocio
{
    public class BrRefreshToken
    {
        DaoRefreshToken dao = null;
        ResultadoOperacion oResultado = null;
        public BrRefreshToken()
        {
            dao = new DaoRefreshToken();
            oResultado = new ResultadoOperacion();
        }

        public ResultadoOperacion grabarRefreshToken(REFRESH_TOKEN oModelo)
        {
            SqlTransaction trx = null;
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    trx = con.BeginTransaction();
                    dao.grabarRefreshToken(con, trx, oModelo);
                    oResultado.SetResultado(true, Helper.Constantes.sMensajeGrabadoOk);
                    trx.Commit();
                }
                catch (Exception ex)
                {
                    oResultado.SetResultado(false, ex.Message.ToString());
                    trx.Rollback();
                    Elog.save(this, ex);
                }
            }
            return oResultado;
        }

        public ResultadoOperacion refreshTokenPorCodigo(string idRefreshToken)
        {
            using (SqlConnection con = new SqlConnection(Conexion.sConexion))
            {
                try
                {
                    con.Open();
                    REFRESH_TOKEN modelo = dao.refreshTokenPorCodigo(con, idRefreshToken);

                    oResultado.SetResultado(true, "", modelo);
                }
                catch (Exception ex)
                {
                    Elog.save(this, ex);
                    oResultado.SetResultado(false, ex.Message);
                }
            }
            return oResultado;
        }
    }
}
