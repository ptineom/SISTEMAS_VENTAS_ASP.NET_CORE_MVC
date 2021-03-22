'use strict'
var oModalConsultarProveedor = {
    tblConsultarProveedor: null,
    init: function () {
        //Nombre del modal
        let modal = document.getElementById('modalConsultarProveedor');
        let myModal = new bootstrap.Modal(modal, {
            backdrop: false
        });

        document.getElementById('prueba').addEventListener('click', () => {
            debugger;
            myModal.hide();
        })

        modal.addEventListener('shown.bs.modal', function () {
            oModalConsultarProveedor.initTblConsultarProveedor();
            document.getElementById('txtFiltroProveedor').focus();
        });
        modal.addEventListener('hidden.bs.modal', function () {
            debugger;
            oModalConsultarProveedor.clearDatatable(document.getElementById('tblConsultarProveedor'));
            document.getElementById('txtFiltroProveedor').value = "";
            document.getElementById('rbPorDescripcion').cheked = true;
        });

        document.getElementById('txtFiltroProveedor').addEventListener('keyup', (e) => {
            if (e.key == "Enter")
                oModalConsultarProveedor.consultar();
        })
        document.getElementById('btnBuscarProveedor').addEventListener('click', oModalConsultarProveedor.consultar);

        Array.from(modalConsultarProveedor.querySelectorAll('input[type=radio][name="radioFiltros"]')).forEach(rb => {
            rb.addEventListener('change', () => {
                let txtFiltroProveedor = document.getElementById('txtFiltroProveedor');
                txtFiltroProveedor.value = "";
                txtFiltroProveedor.focus();
            })
        });

        $("#tblConsultarProveedor").find("tbody").on("click", "td button", (e) => {
            let button = e.currentTarget;
            let row = button.parentElement.parentElement;
            let modelo = {
                idProveedor: row.cells[1].textContent,
                nomProveedor: row.cells[2].textContent,
                idTipoDocumento: row.cells[6].textContent,
                numDocumento: row.cells[4].textContent
            }
            myModal.hide();
            oModalConsultarProveedor.resolve(modelo);
            let hh = "";
            //setTimeout(function () {
            //    myModal.hide();
            //},3000)


            ////Nombre del modal
            //let modal = document.getElementById('modalConsultarProveedor');

            //let myModal = new bootstrap.Modal(modal, {});

            // myModal.toggle()
            //$("#modalConsultarProveedor").hide();
        });
    },
    initTblConsultarProveedor: function () {
        let aoColumns = [
            { "bSortable": false },
            { "bSortable": false },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": true },
            { "bSortable": false }];

        let config = {
            arrColumns: aoColumns,
            bPaginate: true,
            bInfo: true,
            fnCallBack: function (optTable) {
                oModalConsultarProveedor.tblConsultarProveedor = $('#tblConsultarProveedor').DataTable(optTable);
            },
        };
        oConfigControls.inicializarDataTable(config);
    },
    resolve: null,
    reject: null,
    show: function () {
        let options = {};
        let modalConsultarProveedor = document.getElementById('modalConsultarProveedor')
        let myModalConsultarProveedor = new bootstrap.Modal(modalConsultarProveedor, options);

        myModalConsultarProveedor.show();

        return new Promise((resolve, reject) => {
            oModalConsultarProveedor.resolve = resolve;
            oModalConsultarProveedor.reject = reject;
        })
    },
    clearDatatable: function (table) {
        oHelper.limpiarTabla(table);
        if (oModalConsultarProveedor.tblConsultarProveedor != null) {
            oModalConsultarProveedor.tblConsultarProveedor.rows().remove().draw();
            oModalConsultarProveedor.tblConsultarProveedor.destroy();
        }
    },
    consultar: function () {
        let txtFiltro = document.getElementById('txtFiltroProveedor');
        if (txtFiltro.value == "") {
            oAlerta.alerta({
                title: "Debe de ingresar al menos 1 caracter.",
                type: "warning"
            });
            return;
        }

        //Checkbox seleccionado.
        let tipoFiltro = document.querySelector('#modalConsultarProveedor input[type=radio][name="radioFiltros"]:checked').value;

        let table = document.getElementById('tblConsultarProveedor');
        let tbody = table.getElementsByTagName('tbody')[0];

        //Limpiamos la tabla(tabla nativa y datatable)
        oModalConsultarProveedor.clearDatatable(table);

        oHelper.showLoading("#modalConsultarProveedor .modal-content");
        axios.get(`/Proveedor/GetAllByFilters/${tipoFiltro}/${txtFiltro.value}/false`).then(response => {
            let listaProveedor = response.data.Data;

            let frag = document.createDocumentFragment();
            listaProveedor.forEach(x => {
                let td = `<td class="py-1"><button type="button" class="btn btn-sm warning-intenso"><i class="bi bi-hand-index-fill" style='color:#fff'></i></button></td>
                                <td class='d-none'>${x.IdCliente}</td>
                                <td>${x.NomCliente}</td>
                                <td>${x.NomTipoDocumento}</td>
                                <td>${x.NroDocumento}</td>
                                <td>${x.DirCliente}</td>
                                <td class='d-none'>${x.IdTipoDocumento}</td>`;

                let tr = document.createElement('tr');
                tr.innerHTML = td
                frag.appendChild(tr);
            });
            tbody.appendChild(frag);
           oModalConsultarProveedor.initTblConsultarProveedor();
        }).catch(error => {
            oAlerta.alerta({
                title: error.response.data.Message,
                type: "warning"
            });
        }).finally(() => oHelper.hideLoading());
    }
}
$(document).ready(function () {
    oModalConsultarProveedor.init();
})
//document.addEventListener('DOMContentLoaded', oModalConsultarProveedor.init);

