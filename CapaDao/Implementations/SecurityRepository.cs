using CapaDao.Contracts;
using Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace CapaDao.Implementations
{
    public class SecurityRepository : ISecurityRepository
    {
        private readonly IConnection _sqlConnection;
        public SecurityRepository(IConnection db)
        {
            _sqlConnection = db;
        }

        public async Task<USUARIO> UserValidateAsync(string userId, string password, bool noValidar = false)
        {
            USUARIO model = null;
            using (SqlCommand cmd = new SqlCommand("PA_VALIDA_USUARIO", _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 50).Value = userId;
                cmd.Parameters.Add("@CLAVE", SqlDbType.VarChar, 64).Value = password;
                cmd.Parameters.Add("@NO_VALIDAR", SqlDbType.Bit).Value = noValidar;

                SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult);
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model = new USUARIO();
                            model.ID_USUARIO = reader.GetString(reader.GetOrdinal("ID_USUARIO"));
                            model.NOM_USUARIO = reader.GetString(reader.GetOrdinal("NOM_USUARIO"));
                            model.NOM_ROL = reader.GetString(reader.GetOrdinal("NOM_PERFIL"));
                            model.FOTO = reader.IsDBNull(reader.GetOrdinal("FOTO")) ? default(string) : reader.GetString(reader.GetOrdinal("FOTO"));
                            model.ID_SUCURSAL = reader.IsDBNull(reader.GetOrdinal("ID_SUCURSAL")) ? default(string) : reader.GetString(reader.GetOrdinal("ID_SUCURSAL"));
                            model.COUNT_SEDES = reader.GetInt32(reader.GetOrdinal("COUNT_SEDES"));
                            model.NOM_SUCURSAL = reader.IsDBNull(reader.GetOrdinal("NOM_SUCURSAL")) ? default(string) : reader.GetString(reader.GetOrdinal("NOM_SUCURSAL"));
                            model.FLG_CTRL_TOTAL = reader.GetBoolean(reader.GetOrdinal("FLG_CTRL_TOTAL"));
                            model.ID_GRUPO_ACCESO = reader.GetInt32(reader.GetOrdinal("ID_GRUPO_ACCESO"));
                            model.ID_EMPLEADO = reader.GetString(reader.GetOrdinal("ID_EMPLEADO"));
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return model;
        }
        public async Task<List<APLICACION>> GetMenuByUserId(string userId)
        {
            List<APLICACION> list = null;
            using (SqlCommand cmd = new SqlCommand("PA_LISTA_MENU_PROYECTO", _sqlConnection.DbConnection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@ID_USUARIO", SqlDbType.VarChar, 2).Value = userId;

                SqlDataReader reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleResult);
                if (reader != null)
                {
                    if (reader.HasRows)
                    {
                        list = new List<APLICACION>();
                        while (reader.Read())
                        {
                            list.Add(new APLICACION()
                            {
                                ID_APLICACION = reader.GetInt32(reader.GetOrdinal("ID_APLICACION")),
                                NOM_APLICACION = reader.GetString(reader.GetOrdinal("NOM_APLICACION")),
                                ID_APLICACION_PADRE = reader.IsDBNull(reader.GetOrdinal("ID_APLICACION_PADRE")) ? default(int) : reader.GetInt32(reader.GetOrdinal("ID_APLICACION_PADRE")),
                                FLG_FORMULARIO = reader.GetBoolean(reader.GetOrdinal("FLG_FORMULARIO")),
                                NOM_FORMULARIO = reader.GetString(reader.GetOrdinal("NOM_FORMULARIO")),
                                NOM_CONTROLADOR = reader.GetString(reader.GetOrdinal("NOM_CONTROLADOR")),
                                FLG_RAIZ = reader.GetBoolean(reader.GetOrdinal("FLG_RAIZ")),
                                ICON = reader.IsDBNull(reader.GetOrdinal("ICON")) ? string.Empty : reader.GetString(reader.GetOrdinal("ICON")),
                                BREADCRUMS = reader.IsDBNull(reader.GetOrdinal("BREADCRUMS")) ? string.Empty : reader.GetString(reader.GetOrdinal("BREADCRUMS")),
                                FLG_ACCESO_DIRECTO = reader.GetBoolean(reader.GetOrdinal("FLG_ACCESO_DIRECTO")),
                                BG_COLOR = reader.IsDBNull(reader.GetOrdinal("BG_COLOR")) ? string.Empty : reader.GetString(reader.GetOrdinal("BG_COLOR"))
                            });
                        }
                    }
                }
                reader.Close();
                reader.Dispose();
            }
            return list;
        }
    }
}
