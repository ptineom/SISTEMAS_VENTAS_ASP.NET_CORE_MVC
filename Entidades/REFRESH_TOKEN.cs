using System;
using System.Collections.Generic;
using System.Text;

namespace Entidades
{
    public class REFRESH_TOKEN
    {
        public string ID_REFRESH_TOKEN { get; set; }
        public int TIEMPO_EXPIRACION_MINUTOS { get; set; }
        public string ID_USUARIO_TOKEN { get; set; }
        public string IP_ADDRESS { get; set; }
        public string ID_USUARIO_REGISTRO { get; set; }
        public DateTime FEC_CREACION_UTC { get; set; }
        public DateTime FEC_EXPIRACION_UTC { get; set; }
        public string JSON_CLAIMS { get; set; }
    }
}
