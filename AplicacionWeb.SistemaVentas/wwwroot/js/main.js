'use strict'

var oMain = {
    init: function () {

        //Scroll
        $("#sidebar").mCustomScrollbar({
            theme: "minimal"
        });

        //Sidebar
        Array.from(document.querySelectorAll('.sidebar-expandir')).forEach(el => {
            el.addEventListener('click', () => {
                document.getElementById('sidebar').classList.toggle('active');
                document.getElementById('content').classList.toggle('active');
            })
        })

        //Mostrar vista del menú
        Array.from(document.querySelectorAll('#sidebar-menu li a[data-flgFormulario="True"]')).forEach(a => {
            a.addEventListener('click', (e) => {
                let elem = e.currentTarget;

                //Se usa un sessionStorage con el fin a que se elimine cuando cerramos la ventana si nos olvidamos cerrar sessión.
                sessionStorage.setItem("breadcrumb", elem.getAttribute("data-breadcrumb"));

                window.location.href = elem.getAttribute("data-route");
            });
        })

        //Con este procedimiento cada vez que hacemos click en cualquier item del menú que tenga hijos, se
        //limpiará si detecta que otros items tienen los hijos expandidos para luego expandir el mismo item con sus ancestros.
        Array.from(document.querySelectorAll('#sidebar-menu li a[data-bs-toggle="collapse"] + ul')).forEach(ul => {
            //Despues de haberse mostrado el panel de hijos.
            ul.addEventListener('shown.bs.collapse', (e) => {
                //ocultanos todos los expandidos.
                Array.from(document.querySelectorAll('#sidebar-menu li a[aria-expanded=true')).forEach(a => {
                    a.setAttribute('aria-expanded', false);
                    a.classList.add('collapsed');
                    a.nextElementSibling.classList.remove('show');
                });

                let a = e.target.previousElementSibling;
                //obtenemos el breadcrumb
                let breadcrumb = a.getAttribute("data-breadcrumb");

                //Expandimos todos los ancestrales del elemento a.
                let rows = breadcrumb.split(',');
                rows.forEach(c => {
                    let id = c.split('|')[0];

                    let elem = document.querySelector(`#sidebar-menu li a[data-id="${id}"`);
                    if (elem.parentElement.classList.contains("treeview")) {
                        elem.setAttribute('aria-expanded', true);
                        elem.classList.remove('collapsed');
                        elem.nextElementSibling.classList.add('show');
                    }
                });
            })
        });

        document.getElementById('btnCerrarSesion').addEventListener('click', (e) => {
            e.preventDefault();

            if (sessionStorage.getItem('breadcrumb') != null)
                sessionStorage.removeItem("breadcrumb");

            window.location.href = "/Login/IdentitySignOn";
        });

        //Agregar breadcrumb referente al menu item elegido en el menu y expandir.
        oMain.showComponentsLayout();

        //Interceptores de axios
        axios.interceptors.request.use(function (config) {
            return config;
        }, function (error) {
            return Promise.reject(error);
        });

        //Botón de aperturar/cerrar caja
        let btnAperturarCaja = document.getElementById('btnAperturarCaja');
        btnAperturarCaja.addEventListener('click', () => {
            oModalCajaApertura.show().then(() => {
                //Verificar estado de la caja
                oMain.verificarEstadoCaja(true);
            });
        });
    
        //Verificar estado de la caja
        oMain.verificarEstadoCaja(false);

        ///*Configurando signalr*/
        var connection = new signalR.HubConnectionBuilder().withUrl("/cambiarestadocajahub").build();

        //Iniciamos la conección
        connection.start().then(() => {
            console.log("Signalr conectado");
        }).catch((error) => {
            console.error(error.toString());
            //setTimeout(() => start(), 5000);
        });

        //connection.onclose(async () => start());

        //Método que se invocará desde el servidor.
        connection.on("actualizarEstadoCaja", () => {
            console.log("entro");
            //Verificar estado de la caja
            oMain.verificarEstadoCaja(true);
        });
    },
    //start: async function () {
    //    try {
    //        await connection.start();
    //    } catch (err) {
    //        setTimeout(() => start(), 5000);
    //    }
    //},
    verificarEstadoCaja: function (reload) {
        if (reload == undefined)
            reload = false;

        //Se recargará la pagina cuando es llamado desde el modal de apertura/cierre de caja y al usar el metodo de signalr.
        //con el fin que las vistas que trabajen segun el estado de la caja, púedan detectar algun cambio en la caja.
        if (reload) {
            location.reload(true);
        } else {
            oModalCajaApertura.verificarEstadoCaja().then((bCajaAbierta) => {
                if (bCajaAbierta) {
                    btnAperturarCaja.classList.remove("btn-warning");
                    btnAperturarCaja.classList.add("btn-success");

                    btnAperturarCaja.querySelector('i').classList.remove('bi-lock');
                    btnAperturarCaja.querySelector('i').classList.add('bi-unlock');
                } else {
                    btnAperturarCaja.classList.remove("btn-success");
                    btnAperturarCaja.classList.add("btn-warning");

                    btnAperturarCaja.querySelector('i').classList.remove('bi-unlock');
                    btnAperturarCaja.querySelector('i').classList.add('bi-lock');
                }

            }).catch((error) => {
                debugger;
                oAlerta.alerta({
                    title: error,
                    type: "warning"
                });
            })
        }
    },
    showComponentsLayout: function () {
        //Si detecta que tiene un breadcrumb los mostrará y expandira el menú referente al breadcrumb.
        let breadcrumb = sessionStorage.getItem('breadcrumb');
        if (breadcrumb == null)
            return;

        let rows = breadcrumb.split(',');
        let li = '';

        let count = rows.length;
        for (var i = 0; i < count; i++) {
            let id = rows[i].split('|')[0];
            let name = rows[i].split('|')[1];

            //construcción del breadcrumb.
            li += `<li class='breadcrumb-item ${((i + 1) == count ? 'active' : '')}'><a href='#'>${name}</a></li>`;

            //Expandir menus hijos.
            let a = document.querySelector(`#sidebar-menu li a[data-id="${id}"]`);
            let parent = a.parentElement;
            if (parent.classList.contains('treeview')) {
                a.setAttribute("aria-expanded", true);
                a.nextElementSibling.classList.add('show');
            }
        }
        //breadcrumb
        document.getElementById('breadcrumb').innerHTML = li;

        //Titulo de la vista
        let title = rows[rows.length - 1].split('|')[1];
        document.getElementById('bc-titulo').textContent = title;
    }
}

document.addEventListener("DOMContentLoaded", oMain.init);