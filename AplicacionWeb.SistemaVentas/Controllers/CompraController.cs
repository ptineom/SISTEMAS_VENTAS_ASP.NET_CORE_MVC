using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
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
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class CompraController : Controller
    {
        private IDocCompraService _docCompraService = null;
        private string _idUsuario = string.Empty;
        private string _idSucursal = string.Empty;

        private readonly ISessionIdentity _sessionIdentity;

        public CompraController(IDocCompraService docCompraService, ISessionIdentity sessionIdentity)
        {
            _docCompraService = docCompraService;

            _sessionIdentity = sessionIdentity;
            UsuarioIdentityViewModel usuario = _sessionIdentity.GetUserLogged();
            _idUsuario = usuario.IdUsuario;
            _idSucursal = usuario.IdSucursal;
        }

        [Route("[action]")]
        public async Task<IActionResult> Index()
        {
            var result = await _docCompraService.GetLoadObjects(_idSucursal, _idUsuario);

            if (result == null)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    Message = Constantes.NOT_FOUND,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = Constantes.NOT_FOUND
                    }
                });


            //objeto 
            OBJETOS_DOC_COMPRA model = (OBJETOS_DOC_COMPRA)result;

            //Lista con solo los campos que necesitamos.
            List<TipoDocumentoSelectViewModel> listTipoDocumento = null;
            List<TipoComprobanteSelectViewModel> listTipoComprobante = null;
            List<TipoPagoSelectViewModel> listTipoPago = null;
            List<TipoCondicionPagoSelectViewModel> listTipoCondicionPago = null;
            List<EstadoSelectViewModel> listEstado = null;
            List<MonedaSelectViewModel> listMoneda = null;
            List<DepartamentoSelectViewModel> listDepartamento = null;

            if (model.ListTipoDocumento != null)
            {
                listTipoDocumento = model.ListTipoDocumento.Select(x => new TipoDocumentoSelectViewModel()
                {
                    IdTipoDocumento = x.ID_TIPO_DOCUMENTO,
                    NomTipoDocumento = x.NOM_TIPO_DOCUMENTO,
                    Abreviatura = x.ABREVIATURA,
                    MaxDigitos = x.MAX_DIGITOS,
                    FlgRuc = x.FLG_RUC
                }).ToList<TipoDocumentoSelectViewModel>();
                listTipoDocumento.Insert(0, new TipoDocumentoSelectViewModel() { IdTipoDocumento = null, Abreviatura = "---Seleccione---" });
            }

            if (model.ListTipoComprobante != null)
            {
                listTipoComprobante = model.ListTipoComprobante.Select(x => new TipoComprobanteSelectViewModel()
                {
                    IdTipoComprobante = x.ID_TIPO_COMPROBANTE,
                    NomTipoComprobante = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_COMPROBANTE),
                    FlgRendirSunat = x.FLG_RENDIR_SUNAT
                }).ToList<TipoComprobanteSelectViewModel>();
                listTipoComprobante.Insert(0, new TipoComprobanteSelectViewModel() { IdTipoComprobante = null, NomTipoComprobante = "---Seleccione---" });
            }

            if (model.ListMoneda != null)
            {
                listMoneda = model.ListMoneda.Select(x => new MonedaSelectViewModel()
                {
                    IdMoneda = x.ID_MONEDA,
                    NomMoneda = x.NOM_MONEDA,
                    FlgLocal = x.FLG_LOCAL,
                    SgnMoneda = x.SGN_MONEDA
                }).ToList<MonedaSelectViewModel>();
            }

            if (model.ListTipoPago != null)
            {
                listTipoPago = model.ListTipoPago.Select(x => new TipoPagoSelectViewModel()
                {
                    IdTipoPago = x.ID_TIPO_PAGO,
                    NomTipoPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_PAGO)
                }).ToList<TipoPagoSelectViewModel>();
                listTipoPago.Insert(0, new TipoPagoSelectViewModel() { IdTipoPago = null, NomTipoPago = "---Seleccione---" });
            }

            if (model.ListTipoCondicionPago!= null)
            {
                listTipoCondicionPago = model.ListTipoCondicionPago.Select(x => new TipoCondicionPagoSelectViewModel()
                {
                    IdTipoCondicionPago = x.ID_TIPO_CONDICION_PAGO,
                    NomTipoCondicionPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_CONDICION_PAGO),
                    FlgEvaluaCredito = x.FLG_EVALUA_CREDITO
                }).ToList<TipoCondicionPagoSelectViewModel>();
                listTipoCondicionPago.Insert(0, new TipoCondicionPagoSelectViewModel() { IdTipoCondicionPago = null, NomTipoCondicionPago = "---Seleccione---" });
            }

            if (model.ListEstado != null)
            {
                listEstado = model.ListEstado.Select(x => new EstadoSelectViewModel()
                {
                    IdEstado = x.ID_ESTADO,
                    NomEstado = ViewHelper.CapitalizeFirstLetter(x.NOM_ESTADO)
                }).ToList<EstadoSelectViewModel>();
                listEstado.Insert(0, new EstadoSelectViewModel() { IdEstado = null, NomEstado = "---Todos---" });
            }

            if (model.ListDepartamento != null)
            {
                listDepartamento = model.ListDepartamento.Select(x => new DepartamentoSelectViewModel()
                {
                    IdDepartamento = x.ID_UBIGEO,
                    NomDepartamento = ViewHelper.CapitalizeFirstLetter(x.UBIGEO_DEPARTAMENTO)
                }).ToList<DepartamentoSelectViewModel>();
                listDepartamento.Insert(0, new DepartamentoSelectViewModel() { IdDepartamento = null, NomDepartamento = "---Seleccione---" });
            }


            ViewBag.listaTipoDocumento = listTipoDocumento;
            ViewBag.listaComprobante = listTipoComprobante;
            ViewBag.listaMoneda = listMoneda;
            ViewBag.listaTipPag = listTipoPago;
            ViewBag.listaTipConPag = listTipoCondicionPago;
            ViewBag.listaEstado = listEstado;
            ViewBag.tasIgv = model.TasIgv;
            ViewBag.listaDepartamento = listDepartamento;

            return View();
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync([FromBody]CompraRequest request)
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

            if (string.IsNullOrEmpty(request.JsonArticulos))
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "No existe detalle en la compra."
                    }
                });


            //Ejecutar el grabado de la venta.
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
                FEC_DOCUMENTO = Convert.ToDateTime(request.FecCompra),
                FEC_VENCIMIENTO = Convert.ToDateTime(request.FecVencimiento),
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
                TAS_IGV = request.TasIgv,
                ID_CAJA_CA = request.IdCaja,
                ID_USUARIO_CA = _idUsuario,
                CORRELATIVO_CA = request.CorrelativoCa,
                FLG_RETIRAR_CAJA = request.FlgRetirarCaja,
                MONTO_RETIRA_CAJA = request.MontoRetiraCaja
            };

            var result = await _docCompraService.RegisterAsync(docCompra);

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

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string idTipoComprobante, [FromQuery] string nroSerie,
            [FromQuery] int nroDocumento, [FromQuery] string idProveedor, [FromQuery] string fechaInicial, [FromQuery] string fechaFinal, [FromQuery] int idEstado)
        {
            var result = await _docCompraService.GetAllAsync(new DOC_COMPRA()
            {
                ID_SUCURSAL = _idSucursal,
                ID_TIPO_COMPROBANTE = idTipoComprobante,
                NRO_SERIE = nroSerie,
                NRO_DOCUMENTO = nroDocumento,
                ID_PROVEEDOR = idProveedor,
                FECHA_INICIAL = Convert.ToDateTime(fechaInicial),
                FECHA_FINAL = Convert.ToDateTime(fechaFinal)
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

            var response = result.Select(x => new
            {
                Comprobante = x.COMPROBANTE,
                DocProveedor = x.DOC_PROVEEDOR,
                NomProveedor = ViewHelper.CapitalizeAll(x.NOM_PROVEEDOR),
                SgnMoneda = x.SGN_MONEDA,
                TotCompra = x.TOT_COMPRA,
                FecDocumento = x.FEC_DOCUMENTO.ToString("dd/MM/yyyy"),
                FlgEvaluaCredito = x.FLG_EVALUA_CREDITO,
                NomTipoCondicionPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_CONDICION_PAGO),
                EstDocumento = x.EST_DOCUMENTO,
                NomEstado = ViewHelper.CapitalizeAll(x.NOM_ESTADO),
                IdTipoComprobante = x.ID_TIPO_COMPROBANTE,
                NroSerie = x.NRO_SERIE,
                NroDocumento = x.NRO_DOCUMENTO,
                IdProveedor = x.ID_PROVEEDOR
            }).ToList<object>();

            return Ok(response);
        }

        [HttpGet("GetById/{idTipoComprobante}/{nroSerie}/{nroDocumento}/{idProveedor}")]
        public async Task<IActionResult> GetByIdAsync(string idTipoComprobante, string nroSerie, int nroDocumento, string idProveedor)
        {
            var result = await _docCompraService.GetByIdAsync(new DOC_COMPRA()
            {
                ID_TIPO_COMPROBANTE = idTipoComprobante,
                NRO_SERIE = nroSerie,
                NRO_DOCUMENTO = nroDocumento,
                ID_PROVEEDOR = idProveedor,
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

            //Construímos la data que queremos mostrar al cliente.
            var response = new ResponseObject()
            {
                Data = new
                {
                    Cabecera = new
                    {
                        IdTipoComprobante = result.ID_TIPO_COMPROBANTE,
                        NroSerie = result.NRO_SERIE,
                        NroDocumento = Helper.ViewHelper.GetNroComprobante(result.NRO_DOCUMENTO.ToString()),
                        IdProveedor = result.ID_PROVEEDOR,
                        NomProveedor = ViewHelper.CapitalizeAll(result.NOM_PROVEEDOR),
                        DirProveedor = result.DIR_PROVEEDOR,
                        IdTipoDocumento = result.ID_TIPO_DOCUMENTO,
                        NroDocumentoProveedor = result.NRO_DOCUMENTO_PROVEEDOR,
                        IdMoneda = result.ID_MONEDA,
                        SgnMoneda = result.SGN_MONEDA,
                        IdTipoPago = result.ID_TIPO_PAGO,
                        IdTipoCondicion = result.ID_TIPO_CONDICION_PAGO,
                        FecDocumento = result.FEC_DOCUMENTO,
                        FecVencimiento = result.FEC_VENCIMIENTO,
                        Observacion = result.OBS_COMPRA,
                        TotBruto = result.TOT_BRUTO,
                        TotDescuento = result.TOT_DESCUENTO,
                        TotImpuesto = result.TOT_IMPUESTO,
                        TotCompra = result.TOT_COMPRA,
                        TotCompraRedondeo = result.TOT_COMPRA_REDONDEO,
                        TasDescuento = result.TAS_DESCUENTO,
                        EstDocumento = result.EST_DOCUMENTO,
                        TasIgv = result.TAS_IGV,
                        NomEstado = ViewHelper.CapitalizeAll(result.NOM_ESTADO)
                    },
                    Detalle = result.detalle.Select(x => new
                    {
                        IdArticulo = x.ID_ARTICULO,
                        NomArticulo = ViewHelper.CapitalizeAll(x.NOM_ARTICULO),
                        IdUm = x.ID_UM,
                        Cantidad = x.CANTIDAD,
                        TasDescuento = x.TAS_DESCUENTO,
                        NroFactor = x.NRO_FACTOR,
                        PrecioArticulo = x.PRECIO_ARTICULO,
                        Importe = x.IMPORTE,
                        Codigo = string.IsNullOrEmpty(x.CODIGO_BARRA) ? x.ID_ARTICULO : x.CODIGO_BARRA,
                        JsonListaUm = JsonSerializer.Deserialize<List<UnidadMedidaSelectViewModel>>(x.JSON_UM.Replace("ID_UM", "IdUm").Replace("NOM_UM", "NomUm").Replace("NRO_FACTOR", "NroFactor"))
                    }).ToList<object>()
                }
            };

            return Ok(response);
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> DeleteAsync([FromBody] DeleteCompraRequest request)
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

            var result = await _docCompraService.DeleteAsync(new DOC_COMPRA()
            {
                ID_USUARIO_REGISTRO = _idUsuario,
                ID_SUCURSAL = _idSucursal,
                ID_TIPO_COMPROBANTE = request.IdTipoComprobante,
                NRO_SERIE = request.NroSerie,
                NRO_DOCUMENTO = request.NroDocumento,
                ID_PROVEEDOR = request.IdProveedor
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
