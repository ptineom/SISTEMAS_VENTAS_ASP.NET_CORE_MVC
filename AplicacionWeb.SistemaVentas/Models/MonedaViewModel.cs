﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models
{
    public class MonedaViewModel
    {
        public string IdMoneda { get; set; }
        public string NomMoneda { get; set; }
        public bool FlgLocal { get; set; }
        public string SgnMoneda { get; set; }
    }
}
