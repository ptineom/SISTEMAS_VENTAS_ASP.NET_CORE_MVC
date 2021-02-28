'use strict'
var oSeleccionSucursal = {
    init: function () {
        document.getElementById('btnAtras').addEventListener('click', () => window.history.back());
        let btnSeguir = document.getElementById('btnSeguir');
        btnSeguir.addEventListener('click', oSeleccionSucursal.seleccionarSucursal);
        btnSeguir.innerHTML = '<i class="bi bi-save"></i> Guardar cambios';

        //validaciones creados en el cliente
        $("#form-seleccionSucursal").validate({
            rules: {
                cboSucursales: {
                    required: true
                }
            },
            messages: {
                cboSucursales: "Debe de seleccionar la sucursal"
            }
        })
    },
    seleccionarSucursal: function () {
        if ($("#form-seleccionSucursal").valid()) {
            oMetodos.showLoading('#cardSeleccionSucursal');
            let cboSucursales = document.getElementById('cboSucursales');
            let nomSucursal = cboSucursales.options[cboSucursales.selectedIndex].text;

            let parameters = {
                idSucursal: cboSucursales.value,
                nomSucursal
            };
            axios.post("/Login/cambiarSucursal", parameters).then((response) => {
                if (response.data.bResultado) {
                    let returnUrl = response.data.data;
                    window.location.href = returnUrl;
                }
            }).catch((error) => {
                alert(error.response.data.message);
            }).finally(() => oMetodos.hideLoading('#cardSeleccionSucursal'));
        }
    },
}

document.addEventListener("DOMContentLoaded", function () {
    oSeleccionSucursal.init();
})