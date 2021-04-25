'use strict'
var oReaperturaCaja = {
    init: function () {
        oConfigControls.inicializarDatePicker(".input-daterange");

        oReaperturaCaja.inicializarRangoFechas();

        $('#txtRanFecIni').datepicker('update',);
        $('#txtRanFecFin').datepicker('update', dayjs().format('DD/MM/YYYY'));

        oReaperturaCaja.initTblConsultarCajaApertura();

        document.getElementById('btnConsultar').addEventListener('click', oReaperturaCaja.consultar);

        $("#tblConsultarCajaApertura").find("tbody").on("click", "td button", (e) => {
            let button = e.currentTarget;
            let row = button.parentElement.parentElement;

            oAlertaModal.showConfirmation({
                title: "Reapertura de caja",
                message: "¿Desea reaperturar la caja seleccionada?"
            }).then(() => {
                let parameters = {
                    IdCaja: row.cells[8].textContent,
                    IdUsuario: row.cells[9].textContent,
                    Correlativo: row.cells[10].textContent,
                };
                oHelper.showLoading();
                axios.post("/CajaApertura/ReopenBox", parameters).then((response) => {
                    let data = response.data;
                    if (data.Resultado) {
                        oAlerta.alerta({
                            title: "Se reaperturó la caja seleccionada exitosamente.",
                            type: "success"
                        });
                    }
                }).catch((error) => {
                    oAlerta.alerta({
                        title: error.response.data.Message,
                        type: "warning"
                    });
                }).finally(() => {
                    oHelper.hideLoading();
                });
            })
        });
    },
    inicializarRangoFechas: function () {
        let fechaActual = dayjs();
        let fechaInicial = fechaActual.subtract(1, 'month').format("DD/MM/YYYY");
        let fechaFinal = fechaActual.format("DD/MM/YYYY");
        $('#txtRanFecIni').datepicker('update', fechaInicial);
        $('#txtRanFecFin').datepicker('update', fechaFinal);
    },
    validarConsulta: function () {
        return new Promise((resolve, reject) => {
            let txtRanFecIni = document.getElementById('txtRanFecIni');
            let txtRanFecFin = document.getElementById('txtRanFecFin');

            if (txtRanFecIni.value == "")
                return reject("Ingrese la fecha inicial para la consulta.");

            if (txtRanFecFin.value == "")
                return reject("Ingrese la fecha final para la consulta.");


            let fechaInicial = dayjs(txtRanFecIni.value, 'DD/MM/YYYY');
            let fechaFinal = dayjs(txtRanFecFin.value, 'DD/MM/YYYY');

            if (fechaInicial.isAfter(fechaFinal))
                return reject("La fecha inicial no debe ser mayor a la fecha final");

            resolve();
        });
    },
    consultar: function () {
        oReaperturaCaja.validarConsulta().then(() => {
            let table = document.getElementById('tblConsultarCajaApertura');
            let tbody = table.getElementsByTagName('tbody')[0];

            //Limpiamos la tabla
            oReaperturaCaja.cleartblConsultarCajaApertura();

            oHelper.showLoading();
            let parameters = {
                params: {
                    idCaja: document.getElementById('cboCaja').value,
                    idUsuario: document.getElementById('cboUsuario').value,
                    fechaInicial: document.getElementById('txtRanFecIni').value,
                    fechaFinal: document.getElementById('txtRanFecFin').value
                }
            }
            axios.get("/CajaApertura/GetAllByFilters", parameters).then(response => {
                let ListaCajaApertura = response.data.Data;

                let frag = document.createDocumentFragment();
                ListaCajaApertura.forEach(x => {
                    let td = `<td class="py-1"><button type="button" class="btn btn-sm warning-intenso" ${(x.FlgCierre ? '' : 'disabled')}><i class="bi bi-hand-index-fill" style='color:#fff'></i></button></td>
                        <td>${x.NomCaja}</td>
                        <td>${x.NomUsuario}</td>
                        <td>${x.FechaApertura}</td>
                        <td>${x.FechaCierre}</td>
                        <td class='text-end'>${oHelper.formatoMoneda(x.SgnMoneda, x.MontoApertura, 2)}</td>
                        <td class='text-end'>${oHelper.formatoMoneda(x.SgnMoneda, x.MontoTotal, 2)}</td>
                        <td><span class="badge rounded-pill bg-${(x.FlgCierre ? 'danger' : 'success')} ">${(x.FlgCierre ? 'Cerrado' : 'Abierto')}</span></td>
                        <td class='d-none' >${x.IdCaja}</td>
                        <td class='d-none' >${x.IdUsuario}</td>
                        <td class='d-none'>${x.Correlativo}</td>`;

                    let tr = document.createElement('tr');
                    tr.innerHTML = td
                    frag.appendChild(tr);
                });
                tbody.appendChild(frag);
            }).catch(error => {
                oAlerta.alerta({
                    title: error.response.data.Message,
                    type: "warning"
                });
            }).finally(() => {
                oHelper.hideLoading();
                oReaperturaCaja.initTblConsultarCajaApertura();
            });

        }).catch(error => {
            oAlerta.alerta({
                title: error,
                type: "warning"
            });
        })

    },
    initTblConsultarCajaApertura: function () {
        let aoColumns = [
            { "bSortable": false },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": false },
            { "bSortable": false },
            { "bSortable": false }];

        oConfigControls.inicializarDataTable({
            selector: "#tblConsultarCajaApertura",
            arrColumns: aoColumns,
            bPaginate: true,
            bInfo: true,
        });
    },
    cleartblConsultarCajaApertura: function () {
        let tbl = $("#tblConsultarCajaApertura").DataTable();
        if (tbl != null) {
            tbl.rows().remove().draw();
            tbl.destroy();
        }
    },
}

document.addEventListener('DOMContentLoaded', oReaperturaCaja.init);