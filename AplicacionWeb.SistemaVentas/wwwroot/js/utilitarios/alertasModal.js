'use strict'
var oAlertaModal = {
    resolve: null,
    reject: null,
    aceptar: false,
    showConfirmation: function (config) {
        if (typeof config != 'object')
            throw new Error("El parámetro debe ser un objeto");

        let title = "Confirmación";
        let message = "";
        let textButton1 = "Cancelar";
        let textButton2 = "Aceptar";
        let iconButton1 = '<i class="bi bi-x-circle-fill"></i>';
        let iconButton2 = '<i class="bi bi-check-circle-fill"></i>';
        let size = "modal-sm";

        if (config.title != undefined)
            title = config.title;

        if (config.message != undefined)
            message = config.message;

        if (config.textButton1 != undefined)
            textButton1 = config.textButton1;

        if (config.textButton2 != undefined)
            textButton2 = config.textButton2;

        if (config.iconButton1 != undefined)
            iconButton1 = config.iconButton1;

        if (config.iconButton2 != undefined)
            iconButton2 = config.iconButton2;

        if (config.size != undefined)
            size = config.size;

        let html = `<div class="modal fade" tabindex="-1" aria-hidden="true" id="alerta-modal">
                      <div class="modal-dialog ${size}">
                        <div class="modal-content">
                          <div class="modal-header py-3">
                            <h5 class="modal-title">${title}</h5>
                            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                          </div>
                          <div class="modal-body">
                            <p>${message}</p>
                          </div>
                          <div class="modal-footer py-2">
                            <button type="button" class="btn btn-danger" id="btnCancelar" data-bs-dismiss="modal">${iconButton1} ${textButton1}</button>
                            <button type="button" class="btn btn-success" id="btnAceptar">${iconButton2} ${textButton2}</button>
                          </div>
                        </div>
                      </div>
                    </div>`;

        //Lo agregamos temporalmente al main del html
        let content = document.getElementsByTagName('body')[0]; // document.getElementById('content');
        let main = content.querySelector('main');
        main.insertAdjacentHTML("afterbegin", html);

        let options = {
            backdrop:false
        };
        let modal = document.getElementById('alerta-modal')
        let myModal = new bootstrap.Modal(modal, options)

        modal.addEventListener('shown.bs.modal', function () {

        });
        modal.addEventListener('hidden.bs.modal', function () {
            //Si no se dio click en el boton guardar cambios.
            if (!oAlertaModal.aceptar)
                oAlertaModal.reject();

            oAlertaModal.aceptar = false;
            //Destruimos el modal.
            main.removeChild(modal);
        });

        modal.querySelector('#btnAceptar').addEventListener('click', () => {
            oAlertaModal.aceptar = true;
            oAlertaModal.resolve();
            myModal.hide();
        })


        myModal.show();

        return new Promise((resolve, reject) => {
            oAlertaModal.resolve = resolve;
            oAlertaModal.reject = reject;
        })
    }
}