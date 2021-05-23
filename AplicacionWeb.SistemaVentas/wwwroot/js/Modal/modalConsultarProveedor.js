'use strict'
var oModalConsultarProveedor = {
    seleccionado: false,
    instance: null,
    init: function () {
        let modal = document.getElementById('modalConsultarProveedor');
        modal.addEventListener('shown.bs.modal', function () {
            //Inicializamos la configuración del datatable del tabla proveedor
            oModalConsultarProveedor.initTblConsultarProveedor();

            document.getElementById('txtFiltroProveedor').focus();
        });

        modal.addEventListener('hidden.bs.modal', function () {
            //Limpiamos los controles
            oModalConsultarProveedor.clearTblConsultarProveedor();
            document.getElementById('txtFiltroProveedor').value = "";
            document.getElementById('rbProveedorPorDescripcion').cheked = true;

            //Será reject cada vez que se cierre el modal sin haber seleccionado algún registro.
            if (!oModalConsultarProveedor.seleccionado)
                oModalConsultarProveedor.reject();
            else
                oModalConsultarProveedor.seleccionado = false;

            oModalConsultarProveedor.instance = null;
        });

        document.getElementById('txtFiltroProveedor').addEventListener('keyup', (e) => {
            if (e.key == "Enter")
                oModalConsultarProveedor.consultar();
        })
        document.getElementById('btnBuscarProveedor').addEventListener('click', oModalConsultarProveedor.consultar);

        Array.from(modal.querySelectorAll('input[type=radio][name="rbProveedor"]')).forEach(rb => {
            rb.addEventListener('change', () => {
                let txtFiltroProveedor = document.getElementById('txtFiltroProveedor');
                txtFiltroProveedor.value = "";
                txtFiltroProveedor.focus();
            })
        });

        $("#tblConsultarProveedor").find("tbody").on("click", "td button", (e) => {
            let button = e.currentTarget;
            let row = button.parentElement.parentElement;

            oModalConsultarProveedor.seleccionarProveedor(row);
        });

        window.addEventListener("keydown", (e) => {
            //Indicamos la instancia del modal, porque cuando cerramos cualquier modal la instancia se convierte en null.
            if ((e.key == "Enter" || e.key == "Escape" || e.key == "ArrowDown" || e.key == "ArrowUp") && oModalConsultarProveedor.instance != null) {
                let tblConsultarProveedor = $("#tblConsultarProveedor").DataTable();
                if (tblConsultarProveedor.rows().count() == 0)
                    return;

                oConfigControls.direccionarFilasGrilla(e, {
                    table: document.getElementById('tblConsultarProveedor'),
                    txtFiltro: document.getElementById('txtFiltroProveedor'),
                    callback: oModalConsultarProveedor.seleccionarProveedor,
                    callbackEsc: () => oModalConsultarProveedor.instance.hide()
                });
            }
        });
    },
    seleccionarProveedor(row) {
        let modelo = {
            idProveedor: row.cells[1].textContent,
            nomProveedor: row.cells[2].textContent,
            idTipoDocumento: row.cells[6].textContent,
            numDocumento: row.cells[4].textContent
        }

        oModalConsultarProveedor.seleccionado = true;
        oModalConsultarProveedor.resolve(modelo);
        oModalConsultarProveedor.instance.hide();
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

        oConfigControls.inicializarDataTable({
            selector: "#tblConsultarProveedor",
            arrColumns: aoColumns,
            bPaginate: true,
            bInfo: true
        });
    },
    resolve: null,
    reject: null,
    show: function () {
        let options = {};
        let modal = document.getElementById('modalConsultarProveedor');

        //Creamos las instancia para usar los metodos del modal.
        oModalConsultarProveedor.instance = new bootstrap.Modal(modal, options);
        oModalConsultarProveedor.instance.show();

        return new Promise((resolve, reject) => {
            oModalConsultarProveedor.resolve = resolve;
            oModalConsultarProveedor.reject = reject;
        })
    },
    clearTblConsultarProveedor: function () {
        let tblConsultarProveedor = $("#tblConsultarProveedor").DataTable();
        if (tblConsultarProveedor != null) {
            tblConsultarProveedor.rows().remove().draw();
            tblConsultarProveedor.destroy();
        }
    },
    consultar: function () {
        let txtFiltro = document.getElementById('txtFiltroProveedor');
        if (txtFiltro.value == "") {
            oAlerta.alerta({
                title: "Debe de ingresar al menos 1 caracter.",
                type: "warning",
                contenedor: "#modalConsultarProveedor .modal-dialog",
                closeAutomatic: true,
                notTitle: true,
                notIcon: true
            });
            return;
        }
        
        //Checkbox seleccionado.
        let tipoFiltro = document.querySelector('#modalConsultarProveedor input[type=radio][name="rbProveedor"]:checked').value;

        let table = document.getElementById('tblConsultarProveedor');
        let tbody = table.getElementsByTagName('tbody')[0];

        //Limpiamos la tabla
        oModalConsultarProveedor.clearTblConsultarProveedor();

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
                type: "warning",
                contenedor: "#modalConsultarProveedor .modal-dialog",
                closeAutomatic: true,
                notTitle: true,
                notIcon: true
            });
        }).finally(() => oHelper.hideLoading());
    }
}

document.addEventListener('DOMContentLoaded', oModalConsultarProveedor.init);

