using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Response
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public bool FlgRaiz { get; set; }
        public List<MenuItemViewModel> Children { get; set; }
        public string Breadcrumbs { get; set; }
        public bool FlgHome { get; set; }
        public bool FlgFormulario { get; set; }
    }
}
