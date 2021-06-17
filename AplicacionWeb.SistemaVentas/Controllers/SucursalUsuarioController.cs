using AplicacionWeb.SistemaVentas.Models;
using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using CapaNegocio;
using Entidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        BrSucursalUsuario _brSucursalUsuario = null;
        IHttpContextAccessor _accessor = null;
        string _idUsuario = string.Empty;
        private readonly ISessionIdentity _sessionIdentity;

        public SucursalUsuarioController(IResultadoOperacion resultado, IHttpContextAccessor accessor, ISessionIdentity sessionIdentity)
        {
            _resultado = resultado;
            _brSucursalUsuario = new BrSucursalUsuario();
            _accessor = accessor;

            _sessionIdentity = sessionIdentity;
            UsuarioIdentityViewModel usuario = _sessionIdentity.GetUserLogged();
            _idUsuario = usuario.IdUsuario;
        }

        [Route("[action]")]
        public IActionResult IndexSeleccionSucursal()
        {
             //Lista de sucursales por usuario.
            _resultado = _brSucursalUsuario.GetAllByUserId(_idUsuario);

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            List<SucursalViewModel> sucursales = ((List<SUCURSAL>)_resultado.Data).Select(x => new SucursalViewModel() { 
                IdSucursal = x.ID_SUCURSAL,
                NomSucursal = x.NOM_SUCURSAL
            }).ToList<SucursalViewModel>();

            sucursales.Insert(0, new SucursalViewModel()
            {
                IdSucursal="-1",
                NomSucursal = "---SELECCIONE---"
            });

            return View(sucursales);
        }

    }
}
