using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Seguridad
{
    public class UsuarioLogueadoModel
    {
        public string IdUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string NomRol { get; set; }
        public string IdSucursal { get; set; }
        public string NomSucursal { get; set; }
        public bool FlgCtrlTotal { get; set; }
        public string AvatarB64 { get; set; }
        public string AvatarUri { get; set; }
    }
}
