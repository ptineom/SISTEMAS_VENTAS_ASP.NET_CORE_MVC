using AplicacionWeb.SistemaVentas.Hubs;
using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using CapaNegocio;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class CajaAperturaController : Controller
    {
        private IHubContext<CambiarEstadoCajaHub> _hubContext;
        private readonly ICajaAperturaService _cajaAperturaService;
        private readonly ISessionIdentity _sessionIdentity;
        private readonly IMonedaService _monedaService;
        private readonly ISucursalCajaUsuarioService _sucursalCajaUsuarioService;
        private string _idSucursal;
        private string _idUsuario;

        public CajaAperturaController(ICajaAperturaService cajaAperturaService, ISessionIdentity sessionIdentity,
            IMonedaService monedaService, ISucursalCajaUsuarioService sucursalCajaUsuarioService,
            IHubContext<CambiarEstadoCajaHub> hubContext)
        {
            _cajaAperturaService = cajaAperturaService;
            _monedaService = monedaService;
            _sucursalCajaUsuarioService = sucursalCajaUsuarioService;

            _sessionIdentity = sessionIdentity;

            UsuarioIdentityViewModel usuario = _sessionIdentity.GetUserLogged();
            _idUsuario = usuario.IdUsuario;
            _idSucursal = usuario.IdSucursal;

            //Hub para signalr
            _hubContext = hubContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("GetData")]
        public async Task<IActionResult> GetDataAsync()
        {
            //Lista de monedas.
            List<MONEDA> listMonedas = await _monedaService.GetAllAsync();
            //Moneda local.
            listMonedas = listMonedas.Where(x => x.FLG_LOCAL).ToList<MONEDA>();

            //Lista de cajas.
            List<CAJA> listCajas = await _sucursalCajaUsuarioService.GetAllBoxes(new SUCURSAL_CAJA_USUARIO()
            {
                ID_SUCURSAL = _idSucursal,
                ID_USUARIO = _idUsuario
            });

            if (listMonedas == null || listMonedas.Count == 0)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.NOT_FOUND,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Debe de configurar la moneda local."
                    }
                });

            if (listCajas == null || listCajas.Count == 0)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.NOT_FOUND,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "Debe de configurar al menos una caja."
                    }
                });

            var response = new ResponseObject()
            {
                Data = new
                {
                    moneda = listMonedas.Select(y => new
                    {
                        IdMoneda = y.ID_MONEDA,
                        NomMoneda = ViewHelper.CapitalizeFirstLetter(y.NOM_MONEDA),
                        SgnMoneda = y.SGN_MONEDA
                    }).FirstOrDefault(),
                    listCajas = listCajas.Select(x => new
                    {
                        IdCaja = x.ID_CAJA,
                        NomCaja = ViewHelper.CapitalizeFirstLetter(x.NOM_CAJA)
                    }).ToList(),
                }
            };
            return Ok(response);
        }

        [HttpGet("GetStateBox")]
        public async Task<IActionResult> GetStateBoxAsync()
        {
            //Obtenemos el estado de la caja
            var result = await _cajaAperturaService.GetBoxStateAsync(new CAJA_APERTURA()
            {
                ID_SUCURSAL = _idSucursal,
                ID_USUARIO = _idUsuario
            });

            ResponseObject response = new ResponseObject();

            //En caso la caja este cerrado devolverá el objeto cajaApertura null.
            if (result != null)
            {
                //var qq2 = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                //var qq3 = DateTime.Now.ToString("dd/MM/yyyy hh:mm tt");
                response.Data = new
                {
                    IdCaja = result.ID_CAJA,
                    Correlativo = result.CORRELATIVO,
                    FechaApertura = result.FECHA_APERTURA.ToString("dd/MM/yyyy HH:mm:ss"),
                    MontoApertura = result.MONTO_APERTURA,
                    IdMoneda = result.ID_MONEDA,
                    SgnMoneda = result.SGN_MONEDA,
                    FlgReaperturado = result.FLG_REAPERTURADO,
                    Item = result.ITEM,
                    FlgCierreDiferido = result.FLG_CIERRE_DIFERIDO,
                    FechaCierre = result.FECHA_CIERRE,
                    HoraCierre = result.HORA_CIERRE,
                    NomCaja = result.NOM_CAJA
                };
            }

            return Ok(response);
        }

        [HttpGet("GetTotalsByUserId/{idCaja}/{correlativo}")]
        public async Task<IActionResult> GetTotalsByUserIdAsync(string idCaja, int correlativo)
        {
            var result = await _cajaAperturaService.GetTotalsByUserIdAsync(new CAJA_APERTURA()
            {
                ID_SUCURSAL = _idSucursal,
                ID_CAJA = idCaja,
                ID_USUARIO = _idUsuario,
                CORRELATIVO = correlativo
            });

            if (result == null)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = Constantes.NOT_FOUND
                    }
                });

            DINERO_EN_CAJA dineroEnCaja = (DINERO_EN_CAJA)result;

            var response = new ResponseObject()
            {
                Data = new
                {
                    MontoAperturaCaja = dineroEnCaja.MONTO_APERTURA_CAJA,
                    MontoCobradoContado = dineroEnCaja.MONTO_COBRADO_CONTADO,
                    MontoCobradoCredito = dineroEnCaja.MONTO_COBRADO_CREDITO,
                    MontoCajaOtrosIngreso = dineroEnCaja.MONTO_CAJA_OTROS_INGRESO,
                    MontoCajaSalida = dineroEnCaja.MONTO_CAJA_SALIDA,
                    MontoTotal = dineroEnCaja.MONTO_TOTAL
                }
            };

            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CajaAbiertaRequest request)
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

            var result = await _cajaAperturaService.RegisterAsync(new CAJA_APERTURA()
            {
                ACCION = request.Accion,
                ID_SUCURSAL = _idSucursal,
                ID_CAJA = request.IdCaja,
                ID_USUARIO = _idUsuario,
                MONTO_APERTURA = request.MontoApertura,
                MONTO_COBRADO = request.MontoTotal,
                FECHA_CIERRE = request.FechaCierre,
                ID_MONEDA = request.IdMoneda,
                CORRELATIVO = request.Correlativo,
                ID_USUARIO_REGISTRO = _idUsuario,
                FLG_REAPERTURADO = request.flgReaperturado,
                ITEM = request.Item,
                FLG_CIERRE_DIFERIDO = request.flgCierreDiferido
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

            if (result.Data != null)
            {
                CAJA_APERTURA cajaApertura = (CAJA_APERTURA)result.Data;

                result.Data = new
                {
                    IdCaja = cajaApertura.ID_CAJA,
                    Correlativo = cajaApertura.CORRELATIVO,
                    FechaApertura = cajaApertura.FECHA_APERTURA,
                    MontoApertura = cajaApertura.MONTO_APERTURA,
                    IdMoneda = cajaApertura.ID_MONEDA,
                    SgnMoneda = cajaApertura.SGN_MONEDA,
                    FlgReaperturado = cajaApertura.FLG_REAPERTURADO,
                    Item = cajaApertura.ITEM
                };
            }

            return Ok(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string idCaja, [FromQuery] string idUsuario,
            [FromQuery] string fechaInicial, [FromQuery] string fechaFinal)
        {
            var result = await _cajaAperturaService.GetAllAsync(new CAJA_APERTURA()
            {
                ID_SUCURSAL = _idSucursal,
                ID_CAJA = idCaja,
                ID_USUARIO = idUsuario,
                FECHA_INICIAL = string.IsNullOrEmpty(fechaInicial) ? null : Convert.ToDateTime(fechaInicial),
                FECHA_FINAL = string.IsNullOrEmpty(fechaFinal) ? null : Convert.ToDateTime(fechaFinal)
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
                    NomUsuario = ViewHelper.CapitalizeAll(x.NOM_USUARIO),
                    NomCaja = ViewHelper.CapitalizeAll(x.NOM_CAJA),
                    FechaApertura = x.FECHA_APERTURA.ToString("dd/MM/yyyy HH:mm"),
                    FechaCierre = x.FECHA_CIERRE.HasValue? Convert.ToDateTime(x.FECHA_APERTURA).ToString("dd/MM/yyyy HH:mm") : "",
                    SgnMoneda = x.SGN_MONEDA,
                    MontoApertura = x.MONTO_APERTURA,
                    MontoTotal = x.MONTO_COBRADO,
                    FlgCierre = x.FLG_CIERRE,
                    IdUsuario = x.ID_USUARIO,
                    IdCaja = x.ID_CAJA,
                    Correlativo = x.CORRELATIVO,
                    FlgReaperturado = x.FLG_REAPERTURADO
                })
            };

            return Ok(response);
        }

        [Route("[action]")]
        public async Task<IActionResult> ReaperturaCajaIndex()
        {
            //Lista de cajas por sucursal.
            List<CAJA> cajas = await _sucursalCajaUsuarioService.GetAllBoxes(new SUCURSAL_CAJA_USUARIO()
            {
                ID_SUCURSAL = _idSucursal
            });

            //Lista de usuarios por sucursal.
            List<USUARIO> usuarios = await _sucursalCajaUsuarioService.GetAllUsersByCampusId(_idSucursal);

            List<CajaSelectViewModel> listCajas = null; ;
            List<UsuarioSelectViewModel> listUsuarios = null;

            if (cajas != null)
            {
                listCajas = cajas.Select(x => new CajaSelectViewModel()
                {
                    IdCaja = x.ID_CAJA,
                    NomCaja = x.NOM_CAJA
                }).ToList();
            };

            if (usuarios != null)
            {
                listUsuarios = usuarios.Select(x => new UsuarioSelectViewModel()
                {
                    IdUsuario = x.ID_USUARIO,
                    NomUsuario = x.NOM_USUARIO
                }).ToList();
            };

            ViewBag.listCajas = listCajas;
            ViewBag.listUsuarios = listUsuarios;

            return View();
        }

        [HttpPost("ReopenBox")] 
        public async Task<IActionResult> ReopenBoxAsync([FromBody] ReaperturarCajaRequest request)
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

            var result = await _cajaAperturaService.ReopenBoxAsync(new CAJA_APERTURA()
            {
                ID_SUCURSAL = _idSucursal,
                ID_CAJA = request.IdCaja,
                ID_USUARIO = request.IdUsuario,
                CORRELATIVO = request.Correlativo,
                ID_USUARIO_REGISTRO = _idUsuario,
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

            //Enviamos el mensaje signalr al usuario indicado
            await _hubContext.Clients.User(request.IdUsuario).SendAsync("actualizarEstadoCaja");

            return Ok(result);
        }
    }
}
