using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using Helper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Services.Security.Implementations
{
    public class TokenProcess : ITokenProcess
    {
         private IHttpContextAccessor _accessor;
        public TokenProcess(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }
        public IEnumerable<Claim> GenerateClaims(UsuarioAuthViewModel obj)
        {
            // CREAMOS EL PAYLOAD //
            IEnumerable<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Role, obj.NomRol),
                new Claim(ClaimTypes.Name, obj.IdUsuario),
                new Claim(ClaimTypes.NameIdentifier, obj.IdUsuario),
                new Claim("FullName", obj.NomUsuario),
                new Claim("IdSucursal", obj.IdSucursal),
                new Claim("NomSucursal", obj.NomSucursal),
                new Claim("FlgCtrlTotal", obj.FlgCtrlTotal.ToString()),
                new Claim("AvatarUri", obj.Foto)
            };

            return claims;
        }

        public async Task IdentitySignInAsync(IEnumerable<Claim> claims)
        {
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddDays(7)
            };

            await _accessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        }

        public async Task IdentitySignOnAsync()
        {
            await _accessor.HttpContext.SignOutAsync();
        }
    }
}
