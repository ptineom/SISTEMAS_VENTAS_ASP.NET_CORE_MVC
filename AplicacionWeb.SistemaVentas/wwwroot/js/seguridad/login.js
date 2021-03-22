﻿"use strict"
var oLogin = {
    init: function () {
        $.validator.unobtrusive.parse("#form-login");
        document.getElementById('btnIniciar').addEventListener('click', oLogin.ingresarUsuario);
        document.getElementById('txtUsuario').addEventListener('keyup', (e) => oHelper.teclaEnter(e, 'txtPassword'));
        document.getElementById('txtPassword').addEventListener('keyup', function (e) {
            if (e.key == "Enter")
                oLogin.ingresarUsuario();
        });
        document.getElementById('btnAtras').addEventListener('click', () => {
            oLogin.cambiarCard("vistaLogin");

            //Ocultamos la contraseña en caso este en estado mostrado.
            let icon = document.getElementById('iconPassword').children[0];
            if (icon.classList.contains('bi-eye-slash-fill')) {
                icon.classList.remove('bi-eye-slash-fill');
                icon.classList.add('bi-eye-fill');
                document.getElementById('txtPassword').type = "password";
            };

            var validator = $("#form-seleccionSucursal").validate();
            validator.resetForm();
        });
        document.getElementById('btnSeguir').addEventListener('click', oLogin.seleccionarSucursal);
        document.getElementById('iconPassword').addEventListener('click', function () {
            let icon = this.children[0];
            if (icon.classList.contains('bi-eye-fill')) {
                icon.classList.remove('bi-eye-fill');
                icon.classList.add('bi-eye-slash-fill');
                document.getElementById('txtPassword').type = "text";
            } else {
                icon.classList.remove('bi-eye-slash-fill');
                icon.classList.add('bi-eye-fill');
                document.getElementById('txtPassword').type = "password";
            }
        })

        oLogin.cambiarCard('vistaLogin');

        //validaciones creados en el cliente
        $("#form-seleccionSucursal").validate({
            rules: {
                cboSucursales: {
                    required:true
                }
            },
            messages: {
                cboSucursales: "Debe de seleccionar la sucursal"
            }
        })

       setTimeout(() => document.getElementById('txtUsuario').focus(), 300);
    },
    verPassword: function (icon) {
        if (icon.classList.contains('bi-eye-fill')) {
            icon.classList.remove('bi-eye-fill');
            icon.classList.add('bi-eye-slash-fill');
            document.getElementById('txtPassword').type = "text";
        } else {
            icon.classList.remove('bi-eye-slash-fill');
            icon.classList.add('bi-eye-fill');
            document.getElementById('txtPassword').type = "password";
        }
    },
    seleccionarSucursal: function () {
        if ($("#form-seleccionSucursal").valid()) {
            oHelper.showLoading('#cardSeleccionSucursal');
            let cboSucursales = document.getElementById('cboSucursales');
            let nomSucursal = cboSucursales.options[cboSucursales.selectedIndex].text;

            let parameters = {
                idUsuario: document.getElementById('txtUsuario').value,
                password: document.getElementById('txtPassword').value,
                idSucursal: cboSucursales.value,
                nomSucursal
            };
            axios.post("/Login/CreateIdentitySignIn", parameters).then((response) => {
                if (response.data.Resultado) {
                    let returnUrl = response.data.Data;
                    window.location.href = returnUrl;
                }
            }).catch((error) => {
                alert(error.response.data.message);
            }).finally(() => oHelper.hideLoading());
        }
    },
    cambiarCard: function (vista) {
        if (vista == "vistaLogin") {
            document.getElementById('vistaLogin').style = "display:block";
            document.getElementById('vistaSeleccionSucursal').style = "display:none";
        } else if (vista == "vistaSeleccionSucursal") {
            document.getElementById('vistaLogin').style = "display:none";
            document.getElementById('vistaSeleccionSucursal').style = "display:block";
        }
    },
    ingresarUsuario: function () {
        if ($("#form-login").valid()) {
            oHelper.showLoading('#cardLogin');

            let parameters = {
                idUsuario: document.getElementById('txtUsuario').value,
                password: document.getElementById('txtPassword').value
            };

            axios.post("/Login/ValidateUser", parameters).then((response) => {
                if (response.data.Resultado) {
                    let data = response.data.Data;
                    let masDeUnaSucursal = data.MasDeUnaSucursal

                    //Si tiene mas de de una sucursal mostramos el formulario para que pueda seleccionar.
                    if (masDeUnaSucursal) {
                        let sucursales = data.Sucursales;
                        let frag = document.createDocumentFragment();
                        let cboSucursales = document.getElementById('cboSucursales');
                        cboSucursales.innerHTML = "";

                        //Cargamos el combo de sucursales
                        sucursales.forEach(x => {
                            let option = document.createElement('option');
                            option.value = x.IdSucursal;
                            option.text = x.NomSucursal;
                            frag.appendChild(option);
                        });
                        cboSucursales.appendChild(frag);

                        //No redijimos al formulario para poder seleccionar el sucursal.
                        oLogin.cambiarCard("vistaSeleccionSucursal");
                    } else {
                        //Si tiene una sucursal ya asignada entonces nos redijimos al home
                        window.location.href = data.returnUrl;
                    }
                }
            }).catch((error) => {
                alert(error.response.data.message);
            }).finally(() => oHelper.hideLoading());
        }
    }

}

document.addEventListener("DOMContentLoaded", function () {
    oLogin.init();
});