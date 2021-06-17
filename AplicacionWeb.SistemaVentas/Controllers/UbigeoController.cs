using CapaNegocio;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
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
        private readonly IUbigeoService _ubigeo;
        public UbigeoController(IUbigeoService ubigeo)
        {
            _ubigeo = ubigeo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetAllProvinces/{idDepartamento}")]
        public async Task<IActionResult> GetAllProvincesAsync(string idDepartamento)
        {
            var result = await _ubigeo.GetAllProvincesAsync(idDepartamento);

            if (result == null || result.Count == 0)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = "Debe de configurar al menos una provincia del ubigeo",
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = Constantes.NOT_FOUND
                    }
                });

            var response = new ResponseObject()
            {
                Data = result.Select(x => new
                {
                    IdProvincia = x.ID_UBIGEO,
                    NomProvincia = ViewHelper.CapitalizeAll(x.UBIGEO_PROVINCIA)
                })
            };

            return Ok(response);
        }

        [HttpGet("GetAllDistricts/{idProvincia}")]
        public async Task<IActionResult> GetAllDistrictsAsync(string idProvincia)
        {
            var result = await _ubigeo.GetAllDistrictsAsync(idProvincia);

            if (result == null || result.Count == 0)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = "Debe de configurar al menos un distrito del ubigeo",
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = Constantes.NOT_FOUND
                    }
                });

            var response = new ResponseObject()
            {
                Data = result.Select(x => new
                {
                    IdDistrito = x.ID_UBIGEO,
                    NomDistrito = ViewHelper.CapitalizeAll(x.UBIGEO_DISTRITO)
                })
            };

            return Ok(response);
        }
    }
}
