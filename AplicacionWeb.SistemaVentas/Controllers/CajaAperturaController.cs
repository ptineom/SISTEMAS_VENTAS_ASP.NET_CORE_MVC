using AplicacionWeb.SistemaVentas.Hubs;
using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Servicios.Seguridad;
using CapaNegocio;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class CajaAperturaController : Controller
    {
        private IConfiguration _configuration = null;
        private IHttpContextAccessor _accessor;
        private IResultadoOperacion _resultado = null;
        private BrCajaApertura _brCajaApertura = null;
        private string _idSucursal;
        private string _idUsuario;
        private IHubContext<CambiarEstadoCajaHub> _hubContext;

        public CajaAperturaController(IResultadoOperacion resultado, IConfiguration configuration,
    IHttpContextAccessor accessor, IHubContext<CambiarEstadoCajaHub> hubContext)
        {
            _configuration = configuration;
            _resultado = resultado;
            _brCajaApertura = new BrCajaApertura(_configuration);
            _accessor = accessor;

            UsuarioLogueadoModel usuario = new Session(_accessor).GetUserLogged();
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
            List<MONEDA> listaMoneda = null;
            List<CAJA> listaCaja = null;
            _resultado = await Task.Run(() => _brCajaApertura.GetData(_idSucursal, _idUsuario, ref listaMoneda, ref listaCaja));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (listaMoneda == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar la moneda en el sistema.", Status = "Error" });

            if (listaCaja == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar las cajas en el sistema.", Status = "Error" });


            //Moneda local
            var monedaLocal = listaMoneda.Where(x => x.FLG_LOCAL == true).Select(y => new
            {
                IdMoneda = y.ID_MONEDA,
                NomMoneda = y.NOM_MONEDA,
                SgnMoneda = y.SGN_MONEDA
            }).FirstOrDefault();


            //Resultado final 
            _resultado = new ResultadoOperacion();
            _resultado.SetResultado(true, new
            {
                ListaCaja = listaCaja.Select(x => new
                {
                    IdCaja = x.ID_CAJA,
                    NomCaja = ViewHelper.CapitalizeFirstLetter(x.NOM_CAJA)
                }).ToList<object>(),
                MonedaLocal = monedaLocal
            });

            return Ok(_resultado);
        }

        [HttpGet("GetStateBox")]
        public async Task<IActionResult> GetStateBoxAsync()
        {
            _resultado = await Task.Run(() => _brCajaApertura.GetStateBox(_idSucursal, _idUsuario));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data != null)
            {
                CAJA_APERTURA cajaApertura = (CAJA_APERTURA)_resultado.Data;

                _resultado.Data = new
                {
                    IdCaja = cajaApertura.ID_CAJA,
                    Correlativo = cajaApertura.CORRELATIVO,
                    FechaApertura = cajaApertura.FECHA_APERTURA,
                    MontoApertura = cajaApertura.MONTO_APERTURA,
                    IdMoneda = cajaApertura.ID_MONEDA,
                    SgnMoneda = cajaApertura.SGN_MONEDA,
                    FlgReaperturado = cajaApertura.FLG_REAPERTURADO,
                    Item = cajaApertura.ITEM,
                    FlgCierreDiferido = cajaApertura.FLG_CIERRE_DIFERIDO,
                    FechaCierre = cajaApertura.FECHA_CIERRE,
                    HoraCierre = cajaApertura.HORA_CIERRE,
                    NomCaja = cajaApertura.NOM_CAJA
                };
            }

            return Ok(_resultado);
        }

        [HttpGet("GetTotalsByUserId/{idCaja}/{correlativo}")]
        public async Task<IActionResult> GetTotalsByUserIdAsync(string idCaja, int correlativo)
        {
            _resultado = await Task.Run(() => _brCajaApertura.GetTotalsByUserId(_idSucursal, idCaja, _idUsuario, correlativo));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos.", Status = "Error" });

            if (_resultado.Data != null)
            {
                DINERO_EN_CAJA dineroEnCaja = (DINERO_EN_CAJA)_resultado.Data;

                _resultado.Data = new
                {
                    MontoAperturaCaja = dineroEnCaja.MONTO_APERTURA_CAJA,
                    MontoCobradoContado = dineroEnCaja.MONTO_COBRADO_CONTADO,
                    MontoCobradoCredito = dineroEnCaja.MONTO_COBRADO_CREDITO,
                    MontoCajaOtrosIngreso = dineroEnCaja.MONTO_CAJA_OTROS_INGRESO,
                    MontoCajaSalida = dineroEnCaja.MONTO_CAJA_SALIDA,
                    MontoTotal = dineroEnCaja.MONTO_TOTAL
                };
            }

            return Ok(_resultado);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] CajaAbiertaRequest request)
        {

            _resultado = await Task.Run(() => _brCajaApertura.Register(new CAJA_APERTURA()
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
            }));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data != null)
            {
                CAJA_APERTURA cajaApertura = (CAJA_APERTURA)_resultado.Data;

                _resultado.Data = new
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

            return Ok(_resultado);
        }

        [HttpGet("GetAllByFilters")]
        public async Task<IActionResult> GetAllByFiltersAsync([FromQuery] string idCaja, [FromQuery] string idUsuario,
            [FromQuery] string fechaInicial, [FromQuery] string fechaFinal)
        {
            _resultado = await Task.Run(() => _brCajaApertura.GetAllByFilters(_idSucursal, idCaja, idUsuario, fechaInicial, fechaFinal));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos.", Status = "Error" });

            if (_resultado.Data != null)
            {
                List<CAJA_APERTURA> listaCajaApertura = (List<CAJA_APERTURA>)_resultado.Data;

                _resultado.Data = listaCajaApertura.Select(x => new
                {
                    NomUsuario = ViewHelper.CapitalizeAll(x.NOM_USUARIO),
                    NomCaja = ViewHelper.CapitalizeAll(x.NOM_CAJA),
                    FechaApertura = x.FECHA_APERTURA,
                    FechaCierre = x.FECHA_CIERRE,
                    SgnMoneda = x.SGN_MONEDA,
                    MontoApertura = x.MONTO_APERTURA,
                    MontoTotal = x.MONTO_COBRADO,
                    FlgCierre = x.FLG_CIERRE,
                    IdUsuario = x.ID_USUARIO,
                    IdCaja = x.ID_CAJA,
                    Correlativo = x.CORRELATIVO,
                    FlgReaperturado = x.FLG_REAPERTURADO
                });
            }

            return Ok(_resultado);
        }

        [Route("[action]")]
        public async Task<IActionResult> ReaperturaCajaIndex()
        {
            List<USUARIO> listaUsuario = null;
            List<CAJA> listaCaja = null;
            _resultado = await Task.Run(() => _brCajaApertura.GetDataQuerys(_idSucursal, ref listaUsuario, ref listaCaja));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (listaUsuario == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar los usuarios en el sistema.", Status = "Error" });

            if (listaCaja == null)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar las cajas en el sistema.", Status = "Error" });

            List<UsuarioModel> ListaUsuario = listaUsuario.Select(x => new UsuarioModel()
            {
                IdUsuario = x.ID_USUARIO,
                NomUsuario = ViewHelper.CapitalizeAll(x.NOM_USUARIO)
            }).ToList<UsuarioModel>();

            List<CajaModel> ListaCaja = listaCaja.Select(x => new CajaModel()
            {
                IdCaja = x.ID_CAJA,
                NomCaja = ViewHelper.CapitalizeFirstLetter(x.NOM_CAJA)
            }).ToList<CajaModel>();

            ViewBag.ListaUsuario = ListaUsuario;
            ViewBag.ListaCaja = ListaCaja;

            return View();
        }

        [HttpPost("ReopenBox")]
        public async Task<IActionResult> ReopenBoxAsync([FromBody] ReaperturarCajaRequest request)
        {

            _resultado = await Task.Run(() => _brCajaApertura.ReopenBox(new CAJA_APERTURA()
            {
                ACCION = "REA",
                ID_SUCURSAL = _idSucursal,
                ID_CAJA = request.IdCaja,
                ID_USUARIO = request.IdUsuario,
                CORRELATIVO = request.Correlativo,
                ID_USUARIO_REGISTRO = _idUsuario
            }));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            //Enviamos el mensaje signalr al usuario indicado
            await _hubContext.Clients.User(request.IdUsuario).SendAsync("actualizarEstadoCaja");

            return Ok(_resultado);
        }
    }
}
