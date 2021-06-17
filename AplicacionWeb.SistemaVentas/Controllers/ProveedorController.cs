using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class ProveedorController : Controller
    {
        private readonly IProveedorService _proveedorService;
        private readonly ISessionIdentity _sessionIdentity;
        string _idUsuario = string.Empty;

        public ProveedorController(IProveedorService proveedorService, ISessionIdentity sessionIdentity)
        {
            _proveedorService = proveedorService;
            _sessionIdentity = sessionIdentity;

            UsuarioIdentityViewModel usuario = _sessionIdentity.GetUserLogged();
            _idUsuario = usuario.IdUsuario;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetByDocument/{idTipoDocumento}/{nroDocumento}")]
        public async Task<IActionResult> GetByDocumentAsync(int idTipoDocumento, string nroDocumento)
        {
            var result = await _proveedorService.GetByDocument(new PROVEEDOR()
            {
                ID_TIPO_DOCUMENTO = idTipoDocumento,
                NRO_DOCUMENTO = nroDocumento
            });

            if (result == null)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = "El proveedor no existe.",
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = Constantes.NOT_FOUND
                    }
                });

            var response = new ResponseObject()
            {
                Data = new
                {
                    IdProveedor = result.ID_PROVEEDOR,
                    IdTipoDocumento = result.ID_TIPO_DOCUMENTO,
                    NroDocumento = result.NRO_DOCUMENTO,
                    NomProveedor = result.NOM_PROVEEDOR,
                    DirProveedor = result.DIR_PROVEEDOR
                }
            };

            return Ok(response);
        }

        [HttpGet("GetById/{idProveedor}")]
        public async Task<IActionResult> GetByIdAsync(string idProveedor)
        {
            var result = await _proveedorService.GetByIdAsync(idProveedor);

            if (result == null)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = "El proveedor no existe.",
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = Constantes.NOT_FOUND
                    }
                });

            var response = new ResponseObject()
            {
                Data = new
                {
                    IdProveedor = result.ID_PROVEEDOR,
                    IdTipoDocumento = result.ID_TIPO_DOCUMENTO,
                    NroDocumento = result.NRO_DOCUMENTO,
                    NomProveedor = result.NOM_PROVEEDOR,
                    DirProveedor = result.DIR_PROVEEDOR,
                    TelProveedor = result.TEL_PROVEEDOR,
                    EmailProveedor = result.EMAIL_PROVEEDOR,
                    IdUbigeo = result.ID_UBIGEO,
                    ObsProveedor = result.OBS_PROVEEDOR,
                    Contacto = result.CONTACTO,
                    FlgInactivo = result.FLG_INACTIVO
                }
            };

            return Ok(response);
        }

        [HttpGet("GetAll/{tipoFiltro?}/{filtro?}/{flgConInactivos?}")]
        public async Task<IActionResult> GetAllAsync(string tipoFiltro, string filtro, bool flgConInactivos = false)
        {
            var result = await _proveedorService.GetAllAsync(new PROVEEDOR()
            {
                ID_PROVEEDOR = tipoFiltro == "codigo" ? filtro : string.Empty,
                NOM_PROVEEDOR = tipoFiltro == "descripcion" ? filtro : string.Empty,
                NRO_DOCUMENTO = tipoFiltro == "numDoc" ? filtro : string.Empty,
                FLG_INACTIVO = flgConInactivos
            });

            if (result == null || result.Count == 0)
                return NotFound(new ResponseObject()
                {
                    Success = false,
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
                    IdCliente = x.ID_PROVEEDOR,
                    NomCliente = ViewHelper.CapitalizeAll(x.NOM_PROVEEDOR),
                    NomTipoDocumento = x.ABREVIATURA,
                    NroDocumento = x.NRO_DOCUMENTO,
                    DirProveedor = ViewHelper.CapitalizeAll(x.DIR_PROVEEDOR),
                    IdTipoDocumento = x.ID_TIPO_DOCUMENTO
                })
            };

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] ProveedorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = JsonSerializer.Serialize(ModelState)
                    }
                });
            }

            var result = await _proveedorService.RegisterAsync(new PROVEEDOR()
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
            });

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = result.ErrorDetails.Message
                    }
                });
            }

            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] ProveedorRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = JsonSerializer.Serialize(ModelState)
                    }
                });
            }

            var result = await _proveedorService.UpdateAsync(new PROVEEDOR()
            {
                ID_PROVEEDOR = request.IdProveedor,
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
                ACCION = "UPD"
            });

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = result.ErrorDetails.Message
                    }
                });
            }

            return Ok(result);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync(string idProveedor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = JsonSerializer.Serialize(ModelState)
                    }
                });
            }

            var result = await _proveedorService.DeleteAsync(new PROVEEDOR()
            {
                ID_PROVEEDOR = idProveedor,
                ID_USUARIO_REGISTRO = _idUsuario
            });

            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status500InternalServerError,
                        Message = result.ErrorDetails.Message
                    }
                });
            }

            return Ok(result);
        }
    }
}
