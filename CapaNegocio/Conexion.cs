using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Helper;
using System.Data.SqlClient;

namespace CapaNegocio
{
    public class Conexion
    {
        private IConfiguration _configuration;
        public Conexion(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string getConexion
        {
            get
            {
                //La conexión la obtenemos del appsetting.json
                var sqlConnectionStringBuilder = new SqlConnectionStringBuilder(_configuration.GetConnectionString("ConnectionSql"));

                //Las credenciales las obtenemos del administrador de secretos.
                sqlConnectionStringBuilder.UserID = _configuration["ConnectionStrings:Credentials:Sql:UserId"];
                sqlConnectionStringBuilder.Password = _configuration["ConnectionStrings:Credentials:Sql:Password"];

                return sqlConnectionStringBuilder.ConnectionString;
            }
        }

        public static string sConexion
        {
            get
            {
                return ViewHelper.getValueConfiguration("CONNECTION_STRINGS:CONEXION_SQL");
            }
        }
    }
}
