'use strict'
var oAlertaModal = {
    resolve: null,
    reject: null,
    aceptar: false,
    instance:null,
    showConfirmation: function (config) {
        if (typeof config != 'object')
            throw new Error("El parámetro del modal de confirmación debe ser un objeto");

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

        let html = `<div class="modal fade" tabindex="-1" aria-hidden="true" id="alerta-modal" aria-labelledby="staticBackdropLabel" style="z-index:99999">
                      <div class="modal-dialog ${size} modal-dialog-centered">
                        <div class="modal-content" >
                          <div class="modal-header py-3">
                            <h5 class="modal-title">${title}</h5>
                            <button type="button" class="btn-close" aria-label="Close" id="btnCloseAlerta"></button>
                          </div>
                          <div class="modal-body">
                            <div class="row">
                                <div class="col-9"><p class="mb-0">${message}</p></div>
                                <div class="col-3 "><i class="bi bi-question-circle-fill text-primary h2"></i></div>
                            </div>
                          </div>
                          <div class="modal-footer py-2">
                            <button type="button" class="btn btn-danger" id="btnCancelarAlerta" >${iconButton1} ${textButton1}</button>
                            <button type="button" class="btn btn-success" id="btnAceptarAlerta">${iconButton2} ${textButton2}</button>
                          </div>
                        </div>
                      </div>
                    </div>`;

        //Lo agregamos temporalmente al main del html
        let content = document.getElementsByTagName('body')[0]; // document.getElementById('content');
        let main = content.querySelector('main');
        main.insertAdjacentHTML("afterbegin", html);

        let options = {
            backdrop: "static"
        };
        let alertaModal = document.getElementById('alerta-modal')
        oAlertaModal.instance = new bootstrap.Modal(alertaModal, options);

        alertaModal.addEventListener('hidden.bs.modal', function () {
            if (!oAlertaModal.aceptar)
                oAlertaModal.reject();

            //Destruimos el modal.
            alertaModal.parentElement.removeChild(alertaModal);
        });

        alertaModal.querySelector('#btnAceptarAlerta').addEventListener('click', () => {
            oAlertaModal.aceptar = true;
            oAlertaModal.resolve();
            oAlertaModal.instance.hide();
        });

        Array.from(alertaModal.querySelectorAll('#btnCancelarAlerta, #btnCloseAlerta')).forEach(btn => {
            btn.addEventListener('click', () => {
                oAlertaModal.instance.hide();
            })
        })

        oAlertaModal.instance.show();

        return new Promise((resolve, reject) => {
            oAlertaModal.resolve = resolve;
            oAlertaModal.reject = reject;
        })
    }
}