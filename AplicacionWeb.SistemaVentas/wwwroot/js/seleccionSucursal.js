'use strict'
var oSeleccionSucursal = {
    init: function () {
        document.getElementById('btnAtras').addEventListener('click', () => window.history.back());
        let btnSeguir = document.getElementById('btnSeguir');
        btnSeguir.addEventListener('click', oLogin.seleccionarSucursal);
        btnSeguir.innerHTML = '<i class="bi bi-save"></i> Guardar cambios'
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
                    window.history.back();
                }
            }).catch((error) => {
                alert(error.response.data.message);
            }).finally(() => oMetodos.hideLoading());
        }
    },
}

document.addEventListener("DOMContentLoaded", function () {
    oSeleccionSucursal.init();
})