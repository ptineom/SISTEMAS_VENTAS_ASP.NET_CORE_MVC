using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Models.Response
{
    public class UbigeoSelectViewModel
    {

    }
    public class DepartamentoSelectViewModel
    {
        public string IdDepartamento { get; set; }
        public string NomDepartamento { get; set; }
    }

    public class ProvinciaSelectViewModel
    {
        public string IdProvincia { get; set; }
        public string NomProvincia { get; set; }
    }
    public class DistritoSelectViewModel
    {
        public string IdDistrito { get; set; }
        public string NomDistrito { get; set; }
    }
}
