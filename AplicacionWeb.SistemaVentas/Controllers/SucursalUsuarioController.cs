using AplicacionWeb.SistemaVentas.Models;
using AplicacionWeb.SistemaVentas.Servicios.Seguridad;
using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class SucursalUsuarioController : Controller
    {
        IResultadoOperacion _resultado = null;
        BrSucursalUsuario brSucursalUsuario = null;
        IHttpContextAccessor _httpContextAccessor = null;
        string _idUsuario = string.Empty;

        public SucursalUsuarioController(IResultadoOperacion resultado, IHttpContextAccessor httpContextAccessor)
        {
            _resultado = resultado;
            brSucursalUsuario = new BrSucursalUsuario();
            _httpContextAccessor = httpContextAccessor;
            _idUsuario = new Session(_httpContextAccessor).obtenerUsuarioLogueado().idUsuario;
        }

        [Route("[action]")]
        public IActionResult IndexSeleccionSucursal()
        {
             //Lista de sucursales por usuario.
            _resultado = brSucursalUsuario.listaSucursalPorUsuario(_idUsuario);

            if (!_resultado.bResultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.sMensaje, Status = "Error" });

            List<SucursalViewModel> sucursales = ((List<SUCURSAL>)_resultado.data).Select(x => new SucursalViewModel() { 
                idSucursal = x.ID_SUCURSAL,
                nomSucursal = x.NOM_SUCURSAL
            }).ToList<SucursalViewModel>();

            sucursales.Insert(0, new SucursalViewModel()
            {
                idSucursal="",
                nomSucursal = "---SELECCIONE---"
            });

            return View(sucursales);
        }

    }
}
