using AplicacionWeb.SistemaVentas.Models.Seguridad;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Servicios.Seguridad
{
    public class Session
    {
        private IHttpContextAccessor _accessor;
        private IWebHostEnvironment _enviroment;

        public Session(IHttpContextAccessor accessor, IWebHostEnvironment environment)
        {
            _accessor = accessor;
            _enviroment = environment;
        }
        public Session(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        public bool ExistUserInSession()
        {
            bool bExiste = false;

            if (_accessor.HttpContext.User != null)
            {
                bExiste = _accessor.HttpContext.User.Identity.IsAuthenticated;
            }

            return bExiste;
        }
        
        public UsuarioLogueadoViewModel GetUserLoggedFull()
        {
            UsuarioLogueadoViewModel modelo = null;
            if (_accessor.HttpContext.User != null && _accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = (ClaimsIdentity)_accessor.HttpContext.User.Identity;
                if (identity != null)
                {
                    var claims = identity.Claims;

                    string avatarUri = claims.FirstOrDefault(x => x.Type == "avatarUri").Value;
                    string avatarB64 = string.Empty;

                    if (!string.IsNullOrEmpty(avatarUri))
                    {
                        string uri = Path.Combine(_enviroment.WebRootPath, Configuraciones.UPLOAD_EMPLEADOS, avatarUri);
                        byte[] file = File.ReadAllBytes(uri);
                        avatarB64 = Convert.ToBase64String(file);
                    }

                    modelo = new UsuarioLogueadoViewModel
                    {
                        IdUsuario = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                        NomUsuario = claims.FirstOrDefault(x => x.Type == "fullName").Value,
                        NomRol = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        IdSucursal = claims.FirstOrDefault(x => x.Type == "idSucursal").Value,
                        NomSucursal = claims.FirstOrDefault(x=> x.Type == "nomSucursal").Value,
                        FlgCtrlTotal = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "flgCtrlTotal").Value),
                        AvatarB64 = avatarB64,
                        AvatarUri = avatarUri
                    };
                }
            }
            return modelo;
        }

        public UsuarioLogueadoViewModel GetUserLogged()
        {
            UsuarioLogueadoViewModel modelo = null;
            if (_accessor.HttpContext.User != null && _accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = (ClaimsIdentity)_accessor.HttpContext.User.Identity;
                if (identity != null)
                {
                    var claims = identity.Claims;

                    modelo = new UsuarioLogueadoViewModel
                    {
                        IdUsuario = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                        NomUsuario = claims.FirstOrDefault(x => x.Type == "fullName").Value,
                        NomRol = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        IdSucursal = claims.FirstOrDefault(x => x.Type == "idSucursal").Value,
                        NomSucursal = claims.FirstOrDefault(x => x.Type == "nomSucursal").Value,
                        FlgCtrlTotal = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "flgCtrlTotal").Value),
                        AvatarUri = claims.FirstOrDefault(x => x.Type == "avatarUri").Value
                    };
                }
            }
            return modelo;
        }
    }
}
