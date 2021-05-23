'use strict'
var oModalPagarCompra = {
    modelo: null,
    instance: null,
    resolve: null,
    reject: null,
    vista: 1,
    bGuardar: false,
    init: function () {
        let modalPagarCompra = document.getElementById('modalPagarCompra')

        modalPagarCompra.addEventListener('shown.bs.modal', function () {

            //Inicializando la vista 1
            document.getElementById('modalVista1').style.display = "block";
            document.getElementById('modalVista2').style.display = "none";

            let btnLeft = document.getElementById('btnLeft_modPagCom');
            btnLeft.classList.remove('btn-danger');
            btnLeft.classList.add('btn-success');

            btnLeft.childNodes[0].classList.remove("bi-x-circle-fill");
            btnLeft.childNodes[0].classList.add("bi-save");
            btnLeft.childNodes[1].textContent = " No"

            let btnRight = document.getElementById('btnRight_modPagCom');

            btnRight.classList.remove('btn-success');
            btnRight.classList.add('btn-primary');

            btnRight.childNodes[0].classList.remove("bi-check-circle-fill");
            btnRight.childNodes[0].classList.add("bi-arrow-right");
            btnRight.childNodes[1].textContent = " Si"
        });

        modalPagarCompra.addEventListener('hidden.bs.modal', function () {
            //Si en la vista 1 se hizo click en el boton izquierdo(NO) o en la vista 2 en el botón derecho(Aceptar)
            //se guardo bGuardar=true, indicandole que el registro será guardado en BD.
            if (oModalPagarCompra.bGuardar) {
                oModalPagarCompra.resolve({
                    bRetirarDinero: oModalPagarCompra.vista == 2 ? true : false,
                    montoRetiro: oModalPagarCompra.vista == 2 ? document.getElementById('txtMonRet_modPagCom').value : 0
                });

                oModalPagarCompra.bGuardar = false;
            } else {
                oModalPagarCompra.reject();
            };

            //Limpiamos los elementos al cerrar.
            oModalPagarCompra.modelo = null;
            document.getElementById('txtMonCaj_modPagCom').value = "";
            document.getElementById('txtSalPag_modPagCom').value = "";
            document.getElementById('txtMonTotCom_modPagCom').value = "";
            document.getElementById('txtMonRet_modPagCom').value = "";
        });

        document.getElementById('btnLeft_modPagCom').addEventListener('click', () => {
            //Si estamos en la vista 1 y le damos al boton NO, se guardará registro.
            if (oModalPagarCompra.vista == 1)
                oModalPagarCompra.bGuardar = true;

            oModalPagarCompra.instance.hide();
        })
        document.getElementById('btnRight_modPagCom').addEventListener('click', (e) => {
            let btnRight = e.currentTarget;

            if (oModalPagarCompra.vista == 1) {
                //Cambiando a la vista 2

                //Configurando los elementos para la vista 2
                oModalPagarCompra.vista = 2;
                document.getElementById('modalVista1').style.display = "none";
                document.getElementById('modalVista2').style.display = "block";

                let btnLeft = document.getElementById('btnLeft_modPagCom');
                btnLeft.classList.remove('btn-success');
                btnLeft.classList.add('btn-danger');

                btnLeft.childNodes[0].classList.remove("bi-save");
                btnLeft.childNodes[0].classList.add("bi-x-circle-fill");
                btnLeft.childNodes[1].textContent = " Cancelar"

                btnRight.classList.remove('btn-primary');
                btnRight.classList.add('btn-success');

                btnRight.childNodes[0].classList.remove("bi-arrow-right");
                btnRight.childNodes[0].classList.add("bi-check-circle-fill");
                btnRight.childNodes[1].textContent = " Aceptar"

                //Mostrando los montos en la vista 2
                let montoTotalCaja = oHelper.formatoMoneda(oModalPagarCompra.modelo.sgnMoneda, oModalPagarCompra.modelo.montoTotalCaja);
                document.getElementById('txtMonCaj_modPagCom').value = montoTotalCaja;

                let txtMonTotCom = document.getElementById('txtMonTotCom_modPagCom');
                txtMonTotCom.value = oHelper.formatoMoneda(oModalPagarCompra.modelo.sgnMoneda, oModalPagarCompra.modelo.montoCompra);
                txtMonTotCom.previousElementSibling.textContent = oModalPagarCompra.modelo.esAcredito ? "Monto abonado de la compra" : "Monto total de la compra";
                document.getElementById('txtSalPag_modPagCom').value = txtMonTotCom.value;

                document.getElementById('txtMonRet_modPagCom').focus();
            } else {
                if ($("#form-pagarCompra").valid()) {
                    //Si estamos en la vista 2 y le damos aceptar, se guardará el registro con el monto a retirar.
                    oModalPagarCompra.bGuardar = true;

                    oModalPagarCompra.instance.hide();
                }
            }
        })
        let txtMonret = document.getElementById('txtMonRet_modPagCom');
        txtMonret.addEventListener('keypress', (e) => { oHelper.numerosDecimales(e); });
        txtMonret.addEventListener('input', (e) => {
            let montoRetiro = 0;
            let montoTotalCompra = parseFloat(oHelper.numeroSinMoneda(document.getElementById('txtMonTotCom_modPagCom').value));
            if (e.target.value != "")
                montoRetiro = parseFloat(e.target.value);

            let saldoTotalCompra = (montoTotalCompra - montoRetiro);
            let txtSalPag = document.getElementById('txtSalPag_modPagCom');

            txtSalPag.style.color = saldoTotalCompra < 0 ? 'red' : 'black';
            txtSalPag.value = oHelper.formatoMoneda(oModalPagarCompra.modelo.sgnMoneda, saldoTotalCompra)

        });

        oModalPagarCompra.validaciones();
    },
    validaciones: function () {
        //No debe ser vacío ni cero.
        $.validator.addMethod("notEmptyAndZero", function (value, element) {
            if (value == 0)
                return false;
            else
                return true;

        }, "Ingrese el monto a retirar de caja.");

        //No debe ser mayor al monto de caja.
        $.validator.addMethod("notOlderAmountBox", function (value, element) {
            let montoEnCaja = oHelper.numeroSinMoneda(document.getElementById('txtMonCaj_modPagCom').value);
            if (parseFloat(value) > parseFloat(montoEnCaja))
                return false;
            else
                return true;

        }, "No debe ser mayor al monto total de caja.");

        //No debe ser mayor al monto de la compra.
        $.validator.addMethod("notOlderAmountBuy", function (value, element) {
            let montoCompra = oHelper.numeroSinMoneda(document.getElementById('txtMonTotCom_modPagCom').value);
            if (parseFloat(value) > parseFloat(montoCompra))
                return false;
            else
                return true;

        }, "No debe ser mayor al monto total o abonado");

        $("#form-pagarCompra").validate({
            rules: {
                txtMonRet_modPagCom: {
                    notEmptyAndZero: true,
                    notOlderAmountBox: true,
                    notOlderAmountBuy: true
                },
            }
        })
    },
    show: function (params) {
        oModalPagarCompra.vista = 1;
        oModalPagarCompra.modelo = params;

        let options = {};
        let modal = document.getElementById('modalPagarCompra');

        //Creamos las instancia para usar los metodos del modal.
        oModalPagarCompra.instance = new bootstrap.Modal(modal, options);
        oModalPagarCompra.instance.show();

        return new Promise((resolve, reject) => {
            oModalPagarCompra.resolve = resolve;
            oModalPagarCompra.reject = reject;
        })
    }
}

document.addEventListener('DOMContentLoaded', oModalPagarCompra.init);