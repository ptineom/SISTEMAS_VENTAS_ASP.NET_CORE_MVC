"use strict"
var oAlerta = {
    intervalId: 0,
    timeOutId: 0,
    alertInstance: null,
    show: function (config) {
        let boldText = !!config.boldText ? config.boldText : '';
        let message = !!config.message ? config.message : '';
        let simpleAlert = !!config.simpleAlert ? config.simpleAlert : false;
        let type = !!config.type ? config.type : 'default';
        let closeAutomatic = config.closeAutomatic != undefined ? config.closeAutomatic : true;

        let header = '', iconSvg = '', alertString = '';
        let content, position, top;

        //Llamado desde un modal.
        if (!!config.container) {
            content = document.querySelector(config.container);
            position = "absolute";
            top = "50"
        } else {
            content = document.getElementById('content');
            position = "fixed";
            top = "60"
        }

        switch (type) {
            case "success":
                header = "¡Éxito!";
                iconSvg = "check-circle-fill";
                break;
            case "info":
                header = "¡Información!"
                iconSvg = "info-fill";
                break;
            case "warning":
                header = "¡Advertencia!";
                iconSvg = "exclamation-triangle-fill";
                break;
            case "danger":
                header = "¡Error!";
                iconSvg = "exclamation-triangle-fill";
                break;
            default:
                header = "¡Alerta!";
                iconSvg = "";
                break;
        }

        //Si ya existe una alerta abierta pasará a cerrarla, para poder crear uno nuevo.
        if (document.getElementById('alerta') != null) 
            oAlerta.alertInstance.close();

        let style = `style='position: ${position};top: ${top}px;z-index: 9999;right: 10px; opacity:1; max-width:380px'`;

        //Alerta simple, sin iconos, ni titulo
        if (simpleAlert) {
            alertString = `<div class="alert alert-${type} alert-dismissible fade show" ${style} role="alert" id="alerta">
                        <strong>${boldText != "" ? ("¡" + boldText + "!") : ""}</strong> ${message}
                        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                    </div>`;
        } else {
            //Alerta con titulo, icono
            alertString = `<div class="alert alert-${type}" alert-dismissible fade show role="alert" ${style} id="alerta">
                        <div class="d-flex">
                            <h5>${header}</h5>
                            <div class="flex-grow-1 text-end">
                                <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
                            </div>
                        </div>
                        <div class="d-flex align-items-center">
                            <svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="${type}:"><use xlink:href="#${iconSvg}" /></svg>
                            <div class="flex-grow-1">
                                ${message}
                            </div>
                        </div>
                    </div>`;
        }
        //Creamos el componente en el DOM.
        content.insertAdjacentHTML('afterbegin', alertString);

        //Creamos la instancia del componenete en botstrap
        let alerta = document.getElementById('alerta')
        oAlerta.alertInstance = new bootstrap.Alert(alerta)

        //Recordar que hemos utilizado "close.bs.alert" y no "closed.bs.alert"(espera que se cierre la transicion de css).
        alerta.addEventListener('close.bs.alert', function (e) {
            clearTimeout(oAlerta.timeOutId);
            clearInterval(oAlerta.intervalId);
            e.target.parentElement.removeChild(e.target);
            oAlerta.alertInstance = null;
        })

        //Cierre automatico
        if (closeAutomatic) {
            let msTimeout = 5000;
            let msInterval = 200;

            //Después de cumplir el intervalo indicado en el timeout, desaparecerá lentamente.
            oAlerta.timeOutId = setTimeout(() => {
                //Disminuirá la opacidad del componente hasta desaparecer.
                oAlerta.intervalId = setInterval(function () {
                    if (alerta.style.opacity > 0) {
                        alerta.style.opacity -= 0.1;
                    } else {
                        clearInterval(oAlerta.intervalId);
                        alerta.parentElement.removeChild(alerta);
                        oAlerta.alertInstance = null;
                    }
                }, msInterval);
            }, msTimeout);
        }
    }
}