using AplicacionWeb.SistemaVentas.Models.Response;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Services.Security.Contracts
{
    public interface ITokenProcess
    {
        IEnumerable<Claim> GenerateClaims(UsuarioAuthViewModel obj);
        Task IdentitySignInAsync(IEnumerable<Claim> claims);
        Task IdentitySignOnAsync();
    }
}
