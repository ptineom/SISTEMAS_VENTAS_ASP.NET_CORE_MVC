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
        private IHttpContextAccessor _httpContext { get; }
        private IWebHostEnvironment _enviroment { get; }
        public Session(IHttpContextAccessor httpContext, IWebHostEnvironment environment)
        {
            _httpContext = httpContext;
            _enviroment = environment;
        }

        public bool existUserInSession()
        {
            bool bExiste = false;

            if (_httpContext.HttpContext.User != null)
            {
                bExiste = _httpContext.HttpContext.User.Identity.IsAuthenticated;
            }

            return bExiste;
        }

        public UsuarioLogueadoViewModel obtenerUsuarioLogueado()
        {
            UsuarioLogueadoViewModel modelo = null;
            if (existUserInSession())
            {
                ClaimsIdentity identity = (ClaimsIdentity)_httpContext.HttpContext.User.Identity;
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
                        idUsuario = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                        nomUsuario = claims.FirstOrDefault(x => x.Type == "fullName").Value,
                        nomRol = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        idSucursal = claims.FirstOrDefault(x => x.Type == "idSucursal").Value,
                        nomSucursal = claims.FirstOrDefault(x=> x.Type == "nomSucursal").Value,
                        flgCtrlTotal = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "flgCtrlTotal").Value),
                        avatarB64 = avatarB64,
                        avatarUri = avatarUri
                    };
                }
            }
            return modelo;
        }

        public static bool existUserInSessionStatic()
        {
            bool bExiste = false;
            IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();

            if (httpContextAccessor.HttpContext.User != null)
                  bExiste = httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

            return bExiste;
        }

        public static UsuarioLogueadoViewModel obtenerUsuarioLogueadoStatic()
        {
            UsuarioLogueadoViewModel modelo = null;
            if (existUserInSessionStatic())
            {
                IHttpContextAccessor httpContextAccessor = new HttpContextAccessor();
                ClaimsIdentity identity = (ClaimsIdentity)httpContextAccessor.HttpContext.User.Identity;

                if (identity != null)
                {
                    var claims = identity.Claims;
                    modelo = new UsuarioLogueadoViewModel
                    {
                        idUsuario = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                        nomUsuario = claims.FirstOrDefault(x => x.Type == "fullName").Value,
                        nomRol = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        idSucursal = claims.FirstOrDefault(x => x.Type == "idSucursal").Value,
                        flgCtrlTotal = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "flgCtrlTotal").Value)
                    };
                }
            }
            return modelo;
        }

       
    }
}
