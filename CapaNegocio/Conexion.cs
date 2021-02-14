using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Helper;
namespace CapaNegocio
{
    public class Conexion
    {
        public static string sConexion
        {
            get
            {
                return ViewHelper.getValueConfiguration("CONNECTION_STRINGS:CONEXION_SQL");
            }
        }
    }
}
