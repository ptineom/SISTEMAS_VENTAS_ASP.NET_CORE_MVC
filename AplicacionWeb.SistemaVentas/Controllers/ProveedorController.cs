using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.Seguridad;
using AplicacionWeb.SistemaVentas.Servicios.Seguridad;
using CapaNegocio;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class ProveedorController : Controller
    {
        IResultadoOperacion _resultado = null;
        BrProveedor _brProveedor = null;
        IHttpContextAccessor _accessor = null;
        string _idUsuario = string.Empty;
        string _idSucursal = string.Empty;

        public ProveedorController(IResultadoOperacion resultado, IHttpContextAccessor accessor)
        {
            _resultado = resultado;
            _brProveedor = new BrProveedor();
            _accessor = accessor;
            UsuarioLogueadoModel usuario = new Session(_accessor).GetUserLogged();
            _idUsuario = usuario.IdUsuario;
            _idSucursal = usuario.IdSucursal;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetByDocument/{idTipoDocumento}/{nroDocumento}")]
        public async Task<IActionResult> GetByDocumentAsync(int idTipoDocumento, string nroDocumento)
        {
            _resultado = await Task.Run(() => _brProveedor.GetByDocument(idTipoDocumento, nroDocumento));

            if(!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if(_resultado.Data == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = "Proveedor no encontrado", Status = "Error" });

            PROVEEDOR proveedor = (PROVEEDOR)_resultado.Data;

            _resultado.Data = new
            {
                IdProveedor = proveedor.ID_PROVEEDOR,
                IdTipoDocumento = proveedor.ID_TIPO_DOCUMENTO,
                NroDocumento = proveedor.NRO_DOCUMENTO,
                NomProveedor = proveedor.NOM_PROVEEDOR,
                DirProveedor = proveedor.DIR_PROVEEDOR
            };

            return Ok(_resultado);
        }

        [HttpGet("GetAllByFilters/{tipoFiltro?}/{filtro?}/{flgConInactivos?}")]
        public async Task<IActionResult> GetAllByFilters(string tipoFiltro, string filtro, bool flgConInactivos = false)
        {
            _resultado = await Task.Run(() => _brProveedor.GetAllByFilters(tipoFiltro, filtro, flgConInactivos));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos", Status = "Eror" });

            List<PROVEEDOR> lista = (List<PROVEEDOR>)_resultado.Data;


            _resultado.Data = lista.Select(x => new
            {
                IdCliente = x.ID_PROVEEDOR,
                NomCliente = ViewHelper.CapitalizeAll(x.NOM_PROVEEDOR),
                NomTipoDocumento = x.ABREVIATURA,
                NroDocumento = x.NRO_DOCUMENTO,
                DirCliente = ViewHelper.CapitalizeAll(x.DIR_PROVEEDOR),
                IdTipoDocumento = x.ID_TIPO_DOCUMENTO
            });

            return Ok(_resultado);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] ProveedorRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            _resultado = await Task.Run(() => _brProveedor.Register(new PROVEEDOR()
            {
                ID_TIPO_DOCUMENTO = request.IdTipoDocumento,
                NRO_DOCUMENTO = request.NroDocumento,
                NOM_PROVEEDOR = request.RazonSocial,
                CONTACTO = request.Contacto,
                EMAIL_PROVEEDOR = request.Email,
                TEL_PROVEEDOR = request.Telefono,
                DIR_PROVEEDOR = request.Direccion,
                ID_UBIGEO = request.IdDistrito,
                ID_USUARIO_REGISTRO = _idUsuario,
                OBS_PROVEEDOR = request.Observacion,
                FLG_INACTIVO = request.FlgInactivo,
                ACCION = "INS"
            }));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            return Ok(_resultado);
        }
    }
}
