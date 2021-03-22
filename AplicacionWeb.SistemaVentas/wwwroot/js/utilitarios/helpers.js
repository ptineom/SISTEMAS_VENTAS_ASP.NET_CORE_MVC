"use strict"
var oHelper = {
    showLoading: function (contenedor) {
        if (!!contenedor) {
            if (document.querySelector(contenedor) != null)
                document.querySelector(contenedor).insertAdjacentHTML("afterbegin", "<div id='dvLoading-partial'></div>")
        } else {
            let body = document.getElementsByTagName('body')[0];
            body.insertAdjacentHTML("afterbegin", "<div id='dvLoading'></div>")
        }
    },
    hideLoading: function () {
        let loadingPartial = document.getElementById('dvLoading-partial');
        let dvLoading = document.getElementById('dvLoading');

        if (dvLoading != null) {
            let body = document.getElementsByTagName('body')[0];
            body.removeChild(dvLoading);
        } else if (loadingPartial != null) {
            loadingPartial.parentElement.removeChild(loadingPartial);
        }
    },
    mayusInput: function (e) {
        var ss = e.target.selectionStart;
        var se = e.target.selectionEnd;
        e.target.value = e.target.value.toUpperCase();
        e.target.selectionStart = ss;
        e.target.selectionEnd = se;
    },
    //Método para el evento keyup
    mayus: function (obj) {
        obj.value = obj.value.toUpperCase();
    },
    //Método para el evento keyup, tambien se puede usar en keypress,pero recomendable en keyup, para que de el salto al soltar la tecla.
    teclaEnter: function (e, t) {
        if (e.key == 'Enter')
            document.getElementById(t).focus();
    },

    //Método para el evento keypress
    soloNumerosEnteros: function (e) {
        let regex = /^[0-9]+$/;
        if (!regex.test(e.key))
            event.preventDefault();
    },
    //Método para el evento keypress
    numerosDecimales: function (evt, decimales) {
        // Backspace = 8, Enter = 13, ‘0′ = 48, ‘9′ = 57, ‘.’ = 46, ‘-’ = 43
        let key = window.Event ? evt.which : evt.keyCode;
        let chark = String.fromCharCode(key);
        // Se realiza esta lógica por que cuando estan insertados todos los numeros decimales, no permite insertar
        // en la parte entera, ni modificar en cualquier parte del texto.
        // indices de la posicion del cursor.
        // si el selectionStart y selectionEnd son iguales quiere decir que no hubo seleccion de texto, pero el cursor
        // si esta situado en alguna posicion.
        let input = evt.target;
        let si = input.selectionStart;
        let sf = input.selectionEnd
        var resultado = "";
        if (si != sf) {// hubo seleccion de texto
            // capturamos los textos que se encuentra antes y despues del texto seleccionado. 
            let textoInicial = input.value.substring(0, si);
            let textoFinal = input.value.substring(sf, input.value.length);
            resultado = textoInicial + chark + textoFinal;
        } else {// solo situamos el cursor en alguna posicion del texto e intemos escribir.
            let inxPunto = input.value.indexOf('.');
            if (si <= inxPunto) {// si el intento de escribir es en el lado izquierdo del punto.
                let textoInicial = input.value.substring(0, si);
                let textoFinal = input.value.substring(si, input.value.length);
                resultado = textoInicial + chark + textoFinal;
            }
        }
        let tempValue = ''
        if (resultado != "") {
            tempValue = resultado;
        } else {
            tempValue = input.value + chark;
        }
        if (key >= 48 && key <= 57) {
            if (oHelper.filter(tempValue, decimales) === false) {
                evt.preventDefault();
            }
        } else {
            if (key == 8 || key == 13 || key == 0 || key == 46) {
                if (key == 46) {
                    if (oHelper.filter(tempValue, decimales) === false) {
                        evt.preventDefault();
                    }
                }
            } else {
                evt.preventDefault();
            }
        }
    },
    // método que se utiliza para verificar que el formato de dicho textbox solo sea numeros con 2 decimales
    filter: function filter(__val__, decimales) {
        let preg;
        if (decimales != undefined) {
            preg = new RegExp("^([0-9]+\.?[0-9]{0," + decimales + "})$");
        } else {
            preg = /^([0-9]+\.?[0-9]{0,2})$/;
        }
        if (preg.test(__val__) === true) {
            return true;
        } else {
            return false;
        }

    },
    isValidEmail: function (mail) {
        return /^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,4})+$/.test(mail);
    },
    limpiarTabla: function (table) {
        let tBody = table.getElementsByTagName('tbody')[0];
        while (tBody.hasChildNodes()) {
            tBody.removeChild(tBody.firstChild);
        }
    },
    convertDateCsToJs: function (value) {
        var jsDate = new Date(parseInt(value.replace(/[^0-9 +]/g, '')));
        return jsDate;
    },
    fechaCortaCsToJs: function (value) {
        var date = new Date(parseInt(value.replace(/[^0-9 +]/g, '')));
        var dd = date.getDate();
        var mm = date.getMonth() + 1; //hoy es 0!
        var yyyy = date.getFullYear();
        if (dd < 10) dd = '0' + dd
        if (mm < 10) mm = '0' + mm
        date = dd + '/' + mm + '/' + yyyy;
        return date
    },
    horaCortaCsToJs: function (value) {
        var date = new Date(parseInt(value.replace(/[^0-9 +]/g, '')));
        var hora = date.getHours();
        var minuto = date.getMinutes(); //hoy es 0!
        var segundo = date.getSeconds();
        if (hora < 10) hora = '0' + hora;
        if (minuto < 10) minuto = '0' + minuto;
        if (segundo < 10) segundo = '0' + segundo;
        return (hora + ':' + minuto + ':' + segundo);
    },
    fechaCorta: function (date) {
        var dd = date.getDate();
        var mm = date.getMonth() + 1; //hoy es 0!
        var yyyy = date.getFullYear();
        if (dd < 10) dd = '0' + dd
        if (mm < 10) mm = '0' + mm
        date = dd + '/' + mm + '/' + yyyy;
        return date
    },
    horaCorta: function (date) {
        var hora = date.getHours();
        var minuto = date.getMinutes(); //hoy es 0!
        var segundo = date.getSeconds();
        if (hora < 10) hora = '0' + hora;
        if (minuto < 10) minuto = '0' + minuto;
        if (segundo < 10) segundo = '0' + segundo;
        return (hora + ':' + minuto + ':' + segundo);
    },
    fechaActual: function () {
        var hoy = new Date();
        var dd = hoy.getDate();
        var mm = hoy.getMonth() + 1; //hoy es 0!
        var yyyy = hoy.getFullYear();
        if (dd < 10) dd = '0' + dd
        if (mm < 10) mm = '0' + mm
        hoy = dd + '/' + mm + '/' + yyyy;
        return hoy
    },
    horaActual: function () {
        var hoy = new Date();
        var hh = hoy.getHours();
        var mm = hoy.getMinutes; //hoy es 0!
        var ss = hoy.getSeconds();
        if (hh < 10) hh = '0' + hh;
        if (mm < 10) mm = '0' + mm;
        if (ss < 10) ss = '0' + ss;
        hoy = hh + ':' + mm + ':' + ss;
        return hoy
    },
    fechaHoraFormatoLocal: function (fecha) {
        var hoy = fecha;
        // fecha
        var dia = hoy.getDate();
        var mes = hoy.getMonth() + 1; //hoy es 0!
        var anio = hoy.getFullYear();
        if (dia < 10) dia = '0' + dia
        if (mes < 10) mes = '0' + mes
        // hora
        var hora = hoy.getHours();
        var minuto = hoy.getMinutes(); //hoy es 0!
        var segundo = hoy.getSeconds();
        if (hora < 10) hora = '0' + hora;
        if (minuto < 10) minuto = '0' + minuto;
        if (segundo < 10) segundo = '0' + segundo;
        return (dia + '/' + mes + '/' + anio) + ' ' + (hora + ':' + minuto + ':' + segundo);
    },
    validarFormatoFecha: function (campo) {
        var RegExPattern = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/;
        if ((campo.match(RegExPattern)) && (campo != '')) {
            return true;
        } else {
            return false;
        }
    },
    validarFechaFormato: function (fecha) {
        if ((fecha.length > 10) || (fecha.length < 10)) return false;
        var RegExPattern = /^\d{1,2}\/\d{1,2}\/\d{2,4}$/;
        if (!fecha.match(RegExPattern)) return false;
        var fechaf = fecha.split("/");
        var day = parseInt(fechaf[0]);
        var month = parseInt(fechaf[1]);
        var year = parseInt(fechaf[2]);
        var date, _mes;
        if (!isNaN(day) && !isNaN(month) && !isNaN(year)) {
            month = (parseInt(month) - 1); // los meses en javascript : 0 - 11
            date = new Date(year, month, day);
            if (day == date.getDate() && month == date.getMonth() && year == date.getFullYear()) {
                return true;
            } else {
                return false;
            }
        } else {
            return false;
        }
        return true;
    },
    formatoFechaStandar: function (fecha) {
        if (!oHelper.validarFechaFormato(fecha)) return "";
        var fechaf = fecha.split("/");
        var day = parseInt(fechaf[0]);
        var month = parseInt(fechaf[1]);
        var year = parseInt(fechaf[2]);
        return month + "/" + day + "/" + year;
    },
    convertDateToPeriodo: function (fecha) {
        var mes = 0;
        var anio = 0;
        var periodo = "";
        if (fecha.trim().length > 0 && fecha.trim().length == 10) {
            mes = fecha.substring(5, 3);
            anio = fecha.substring(10, 6);
            periodo = anio + mes;
        }
        return periodo;
    },
    isMobile: {
        android: function () {
            return navigator.userAgent.match(/Android/i);
        },
        blackBerry: function () {
            return navigator.userAgent.match(/BlackBerry/i);
        },
        iOS: function () {
            return navigator.userAgent.match(/iPhone|iPad|iPod/i);
        },
        opera: function () {
            return navigator.userAgent.match(/Opera Mini/i);
        },
        windows: function () {
            return navigator.userAgent.match(/IEMobile/i);
        },
        any: function () {
            return (oHelper.isMobile.android() || oHelper.isMobile.blackBerry() || oHelper.isMobile.iOS() || oHelper.isMobile.opera() || oHelper.isMobile.windows());
        }
    },
    addEvent: function (element, eventName, callback) {
        if (element.addEventListener) {
            element.addEventListener(eventName, callback, false);
        } else if (element.attachEvent) {
            element.attachEvent("on" + eventName, callback);
        } else {
            element["on" + eventName] = callback;
        }
    },
    formatoComprobante: function (serie, documento) {
        let maxLDocumento = 6;
        if (documento.toString().length > maxLDocumento) {
            throw new Error("La longitud del nro del comprobante no debe de sobrepasar de los " + maxLDocumento + " caracteres.");
        }
        let nroComprobante = ('000000' + documento);
        nroComprobante = nroComprobante.slice(nroComprobante.length - 6);
        return { serie: serie, documento: nroComprobante }
    },
    formatoLeftSerie: function (prefijo, valor) {
        if (prefijo == undefined) {
            throw new Error("Prefijo de serie indefinido");
        }
        if (valor == undefined) {
            throw new Error("Número de serie indefinido");
        }
        let lengthMax = 4, res1 = 0, res2 = 0, resultado = '';
        res1 = (lengthMax - prefijo.length);
        if (valor.length <= res1) {
            res2 = (res1 - valor.length);
        } else {
            res2 = 0;
        }
        return resultado = (prefijo + ('0').repeat(res2));
    },
    convertToInt: function (numero) {
        let entera = parseInt(numero);
        let decimal = (numero - entera);
        let resultado = decimal > 0 ? numero : entera;
        return resultado;
    },
    formatoMiles: function (numero, decimales, siEsCeroEsvacio) {
        if (isNaN(numero)) 
            numero = 0;

        if ((decimales != undefined && isNaN(decimales)) || (decimales == undefined)) 
            decimales = 2;

        if (siEsCeroEsvacio != undefined) {
            if (siEsCeroEsvacio && numero == 0)
                return '';
        }

        return parseFloat(numero).toFixed(decimales).replace(/(\d)(?=(\d{3})+\.)/g, "$1,");
    },
    formatoMoneda: function (simbolo, numero, decimales) {
        let numeroEnMiles = oHelper.formatoMiles(numero, decimales);
        let nuevoFormato = simbolo + ' ' + (numeroEnMiles == '' ? 0 : numeroEnMiles);
        return nuevoFormato;
    },
    numeroSinMoneda: function (numero) {
        return numero.split(' ')[1].replace(/\,/g, '');
    },
    numeroSinMiles: function (numero) {
        return numero.replace(/\,/g, '');
    },
    dynamicSortMultiple: function () {
        /*
         * save the arguments object as it will be overwritten
         * note that arguments object is an array-like object
         * consisting of the names of the properties to sort by
         */
        var props = arguments;
        return function (obj1, obj2) {
            var i = 0, result = 0, numberOfProperties = props.length;
            /* try getting a different result from 0 (equal)
             * as long as we have extra properties to compare
             */
            while (result === 0 && i < numberOfProperties) {
                result = oHelper.dynamicSort(props[i])(obj1, obj2);
                i++;
            }
            return result;
        }
    },
    dynamicSort: function (property) {
        var sortOrder = 1;
        if (property[0] === "-") {
            sortOrder = -1;
            property = property.substr(1);
        }
        return function (a, b) {
            /* next line works with strings and numbers, 
             * and you may want to customize it to your needs
             */
            var result = (a[property] < b[property]) ? -1 : (a[property] > b[property]) ? 1 : 0;
            return result * sortOrder;
        }
    },
    popupCenter: function (url, title, w, h) {
        let nWidth = 0;
        let nHeight = 0;
        if (w == undefined && h == undefined) {
            //Porcentaje que servirá para calcular el tamaño del popup.
            let porc = 80;
            if (oHelper.isMobile.any()) {
                nWidth = screen.width;
                nHeight = screen.height;
            } else {
                nWidth = (screen.width * porc) / 100;
                nHeight = (screen.height * porc) / 100;
            }
        } else if ((w != undefined && h == undefined) || (w == undefined && h != undefined)) {
            throw new Error("Debe de especificar el width y/o height en el método popupCenter.");
        } else {
            nWidth = w;
            nHeight = h;
        }

        // Fixes dual-screen position                         Most browsers      Firefox  
        var dualScreenLeft = window.screenLeft != undefined ? window.screenLeft : screen.left;
        var dualScreenTop = window.screenTop != undefined ? window.screenTop : screen.top;
        // window.innerWidth: Ancho de todo el navegador(incluido margenes, bordes, scrollbar). Cambiara de valor si es que cambia de tamaño el navegador
        // document.documentElement.clientWidth: Ancho del documento html sin margenes, bordes, scrollbar.
        // screen.width: Ancho de la pantalla y no cambia de valor asi cambie de tamaño el navegador.
        let width = window.innerWidth ? window.innerWidth : document.documentElement.clientWidth ? document.documentElement.clientWidth : screen.width;
        let height = window.innerHeight ? window.innerHeight : document.documentElement.clientHeight ? document.documentElement.clientHeight : screen.height;

        var left = ((width / 2) - (nWidth / 2)) + dualScreenLeft;
        var top = ((height / 2) - (nHeight / 2)) + dualScreenTop;
        var newWindow = window.open(url, title, 'scrollbars=yes, width=' + nWidth + ', height=' + nHeight + ', top=' + top + ', left=' + left);

        // Puts focus on the newWindow  
        if (window.focus) {
            newWindow.focus();
        }
    },
    evaluarSiEsMismoUsuario(usuarioSeleccionado, fnCallback) {
        if (usuarioSeleccionado == undefined) {
            throw new Error("Parámetro del usuario indefinido.");
        };
        if (fnCallback == undefined) {
            throw new Error("Parámetro callback indefinido.");
        } else {
            if (typeof fnCallback != 'function') {
                throw new Error("El parámetro callback debe ser de tipo function.");
            }
        };

        let bMismoUsuario = false;
        //Solo hará la compración del usuario cuando sea diferente a nuevo.
        let usuarioActual = JSON.parse(localStorage.getItem("usuarioActual"));
        if (usuarioActual != undefined) {
            if (usuarioActual.ID_USUARIO == usuarioSeleccionado) {
                bMismoUsuario = true;
            };
            fnCallback(bMismoUsuario);
        }
    },
};

