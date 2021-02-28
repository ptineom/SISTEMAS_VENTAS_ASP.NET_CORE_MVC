using AplicacionWeb.SistemaVentas.Models;
using AplicacionWeb.SistemaVentas.Models.Seguridad;
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
        private IResultadoOperacion _resultado { get; set; }
        private BrUsuario brUsuario = null;
        private IHttpContextAccessor _httpContextAccessor = null;
        private IWebHostEnvironment _environment= null;

        public LoginController(IResultadoOperacion resultado, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment environment)
        {
            _resultado = resultado;
            brUsuario = new BrUsuario();
            _httpContextAccessor = httpContextAccessor;
            _environment = environment;
        }
        [Route("[action]")]
        public IActionResult Index()
        {
            return View(new ReqLoginViewModel());
        }

        [HttpPost("acceder")]
        [AllowAnonymous]
        public async Task<IActionResult> accederAsync([FromBody] ReqLoginViewModel login)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            if (string.IsNullOrWhiteSpace(login.idUsuario) || string.IsNullOrWhiteSpace(login.password))
                return BadRequest(new { Message = "Usuario y/o contraseña incorrectas", Status = "Error" });

            //Validamos la existencia del usuario en la BD.
            string passwordHash256 = HashHelper.GetHash256(login.password);
            _resultado = await Task.Run(() => brUsuario.acceder(login.idUsuario, passwordHash256));

            if (!_resultado.bResultado)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = _resultado.sMensaje, Status = "Error" });

            try
            {
                //Datos del usuario
                USUARIO modelo = (USUARIO)_resultado.data;
                int countSedes = modelo.COUNT_SEDES;

                if (countSedes == 0)
                    return NotFound(new { Message = "Este usuario no tiene configurado al menos una sede.", Status = "Error" });

                ResultadoLoginViewModel resultadoLogin = null;

                //**** Si tiene una sede asignada, generamos el token.
                if (countSedes == 1)
                {
                    //Generamos la identidad y cookie.
                    await identitySignIn(modelo);

                    resultadoLogin = new ResultadoLoginViewModel()
                    {
                        returnUrl = "/Home/Index",
                        masDeUnaSucursal = false
                    };
                }
                else if (countSedes > 1)
                {
                    BrSucursalUsuario brSucursalUsuario = new BrSucursalUsuario();
                    _resultado = new ResultadoOperacion();

                    //Lista de sucursales por usuario.
                    _resultado = brSucursalUsuario.listaSucursalPorUsuario(login.idUsuario);

                    if (!_resultado.bResultado)
                        return StatusCode(StatusCodes.Status500InternalServerError, new { Message = _resultado.sMensaje, Status = "Error" });

                    List<SucursalViewModel> sucursales = ((List<SUCURSAL>)_resultado.data).Select(x => new SucursalViewModel()
                    {
                        idSucursal = x.ID_SUCURSAL,
                        nomSucursal = x.NOM_SUCURSAL
                    }).ToList<SucursalViewModel>();

                    resultadoLogin = new ResultadoLoginViewModel()
                    {
                        masDeUnaSucursal = true,
                        sucursales = sucursales
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
        [HttpPost("createIdentitySignIn")]
        public async Task<IActionResult> createIdentitySignInAsync([FromBody] ReqSeleccionSucursalViewModel sucursal)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            //Validamos la existencia del usuario en la BD.
            string passwordHash256 = HashHelper.GetHash256(sucursal.password);
            _resultado = await Task.Run(() => brUsuario.acceder(sucursal.idUsuario, passwordHash256));

            if (!_resultado.bResultado)
                return StatusCode(StatusCodes.Status404NotFound, new { Message = _resultado.sMensaje, Status = "Error" });

            //Datos del usuario
            USUARIO modelo = (USUARIO)_resultado.data;
            modelo.ID_SUCURSAL = sucursal.idSucursal;
            modelo.NOM_SUCURSAL = sucursal.nomSucursal;

            //Generamos la identidad y cookie.
            await identitySignIn(modelo);

            _resultado = new ResultadoOperacion();
            _resultado.SetResultado(true, "", "/Home/Index");

            return Ok(_resultado);
        }

        [HttpPost("cambiarSucursal")]
        public async Task<IActionResult> cambiarSucursalAsync([FromBody] ReqCambiarSucursalViewModel sucursal)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Mesagge = ModelState, Status = "Error" });

            UsuarioLogueadoViewModel userCurrent = new Session(_httpContextAccessor, _environment).obtenerUsuarioLogueado();

            //Datos del usuario
            USUARIO modelo = new USUARIO()
            {
                ID_SUCURSAL = sucursal.idSucursal,
                NOM_SUCURSAL = sucursal.nomSucursal,
                ID_USUARIO = userCurrent.idUsuario,
                NOM_USUARIO = userCurrent.nomUsuario,
                NOM_ROL = userCurrent.nomRol,
                FLG_CTRL_TOTAL = userCurrent.flgCtrlTotal,
                FOTO = userCurrent.avatarUri
            };

            //Generamos la identidad y cookie.
            await identitySignIn(modelo);

            _resultado = new ResultadoOperacion();
            _resultado.SetResultado(true,"", "/Home/Index");

            return Ok(_resultado);
        }

        [Route("identitySignOn")]
        [AllowAnonymous]
        public async Task<IActionResult> identitySignOn()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #region Metodos privados
        public async Task identitySignIn(USUARIO usuario)
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
    public record ReqCambiarSucursalViewModel(string idSucursal, string nomSucursal);
    #endregion

}
