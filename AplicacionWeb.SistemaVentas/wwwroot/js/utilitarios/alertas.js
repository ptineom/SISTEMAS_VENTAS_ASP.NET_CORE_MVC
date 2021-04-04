"use strict"
var oAlerta = {
    alerta: function (config) {
        let title = !!config.title ? config.title : '';
        let subtitle = !!config.subtitle ? config.subtitle : '';
        let type = !!config.type ? config.type : 'default';
        let header = '', color = '', icon = '';
        let closeAutomatic = config.closeAutomatic != undefined ? config.closeAutomatic : false;
        let notTitle = !!config.notTitle ? config.notTitle : false;
        let notIcon = !!config.notIcon ? config.notIcon : false;

        let content;
        let position;
        let top;

        if (!!config.contenedor) {
            content = document.querySelector(config.contenedor);
            position = "absolute";
            top= "50"
        }
        else {
            content = document.getElementById('content');
            position = "fixed";
            top="60"
        }

        switch (type) {
            case "success":
                header = notTitle ? "" : "¡Éxito!";
                color = "#008d4c";
                icon = "bi-check-circle";
                break;
            case "info":
                header = notTitle ? "" : "¡Información!"
                color = "#00a7d0";
                icon = "bi-info-circle";
                break;
            case "warning":
                header = notTitle ? "" : "¡Advertencia!";
                color = "#db8b0b";
                icon = "bi-exclamation-triangle";
                break;
            case "error":
                header = notTitle ? "" : "¡Error!";
                color = "#d33724";
                icon = "bi-exclamation-octagon";
                break;
            default:
                header = notTitle ? "" : "¡Alerta!";
                color = "#b5bbc8";
                icon = "";
                break;
        }

        if (document.getElementById('alerta') != null)
            return;

        let style = `style='position: ${position};top: ${top}px;z-index: 9999;right: 10px;background-color:${color}; color:#fff; opacity:1'`
        let i = notIcon ? '' : `<i class="bi ${icon} fs-2 me-2"></i>`
        let alerta = `<div  class="alert alert-dismissible fade show" role="alert" ${style} id="alerta">
                            <h4 class="mb-0">${header}</h4>
                            ${i}
                            <strong>${title}</strong> <span>${subtitle}</span>
                            <button type="button" class="btn-close" data-bs-dismiss="alert" style='color:white' aria-label="Close"></button>
                        </div>`;

        content.insertAdjacentHTML('afterbegin', alerta);

        document.getElementById('alerta').addEventListener('closed.bs.alert', function () {
            clearInterval(oCompra.intervalId);
        })

        if (closeAutomatic) {
            let timeout = 3000;
            let milisegundos = 200;

            //Despué de cumplir el intervalo indicado en el timeout, desaparecerá lentamente.
            setTimeout(() => {
                let myAlert = document.getElementById('alerta');
                //Disminuirá la opacidad del componente hasta desaparecer.
                oCompra.intervalId = setInterval(function () {
                    if (myAlert.style.opacity > 0) {
                        myAlert.style.opacity -= 0.1;
                    } else {
                        clearInterval(oCompra.intervalId);
                        myAlert.parentElement.removeChild(myAlert);
                    }
                }, milisegundos);
            }, timeout);
        }
    },
    alertaBootbox: function (mensaje, tipoAlerta, callBack) {
        let icono = "";
        switch (tipoAlerta) {
            case "success":
                icono = "<i class='fas fa-check-circle' style='color:green'></i>";
                break;
            case "danger":
                icono = "<i class='fas fa-times-circle' style='color:red' ></i>";
                break;
            case "warning":
                icono = "<i class='fas fa-exclamation-triangle' style='color:#FFC300' ></i>";
                break;
            case "info":
                icono = "<i class='fas fa-info-circle' style='color:#1b95e0'></i>";
                break;
            default:
                break;
        }
        var msg = "<div class = 'iconoBootbox' >" + icono + "</div>";
        mensaje = mensaje.replace(/(?:\r\n|\r|\n)/g, '<br />');
        msg += "<div class='mensajeBootbox' ><strong>" + mensaje + "</strong></div>";

        let tituloFormulario = document.getElementById('tituloFormulario');
        let titulo = "Sistema de ventas"
        if (tituloFormulario != undefined) {
            titulo = tituloFormulario.textContent;
        }

        bootbox.alert({
            size: "small",
            title: titulo,
            message: msg,
            callback: function () {
                if (callBack != undefined) {
                    callBack();
                }
            }
        });
        setTimeout(function () {
            $('.mensajeBootbox').closest('.modal-content').animate({ top: '50px' }, "slow");
        }, 250);
    },
    confirmacionBootbox: function (obj) {
        var msg = "<div class = 'iconoBootbox' ><i class='fas fa-question-circle' style='color:#1b95e0'></i></div>";
        msg += "<div class='mensajeBootbox' ><strong>" + (obj.mensaje == undefined ? "" : obj.mensaje) + "</strong></div>";

        let tituloFormulario = document.getElementById('tituloFormulario');
        let titulo = "Sistema de ventas"
        if (tituloFormulario != undefined) {
            titulo = tituloFormulario.textContent;
        }
        //el texto que mostrará en el botón.
        let buttonOkText = "Aceptar", buttonCancelText = "Cancelar";
        if (obj.button != undefined) {
            if (obj.button.okText != undefined) buttonOkText = obj.button.okText;
            if (obj.button.cancelText != undefined) buttonCancelText = obj.button.cancelText;
        }

        bootbox.confirm({
            title: "<span style='color:#615f5f;'>" + titulo + "</span>",
            size: "small",
            message: msg,
            callback: function (result) {
                /* result is a boolean; true = OK, false = Cancel*/
                if (result) {
                    if (obj.callbackOk != undefined) {
                        obj.callbackOk();
                    }
                } else {
                    if (obj.callbackCancel != undefined) {
                        obj.callbackCancel();
                    }
                }
            },
            buttons: {
                confirm: {
                    label: '<i class="fas fa-check"></i> ' + buttonOkText,
                    className: 'btn-success buttonConfirm'
                },
                cancel: {
                    label: '<i class="fas fa-times"></i> ' + buttonCancelText,
                    className: 'btn-danger pull-right'
                }
            },
        });
        //Situamos el modal de confirmación a nuestro gusto.
        setTimeout(function () {
            var mensajeBootbox = $('.mensajeBootbox');
            mensajeBootbox.closest('.modal-content').animate({ top: '50px' }, "slow");
            setTimeout(function () {
                mensajeBootbox.parent().parent().siblings('.modal-footer').children('button[data-bb-handler="confirm"]')[0].focus();
            }, 500);

        }, 250);
    },
}