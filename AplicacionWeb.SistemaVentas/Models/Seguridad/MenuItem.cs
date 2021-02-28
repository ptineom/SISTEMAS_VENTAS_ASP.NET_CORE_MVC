using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Seguridad
{
    public class MenuItem
    {
        public int id { get; set; }
        public string label { get; set; }
        public string icon { get; set; }
        public string route { get; set; }
        public bool flgRaiz { get; set; }
        public List<MenuItem> children { get; set; }
        public string breadcrumbs { get; set; }
        public bool flgHome { get; set; }
        public bool flgFormulario { get; set; }
    }
}
