using AplicacionWeb.SistemaVentas.Models;
using AplicacionWeb.SistemaVentas.Models.Seguridad;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Servicios.Seguridad;
using CapaNegocio;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private IResultadoOperacion _resultado = null;
        private BrUsuario _brUsuario = null;
        private IHttpContextAccessor _accessor = null;
        private IWebHostEnvironment _environment= null;
        private IConfiguration _configuration = null;

        public LoginController(IResultadoOperacion resultado, IHttpContextAccessor accessor, 
            IWebHostEnvironment environment, IConfiguration configuration)
        {
            _configuration = configuration;
            _resultado = resultado;
            _brUsuario = new BrUsuario(_configuration);
            _accessor = accessor;
            _environment = environment;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(new ReqLoginViewModel());
        }

        [HttpPost("ValidateUser")]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateUserAsync([FromBody] ReqLoginViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            if (string.IsNullOrWhiteSpace(request.idUsuario) || string.IsNullOrWhiteSpace(request.password))
                return BadRequest(new { Message = "Usuario y/o contraseña incorrectas", Status = "Error" });

            //Validamos la existencia del usuario en la BD.
            string passwordHash256 = HashHelper.GetHash256(request.password);
            _resultado = await Task.Run(() => _brUsuario.ValidateUser(request.idUsuario, passwordHash256));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = _resultado.Mensaje, Status = "Error" });

            try
            {
                //Datos del usuario
                USUARIO modelo = (USUARIO)_resultado.Data;
                int countSedes = modelo.COUNT_SEDES;

                if (countSedes == 0)
                    return NotFound(new { Message = "Este usuario no tiene configurado al menos una sede.", Status = "Error" });

                ResultadoLoginModel resultadoLogin = null;

                //**** Si tiene una sede asignada, generamos el token.
                if (countSedes == 1)
                {
                    //Generamos la identidad y cookie.
                    await IdentitySignInAsync(modelo);

                    resultadoLogin = new ResultadoLoginModel()
                    {
                        ReturnUrl = "/Home/Index",
                        MasDeUnaSucursal = false
                    };
                }
                else if (countSedes > 1)
                {
                    BrSucursalUsuario brSucursalUsuario = new BrSucursalUsuario();
                    _resultado = new ResultadoOperacion();

                    //Lista de sucursales por usuario.
                    _resultado = brSucursalUsuario.GetAllByUserId(request.idUsuario);

                    if (!_resultado.Resultado)
                        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.Mensaje, Status = "Error" });

                    List<SucursalModel> sucursales = ((List<SUCURSAL>)_resultado.Data).Select(x => new SucursalModel()
                    {
                        IdSucursal = x.ID_SUCURSAL,
                        NomSucursal = x.NOM_SUCURSAL
                    }).ToList<SucursalModel>();
                    sucursales.Insert(0, new SucursalModel() { IdSucursal = "-1", NomSucursal = "---SELECCIONE---" });

                    resultadoLogin = new ResultadoLoginModel()
                    {
                        MasDeUnaSucursal = true,
                        Sucursales = sucursales
                    };
                }
                _resultado = new ResultadoOperacion();
                _resultado.SetResultado(true, "", resultadoLogin);
            }
            catch (InvalidOperationException ex)
            {
                object obj = new { ex.Message, Status = "Error" };
                return StatusCode(StatusCodes.Status500InternalServerError, obj);
            }
            catch (Exception ex)
            {
                object obj = new { ex.Message, Status = "Error" };
                return StatusCode(StatusCodes.Status500InternalServerError, obj);
            }

            return Ok(_resultado);
            // return new JsonResult(_resultado);
        }

        [AllowAnonymous]
        [HttpPost("CreateIdentitySignIn")]
        public async Task<IActionResult> CreateIdentitySignInAsync([FromBody] ReqSeleccionSucursalViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            //Validamos la existencia del usuario en la BD.
            string passwordHash256 = HashHelper.GetHash256(request.password);
            _resultado = await Task.Run(() => _brUsuario.ValidateUser(request.idUsuario, passwordHash256));

            if (!_resultado.Resultado)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = _resultado.Mensaje, Status = "Error" });

            //Datos del usuario
            USUARIO modelo = (USUARIO)_resultado.Data;
            modelo.ID_SUCURSAL = request.idSucursal;
            modelo.NOM_SUCURSAL = request.nomSucursal;

            //Generamos la identidad y cookie.
            await IdentitySignInAsync(modelo);

            _resultado = new ResultadoOperacion();
            _resultado.SetResultado(true, "", "/Home/Index");

            return Ok(_resultado);
        }

        [HttpPost("ChangeSucursal")]
        public async Task<IActionResult> ChangeSucursalAsync([FromBody] ReqCambiarSucursalViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            //Obtenemos el usuario actual 
            UsuarioLogueadoViewModel userCurrent = new Session(_accessor, _environment).GetUserLogged();

            //Datos del usuario
            USUARIO modelo = new USUARIO()
            {
                ID_SUCURSAL = request.IdSucursal,
                NOM_SUCURSAL = request.NomSucursal,
                ID_USUARIO = userCurrent.IdUsuario,
                NOM_USUARIO = userCurrent.NomUsuario,
                NOM_ROL = userCurrent.NomRol,
                FLG_CTRL_TOTAL = userCurrent.FlgCtrlTotal,
                FOTO = userCurrent.AvatarUri
            };

            //Generamos la identidad y cookie.
            await IdentitySignInAsync(modelo);

            _resultado = new ResultadoOperacion();
            _resultado.SetResultado(true,"", "/Home/Index");

            return Ok(_resultado);
        }

        [Route("IdentitySignOn")]
        [AllowAnonymous]
        public async Task<IActionResult> IdentitySignOnAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #region Metodos privados
        public async Task IdentitySignInAsync(USUARIO usuario)
        {
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, usuario.NOM_ROL),
                new Claim(ClaimTypes.Name, usuario.ID_USUARIO),
                new Claim("fullName", usuario.NOM_USUARIO),
                new Claim("idSucursal", usuario.ID_SUCURSAL),
                new Claim("nomSucursal", usuario.NOM_SUCURSAL),
                new Claim("flgCtrlTotal", usuario.FLG_CTRL_TOTAL.ToString()),
                new Claim("avatarUri", usuario.FOTO)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        #endregion
    }

    #region "Records"
    public record ReqCambiarSucursalViewModel(string IdSucursal, string NomSucursal);
    #endregion

}
