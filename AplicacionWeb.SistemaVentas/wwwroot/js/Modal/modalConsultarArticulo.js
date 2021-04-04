'use strict'
var oModalConsultarArticulo = {
    callback: null,
    tblDetalle: null,
    instance: null,
    init: function () {
        let modal = document.getElementById('modalConsultarArticulo');
        modal.addEventListener('shown.bs.modal', function () {
            //Inicializamos la configuración del datatable del tabla articulo
            oModalConsultarArticulo.initTblConsultarArticulo();

            document.getElementById('txtFiltroArticulo').focus();
        });
        modal.addEventListener('hidden.bs.modal', function () {
            //Limpiamos los controles
            oModalConsultarArticulo.clearTblConsultarArticulo();
            document.getElementById('txtFiltroArticulo').value = "";
            document.getElementById('rbPorDescripcion').cheked = true;

        });

        document.getElementById('txtFiltroArticulo').addEventListener('keyup', (e) => {
            if (e.key == "Enter")
                oModalConsultarArticulo.consultar();
        })
        document.getElementById('btnBuscarArticulo').addEventListener('click', oModalConsultarArticulo.consultar);

        Array.from(modal.querySelectorAll('input[type=radio][name="radioFiltros"]')).forEach(rb => {
            rb.addEventListener('change', () => {
                let txtFiltroArticulo = document.getElementById('txtFiltroArticulo');
                txtFiltroArticulo.value = "";
                txtFiltroArticulo.focus();
            })
        });

        $("#tblConsultarArticulo").find("tbody").on("click", "td button", (e) => {
            let button = e.currentTarget;
            let row = button.parentElement.parentElement;

            let modelo = {
                idArticulo: row.cells[6].textContent,
                codigo: row.cells[1].textContent,
                descripcion: row.cells[2].textContent,
                jsonListaUm: JSON.parse(row.cells[7].textContent),
            };
            let tbody = oModalConsultarArticulo.tblDetalle.getElementsByTagName('tbody')[0];
            let rows = tbody.rows;

            if (rows.length > 0) {
                if (Array.from(tbody.rows).some(x => x.cells[9].textContent == row.cells[6].textContent)) {
                    oAlerta.alerta({
                        title: `${row.cells[2].textContent} ya fue seleccionado.`,
                        type: "warning",
                        contenedor: "#modalConsultarArticulo .modal-content",
                        notTitle: true,
                        notIcon: true,
                        closeAutomatic: true
                    });
                    return;
                }
            };

            oAlerta.alerta({
                title: `${row.cells[2].textContent} seleccionado.`,
                type: "success",
                contenedor: "#modalConsultarArticulo .modal-content",
                notTitle: true,
                notIcon: true,
                closeAutomatic: true
            });

            //Enviamos el modelo al callback como parametro
            oModalConsultarArticulo.callback(modelo);
        });
    },
    show: function (callback, tblDetalle) {
        let options = {};
        let modal = document.getElementById('modalConsultarArticulo');
        oModalConsultarArticulo.callback = callback;
        oModalConsultarArticulo.tblDetalle = tblDetalle;

        //Creamos las instancia para usar los metodos del modal.
        oModalConsultarArticulo.instance = new bootstrap.Modal(modal, options);
        oModalConsultarArticulo.instance.show();
    },
    initTblConsultarArticulo: function () {
        debugger;
        let aoColumns = [
            { "bSortable": false },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": false },
            { "bSortable": false }];

        oConfigControls.inicializarDataTable({
            selector: "#tblConsultarArticulo",
            arrColumns: aoColumns,
            bPaginate: true,
            bInfo: true
        });
    },
    clearTblConsultarArticulo: function () {
        let tblConsultarArticulo = $("#tblConsultarArticulo").DataTable();
        if (tblConsultarArticulo != null) {
            tblConsultarArticulo.rows().remove().draw();
            tblConsultarArticulo.destroy();
        }
    },
    consultar: function () {
        let txtFiltro = document.getElementById('txtFiltroArticulo');
        if (txtFiltro.value == "") {
            oAlerta.alerta({
                title: "Debe de ingresar al menos 1 caracter.",
                type: "warning",
                contenedor: "#modalConsultarArticulo .modal-dialog",
                closeAutomatic: true,
                notTitle: true,
                notIcon: true
            });
            return;
        }

        //Checkbox seleccionado.
        let tipoFiltro = document.querySelector('#modalConsultarArticulo input[type=radio][name="radioFiltros"]:checked').value;

        let table = document.getElementById('tblConsultarArticulo');
        let tbody = table.getElementsByTagName('tbody')[0];

        //Limpiamos la tabla
        oModalConsultarArticulo.clearTblConsultarArticulo();

        oHelper.showLoading("#modalConsultarArticulo .modal-content");
        axios.get(`/Articulo/GetAllByFiltersHelper/${tipoFiltro}/${txtFiltro.value}/false`).then(response => {
            let listaArticulo = response.data.Data;

            let frag = document.createDocumentFragment();
            listaArticulo.forEach(x => {
                let td = `<td class="py-1"><button type="button" class="btn btn-sm warning-intenso"><i class="bi bi-hand-index-fill" style='color:#fff'></i></button></td>
                                <td>${x.Codigo}</td>
                                <td>${x.NomArticulo}</td>
                                <td>${x.NomMarca}</td>
                                <td>${x.NomUm}</td>
                                <td class='text-end'>${x.StockActual}</td>
                                <td class='d-none'>${x.IdArticulo}</td>
                                <td class='d-none'>${JSON.stringify(x.ListaUm)}</td>`;

                let tr = document.createElement('tr');
                tr.innerHTML = td
                frag.appendChild(tr);
            });
            tbody.appendChild(frag);
        }).catch(error => {
            oAlerta.alerta({
                title: error.response.data.Message,
                type: "warning",
                contenedor: "#modalConsultarArticulo .modal-dialog",
                closeAutomatic: true,
                notTitle: true,
                notIcon: true
            });
        }).finally(() => {
            oHelper.hideLoading();
            oModalConsultarArticulo.initTblConsultarArticulo();
        });
    }
}

document.addEventListener('DOMContentLoaded', oModalConsultarArticulo.init);