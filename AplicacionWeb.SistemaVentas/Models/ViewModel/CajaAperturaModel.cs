using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.ViewModel
{
    public class CajaAperturaModel
    {
        public string IdCaja { get; set; }
        public int Correlativo { get; set; }
        public string FechaApertura { get; set; }
        public decimal MontoApertura { get; set; }
        public string IdMoneda { get; set; }
        public string SgnMoneda { get; set; }
        public bool FlgReaperturado { get; set; }
        public int Item { get; set; }
    }
}
