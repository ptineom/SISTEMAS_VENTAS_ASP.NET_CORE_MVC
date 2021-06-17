using AplicacionWeb.SistemaVentas.Models.Request;
using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Helper.DTOGeneric;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly ISecurityService _securityService;
        private readonly ITokenProcess _tokenProcess;
        private readonly ISucursalUsuarioService _sucursalUsuarioService;
        private readonly ISessionIdentity _sessionIdentity;
        public LoginController(ISecurityService securityService, ITokenProcess tokenProcess, 
            ISucursalUsuarioService sucursalUsuarioService, ISessionIdentity sessionIdentity)
        {
            _securityService = securityService;
            _tokenProcess = tokenProcess;
            _sucursalUsuarioService = sucursalUsuarioService;
            _sessionIdentity = sessionIdentity;
        }

        [Route("[action]")]
        public IActionResult Index()
        {
            return View(new LoginRequest());
        }

        [HttpPost("UserValidate")]
        [AllowAnonymous]
        public async Task<IActionResult> UserValidateAsync([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = JsonSerializer.Serialize(ModelState)
                    }
                });

            if (string.IsNullOrWhiteSpace(request.idUsuario) || string.IsNullOrWhiteSpace(request.password))
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Usuario y/o contraseña incorrectas"
                    }
                });

            //Validamos la existencia del usuario en la BD.
            string passwordHash256 = HashHelper.GetHash256(request.password);
            var result = await Task.Run(() => _securityService.UserValidateAsync(request.idUsuario, passwordHash256));

            if (!result.Success)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = result.ErrorDetails.Message
                    }
                });

            //Obtenemos los datos del usuario validado satisfactoriamente.
            var usuario = (USUARIO)result.Data;

            var model = new UsuarioAuthViewModel()
            {
                IdUsuario = usuario.ID_USUARIO,
                NomUsuario = usuario.NOM_USUARIO,
                NomRol = usuario.NOM_ROL,
                IdSucursal = usuario.ID_SUCURSAL,
                NomSucursal = usuario.NOM_SUCURSAL,
                FlgCtrlTotal = usuario.FLG_CTRL_TOTAL,
                Foto = usuario.FOTO,
                CountSucursales = usuario.COUNT_SEDES
            };

            if (model.CountSucursales == 0)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = "El usuario no tiene configurado al menos 1 sucursal."
                    }
                });

            var response = new ResponseObject();


            //**** Si tiene una sede asignada, generamos el token.
            if (model.CountSucursales == 1)
            {
                //Generamos la identidad y cookie.
                var claims = _tokenProcess.GenerateClaims(model);
                await _tokenProcess.IdentitySignInAsync(claims);

                response = new ResponseObject()
                {
                    Data = new
                    {
                        ReturnUrl = "/Home/Index",
                        MasDeUnaSucursal = false
                    }
                };
            }
            else if (model.CountSucursales > 1)
            {
                //Recuperamos las sucursales del usuario
                List<SUCURSAL> list = await Task.Run(() => _sucursalUsuarioService.GetAllByCampusId(model.IdUsuario));

                response = new ResponseObject()
                {
                    Data = new
                    {
                        FlgVariasSucursales = true,
                        ListSucursales = list.Select(x => new SucursalViewModel()
                        {
                            IdSucursal = x.ID_SUCURSAL,
                            NomSucursal = x.NOM_SUCURSAL
                        })
                    }
                };
            }

            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("CreateIdentitySignIn")]
        public async Task<IActionResult> CreateIdentitySignInAsync([FromBody] SeleccionUsuarioSucursalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = JsonSerializer.Serialize(ModelState)
                    }
                });

            //Validamos la existencia del usuario en la BD.
            string passwordHash256 = HashHelper.GetHash256(request.password);
            var result = await Task.Run(() => _securityService.UserValidateAsync(request.idUsuario, passwordHash256));

            if (!result.Success)
                return NotFound(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status404NotFound,
                        Message = result.ErrorDetails.Message
                    }
                });

            //Obtenemos los datos del usuario validado satisfactoriamente.
            var usuario = (USUARIO)result.Data;

            var model = new UsuarioAuthViewModel()
            {
                IdUsuario = usuario.ID_USUARIO,
                NomUsuario = usuario.NOM_USUARIO,
                NomRol = usuario.NOM_ROL,
                IdSucursal = usuario.ID_SUCURSAL,
                NomSucursal = usuario.NOM_SUCURSAL,
                FlgCtrlTotal = usuario.FLG_CTRL_TOTAL,
                Foto = usuario.FOTO
            };

            model.IdSucursal = request.idSucursal;
            model.NomSucursal = request.nomSucursal;

            //Generamos la identidad y cookie.
            var claims = _tokenProcess.GenerateClaims(model);
            await _tokenProcess.IdentitySignInAsync(claims);

            var response = new ResponseObject()
            {
                Data = new
                {
                    ReturnUrl = "/Home/Index"
                }
            };

            return Ok(response);
        }

        [HttpPost("ChangeSucursal")]
        public async Task<IActionResult> ChangeSucursalAsync([FromBody] CambiarSucursalRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseObject()
                {
                    Success = false,
                    ErrorDetails = new ErrorDetails()
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = JsonSerializer.Serialize(ModelState)
                    }
                });

            //Obtenemos el usuario actual 
            UsuarioIdentityViewModel userCurrent = _sessionIdentity.GetUserLogged();

            var model = new UsuarioAuthViewModel()
            {
                IdUsuario = userCurrent.IdUsuario,
                NomUsuario = userCurrent.NomUsuario,
                NomRol = userCurrent.NomRol,
                IdSucursal = request.IdSucursal,
                NomSucursal = request.NomSucursal,
                FlgCtrlTotal = userCurrent.FlgCtrlTotal,
                Foto = userCurrent.AvatarUri
            };

            //Generamos la identidad y cookie.
            var claims = _tokenProcess.GenerateClaims(model);
            await _tokenProcess.IdentitySignInAsync(claims);

            var response = new ResponseObject()
            {
                Data = "/Home/Index"
            };

            return Ok(response);
        }

        [Route("IdentitySignOn")]
        [AllowAnonymous]
        public async Task<IActionResult> IdentitySignOnAsync()
        {
            await _tokenProcess.IdentitySignOnAsync();
            return RedirectToAction("Index", "Home");
        }
    }

    #region "Records"
    public record CambiarSucursalRequest(string IdSucursal, string NomSucursal);
    #endregion

}
