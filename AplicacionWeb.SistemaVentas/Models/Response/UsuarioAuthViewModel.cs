using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Response
{
    //Modelo obtenido de la verificación del usuario a la bd.
    public class UsuarioAuthViewModel
    {
        public string IdUsuario { get; set; }
        public string NomUsuario { get; set; }
        public string NomRol { get; set; }
        public string Foto { get; set; }
        public string IdSucursal { get; set; }
        public string NomSucursal { get; set; }
        public int CountSucursales { get; set; }
        public bool FlgCtrlTotal { get; set; }
        public int IdGrupoAcceso { get; set; }
    }
}
