'use strict'
var oModalAbono = {
    init: function () {

    },
    seleccionado: false,
    resolve: null,
    reject: null,
    fechaEmision: '',
    fechaVencimiento: '',
    show: function (modelo, flgEditar) {
        //flgEditar: true => Edición del monto ya ingresado.
        let sgnMoneda = modelo.total.split(' ')[0];
        let abono = modelo.abono == 0 ? '' : modelo.abono;
        let saldo = modelo.saldo == 0 ? '' : oHelper.formatoMoneda(sgnMoneda, modelo.saldo, 2);

        oModalAbono.fechaEmision = modelo.fechaEmision;
        oModalAbono.fechaCancelacion = modelo.fechaCancelacion;

        let html = `<div class="modal fade" id="modalAbono" tabindex="-1" aria-labelledby="exampleModalLabel" aria-hidden="true" >
                        <div class="modal-dialog modal-sm">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <h5 class="modal-title" ><i class="bi bi-cash-stack"></i> Abono</h5>
                                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                                </div>
                                <div class="modal-body">
                                    <form  method="post" id="form-abono">
                                        <div class="row">
                                            <div class="col-12">
                                                <label class="form-label mb-1">Monto total</label>
                                                <input class="form-control form-control-sm text-end" value='${modelo.total}' type="text" disabled id="txtTotalModalAbono">
                                            </div>
                                            <div class="col-12">
                                                <label class="form-label mb-1">Saldo</label>
                                                <input class="form-control form-control-sm text-end" value='${saldo}' type="text" disabled id="txtSaldoModalAbono">
                                            </div>
                                            <div class="col-12">
                                                <label class="form-label mb-1">Monto a abonar</label>
                                                <input class="form-control form-control-sm text-end" name='txtAbono' value='${abono}' type="text" autocomplete="off" id="txtAbonoModalAbono">
                                            </div>
                                            <div class="col-12">
                                                <label class="form-label mb-1">Fecha a cancelar</label>
                                                <div class="input-group input-group-sm date">
                                                    <span class="input-group-text" id="basic-addon2"><i class="bi bi-calendar-check"></i></span>
                                                    <input type="text" class="form-control date-picker" name='txtFechaModalAbono' value='${oModalAbono.fechaCancelacion}' id="txtFechaModalAbono" data-date-format="dd/mm/yyyy" autocomplete="off">
                                                </div>
                                             </div>
                                        </div>
                                    </form>
                                </div>
                                <div class="modal-footer">
                                    <button type="button" class="btn btn-danger" data-bs-dismiss="modal"><i class="bi bi-box-arrow-left"></i> Salir</button>
                                    <button type="button" class="btn btn-success" id="btnGuardarModalAbono"><i class="bi bi-save2"></i> Guardar cambios</button>
                                </div>
                            </div>
                        </div>
                    </div>`;

        //Lo agregamos temporalmente al main del html
        let content = document.getElementById('content');
        let main = content.querySelector('main');
        main.insertAdjacentHTML("afterbegin", html);

        let options = {};
        let modal = document.getElementById('modalAbono')
        let myModal = new bootstrap.Modal(modal, options)

        modal.addEventListener('shown.bs.modal', function () {
            document.getElementById('txtAbonoModalAbono').focus();
        });
        modal.addEventListener('hidden.bs.modal', function () {
            //Si no se dio click en el boton guardar cambios.
            if (!oModalAbono.seleccionado)
                oModalAbono.reject(flgEditar);

            //Destruimos el modal.
            main.removeChild(modal);
        });

        document.getElementById('btnGuardarModalAbono').addEventListener('click', () => {
            if ($("#form-abono").valid()) {
                oModalAbono.resolve({
                    abono: parseFloat(document.getElementById('txtAbonoModalAbono').value),
                    saldo: parseFloat(oHelper.numeroSinMiles(document.getElementById('txtSaldoModalAbono').value).split(' ')[1]),
                    fechaCancelacion: document.getElementById('txtFechaModalAbono').value
                });
                oModalAbono.seleccionado = true;
                myModal.hide();
            }
        });

        document.getElementById('txtAbonoModalAbono').addEventListener('input', (e) => {
            //calculamos el saldo.
            let abono = e.target.value;
            abono = abono == '' ? 0 : abono;
            let txtTotal = document.getElementById('txtTotalModalAbono');

            let total = oHelper.numeroSinMiles(txtTotal.value);
            let sgnMoneda = total.split(' ')[0];
            total = total.split(' ')[1];
            total = new Decimal(total);

            let saldo = total.minus(abono).toNumber();

            document.getElementById('txtSaldoModalAbono').value = oHelper.formatoMoneda(sgnMoneda, saldo, 2);
        })

        oConfigControls.inicializarDatePicker("#modalAbono .date-picker");

        oModalAbono.validaciones();

        myModal.show();

        return new Promise((resolve, reject) => {
            oModalAbono.resolve = resolve;
            oModalAbono.reject = reject;
        })
    },
    validaciones: function () {
        $.validator.addMethod("notEmptyAndZero", function (value, element) {
            if (value == 0) 
                return false;
            else
                return true;

        }, "Debe de ingresar el monto a abonar.");

        $.validator.addMethod("notOlderDateExpiration", function (value, element) {
            let fechaVencimiento = dayjs(oModalAbono.fechaVencimiento, "DD/MM/YYYY");

            //Si la fecha es mayor a la fecha de vencimiento retorna false
            if (dayjs(value, "DD/MM/YYYY").isAfter(fechaVencimiento)) 
                return false;

            return true;
        }, "La fecha ingresada no debe ser mayor a la fecha de vencimiento.");

        $.validator.addMethod("notLessDateIssue", function (value, element) {
            let fechaEmision = dayjs(oModalAbono.fechaEmision, "DD/MM/YYYY");

            //Si la fecha es menor que la fecha de emision retorna false
            if (dayjs(value, "DD/MM/YYYY").isBefore(fechaEmision)) 
                return false;

            return true;
        }, "La fecha ingresada no debe ser menor a la fecha de emisión");

        $("#form-abono").validate({
            rules: {
                txtFechaModalAbono: {
                    required: true,
                    notOlderDateExpiration: true,
                    notLessDateIssue: true
                },
                txtAbono: {
                    notEmptyAndZero: true
                }
            },
            messages: {
                txtAbono: {
                    //required: "Debe de ingresar el monto a abonar"
                },
                txtFechaModalAbono: {
                    required: "Debe de seleccionar la fecha a cancelar."
                }
            }
        })
    }
}

//document.addEventListener('DOMContentLoaded', oModalAbono.init);