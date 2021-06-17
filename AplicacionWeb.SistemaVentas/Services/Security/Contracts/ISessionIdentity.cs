using AplicacionWeb.SistemaVentas.Models.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Services.Security.Contracts
{
    public interface ISessionIdentity
    {
        bool ExistUserInSession();
        UsuarioIdentityViewModel GetUserLogged();
        UsuarioIdentityViewModel GetUserLoggedFull();
        Task<MenuItemViewModel> GetMenuByUserIdAsync(string userId);
    }
}
