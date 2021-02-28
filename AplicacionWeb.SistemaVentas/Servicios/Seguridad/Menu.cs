using AplicacionWeb.SistemaVentas.Models.Seguridad;
using CapaNegocio;
using Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AplicacionWeb.SistemaVentas.Servicios.Seguridad
{
    public class Menu
    {
        private ResultadoOperacion _resultado = null;
        public Menu()
        {
            _resultado = new ResultadoOperacion();
        }

        /// <summary>
        ///  Lista de menu según el usuario.
        /// </summary>
        /// <param name="idUsuario">Usuario por el que se hará el filtrado del los menús</param>
        /// <returns></returns>
        public MenuItem obtenerMenuPorUsuario(string idUsuario)
        {
            BrAplicacion brAplicacion = new BrAplicacion();

            //Obtenemos la lista de menu según el usuario.
            _resultado = brAplicacion.listarMenuUsuario(idUsuario);

            if (!_resultado.bResultado)
                throw new Exception(_resultado.sMensaje);

            //Construímos el menú a requerimiento del cliente.
            MenuItem menuItem = null;
            if (_resultado.data != null)
            {
                menuItem = new MenuItem();
                List<APLICACION> listaGeneral = (List<APLICACION>)_resultado.data;

                //Raiz del arbol el cual dará inicio.
                APLICACION aplicacionRaiz = listaGeneral.FirstOrDefault(x => x.FLG_RAIZ);

                //Método recursivo que construirá el arbol de menus.
                setChildren(aplicacionRaiz, listaGeneral, menuItem);

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
        private void setChildren(APLICACION aplicacion, List<APLICACION> listaGeneral, MenuItem menuItem)
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
                List<MenuItem> listaSubMenu = new List<MenuItem>();
                foreach (var child in childs)
                {
                    MenuItem subMenu = new MenuItem();
                    setChildren(child, listaGeneral, subMenu);
                    listaSubMenu.Add(subMenu);
                };
                menuItem.children = listaSubMenu;
            }
        }

    }
}


