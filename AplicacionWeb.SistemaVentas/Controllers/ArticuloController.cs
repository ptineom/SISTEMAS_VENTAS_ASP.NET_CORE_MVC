using AplicacionWeb.SistemaVentas.Models.Seguridad;
using AplicacionWeb.SistemaVentas.Servicios.Seguridad;
using CapaNegocio;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Hosting;
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
    public class ArticuloController : Controller
    {
        private IResultadoOperacion _resultado;
        private BrArticulo _brArticulo;
        private string _idSucursal;
        private string _idUsuario;
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;
        private IHttpContextAccessor _accessor;

        public ArticuloController(IResultadoOperacion resultado, IConfiguration configuration,
            IWebHostEnvironment environment, IHttpContextAccessor accessor)
        {
            _configuration = configuration;
            _accessor = accessor;
            _environment = environment;
            _resultado = resultado;
            _brArticulo = new BrArticulo(_configuration, _environment);


            UsuarioLogueadoViewModel usuario = new Session(_accessor).GetUserLogged();
            _idUsuario = usuario.IdUsuario;
            _idSucursal = usuario.IdSucursal;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View();
        }

        //[HttpGet("GetData")]
        //public async Task<IActionResult> GetDataAsync()
        //{
        //    BrEmpresa brEmpresa = new BrEmpresa(_configuration, _environment);
        //    BrGrupo brGrupo = new BrGrupo(_configuration);
        //    BrMoneda brMoneda = new BrMoneda(_configuration);
        //    ResultadoOperacion resultEmpresa = await Task.Run(() => brEmpresa.GetByFilters("", true, false));
        //    ResultadoOperacion resultGrupo = await Task.Run(() => brGrupo.GetAll());
        //    ResultadoOperacion resultMoneda = await Task.Run(() => brMoneda.GetAll());

        //    if (!resultEmpresa.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = resultEmpresa.Mensaje, Status = "Error" });

        //    if (!resultGrupo.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = resultGrupo.Mensaje, Status = "Error" });

        //    if (resultEmpresa.Data == null)
        //        return StatusCode(StatusCodes.Status404NotFound, new { Message = "Datos no encontrado.", Status = "Error" });

        //    if (resultGrupo.Data == null)
        //        return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar las clasificaciones de grupos.", Status = "Error" });

        //    if (resultMoneda.Data == null)
        //        return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar las monedas.", Status = "Error" });

        //    EMPRESA modelo = (EMPRESA)resultEmpresa.Data;

        //    if (modelo.sucursales == null)
        //        return StatusCode(StatusCodes.Status404NotFound, new { Message = "Debe de configurar al menos una sede.", Status = "Error" });


        //    List<object> sucursales = ((List<SUCURSAL>)modelo.sucursales).Select(x => new
        //    {
        //        idSucursal = x.ID_SUCURSAL,
        //        nomSucursal = ViewHelper.capitalizeAll(x.NOM_SUCURSAL),
        //        direccion = ViewHelper.capitalizeFirstLetter(x.DIRECCION),
        //        nomAlmacen = ViewHelper.capitalizeAll(x.NOM_ALMACEN),
        //        stockActual = 0
        //    }).ToList<object>();

        //    List<object> grupos = ((List<GRUPO>)resultGrupo.Data).Select(y => new
        //    {
        //        idGrupo = y.ID_GRUPO,
        //        nomGrupo = y.NOM_GRUPO
        //    }).ToList<object>();

        //    //Moneda local
        //    var monedaLocal = ((List<MONEDA>)resultMoneda.Data).Where(x => x.FLG_LOCAL == true).Select(y => new
        //    {
        //        idMoneda = y.ID_MONEDA,
        //        nomMoneda = y.NOM_MONEDA,
        //        sgnMoneda = y.SGN_MONEDA
        //    }).FirstOrDefault();

        //    //Resultado final 
        //    _resultado = new ResultadoOperacion();
        //    _resultado.SetResultado(true, new
        //    {
        //        Igv = modelo.IGV,
        //        ListaSucursal = sucursales,
        //        ListaGrupo = grupos,
        //        MonedaLocal = monedaLocal
        //    });

        //    return Ok(_resultado);
        //}

        //[HttpGet("GetAllByFilters")]
        //public async Task<IActionResult> GetAllByFiltersAsync([FromQuery] string accion, [FromQuery] string tipoFiltro, [FromQuery] string filtro)
        //{
        //    _resultado = await Task.Run(() => _brArticulo.GetAllByFilters(accion, _idSucursal, tipoFiltro, filtro));

        //    if (!_resultado.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

        //    if (_resultado.Data == null)
        //        return StatusCode(StatusCodes.Status404NotFound, new { Message = "Datos no encontrado.", Status = "Error" });

        //    _resultado.Data = ((List<ARTICULO>)_resultado.Data).Select(x => new
        //    {
        //        IdArticulo = x.ID_ARTICULO,
        //        NomArticulo = x.NOM_ARTICULO,
        //        NomMarca = x.NOM_MARCA,
        //        NomGrupo = x.NOM_GRUPO,
        //        NomFamilia = x.NOM_FAMILIA,
        //        CodigoBarra = x.CODIGO_BARRA,
        //        StockActual = x.STOCK_ACTUAL,
        //        PrecioVentaFinal = x.PRECIO_VENTA_FINAL,
        //        FlgInactivo = x.FLG_INACTIVO,
        //        Foto1 = x.FOTO1,
        //        Foto2 = x.FOTO2
        //    });

        //    return Ok(_resultado);
        //}

        //[HttpPost("Register")]
        //public async Task<IActionResult> RegisterAsync([FromForm] RequestGrabarArticulo request)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(new { Mesagge = ModelState, Status = "Error" });

        //    string idArticulo = string.Empty, namePhoto = string.Empty;
        //    bool flgMismaFoto = false;

        //    IFormFileCollection files = request.FileBinary;
        //    int countFiles = 0;

        //    if (files != null)
        //    {
        //        countFiles = files.Count;
        //        //Si el modelo tiene foto, validamos el formato y tamaño del archivo.
        //        if (countFiles > 0)
        //        {
        //            IFormFile file = files[0];
        //            //Formato permitidos.
        //            string[] arrExtensiones = new string[] { ".png", ".jpg", ".jpeg" };
        //            string mensajeError = ImageHelper.TryParse(file, ImageHelper.TipoTamanio.mb, Configuraciones.TAMANIO_MAX_ARCHIVO_MB, arrExtensiones);

        //            if (!string.IsNullOrEmpty(mensajeError))
        //                return StatusCode(StatusCodes.Status400BadRequest, new { Message = mensajeError, Status = "Error" });

        //            namePhoto = file.FileName;
        //        }
        //    }


        //    ARTICULO articulo = new ARTICULO()
        //    {
        //        ACCION = request.Accion,
        //        ID_ARTICULO = request.IdArticulo,
        //        NOM_ARTICULO = request.NomArticulo,
        //        NOM_VENTA = request.NomVenta,
        //        PRECIO_COMPRA = request.PrecioCompra,
        //        PRECIO_VENTA = request.PrecioBase,
        //        FLG_IMPORTADO = request.FlgImportado,
        //        FLG_INACTIVO = request.FlgInactivo,
        //        ID_MARCA = request.IdMarca,
        //        ID_GRUPO = request.IdGrupo,
        //        ID_FAMILIA = request.IdFamilia,
        //        CODIGO_BARRA = request.CodigoBarra,
        //        STOCK_MINIMO = request.StockMinimo,
        //        ID_USUARIO_REGISTRO = _idUsuario,
        //        ID_SUCURSAL = _idSucursal,
        //        JSON_SUCURSAL = request.Sucursales,
        //        JSON_UM = request.ArticulosUm,
        //        NOM_FOTO = namePhoto
        //    };

        //    //Grabamos el artículo (Si realizamos una actualización, el procedimiento nos devolverá los parámetros de salida: 
        //    //jsonFotos != "" => Si devuelve las fotos, será para borrarlas del directorio
        //    //flgMismaFoto == true => No seguirá con el proceso de guardado de foto en el directorio.)
        //    string jsonFotos = string.Empty;
        //    _resultado = await Task.Run(() => _brArticulo.Register(articulo, ref idArticulo, ref jsonFotos, ref flgMismaFoto));

        //    //Si el grabado fue exitoso, guardamos tambien las fotos en el directorio.
        //    if (!_resultado.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

        //    try
        //    {
        //        if (request.Accion == "UPD")
        //        {
        //            idArticulo = request.IdArticulo;

        //            if (flgMismaFoto)
        //                countFiles = 0;
        //            else
        //            {
        //                //Elimina las fotos del directorio
        //                if (!string.IsNullOrEmpty(jsonFotos))
        //                    BrFotos.DeleteDirectory(jsonFotos, Path.Combine(_environment.WebRootPath, Configuraciones.UPLOAD_ARTICULOS));
        //            }
        //        }

        //        if (countFiles > 0)
        //        {
        //            IFormFile file = files[0];
        //            string uri = Path.Combine(_environment.WebRootPath, Configuraciones.UPLOAD_ARTICULOS, file.FileName);

        //            //Guardamos la imagen y posteriormente la usaremos de modelo para crear nuevas imagenes.
        //            using (FileStream fs = new FileStream(uri, FileMode.Create))
        //            {
        //                file.CopyTo(fs);
        //            }

        //            ResultadoOperacion resultFoto = new ResultadoOperacion();
        //            BrFotos brFoto = new BrFotos();

        //            //La extensión a guardar será jpg.
        //            string extension = Path.GetExtension(file.FileName);
        //            if (extension.Equals(".png", StringComparison.OrdinalIgnoreCase))
        //                extension = ".jpg";

        //            //Generamos las fotos en 2 medidas, apartir de la imagen ya guardada.
        //            int[] medidas = Configuraciones.SCALES_IMAGES_ARTICULOS;
        //            FOTOS oFoto = new FOTOS()
        //            {
        //                ID_TIPO_FOTO = Configuraciones.ID_TIPO_FOTO_ARTICULO,
        //                ID_ELEMENTO = idArticulo,
        //                ID_USUARIO_REGISTRO = _idUsuario
        //            };
        //            //Proceso de guardado de fotos en bd y creacion de imagenes según las escalas indicadas.
        //            resultFoto = await Task.Run(() => brFoto.ProcessSave(uri, medidas, oFoto, extension));

        //            if (!resultFoto.Resultado)
        //                throw new Exception(resultFoto.Mensaje);

        //            //Después crear la imágenes nuevas, eliminamos la imágen modelo.
        //            ImageHelper.DeleteFile(uri);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Helper.Elog.save(this, ex);
        //    }

        //    return Ok(_resultado);
        //}

        //[HttpPost("Delete/{idArticulo}")]
        //public async Task<IActionResult> DeleteAsync(string idArticulo)
        //{
        //    _resultado = await Task.Run(() => _brArticulo.Delete(idArticulo, _idUsuario));

        //    if (!_resultado.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

        //    //Las fotos que se eliminaran el directorio
        //    if (_resultado.Data != null)
        //        BrFotos.DeleteDirectory(_resultado.Data.ToString(), Path.Combine(_environment.WebRootPath, Configuraciones.UPLOAD_ARTICULOS));

        //    return Ok(_resultado);
        //}

        //[HttpGet("GetById/{idArticulo}")]
        //public async Task<IActionResult> GetByIdAsync(string idArticulo)
        //{
        //    ARTICULO articulo = null;
        //    List<object> listaFamilia = null;
        //    List<object> listaUm = null;
        //    List<object> listaArticuloUm = null;
        //    List<object> listaSucursal = null;
        //    object modelo = null;

        //    //Obtenemos el artículo y algunas listas de la BD.
        //    _resultado = await Task.Run(() => _brArticulo.GetById(_idSucursal, idArticulo));

        //    if (!_resultado.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

        //    if (_resultado.Data == null)
        //        return StatusCode(StatusCodes.Status404NotFound, new { Message = "Datos no encontrados.", Status = "Error" });

        //    articulo = (ARTICULO)_resultado.Data;

        //    //Lista de articulo_Um
        //    if (articulo.listaArticuloUm != null)
        //    {
        //        listaArticuloUm = articulo.listaArticuloUm.Select(x => new
        //        {
        //            IdArticulo = x.ID_ARTICULO,
        //            IdUm = x.ID_UM,
        //            NroFactor = x.NRO_FACTOR,
        //            NroOrden = x.NRO_ORDEN,
        //            FlgPromocion = x.FLG_PROMOCION,
        //            Descuento = x.DESCUENTO1,
        //            FecInicioPromocion = x.FEC_INICIO_PROMOCION,
        //            FecFinalPromocion = x.FEC_FINAL_PROMOCION,
        //            PrecioVenta = x.PRECIO_VENTA,
        //            PrecioVentaFinal = x.PRECIO_VENTA_FINAL
        //        }).ToList<object>();
        //    }

        //    //Lista de sucursales
        //    if (articulo.listaSucursal != null)
        //    {
        //        listaSucursal = articulo.listaSucursal.Select(x => new
        //        {
        //            FlgEnUso = x.FLG_EN_USO,
        //            NomAlmacen = x.NOM_ALMACEN,
        //            StockActual = x.STOCK_ACTUAL,
        //            IdSucursal = x.ID_SUCURSAL,
        //            NomSucursal = x.NOM_SUCURSAL,
        //            Direccion = $"{x.DIRECCION} {x.NOM_UBIGEO}",
        //        }).ToList<object>();
        //    }

        //    ResultadoOperacion _resultFamilia = new ResultadoOperacion();
        //    BrFamilia _brFamilia = new BrFamilia();

        //    //Obtenemos las familias por idGrupo de la BD.
        //    _resultFamilia = _brFamilia.GetAllByGroupIdHelper(articulo.ID_GRUPO);

        //    if (!_resultFamilia.Resultado)
        //        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultFamilia.Mensaje, Status = "Error" });

        //    if (_resultFamilia.Data != null)
        //    {
        //        List<FAMILIA> familias = (List<FAMILIA>)_resultFamilia.Data;
        //        listaFamilia = familias.Select(x => new { IdFamilia = x.ID_FAMILIA, NomFamilia = x.NOM_FAMILIA }).ToList<object>();
        //    }

        //    ResultadoOperacion _resultUm = new ResultadoOperacion();
        //    BrUnidadMedida _brUm = new BrUnidadMedida();

        //    //Obtenemos las um_familia por idGrupo y idFamilia de la BD.
        //    if (!string.IsNullOrEmpty(articulo.ID_GRUPO) && !string.IsNullOrEmpty(articulo.ID_FAMILIA))
        //    {
        //        _resultUm = _brUm.GetAllByFamilyId(articulo.ID_GRUPO, articulo.ID_FAMILIA);

        //        if (!_resultUm.Resultado)
        //            return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultFamilia.Mensaje, Status = "Error" });

        //        if (_resultUm.Data != null)
        //        {
        //            List<UNIDAD_MEDIDA> ums = (List<UNIDAD_MEDIDA>)_resultUm.Data;
        //            listaUm = ums.Select(x => new { IdUm = x.ID_UM, NomUm = x.NOM_UM }).ToList<object>();
        //        }
        //    }

        //    //Modelo de artículo.
        //    modelo = new
        //    {
        //        IdArticulo = articulo.ID_ARTICULO,
        //        NomArticulo = articulo.NOM_ARTICULO,
        //        NomVenta = articulo.NOM_VENTA,
        //        CodigoBarra = articulo.CODIGO_BARRA,
        //        IdMarca = articulo.ID_MARCA,
        //        NomMarca = ViewHelper.capitalizeAll(articulo.NOM_MARCA),
        //        IdGrupo = articulo.ID_GRUPO,
        //        IdFamilia = articulo.ID_FAMILIA,
        //        PrecioVenta = articulo.PRECIO_VENTA,
        //        PrecioCompra = articulo.PRECIO_COMPRA,
        //        StockMinimo = articulo.STOCK_MINIMO,
        //        FlgImportado = articulo.FLG_IMPORTADO,
        //        FlgInactivo = articulo.FLG_INACTIVO,
        //        Foto1 = articulo.FOTO1,
        //        Foto2 = articulo.FOTO2,
        //        FotoB64 = articulo.FOTO_B64
        //    };

        //    //modelo y listas a devolver
        //    _resultado.Data = new
        //    {
        //        Modelo = modelo,
        //        ListaArticuloUm = listaArticuloUm,
        //        ListaFamilia = listaFamilia,
        //        ListaUm = listaUm,
        //        ListaSucursal = listaSucursal
        //    };

        //    return Ok(_resultado);
        //}

        [HttpGet("GetAllByFiltersHelper/{tipoFiltro}/{filtro}/{flgCompra}")]
        public async Task<IActionResult> GetAllByFiltersHelperAsync(string tipoFiltro, string filtro, bool flgCompra)
        {
            string accion = "LIS";
            _resultado = await Task.Run(() => _brArticulo.GetAllByFiltersHelper(accion, _idSucursal, tipoFiltro, filtro, flgCompra));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos.", Status = "Error" });

            List<ARTICULO> lista = (List<ARTICULO>)_resultado.Data;

            if (flgCompra)
            {
                _resultado.Data = lista.Select(x => new
                {
                    IdArticulo = x.ID_ARTICULO,
                    Codigo = string.IsNullOrEmpty(x.CODIGO_BARRA) ? x.ID_ARTICULO : x.CODIGO_BARRA,
                    NomArticulo = ViewHelper.CapitalizeAll(x.NOM_ARTICULO),
                    NomMarca = ViewHelper.CapitalizeAll(x.NOM_MARCA),
                    NomUm = ViewHelper.CapitalizeAll(x.NOM_UM),
                    StockActual = x.STOCK_ACTUAL,
                    IdUm = x.ID_UM,
                    NroFactor = x.NRO_FACTOR,
                    ListaUm = x.listaArticuloUm.Select(y => new
                    {
                        IdUm = y.ID_UM,
                        NomUm = ViewHelper.CapitalizeAll(y.NOM_UM),
                        NroFactor = y.NRO_FACTOR
                    }).ToList(),
                    StockMinimo = x.STOCK_MINIMO
                }).ToList();
            }
            else
            {
                _resultado.Data = lista.Select(x => new
                {
                    IdArticulo = x.ID_ARTICULO,
                    Codigo = string.IsNullOrEmpty(x.CODIGO_BARRA) ? x.ID_ARTICULO : x.CODIGO_BARRA,
                    NomArticulo = ViewHelper.CapitalizeAll(x.NOM_ARTICULO),
                    NomMarca = ViewHelper.CapitalizeAll(x.NOM_MARCA),
                    NomUm = ViewHelper.CapitalizeAll(x.NOM_UM),
                    StockActual = x.STOCK_ACTUAL,
                    PrecioVentaFinal = x.PRECIO_VENTA_FINAL,
                    Descuento1 = x.DESCUENTO1,
                    IdUm = x.ID_UM,
                    NroFactor = x.NRO_FACTOR,
                    ListaUm = x.listaArticuloUm.Select(y => new
                    {
                        IdUm = y.ID_UM,
                        NomUm = ViewHelper.CapitalizeAll(y.NOM_UM),
                        NroFactor = y.NRO_FACTOR,
                        Descuento1 = y.DESCUENTO1,
                        PrecioVenta = y.PRECIO_VENTA,
                        PrecioVentaFinal = y.PRECIO_VENTA_FINAL
                    }).ToList(),
                    PrecioBase = x.PRECIO_BASE,
                    PrecioVenta = x.PRECIO_VENTA,
                    StockMinimo = x.STOCK_MINIMO
                }).ToList();
            }

            return Ok(_resultado);
        }

        [HttpGet("GetByBarcode/{codigoBarra?}/{flgCompra}")]
        public async Task<IActionResult> GetByBarcodeAsync(string codigoBarra, bool flgCompra = false)
        {
            _resultado = await Task.Run(() => _brArticulo.GetAllByFiltersHelper("BAR", _idSucursal, "", codigoBarra, flgCompra));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

            if (_resultado.Data == null)
                return NotFound(new { Message = "No se encontraron datos.", Status = "Error" });

            List<ARTICULO> lista = (List<ARTICULO>)_resultado.Data;

            _resultado.Data = lista.Select(x => new
            {
                IdArticulo = x.ID_ARTICULO,
                Codigo = string.IsNullOrEmpty(x.CODIGO_BARRA) ? x.ID_ARTICULO : x.CODIGO_BARRA,
                NomArticulo = ViewHelper.CapitalizeAll(x.NOM_ARTICULO),
                ListaUm = x.listaArticuloUm.Select(y => new
                {
                    IdUm = y.ID_UM,
                    NomUm = ViewHelper.CapitalizeAll(y.NOM_UM),
                    NroFactor = y.NRO_FACTOR,
                    Descuento1 = y.DESCUENTO1,
                    PrecioVenta = y.PRECIO_VENTA,
                    PrecioVentaFinal = y.PRECIO_VENTA_FINAL
                }).ToList()
            }).FirstOrDefault();

            return Ok(_resultado);
        }

    }
}
