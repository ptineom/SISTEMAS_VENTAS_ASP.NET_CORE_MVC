using AplicacionWeb.SistemaVentas.Models.Seguridad;
using Entidades;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Servicios.Seguridad
{
    public class Session
    {
        private IHttpContextAccessor _httpContext { get; }
        public Session(IHttpContextAccessor httpContext)
        {
            _httpContext = httpContext;
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
