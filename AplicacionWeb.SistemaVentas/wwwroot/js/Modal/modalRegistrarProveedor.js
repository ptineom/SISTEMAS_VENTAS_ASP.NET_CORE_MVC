'use strict'

var oModalRegistrarProveedor = {
    seleccionado: false,
    instance: null,
    resolve: null,
    reject: null,
    ok: false,
    init() {
        let modal = document.getElementById('modalRegistrarProveedor');
        modal.addEventListener('shown.bs.modal', function () {
            document.getElementById('cboTipDocRegProv').focus();
        });
        modal.addEventListener('hidden.bs.modal', function () {
            //Limpiamos los controles
            oModalRegistrarProveedor.limpiar();

            //Limpiamos los errores del formulario.
            oConfigControls.formReset(document.getElementById('form-proveedor'));

            //Será reject cada vez que cierre el modal sin haber seleccionado algún registro.
            if (!oModalRegistrarProveedor.ok)
                oModalRegistrarProveedor.reject();
            else
                oModalRegistrarProveedor.ok = false;
        });

        document.getElementById('cboTipDocRegProv').addEventListener('change', (e) => {
            let cbo = e.target;
            let txtNumDoc = document.getElementById('txtNumDocRegProv');

            txtNumDoc.value = "";
            txtNumDoc.maxLength = cbo.options[cbo.selectedIndex].getAttribute("data-max-digitos");
            txtNumDoc.focus();
        });

        let txtNumDoc = document.getElementById('txtNumDocRegProv');
        txtNumDoc.addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));
        txtNumDoc.addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtRazSocRegProv"));

        document.getElementById('txtRazSocRegProv').addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtConRegProv"));
        document.getElementById('txtConRegProv').addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtEmaRegProv"));
        document.getElementById('txtEmaRegProv').addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtTelRegProv"));

        let txtTel = document.getElementById('txtTelRegProv');
        txtTel.addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));
        txtTel.addEventListener('keyup', (e) => oHelper.teclaEnter(e, "cboDepRegProv"));

        document.getElementById('cboDepRegProv').addEventListener('change', (e) => oModalRegistrarProveedor.listarProvincias(e));
        document.getElementById('cboProRegProv').addEventListener('change', (e) => oModalRegistrarProveedor.listarDistritos(e));
        document.getElementById('btnGuardarProveedor').addEventListener('click', oModalRegistrarProveedor.grabar);

        oModalRegistrarProveedor.validaciones();
       
    },
    show: function () {
        let options = {};
        let modal = document.getElementById('modalRegistrarProveedor');

        //Creamos las instancia para usar los metodos del modal.
        oModalRegistrarProveedor.instance = new bootstrap.Modal(modal, options);
        oModalRegistrarProveedor.instance.show();

        return new Promise((resolve, reject) => {
            oModalRegistrarProveedor.resolve = resolve;
            oModalRegistrarProveedor.reject = reject;
        })
    },
    limpiar: function () {
        document.getElementById('cboTipDocRegProv').value = "";
        document.getElementById('txtNumDocRegProv').value = "";
        document.getElementById('txtRazSocRegProv').value = "";
        document.getElementById('txtConRegProv').value = "";
        document.getElementById('txtEmaRegProv').value = "";
        document.getElementById('txtTelRegProv').value = "";
        document.getElementById('txtDirRegProv').value = "";
        document.getElementById('cboDepRegProv').value = "";
        document.getElementById('cboProRegProv').value = "";
        document.getElementById('cboDisRegProv').value = "";
    },
    listarProvincias: function (e) {
        let cboDep = e.target;
        let cboPro = document.getElementById('cboProRegProv');
        let cboDis = document.getElementById('cboDisRegProv');

        cboPro.innerHTML = "<option value >---Seleccione---</option>";
        cboDis.innerHTML = "<option value >---Seleccione---</option>"

        if (cboDep.value == "")
            return;

        oHelper.showLoading("#modalRegistrarProveedor .modal-content");

        axios.get(`/Ubigeo/GetAllProvinces/${cboDep.value}`)
            .then((response) => {
                let listaProvincia = response.data.Data;

                let frag = document.createDocumentFragment();
                listaProvincia.forEach(x => {
                    let option = document.createElement('option');
                    option.value = x.IdProvincia;
                    option.text = x.NomProvincia;
                    frag.appendChild(option);
                });
                cboPro.appendChild(frag);
            })
            .catch((error) => {
                oAlerta.alerta({
                    title: error.response.data.Message,
                    type: "warning"
                });
            }).finally(() => oHelper.hideLoading());
    },
    listarDistritos: function (e) {
        let cboPro = e.target;
        let cboDis = document.getElementById('cboDisRegProv');

        cboDis.innerHTML = "<option value >---Seleccione---</option>"

        if (cboPro.value == "")
            return;

        oHelper.showLoading("#modalRegistrarProveedor .modal-content");

        axios.get(`/Ubigeo/GetAllDistricts/${cboPro.value}`)
            .then((response) => {
                let listaDistrito = response.data.Data;

                let frag = document.createDocumentFragment();
                listaDistrito.forEach(x => {
                    let option = document.createElement('option');
                    option.value = x.IdDistrito;
                    option.text = x.NomDistrito;
                    frag.appendChild(option);
                });
                cboDis.appendChild(frag);
            })
            .catch((error) => {
                oAlerta.alerta({
                    title: error.response.data.Message,
                    type: "warning"
                });
            }).finally(() => oHelper.hideLoading());
    },
    grabar: function () {
        if ($("#form-proveedor").valid()) {
            let cboTipDoc = document.getElementById('cboTipDocRegProv');
            let txtNumDoc = document.getElementById('txtNumDocRegProv');
            let txtRazSoc = document.getElementById('txtRazSocRegProv');

            let parameters = {
                IdTipoDocumento: cboTipDoc.value == "" ? undefined: cboTipDoc.value,
                NroDocumento: txtNumDoc.value,
                RazonSocial: txtRazSoc.value,
                Contacto: document.getElementById('txtConRegProv').value,
                Email: document.getElementById('txtEmaRegProv').value,
                Telefono: document.getElementById('txtTelRegProv').value,
                Direccion: document.getElementById('txtDirRegProv').value,
                IdDepartamento: document.getElementById('cboDepRegProv').value,
                IdProvincia: document.getElementById('cboProRegProv').value,
                IdDistrito: document.getElementById('cboDisRegProv').value,
                MaxDigitosDocumento: cboTipDoc.options[cboTipDoc.selectedIndex].getAttribute("data-max-digitos")
            };

            oHelper.showLoading("#modalRegistrarProveedor .modal-content");

            axios.post("/Proveedor/Register", parameters).then((response) => {
                oModalRegistrarProveedor.resolve({
                    idTipoDocumento: cboTipDoc.value,
                    numDocumento: txtNumDoc.value,
                    idProveedor: response.data.Data,
                    nomProveedor: txtRazSoc.value.capitalizeAll()
                });
                oModalRegistrarProveedor.ok = true;
                oModalRegistrarProveedor.instance.hide();
            }).catch((error) => {
                oAlerta.alerta({
                    title: error.response.data.Message,
                    type: "warning"
                });
            }).finally(() => oHelper.hideLoading());
        }
    },
    validaciones: function () {
        //max-caracteres-documento
        $.validator.addMethod('max-caracteres-documento', function (value, element, params) {
            let cboTipDoc = params[0];
            let maxDigitos = cboTipDoc.options[cboTipDoc.selectedIndex].getAttribute("data-max-digitos");

            if (value.toString().length != maxDigitos)
                return false;

            return true;
        });

        $.validator.unobtrusive.adapters.add('max-caracteres-documento', [], function (options) {
            let element = options.form.querySelector('select#cboTipDocRegProv');

            options.rules['max-caracteres-documento'] = [element];
            options.messages['max-caracteres-documento'] = options.message;
        });

        //Required-provincia
        $.validator.addMethod('required-provincia', function (value, element, params) {
            let cboDepartamento = params[0];

            if (cboDepartamento.value != "" && value == "")
                return false;

            return true;
        });

        $.validator.unobtrusive.adapters.add('required-provincia', [], function (options) {
            let element = options.form.querySelector('select#cboDepRegProv');

            options.rules['required-provincia'] = [element];
            options.messages['required-provincia'] = options.message;
        });

        //Required-distrito
        $.validator.addMethod('required-distrito', function (value, element, params) {
            let cboDepartamento = params[0];

            if (cboDepartamento.value != "" && value == "")
                return false;

            return true;
        });

        $.validator.unobtrusive.adapters.add('required-distrito', [], function (options) {
            let element = options.form.querySelector('select#cboDepRegProv');

            options.rules['required-distrito'] = [element];
            options.messages['required-distrito'] = options.message;
        });

        //Required-direccion
        $.validator.addMethod('required-direccion', function (value, element, params) {
            let cboDepartamento = params[0];

            if (cboDepartamento.value != "" && value == "")
                return false;

            return true;
        });

        $.validator.unobtrusive.adapters.add('required-direccion', [], function (options) {
            let element = options.form.querySelector('select#cboDepRegProv');

            options.rules['required-direccion'] = [element];
            options.messages['required-direccion'] = options.message;
        });
    }
}

document.addEventListener('DOMContentLoaded', oModalRegistrarProveedor.init);