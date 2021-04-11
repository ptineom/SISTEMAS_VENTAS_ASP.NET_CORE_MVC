using AplicacionWeb.SistemaVentas.Models;
using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.Seguridad;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Servicios.Seguridad;
using CapaNegocio;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class CompraController : Controller
    {
        private IResultadoOperacion _resultado = null;
        private BrCompra _brCompra = null;
        private string _idUsuario = string.Empty;
        private string _idSucursal = string.Empty;
        private IHttpContextAccessor _accessor = null;
        private IConfiguration _configuration = null;
        public CompraController(IResultadoOperacion resultado, IHttpContextAccessor accessor, IConfiguration configuration)
        {
            _resultado = resultado;
            _configuration = configuration;
            _brCompra = new BrCompra(_configuration);
            _accessor = accessor;
            UsuarioLogueadoViewModel usuario = new Session(_accessor).GetUserLogged();
            _idUsuario = usuario.IdUsuario;
            _idSucursal = usuario.IdSucursal;
        }

        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            _resultado = await Task.Run(() => _brCompra.GetData(_idSucursal, _idUsuario));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            //objeto compra
            DOC_COMPRA docCompra = (DOC_COMPRA)_resultado.Data;

            //Lista con solo los campos que necesitamos.
            List<TipoDocumentoModel> listaTipoDocumento = null;
            List<TipoComprobanteModel> listaComprobante = null;
            List<TipoPagoModel> listaTipPag = null;
            List<TipoCondicionPagoModel> listaTipConPag = null;
            List<EstadoModel> listaEstado = null;
            List<MonedaModel> listaMoneda = null;
            List<DepartamentoModel> listaDepartamento = null;

            if (docCompra.listaDocumentos != null)
            {
                listaTipoDocumento = docCompra.listaDocumentos.Select(x => new TipoDocumentoModel()
                {
                    IdTipoDocumento = x.ID_TIPO_DOCUMENTO,
                    NomTipoDocumento = x.NOM_TIPO_DOCUMENTO,
                    Abreviatura = x.ABREVIATURA,
                    MaxDigitos = x.MAX_DIGITOS,
                    FlgRuc = x.FLG_RUC
                }).ToList<TipoDocumentoModel>();
                listaTipoDocumento.Insert(0, new TipoDocumentoModel() { IdTipoDocumento = null, Abreviatura = "---Seleccione---" });
            }

            if (docCompra.listaComprobantes != null)
            {
                listaComprobante = docCompra.listaComprobantes.Select(x => new TipoComprobanteModel()
                {
                    IdTipoComprobante = x.ID_TIPO_COMPROBANTE,
                    NomTipoComprobante = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_COMPROBANTE),
                    FlgRendirSunat = x.FLG_RENDIR_SUNAT
                }).ToList<TipoComprobanteModel>();
                listaComprobante.Insert(0, new TipoComprobanteModel() { IdTipoComprobante = null, NomTipoComprobante = "---Seleccione---" });
            }

            if (docCompra.listaMonedas != null)
            {
                listaMoneda = docCompra.listaMonedas.Select(x => new MonedaModel()
                {
                    IdMoneda = x.ID_MONEDA,
                    NomMoneda = x.NOM_MONEDA,
                    FlgLocal = x.FLG_LOCAL,
                    SgnMoneda = x.SGN_MONEDA
                }).ToList<MonedaModel>();
            }

            if (docCompra.listaTipPag != null)
            {
                listaTipPag = docCompra.listaTipPag.Select(x => new TipoPagoModel()
                {
                    IdTipoPago = x.ID_TIPO_PAGO,
                    NomTipoPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_PAGO)
                }).ToList<TipoPagoModel>();
                listaTipPag.Insert(0, new TipoPagoModel() { IdTipoPago = null, NomTipoPago = "---Seleccione---" });
            }

            if (docCompra.listaTipCon != null)
            {
                listaTipConPag = docCompra.listaTipCon.Select(x => new TipoCondicionPagoModel()
                {
                    IdTipoCondicionPago = x.ID_TIPO_CONDICION_PAGO,
                    NomTipoCondicionPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_CONDICION_PAGO),
                    FlgEvaluaCredito = x.FLG_EVALUA_CREDITO
                }).ToList<TipoCondicionPagoModel>();
                listaTipConPag.Insert(0, new TipoCondicionPagoModel() { IdTipoCondicionPago = null, NomTipoCondicionPago = "---Seleccione---" });
            }

            if (docCompra.listaEstados != null)
            {
                listaEstado = docCompra.listaEstados.Select(x => new EstadoModel()
                {
                    IdEstado = x.ID_ESTADO,
                    NomEstado = ViewHelper.CapitalizeFirstLetter(x.NOM_ESTADO)
                }).ToList<EstadoModel>();
                listaEstado.Insert(0, new EstadoModel() { IdEstado = null, NomEstado = "---Todos---" });
            }

            if (docCompra.listaDepartamentos != null)
            {
                listaDepartamento = docCompra.listaDepartamentos.Select(x => new DepartamentoModel()
                {
                    IdDepartamento = x.ID_UBIGEO,
                    NomDepartamento = ViewHelper.CapitalizeFirstLetter(x.UBIGEO_DEPARTAMENTO)
                }).ToList<DepartamentoModel>();
                listaDepartamento.Insert(0, new DepartamentoModel() { IdDepartamento = null, NomDepartamento = "---Seleccione---" });
            }


            ViewBag.listaTipoDocumento = listaTipoDocumento;
            ViewBag.listaComprobante = listaComprobante;
            ViewBag.listaMoneda = listaMoneda;
            ViewBag.listaTipPag = listaTipPag;
            ViewBag.listaTipConPag = listaTipConPag;
            ViewBag.listaEstado = listaEstado;
            ViewBag.tasIgv = docCompra.TAS_IGV;
            ViewBag.listaDepartamento = listaDepartamento;

            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]CompraRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            if (string.IsNullOrEmpty(request.JsonArticulos))
                return BadRequest(new { Mesagge = "No existe detalle en la compra.", Status = "Error" });

            //Ejecutar el grabado de la venta.
            _resultado = await Task.Run(() =>
            {
                //Agregando el detalle de la venta
                request.JsonArticulos = request.JsonArticulos
                    .Replace("IdArticulo", "ID_ARTICULO")
                    .Replace("IdUm", "ID_UM")
                    .Replace("Cantidad", "CANTIDAD")
                    .Replace("PrecioArticulo", "PRECIO_ARTICULO")
                    .Replace("TasDescuento", "TAS_DESCUENTO")
                    .Replace("NroFactor", "NRO_FACTOR")
                    .Replace("Importe", "IMPORTE");

                //Agregando la cabecera de la venta.
                DOC_COMPRA docCompra = new DOC_COMPRA()
                {
                    ACCION = "INS",
                    ID_USUARIO_REGISTRO = _idUsuario,
                    ID_SUCURSAL = _idSucursal,
                    ID_TIPO_COMPROBANTE = request.IdTipoComprobante,
                    NRO_SERIE = request.NroSerie,
                    NRO_DOCUMENTO = request.NroDocumento,
                    ID_PROVEEDOR = request.IdProveedor,
                    ID_MONEDA = request.IdMoneda,
                    FEC_DOCUMENTO = request.FecCompra,
                    FEC_VENCIMIENTO = request.FecVencimiento,
                    OBS_COMPRA = request.Observacion,
                    TOT_BRUTO = request.TotBruto,
                    TOT_DESCUENTO = request.TotDescuento,
                    TOT_IMPUESTO = request.TotImpuesto,
                    TOT_COMPRA = request.TotCompra,
                    TAS_DESCUENTO = request.TasDescuento,
                    ID_TIPO_PAGO = request.IdTipoPago,
                    ID_TIPO_CONDICION_PAGO = request.IdTipoCondicionPago,
                    ABONO = request.Abono,
                    SALDO = request.Saldo,
                    FEC_CANCELACION = request.FechaCancelacion,
                    JSON_ARTICULOS = request.JsonArticulos,
                    TAS_IGV = request.TasIgv
                };

                return _brCompra.Register(docCompra);
            });

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            return Ok(_resultado);
        }

        [HttpGet("GetAllByFilters")]
        public async Task<IActionResult> GetAllByFiltersAsync([FromQuery] string idTipoComprobante, [FromQuery] string nroSerie,
            [FromQuery] int nroDocumento, [FromQuery] string idProveedor, [FromQuery] string fechaInicial, [FromQuery] string fechaFinal, [FromQuery] int idEstado)
        {
            _resultado = await Task.Run(() => _brCompra.GetAllByFilters(_idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor, fechaInicial, fechaFinal, idEstado));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos.", Status = "Error" });

            List<DOC_COMPRA_LISTADO> listaCompra = (List<DOC_COMPRA_LISTADO>)_resultado.Data; ;

            _resultado.Data = listaCompra.Select(x => new
            {
                Comprobante = x.COMPROBANTE,
                DocProveedor = x.DOC_PROVEEDOR,
                NomProveedor = ViewHelper.CapitalizeAll(x.NOM_PROVEEDOR),
                SgnMoneda = x.SGN_MONEDA,
                TotCompra = x.TOT_COMPRA,
                FecDocumento = x.FEC_DOCUMENTO,
                FlgEvaluaCredito = x.FLG_EVALUA_CREDITO,
                NomTipoCondicionPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_CONDICION_PAGO),
                EstDocumento = x.EST_DOCUMENTO,
                NomEstado = ViewHelper.CapitalizeAll(x.NOM_ESTADO),
                IdTipoComprobante = x.ID_TIPO_COMPROBANTE,
                NroSerie = x.NRO_SERIE,
                NroDocumento = x.NRO_DOCUMENTO,
                IdProveedor = x.ID_PROVEEDOR
            }).ToList<object>();

            return Ok(_resultado);
        }

        [HttpGet("GetById/{idTipoComprobante}/{nroSerie}/{nroDocumento}/{idProveedor}")]
        public async Task<IActionResult> GetByIdAsync(string idTipoComprobante, string nroSerie, int nroDocumento, string idProveedor)
        {
            _resultado = await Task.Run(() => _brCompra.GetById(_idSucursal, idTipoComprobante, nroSerie, nroDocumento, idProveedor));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos.", Status = "Error" });

            DOC_COMPRA docCompra = (DOC_COMPRA)_resultado.Data;

            //Construímos la data que queremos mostrar al cliente.
            _resultado.Data = new
            {
                Cabecera = new
                {
                    IdTipoComprobante = docCompra.ID_TIPO_COMPROBANTE,
                    NroSerie = docCompra.NRO_SERIE,
                    NroDocumento = Helper.ViewHelper.GetNroComprobante(docCompra.NRO_DOCUMENTO.ToString()),
                    IdProveedor = docCompra.ID_PROVEEDOR,
                    NomProveedor = ViewHelper.CapitalizeAll(docCompra.NOM_PROVEEDOR),
                    DirProveedor = docCompra.DIR_PROVEEDOR,
                    IdTipoDocumento = docCompra.ID_TIPO_DOCUMENTO,
                    NroDocumentoProveedor = docCompra.NRO_DOCUMENTO_PROVEEDOR,
                    IdMoneda = docCompra.ID_MONEDA,
                    SgnMoneda = docCompra.SGN_MONEDA,
                    IdTipoPago = docCompra.ID_TIPO_PAGO,
                    IdTipoCondicion = docCompra.ID_TIPO_CONDICION_PAGO,
                    FecDocumento = docCompra.FEC_DOCUMENTO,
                    FecVencimiento = docCompra.FEC_VENCIMIENTO,
                    Observacion = docCompra.OBS_COMPRA,
                    TotBruto = docCompra.TOT_BRUTO,
                    TotDescuento = docCompra.TOT_DESCUENTO,
                    TotImpuesto = docCompra.TOT_IMPUESTO,
                    TotCompra = docCompra.TOT_COMPRA,
                    TotCompraRedondeo = docCompra.TOT_COMPRA_REDONDEO,
                    TasDescuento = docCompra.TAS_DESCUENTO,
                    EstDocumento = docCompra.EST_DOCUMENTO,
                    TasIgv = docCompra.TAS_IGV,
                    NomEstado = ViewHelper.CapitalizeAll(docCompra.NOM_ESTADO)
                },
                Detalle = docCompra.detalle.Select(x => new
                {
                    IdArticulo = x.ID_ARTICULO,
                    NomArticulo = ViewHelper.CapitalizeAll(x.NOM_ARTICULO),
                    IdUm = x.ID_UM,
                    NomUm = ViewHelper.CapitalizeAll(x.NOM_UM),
                    Cantidad = x.CANTIDAD,
                    TasDescuento = x.TAS_DESCUENTO,
                    NroFactor = x.NRO_FACTOR,
                    PrecioArticulo = x.PRECIO_ARTICULO,
                    Importe = x.IMPORTE,
                    Codigo = string.IsNullOrEmpty(x.CODIGO_BARRA) ? x.ID_ARTICULO : x.CODIGO_BARRA
                }).ToList<object>()
            };

            return Ok(_resultado);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCompraRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            //Ejecutar el grabado de la venta.
            _resultado = await Task.Run(() =>
            {
                 return _brCompra.Delete(_idSucursal, _idUsuario, 
                     request.IdTipoComprobante, request.NroSerie, request.NroDocumento, request.IdProveedor);
            });

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            return Ok(_resultado);
        }
    }
}
