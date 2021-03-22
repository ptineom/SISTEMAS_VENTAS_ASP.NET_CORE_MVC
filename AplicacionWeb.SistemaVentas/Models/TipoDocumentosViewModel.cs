using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models
{
    public class TipoDocumentosViewModel
    {
        public int IdTipoDocumento { get; set; }
        public string NomTipoDocumento { get; set; }
        public string Abreviatura { get; set; }
        public int MaxDigitos { get; set; }
        public bool FlgRuc { get; set; }
    }
}
