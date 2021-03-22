using AplicacionWeb.SistemaVentas.Models;
using AplicacionWeb.SistemaVentas.Models.Seguridad;
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
            List<TipoDocumentosViewModel> listaDocumentos = null;
            List<TipoComprobanteViewModel> listaComprobantes = null;
            List<TipoPagoViewModel> listaTipPag = null;
            List<TipoCondicionPagoViewModel> listaTipConPag = null;
            List<EstadoViewModel> listaEstados = null;
            List<MonedaViewModel> listaMonedas = null;

            if (docCompra.listaDocumentos != null)
            {
                listaDocumentos = docCompra.listaDocumentos.Select(x => new TipoDocumentosViewModel()
                {
                    IdTipoDocumento = x.ID_TIPO_DOCUMENTO,
                    NomTipoDocumento = x.NOM_TIPO_DOCUMENTO,
                    Abreviatura = x.ABREVIATURA,
                    MaxDigitos = x.MAX_DIGITOS,
                    FlgRuc = x.FLG_RUC
                }).ToList<TipoDocumentosViewModel>();
                listaDocumentos.Insert(0, new TipoDocumentosViewModel() { IdTipoDocumento = -1, Abreviatura = "---Seleccione---" });
            }

            if (docCompra.listaComprobantes != null)
            {
                listaComprobantes = docCompra.listaComprobantes.Select(x => new TipoComprobanteViewModel()
                {
                    IdTipoComprobante = x.ID_TIPO_COMPROBANTE,
                    NomTipoComprobante = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_COMPROBANTE),
                    FlgRendirSunat = x.FLG_RENDIR_SUNAT
                }).ToList<TipoComprobanteViewModel>();
                listaComprobantes.Insert(0, new TipoComprobanteViewModel() { IdTipoComprobante = "-1", NomTipoComprobante = "---Seleccione---" });
            }

            if (docCompra.listaMonedas != null)
            {
                listaMonedas = docCompra.listaMonedas.Select(x => new MonedaViewModel()
                {
                    IdMoneda = x.ID_MONEDA,
                    NomMoneda = x.NOM_MONEDA,
                    FlgLocal = x.FLG_LOCAL,
                    SgnMoneda = x.SGN_MONEDA
                }).ToList<MonedaViewModel>();
            }

            if (docCompra.listaTipPag != null)
            {
                listaTipPag = docCompra.listaTipPag.Select(x => new TipoPagoViewModel()
                {
                    IdTipoPago = x.ID_TIPO_PAGO,
                    NomTipoPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_PAGO)
                }).ToList<TipoPagoViewModel>();
                listaTipPag.Insert(0, new TipoPagoViewModel() { IdTipoPago = "-1", NomTipoPago = "---Seleccione---" });
            }

            if (docCompra.listaTipCon != null)
            {
                listaTipConPag = docCompra.listaTipCon.Select(x => new TipoCondicionPagoViewModel()
                {
                    IdTipoCondicionPago = x.ID_TIPO_CONDICION_PAGO,
                    NomTipoCondicionPago = ViewHelper.CapitalizeFirstLetter(x.NOM_TIPO_CONDICION_PAGO),
                    FlgEvaluaCredito = x.FLG_EVALUA_CREDITO
                }).ToList<TipoCondicionPagoViewModel>();
                listaTipConPag.Insert(0, new TipoCondicionPagoViewModel() { IdTipoCondicionPago = "-1", NomTipoCondicionPago = "---Seleccione---" });
            }

            if (docCompra.listaEstados != null)
            {
                listaEstados = docCompra.listaEstados.Select(x => new EstadoViewModel()
                {
                    IdEstado = x.ID_ESTADO,
                    NomEstado = ViewHelper.CapitalizeFirstLetter(x.NOM_ESTADO)
                }).ToList<EstadoViewModel>();
                listaEstados.Insert(0, new EstadoViewModel() { IdEstado = -1, NomEstado = "---Seleccione---" });
            }


            ViewBag.listaDocumentos = listaDocumentos;
            ViewBag.listaComprobantes = listaComprobantes;
            ViewBag.listaMonedas = listaMonedas;
            ViewBag.listaTipPag = listaTipPag;
            ViewBag.listaTipConPag = listaTipConPag;
            ViewBag.listaEstados = listaEstados;
            ViewBag.tasIgv = docCompra.TAS_IGV;

            return View();
        }
    }
}
