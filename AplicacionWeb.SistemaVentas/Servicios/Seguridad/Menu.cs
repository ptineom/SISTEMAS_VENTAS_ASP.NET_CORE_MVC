using AplicacionWeb.SistemaVentas.Models.Seguridad;
using CapaNegocio;
using Entidades;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Servicios.Seguridad
{
    public class Menu
    {
        private ResultadoOperacion _resultado = null;
        private IConfiguration _configuration = null;

        public Menu(IConfiguration configuration)
        {
            _resultado = new ResultadoOperacion();
            _configuration = configuration;
        }

        /// <summary>
        ///  Lista de menu según el usuario.
        /// </summary>
        /// <param name="idUsuario">Usuario por el que se hará el filtrado del los menús</param>
        /// <returns></returns>
        public MenuItemModel GetMenuByUserId(string idUsuario)
        {
            BrAplicacion brAplicacion = new BrAplicacion(_configuration);

            //Obtenemos la lista de menu según el usuario.
            _resultado = brAplicacion.GetMenuByUserId(idUsuario);

            if (!_resultado.Resultado)
                throw new Exception(_resultado.Mensaje);

            //Construímos el menú a requerimiento del cliente.
            MenuItemModel menuItem = null;
            if (_resultado.Data != null)
            {
                menuItem = new MenuItemModel();
                List<APLICACION> listaGeneral = (List<APLICACION>)_resultado.Data;

                //Raiz del arbol el cual dará inicio.
                APLICACION aplicacionRaiz = listaGeneral.FirstOrDefault(x => x.FLG_RAIZ);

                //Método recursivo que construirá el arbol de menus.
                SetChildren(aplicacionRaiz, listaGeneral, menuItem);

                //Marcamos a los primeros hijos como raiz para la renderización en la vista.
                menuItem.children.ForEach((elem) => elem.flgRaiz = true);
            }
            return menuItem;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aplicacion">Menú actual que se usará para buscar a sus hijos.</param>
        /// <param name="listaGeneral">Lista de todos los menús del usuario</param>
        /// <param name="menuItem">Resultado del arbol de menús</param>
        private void SetChildren(APLICACION aplicacion, List<APLICACION> listaGeneral, MenuItemModel menuItem)
        {
            menuItem.label = aplicacion.NOM_APLICACION;
            menuItem.icon = aplicacion.ICON;
            menuItem.id = aplicacion.ID_APLICACION;
            menuItem.flgFormulario = aplicacion.FLG_FORMULARIO;
            menuItem.breadcrumbs = aplicacion.BREADCRUMS;

            if (aplicacion.FLG_FORMULARIO)
                menuItem.route = $"/{aplicacion.NOM_CONTROLADOR}/{aplicacion.NOM_FORMULARIO}";

            //Si tiene hijos ejeuta la recursividad
            var childs = listaGeneral.Where(x => x.ID_APLICACION_PADRE == aplicacion.ID_APLICACION).ToList();
            if (childs.Count > 0)
            {
                List<MenuItemModel> listaSubMenu = new List<MenuItemModel>();
                foreach (var child in childs)
                {
                    MenuItemModel subMenu = new MenuItemModel();
                    SetChildren(child, listaGeneral, subMenu);
                    listaSubMenu.Add(subMenu);
                };
                menuItem.children = listaSubMenu;
            }
        }

    }
}


