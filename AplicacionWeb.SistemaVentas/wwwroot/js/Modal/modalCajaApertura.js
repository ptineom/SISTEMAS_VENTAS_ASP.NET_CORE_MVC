'use strict'
var oModalCajaApertura = {
    instance: null,
    intervalTime: null,
    bCajaAbierta: false,
    modelo: null,
    moneda: null,
    verificarEstadoCaja: function () {
        return new Promise((resolve, reject) => {
            axios.get("/CajaApertura/GetStateBox").then((response) => {
                const result = response.data;
                const data = result.data;
                if (data != null) {
                    oModalCajaApertura.modelo = {
                        idCaja: data.idCaja,
                        correlativo: data.correlativo,
                        fechaApertura: data.fechaApertura,
                        montoApertura: data.montoApertura,
                        idMoneda: data.idMoneda,
                        sgnMoneda: data.sgnMoneda,
                        flgReaperturado: data.flgReaperturado,
                        item: data.item,
                        flgCierreDiferido: data.flgCierreDiferido,
                        fechaCierre: data.fechaCierre,
                        horaCierre: data.horaCierre,
                        nomCaja: data.nomCaja
                    };
                    oModalCajaApertura.bCajaAbierta = true;
                };
                resolve(oModalCajaApertura.bCajaAbierta);
            }).catch((error) => {
                const data = error.response.data;
                reject(data.errorDetails.message);
            })
        })
    },
    resolve: null,
    reject: null,
    show: function () {
        let icon = oModalCajaApertura.bCajaAbierta ? '<i class="bi bi-box-arrow-up"></i>' : '<i class="bi bi-box-seam"></i>';
        let title = oModalCajaApertura.bCajaAbierta ? `Caja ${oModalCajaApertura.modelo.flgReaperturado ? "reaperturada" : "abierta"}` : "Aperturar caja";

        let html = `<div class="modal fade" id="modalCajaApertura" tabindex="-1" aria-labelledby="exampleModalLabel22" aria-hidden="true" >
            <div class="modal-dialog ">
                <div class="modal-content">
                    <div class="modal-header py-2">
                        <h5 class="modal-title">${icon} ${title}</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <span class="py-2 px-3 mt-1" style="background-color: #B71C1C !important; color:#fff">
                        Fecha y hora de apertura de caja: <span id="spnFechaHoraApertura"></span> 
                    </span>
                    <div class="modal-body pt-1">
                        <form method="post" id="form-cajaApertura">
                            <div class="row">
                                <div class="col-md-3">
                                    <div class="form-check">
                                        <input class="form-check-input" type="checkbox" id="chkCieDif_ca" >
                                        <label class="form-check-label" for="chkCieDif_ca">
                                            Cierre diferido
                                        </label>
                                    </div>
                                </div>
                                <div class="col-md-5">
                                    <label class="form-label mb-1">Fecha cierre</label>
                                    <div class="input-group input-group-sm date">
                                        <span class="input-group-text"><i class="bi bi-calendar-check"></i></span>
                                        <input type="text" class="form-control date-picker" id="txtFecCie_ca" autocomplete="off" data-date-format="dd/mm/yyyy" name="txtFecCie_ca">
                                    </div>
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label mb-1">Hora cierre</label>
                                    <div class="input-group input-group-sm bootstrap-timepicker timepicker">
                                        <span class="input-group-text input-group-addon">
                                            <i class="glyphicon glyphicon-time"></i>
                                        </span>
                                        <input id="txtHorCie_ca" type="text" class="form-control input-small" name="txtHorCie_ca" autocomplete="off">
                                    </div>
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label mb-1">Caja</label>
                                    <select class="form-select form-select-sm" id="cboCaja_ca" aria-label=".form-select-lg example" name="cboCaja_ca">
                                    </select>
                                </div>
                                <div class="col-12 col-md-6">
                                    <label class="form-label mb-1">Monto apertura</label>
                                    <input class="form-control form-control-sm text-end" type="text" id="txtMonApe_ca" autocomplete="off">
                                </div>

                            </div>
                            <div class="row">
                                <div class="col-12 mt-3">
                                    <div class="d-flex justify-content-between align-items-center pe-2">
                                        <h6 class="text-muted fw-bold">Ingresos/Salidas de caja</h6>
                                        <h5><span class="badge rounded-pill bg-warning text-dark" id="spnTotal_ca">Total en caja: S/ 0.00</span></h5>
                                    </div>

                                </div>
                                <div class="col-12 mt-1">
                                    <ul class="list-group">
                                        <li class="list-group-item d-flex justify-content-between align-items-center">
                                            <span class="fw-bold">
                                                Monto apertura
                                            </span>
                                            <span class="badge rounded-pill bg-light text-dark" id="spnMonApe_ca"></span>
                                        </li>
                                        <li class="list-group-item d-flex justify-content-between align-items-center">
                                            <span class="fw-bold">
                                                Monto venta al contado
                                            </span>
                                            <span class="badge rounded-pill bg-light text-dark" id="spnMonCon_ca"></span>
                                        </li>
                                        <li class="list-group-item d-flex justify-content-between align-items-center">
                                            <span class="fw-bold">
                                                Monto pagos pendientes
                                            </span>
                                            <span class="badge rounded-pill bg-light text-dark" id="spnMonPen_ca"></span>
                                        </li>
                                        <li class="list-group-item d-flex justify-content-between align-items-center">
                                            <span class="fw-bold">
                                                Otros ingresos
                                            </span>
                                            <span class="badge rounded-pill bg-light text-dark" id="spnMonOtr_ca"></span>
                                        </li>
                                        <li class="list-group-item d-flex justify-content-between align-items-center text-danger">
                                            <span class="fw-bold">
                                                Salidas de caja
                                            </span>
                                            <span class="badge rounded-pill bg-danger" id="spnMonSal_ca"></span>
                                        </li>
                                    </ul>
                                </div>

                            </div>
                          
                        </form>
                    </div>
                    <div class="modal-footer py-1">
                        <button type="button" class="btn btn-danger" data-bs-dismiss="modal"><i class="bi bi-box-arrow-left"></i> Salir</button>
                        <button type="button" class="btn btn-warning" id="btnGuardar_ca">${icon} ${oModalCajaApertura.bCajaAbierta ? "Cerrar caja" : "Abrir caja"}</button>
                    </div>
                </div>
            </div>
        </div>`;

        //Lo agregamos al main del html
        let content = document.getElementsByTagName('body')[0]; // document.getElementById('content');
        let main = content.querySelector('main');
        main.insertAdjacentHTML("afterbegin", html);

        let options = {};
        let modalCajaApertura = document.getElementById('modalCajaApertura')
        oModalCajaApertura.instance = new bootstrap.Modal(modalCajaApertura, options)

        modalCajaApertura.addEventListener('shown.bs.modal', function () {
            oModalCajaApertura.inicializarSegunEstadoCaja();
        });
        modalCajaApertura.addEventListener('hidden.bs.modal', function () {
            //Destruimos el modal.
            main.removeChild(modalCajaApertura);

            //Limpiamos el intervalo utilizado par mostrar la hora.
            clearInterval(oModalCajaApertura.intervalTime);
        });

        //Inicializar elementos
        let txtFecCie_ca = document.getElementById('txtFecCie_ca');
        let txtHorCie_ca = document.getElementById('txtHorCie_ca');
        let txtMonApe_ca = document.getElementById('txtMonApe_ca');

        txtHorCie_ca.addEventListener('click', function () {
            this.previousElementSibling.click();
        });
        $(txtHorCie_ca).timepicker({
            minuteStep: 5,
            minuteStep: 5,
            showSeconds: true,
            showMeridian: false,
            defaultTime: false
        });

        oConfigControls.inicializarDatePicker("#txtFecCie_ca");

        document.getElementById('chkCieDif_ca').addEventListener('change', (e) => {
            if (e.target.checked) {
                txtFecCie_ca.disabled = false;
                txtHorCie_ca.disabled = false;
            } else {
                txtFecCie_ca.disabled = true;
                txtHorCie_ca.disabled = true;
                $(txtFecCie_ca).datepicker('clearDates');
                $(txtHorCie_ca).timepicker('setTime', "");
            }
        })

        document.getElementById('cboCaja_ca').addEventListener('change', () => txtMonApe_ca.focus());

        txtMonApe_ca.addEventListener('keypress', (e) => oHelper.numerosDecimales(e));
        txtMonApe_ca.addEventListener('input', (e) => {
            let monto = e.target.value == 0 ? 0 : e.target.value;
            document.getElementById('spnTotal_ca').textContent = `Total en caja: ${oHelper.formatoMoneda(oModalCajaApertura.moneda.sgnMoneda, monto)}`;
        });

        document.getElementById('btnGuardar_ca').addEventListener('click', oModalCajaApertura.grabar);

        oModalCajaApertura.validaciones();

        oModalCajaApertura.instance.show();

        return new Promise((resolve, reject) => {
            oModalCajaApertura.resolve = resolve;
            oModalCajaApertura.reject = reject;
        })
    },
    montosTotalesCaja: function () {
        let sgnMoneda = oModalCajaApertura.moneda.sgnMoneda;

        let spnMonApe_ca = document.getElementById('spnMonApe_ca');
        let spnMonCon_ca = document.getElementById('spnMonCon_ca');
        let spnMonPen_ca = document.getElementById('spnMonPen_ca');
        let spnMonOtr_ca = document.getElementById('spnMonOtr_ca');
        let spnMonSal_ca = document.getElementById('spnMonSal_ca');

        spnMonApe_ca.textContent = `${sgnMoneda} 0.00`;
        spnMonCon_ca.textContent = `${sgnMoneda} 0.00`;
        spnMonPen_ca.textContent = `${sgnMoneda} 0.00`;
        spnMonOtr_ca.textContent = `${sgnMoneda} 0.00`;
        spnMonSal_ca.textContent = `${sgnMoneda} 0.00`;

        // Si la caja esta abierta, trae los totales.
        if (oModalCajaApertura.bCajaAbierta) {
            oHelper.showLoading("#modalCajaApertura .modal-content");

            axios.get(`/CajaApertura/GetTotalsByUserId/${oModalCajaApertura.modelo.idCaja}/${oModalCajaApertura.modelo.correlativo}`).then((response) => {
                const result = response.data;
                const data = result.data;

                oModalCajaApertura.modelo = Object.assign(oModalCajaApertura.modelo, {
                    montoApertura: data.montoAperturaCaja,
                    montoTotal: data.montoTotal,
                });

                if (data.montoAperturaCaja > 0) {
                    spnMonApe_ca.textContent = oHelper.formatoMoneda(sgnMoneda, data.montoAperturaCaja, 2);
                    spnMonApe_ca.classList.remove('bg-light', 'text-dark');
                    spnMonApe_ca.classList.add('bg-success');
                }

                if (data.MontoCobradoContado > 0) {
                    spnMonCon_ca.textContent = oHelper.formatoMoneda(sgnMoneda, data.montoCobradoContado, 2);
                    spnMonCon_ca.classList.remove('bg-light', 'text-dark');
                    spnMonCon_ca.classList.add('bg-success');
                }

                if (data.MontoCobradoCredito > 0) {
                    spnMonPen_ca.textContent = oHelper.formatoMoneda(sgnMoneda, data.montoCobradoCredito, 2);
                    spnMonPen_ca.classList.remove('bg-light', 'text-dark');
                    spnMonPen_ca.classList.add('bg-success');
                }

                if (data.montoCajaOtrosIngreso > 0) {
                    spnMonOtr_ca.textContent = oHelper.formatoMoneda(sgnMoneda, data.montoCajaOtrosIngreso, 2);
                    spnMonOtr_ca.classList.remove('bg-light', 'text-dark');
                    spnMonOtr_ca.classList.add('bg-success');
                }

                if (data.montoCajaSalida > 0) {
                    spnMonSal_ca.textContent = oHelper.formatoMoneda(sgnMoneda, data.montoCajaSalida, 2);
                    spnMonSal_ca.classList.remove('bg-light', 'text-dark');
                    spnMonSal_ca.classList.add('bg-success');
                }

                document.getElementById('spnTotal_ca').textContent = `Total en caja: ${oHelper.formatoMoneda(sgnMoneda, data.montoTotal, 2)}`;
            }).catch((error) => {
                const data = error.response.data;
                oAlerta.show({
                    message: data.errorDetails.message,
                    type: "warning"
                });
            }).finally(() => oHelper.hideLoading());
        }
    },
    inicializarData: function (callback) {
        oHelper.showLoading("#modalCajaApertura .modal-content");

        axios.get("/CajaApertura/GetData").then((response) => {
            let result = response.data;
            let data = result.data;
            let listCajas = data.listCajas;
            let monedaLocal = data.moneda;

            //Listado de cajas del usuario.
            let cboCaja_ca = document.getElementById('cboCaja_ca');
            let option = document.createElement('option');

            option.text = "---Seleccione---";
            option.value = "";
            cboCaja_ca.appendChild(option);

            listCajas.forEach((elem) => {
                option = document.createElement('option');
                option.text = elem.nomCaja;
                option.value = elem.idCaja;

                if (listCajas.length == 1)
                    option.selected = true;

                cboCaja_ca.appendChild(option);
            });

            //Moneda local
            oModalCajaApertura.moneda = {
                idMoneda: monedaLocal.idMoneda,
                nomMoneda: monedaLocal.nomMoneda,
                sgnMoneda: monedaLocal.sgnMoneda,
            };

            if (callback != undefined)
                callback();
        }).catch((error) => {
            const data = error.response.data;
            oAlerta.show({
                message: data.errorDetails.message,
                type: "warning"
            });
        }).finally(() => oHelper.hideLoading());
    },
    inicializarSegunEstadoCaja: function () {
        let spnFechaHoraApertura = document.getElementById('spnFechaHoraApertura');
        let chkCieDif_ca = document.getElementById('chkCieDif_ca');
        let txtFecCie_ca = document.getElementById('txtFecCie_ca');
        let txtHorCie_ca = document.getElementById('txtHorCie_ca');

        if (oModalCajaApertura.bCajaAbierta) {
            spnFechaHoraApertura.textContent = oModalCajaApertura.modelo.fechaApertura;
            //Configuración para el cierre diferido
            chkCieDif_ca.disabled = oModalCajaApertura.modelo.flgReaperturado;
            chkCieDif_ca.checked = oModalCajaApertura.modelo.flgCierreDiferido;

            txtFecCie_ca.disabled = true;
            $(txtFecCie_ca).datepicker('update', oModalCajaApertura.modelo.fechaCierre);

            txtHorCie_ca.disabled = true;
            $(txtHorCie_ca).timepicker('setTime', oModalCajaApertura.modelo.horaCierre);
            /* ----------------------------------------------------------------------------------- */

            //Caja
            let cboCaja_ca = document.getElementById('cboCaja_ca');
            let option = document.createElement('option');

            option.text = oModalCajaApertura.modelo.nomCaja;
            option.value = oModalCajaApertura.modelo.idCaja;
            cboCaja_ca.appendChild(option);
            cboCaja_ca.disabled = true;
            cboCaja_ca.value = oModalCajaApertura.modelo.idCaja;
            /* ----------------------------------------------------------------------------------- */

            //Monto apertura
            let txtMonApe_ca = document.getElementById('txtMonApe_ca');

            if (oModalCajaApertura.modelo.montoApertura == "")
                oModalCajaApertura.modelo.montoApertura = 0;

            if (oModalCajaApertura.modelo.montoApertura > 0)
                txtMonApe_ca.value = oHelper.formatoMoneda(oModalCajaApertura.modelo.sgnMoneda, oModalCajaApertura.modelo.montoApertura)

            txtMonApe_ca.disabled = true;


            oModalCajaApertura.moneda = {
                idMoneda: oModalCajaApertura.modelo.idMoneda,
                sgnMoneda: oModalCajaApertura.modelo.sgnMoneda
            }
            /* ----------------------------------------------------------------------------------- */

            //Totales
            oModalCajaApertura.montosTotalesCaja();
        } else {
            //Mostramos la fecha y hora actual()
            let fechaActual = dayjs().format('DD/MM/YYYY');
            let d = new Date();
            oModalCajaApertura.intervalTime = setInterval(() => {
                d.setSeconds(d.getSeconds() + 1);
                spnFechaHoraApertura.textContent = `${fechaActual} ${d.toLocaleTimeString()}`;
            }, 1000);

            //Traer data del servidor para incializar proceso.
            let fn = oModalCajaApertura.montosTotalesCaja;
            oModalCajaApertura.inicializarData(fn);

            //Controles
            chkCieDif_ca.disabled = true;
            txtFecCie_ca.disabled = true;
            txtHorCie_ca.disabled = true;
            $(txtHorCie_ca).timepicker('setTime', '');
        };

    },
    grabar: function () {
        if ($("#form-cajaApertura").valid()) {
            let bCajaAbierta = oModalCajaApertura.bCajaAbierta;
            let modelo = oModalCajaApertura.modelo;

            let titulo = bCajaAbierta ? `Caja ${oModalCajaApertura.modelo.flgReaperturado ? "reaperturada" : "abierta"}` : "Caja cerrada";
            let pregunta = `¿Desea ${bCajaAbierta ? "cerrar la caja" : "abrir la caja"}?`;

            //Abrimos el modal de confirmación.
            oAlertaModal.showConfirmation({
                title: titulo,
                message: pregunta
            }).then(() => {
                //Si es aceptar

                oHelper.showLoading("#modalCajaApertura .modal-content");

                let fechaCierre = "";
                if (document.getElementById('chkCieDif_ca').checked)
                    fechaCierre = `${document.getElementById('txtFecCie_ca').value} ${document.getElementById('txtHorCie_ca').value}`;

                let txtMonApe_ca = document.getElementById('txtMonApe_ca');

                let parameters = {
                    Accion: bCajaAbierta ? "UPD" : "INS",
                    MontoApertura: bCajaAbierta ? oModalCajaApertura.modelo.montoApertura : txtMonApe_ca.value == "" ? 0 : txtMonApe_ca.value,
                    IdMoneda: bCajaAbierta ? oModalCajaApertura.modelo.idMoneda : oModalCajaApertura.moneda.idMoneda,
                    IdCaja: document.getElementById('cboCaja_ca').value,
                    FechaCierre: fechaCierre == "" ? null : fechaCierre,
                    MontoTotal: bCajaAbierta ? modelo.montoTotal : 0,
                    Correlativo: bCajaAbierta ? modelo.correlativo : 0,
                    FlgReaperturado: bCajaAbierta ? modelo.flgReaperturado : false,
                    Item: bCajaAbierta ? modelo.item : 0,
                    FlgCierreDiferido: document.getElementById('chkCieDif_ca').checked
                };

                axios.post("/CajaApertura/Register", parameters).then((response) => {
                    const result = response.data;
                    const data = result.data;

                    if (data != null) {
                        oModalCajaApertura.modelo = {
                            idCaja: data.idCaja,
                            correlativo: data.correlativo,
                            fechaApertura: data.fechaApertura,
                            montoApertura: data.montoApertura,
                            idMoneda: data.idMoneda,
                            sgnMoneda: data.sgnMoneda,
                            flgReaperturado: data.flgReaperturado,
                            item: data.item,
                            flgCierreDiferido: data.flgCierreDiferido,
                            fechaCierre: data.fechaCierre,
                            horaCierre: data.horaCierre,
                            nomCaja: data.nomCaja
                        };
                        oModalCajaApertura.bCajaAbierta = true;
                    } else {
                        oModalCajaApertura.bCajaAbierta = false;
                    }

                    //Si esta todo ok
                    oModalCajaApertura.resolve();

                    //Cerramos el modal de apertura de caja.
                    oModalCajaApertura.instance.hide();

                }).catch((error) => {
                    const data = error.response.data;
                    oAlerta.show({
                        message: data.errorDetails.message,
                        type: "warning"
                    });
                }).finally(() => {
                    oHelper.hideLoading();
                });

                //Si es cancelar en el modal de confirmación.
            }).catch(cancelar => { })
        }
    },
    validaciones: function () {
        //Seleccionar caja 
        $.validator.addMethod("requiredBox", function (value, element) {
            if (!oModalCajaApertura.bCajaAbierta) {
                if (!!value)
                    return true;
                else
                    return false;
            };
            return true;
        }, "Seleccione la caja.");

        /*------------- Fecha de cierre ----------------*/
        //Fecha de cierre requerido 
        $.validator.addMethod("requiredDateOpening", function (value, element) {
            if (document.getElementById('chkCieDif_ca').checked) {
                if (!!value)
                    return true;
                else
                    return false;
            };
            return true;
        }, "Ingrese la fecha de cierre.");

        //Fecha de cierre que no sea menor a la fecha de apertura.
        $.validator.addMethod("notLessDateOpening", function (value, element) {
            if (document.getElementById('chkCieDif_ca').checked) {
                let fechaApertura = dayjs(document.getElementById('spnFechaHoraApertura').textContent, 'DD/MM/YYYY HH:mm:ss').format("YYYY/MM/DD");
                if (dayjs(value, "DD/MM/YYYY").isBefore(dayjs(fechaApertura)))
                    return false;
            }

            return true;
        }, "No debe ser menor a la fecha de apertura.");

        //Fecha de cierre que no sea mayor a la fecha actual
        $.validator.addMethod("notOlderDateCurrent", function (value, element) {
            if (document.getElementById('chkCieDif_ca').checked) {
                let fechaActual = dayjs().format("YYYY/MM/DD");
                if (dayjs(value, "DD/MM/YYYY").isAfter(dayjs(fechaActual)))
                    return false;
            }
            return true;
        }, "No debe ser mayor a la fecha actual.");
        /*------------- Fin fecha de cierre ----------------*/

        ///*------------- Hora de cierre ----------------*/
        //Hora de cierre requerido 
        $.validator.addMethod("requiredHourOpening", function (value, element) {
            if (document.getElementById('chkCieDif_ca').checked) {
                if (!!value)
                    return true;
                else
                    return false;
            };
            return true;
        }, "Ingrese la hora de cierre.");

        //Fecha y hora de cierre no debe ser mayor a la fecha actual.
        $.validator.addMethod("notOlderDateHourCurrent", function (value, element) {
            if (document.getElementById('chkCieDif_ca').checked) {
                let fechaHoraCierre = `${document.getElementById('txtFecCie_ca').value} ${document.getElementById('txtHorCie_ca').value}`;
                fechaHoraCierre = dayjs(fechaHoraCierre, 'DD/MM/YYYY HH:mm');

                let fechaActual = new Date();

                if (fechaHoraCierre.isAfter(fechaActual))
                    return false;
            }
            return true;
        }, "No debe ser mayor a la fecha y hora actual.");

        //Fecha y hora de cierre no debe ser menor a la fecha de apertura.
        $.validator.addMethod("notLessDateHourOpening", function (value, element) {
            if (document.getElementById('chkCieDif_ca').checked) {
                let fechaHoraCierre = `${document.getElementById('txtFecCie_ca').value} ${document.getElementById('txtHorCie_ca').value}`;
                fechaHoraCierre = dayjs(fechaHoraCierre, 'DD/MM/YYYY HH:mm');

                let fechaHoraApertura = dayjs(document.getElementById('spnFechaHoraApertura').textContent, 'DD/MM/YYYY HH:mm:ss')

                if (fechaHoraCierre.isBefore(fechaHoraApertura))
                    return false;
            }
            return true;
        }, "No debe ser menor a la fecha y hora de apertura.");

        $("#form-cajaApertura").validate({
            rules: {
                txtFecCie_ca: {
                    requiredDateOpening: true,
                    notLessDateOpening: true,
                    notOlderDateCurrent: true
                },
                txtHorCie_ca: {
                    requiredHourOpening: true,
                    notOlderDateHourCurrent: true,
                    notLessDateHourOpening: true
                },
                cboCaja_ca: {
                    requiredBox: true
                }
            },
            messages: {
                txtFecCie_ca: {
                    //required: "Debe de ingresar el monto a abonar"
                },
                txtHorCie_ca: {
                    //required: "Debe de seleccionar la fecha a cancelar."
                },
                cboCaja_ca: {

                }
            }
        });
    }
}
