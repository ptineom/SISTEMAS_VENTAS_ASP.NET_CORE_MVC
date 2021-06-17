using AplicacionWeb.SistemaVentas.Models.Response;
using AplicacionWeb.SistemaVentas.Models.ViewModel;
using AplicacionWeb.SistemaVentas.Services.Security.Contracts;
using CapaNegocio.Contracts;
using Entidades;
using Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Services.Security.Implementations
{
    public class SessionIdentity : ISessionIdentity
    {
        private IHttpContextAccessor _accessor;
        private readonly ISecurityService _securityService;
        private readonly IWebHostEnvironment _environment;

        public SessionIdentity(ISecurityService securityService, IHttpContextAccessor accessor, IWebHostEnvironment environment)
        {
            _accessor = accessor;
            _securityService = securityService;
            _environment = environment;
        }

        public bool ExistUserInSession()
        {
            bool bExiste = false;

            if (_accessor.HttpContext.User != null)
                bExiste = _accessor.HttpContext.User.Identity.IsAuthenticated;

            return bExiste;
        }

        public UsuarioIdentityViewModel GetUserLogged()
        {

            UsuarioIdentityViewModel model = null;
            if (_accessor.HttpContext.User != null && _accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = (ClaimsIdentity)_accessor.HttpContext.User.Identity;
                if (identity != null)
                {
                    var claims = identity.Claims;
                    model = new UsuarioIdentityViewModel
                    {
                        IdUsuario = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                        NomUsuario = claims.FirstOrDefault(x => x.Type == "FullName").Value,
                        NomRol = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        IdSucursal = claims.FirstOrDefault(x => x.Type == "IdSucursal").Value,
                        FlgCtrlTotal = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "FlgCtrlTotal").Value),
                        NameIdentifier = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value,
                        AvatarUri = claims.FirstOrDefault(x => x.Type == "AvatarUri").Value
                    };
                }
            }
            return model;
        }

        public async Task<MenuItemViewModel> GetMenuByUserIdAsync(string userId)
        {
            //Obtenemos la lista de menu según el usuario.
            var result = await _securityService.GetMenuByUserIdAsync(userId);

            //Construímos el menú a requerimiento del cliente.
            MenuItemViewModel menuItem = null;
            if (result != null)
            {
                menuItem = new MenuItemViewModel();

                //Raiz del arbol el cual dará inicio.
                APLICACION aplicacionRaiz = result.FirstOrDefault(x => x.FLG_RAIZ);

                //Método recursivo que construirá el arbol de menus.
                SetChildren(aplicacionRaiz, result, menuItem);

                //Marcamos a los primeros hijos como raiz para la renderización en la vista.
                menuItem.Children.ForEach((elem) => elem.FlgRaiz = true);
            }
            return menuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aplicacion">Menú actual que se usará para buscar a sus hijos.</param>
        /// <param name="listaGeneral">Lista de todos los menús del usuario</param>
        /// <param name="menuItem">Resultado del arbol de menús</param>
        private void SetChildren(APLICACION aplicacion, List<APLICACION> listaGeneral, MenuItemViewModel menuItem)
        {
            menuItem.Label = aplicacion.NOM_APLICACION;
            menuItem.Icon = aplicacion.ICON;
            menuItem.Id = aplicacion.ID_APLICACION;
            menuItem.FlgFormulario = aplicacion.FLG_FORMULARIO;
            menuItem.Breadcrumbs = aplicacion.BREADCRUMS;

            if (aplicacion.FLG_FORMULARIO)
                menuItem.Route = $"/{aplicacion.NOM_CONTROLADOR}/{aplicacion.NOM_FORMULARIO}";

            //Si tiene hijos ejeuta la recursividad
            var childs = listaGeneral.Where(x => x.ID_APLICACION_PADRE == aplicacion.ID_APLICACION).ToList();
            if (childs.Count > 0)
            {
                List<MenuItemViewModel> listaSubMenu = new List<MenuItemViewModel>();
                foreach (var child in childs)
                {
                    MenuItemViewModel subMenu = new MenuItemViewModel();
                    SetChildren(child, listaGeneral, subMenu);
                    listaSubMenu.Add(subMenu);
                };
                menuItem.Children = listaSubMenu;
            }
        }

        public UsuarioIdentityViewModel GetUserLoggedFull()
        {
            UsuarioIdentityViewModel model = null;
            if (_accessor.HttpContext.User != null && _accessor.HttpContext.User.Identity.IsAuthenticated)
            {
                ClaimsIdentity identity = (ClaimsIdentity)_accessor.HttpContext.User.Identity;
                if (identity != null)
                {
                    var claims = identity.Claims;

                    string avatarUri = claims.FirstOrDefault(x => x.Type == "AvatarUri").Value;
                    string avatarB64 = string.Empty;

                    if (!string.IsNullOrEmpty(avatarUri))
                    {
                        string uri = Path.Combine(_environment.WebRootPath, Configuraciones.UPLOAD_EMPLEADOS, avatarUri);
                        byte[] file = File.ReadAllBytes(uri);
                        avatarB64 = Convert.ToBase64String(file);
                    }

                    model = new UsuarioIdentityViewModel()
                    {
                        IdUsuario = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                        NomUsuario = claims.FirstOrDefault(x => x.Type == "FullName").Value,
                        NomRol = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value,
                        IdSucursal = claims.FirstOrDefault(x => x.Type == "IdSucursal").Value,
                        NomSucursal = claims.FirstOrDefault(x => x.Type == "NomSucursal").Value,
                        FlgCtrlTotal = Convert.ToBoolean(claims.FirstOrDefault(x => x.Type == "FlgCtrlTotal").Value),
                        AvatarB64 = avatarB64,
                        AvatarUri = avatarUri
                    };
                }
            }
            return model;
        }
    }
}
