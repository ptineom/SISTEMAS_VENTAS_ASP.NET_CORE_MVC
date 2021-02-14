"use strict"
var oAlerta = {
    bodyMensaje: function (sTipoAlerta, sMensaje, idElementoContenedor) {
        var sTitulo = "";
        var sIcono = "";
        var sColorIcono = "";
        switch (sTipoAlerta) {
            case "success":
                sTitulo = "¡Bien!";
                sIcono = "ion-checkmark";
                sColorIcono = "green";
                break;
            case "danger":
                sTitulo = "¡Error!";
                sIcono = "ion-close-circled";
                sColorIcono = "red";
                break;
            case "warning":
                sTitulo = "¡Alerta!";
                sIcono = "ion-alert-circled";
                sColorIcono = "#FFC300";
                break;
            case "info":
                sTitulo = "¡Información!";
                sIcono = "ion-information-circled";
                sColorIcono = "#1b95e0";
                break;
            default:
                break;
        }
        var sContenido = '';
        sContenido += '<div class="alert alert-' + sTipoAlerta + ' alert-dismissible fade show" role="alert" id="alerta">';
        sContenido += '<div class="ContenedorBoton">';
        sContenido += '<button type="button" class="close" data-dismiss="alert" aria-label="Close" >';
        sContenido += '<span aria-hidden="true">&times;</span></button></div>';
        sContenido += '<div class="ContenedorCuerpo">';
        sContenido += '<div class="mensaje">';
        sContenido += '<div class="cabecera-mensaje">';
        sContenido += '<h4 class="alert-heading" style="color:' + sColorIcono + '">' + sTitulo + '</h4></div>';
        sContenido += '<div class="cuerpo-mensaje">' + sMensaje + '</div></div>';
        sContenido += '<div class="icono-personal" >';
        sContenido += '<span class="icono-alertas ' + sIcono + '" style="color:' + sColorIcono + '"></span>';
        sContenido += '</div></div></div>'

        if (idElementoContenedor == null || idElementoContenedor == "" || idElementoContenedor == undefined) {
            $("body").prepend(sContenido);
        } else {
            var modalContent = $("#" + idElementoContenedor).find(".modal-content");
            $("#" + idElementoContenedor).find(".modal-body").prepend(sContenido);
        }
        //setTimeout(function () {
        //    $(".alert").fadeOut(6000, function () {
        //        $(".alert ").remove();
        //    });
        //}, 2000)

    },
    mensajeOk: function (sMensaje) {
        oAlerta.bodyMensaje('success', sMensaje);
    },
    mensajeError: function (sMensaje) {
        oAlerta.bodyMensaje('danger', sMensaje);
    },
    mensajeInformacion: function (sMensaje) {
        oAlerta.bodyMensaje('info', sMensaje);
    },
    mensajeAlerta: function (sMensaje, idElementoContenedor) {
        oAlerta.bodyMensaje('warning', sMensaje, idElementoContenedor);
    },
    mensajeBootbox: function (mensaje, tipoAlerta, callBack) {
        var icono = "";
        var colorIcono = "";
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
    listaEtiquetaError: function (listaIdElementos, idContenedor) {
        var nCount = listaIdElementos.length;
        if (nCount > 0) {
            oAlerta.limpiarEtiquetaError(idContenedor);
            for (var i = 0; i < nCount; i++) {
                var elemento = $("#" + listaIdElementos[i].elemento);
                elemento.after("<span class='label label-danger field-validation-error'>" + listaIdElementos[i].mensajeError + "</span>");
                //elemento.after("<span class='badge badge-danger field-validation-error'>" + listaIdElementos[i].mensajeError + "</span>");
            }
        }
    },
    etiquetaError: function (idElemento, mensajeError) {
        const limpiarEtiqueta = () => {
            return new Promise((resolve, reject) => {
                oAlerta.limpiarEtiquetaError(idElemento);
                let bSeguirOk = true;
                resolve(bSeguirOk);
            });
        };
        const crearEtiqueta = () => {
            return new Promise(() => {
                var elemento = $("#" + idElemento);
                elemento.append("<span class='label label-danger field-validation-error'>" + mensajeError + "</span>");
                //Pasando los 2 seg. se ejecutará el fadeout que hará que se elimine el error lentamente en 3 seg.(animación)
                setTimeout(function () {
                    let validation = elemento.find(".field-validation-error");
                    validation.fadeOut(3000, function () {
                        validation.remove();
                    });
                }, 2000)
            });
        };
        //Ejecutamos las promesas
        limpiarEtiqueta().then((bSeguir) => {
            if (bSeguir) {
                // codigo opcional
            }
            crearEtiqueta();
        });
    },
    limpiarEtiquetaError: function (idElemento) {
        var elemento = $("#" + idElemento).find(".field-validation-error");
        if (elemento.length > 0) {
            elemento.remove();
        }
    }
}