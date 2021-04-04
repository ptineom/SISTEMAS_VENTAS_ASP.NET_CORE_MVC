using CapaNegocio;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class UbigeoController : Controller
    {
        private IResultadoOperacion _resultado;
        private IConfiguration _configuration;
        private BrUbigeo _brUbigeo;

        public UbigeoController(IResultadoOperacion resultado, IConfiguration configuration)
        {
            _configuration = configuration;
            _resultado = resultado;
            _brUbigeo = new BrUbigeo(_configuration);
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllProvinces/{idDepartamento}")]
        public async Task<IActionResult> GetAllProvincesAsync(string idDepartamento)
        {
            _resultado = await Task.Run(() => _brUbigeo.GetAllProvinces(idDepartamento));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos", Status = "Eror" });

            List<UBIGEO> lista = (List<UBIGEO>)_resultado.Data;

            _resultado.Data = lista.Select(x => new
            {
                IdProvincia = x.ID_UBIGEO,
                NomProvincia = ViewHelper.CapitalizeAll(x.UBIGEO_PROVINCIA)
            });

            return Ok(_resultado);
        }

        [HttpGet("GetAllDistricts/{idProvincia}")]
        public async Task<IActionResult> GetAllDistrictsAsync(string idProvincia)
        {
            _resultado = await Task.Run(() => _brUbigeo.GetAllDistricts(idProvincia));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos", Status = "Eror" });

            List<UBIGEO> lista = (List<UBIGEO>)_resultado.Data;


            _resultado.Data = lista.Select(x => new
            {
                IdDistrito = x.ID_UBIGEO,
                NomDistrito = ViewHelper.CapitalizeAll(x.UBIGEO_DISTRITO)
            });

            return Ok(_resultado);
        }
    }
}
