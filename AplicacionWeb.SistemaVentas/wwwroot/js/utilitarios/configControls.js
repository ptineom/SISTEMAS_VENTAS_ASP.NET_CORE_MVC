'use string'
var oConfigControls = {
    inicializarDatePicker: function (selector) {
        selector = selector == undefined ? '.date-picker' : selector;
        $(selector).datepicker({
            autoclose: true,
            todayHighlight: true,
            language: 'es' //debes de descargar el nombredelarchivo.es.js, será el encargado de traducirlo en español. 
        }).prev().on('click', function () {
            $(this).next().focus();
        });
    },
    inicializarDataTable: function (objeto) {
        // Agregar botones
        let buttons = [];
        if (objeto.botones != undefined) {
            //Botón nuevo
            if (objeto.botones.btnNuevo != undefined) {
                let btnNuevo = objeto.botones.btnNuevo;
                if (btnNuevo.action == undefined) {
                    throw new Error("Acción indefinido en el botón nuevo.");
                }
                if (typeof btnNuevo.action != 'function') {
                    throw new Error("Tipo de dato incorrecto en la acción del botón nuevo, debió ser de tipo funcion.");
                }
                let text = '<span style="color:white"><i class="fas fa-file" ></i> ' + btnNuevo.text + '</span>';
                let className = "btn btn-primary"
                if (btnNuevo.className != undefined) {
                    if (!btnNuevo.className.isnullOrEmpty()) {
                        className = btnNuevo.className;
                    }
                }
                buttons.push({
                    text: text,
                    className: className,
                    action: function (e, dt, node, config) {
                        if (btnNuevo.action != undefined) {
                            btnNuevo.action();
                        }
                    }
                })
            }
            //Botón exportar
            if (objeto.botones.btnExportarExcel != undefined) {
                let btnExportarExcel = objeto.botones.btnExportarExcel;
                if (btnExportarExcel.action == undefined) {
                    throw new Error("Acción indefinido en el botón exportar a excel.");
                }
                if (typeof btnExportarExcel.action != 'function') {
                    throw new Error("Tipo de dato incorrecto en la acción del botón exportar a excel, debió ser de tipo funcion.");
                }
                let text = '<span ><i class="fas fa-file-excel" style="color:green"></i> ' + btnExportarExcel.text + '</span>';
                let className = "btn btn-default"
                if (btnExportarExcel.className != undefined) {
                    if (!btnExportarExcel.className.isnullOrEmpty()) {
                        className = btnExportarExcel.className;
                    }
                }
                buttons.push({
                    text: text,
                    className: className,
                    action: function (e, dt, node, config) {
                        if (btnExportarExcel.action != undefined) {
                            btnExportarExcel.action();
                        }
                    }
                })
            }
            //Botón consultar
            if (objeto.botones.btnConsultar != undefined) {
                let btnConsultar = objeto.botones.btnConsultar;
                if (btnConsultar.action == undefined) {
                    throw new Error("Acción indefinido en el botón consultar.");
                }
                if (typeof btnConsultar.action != 'function') {
                    throw new Error("Tipo de dato incorrecto en la acción del botón consultar, debió ser de tipo funcion.");
                }
                let text = '<span ><i class="fas fa-search" ></i> ' + btnConsultar.text + '</span>';
                let className = "btn btn-microsoft"
                if (btnConsultar.className != undefined) {
                    if (!btnConsultar.className.isnullOrEmpty()) {
                        className = btnConsultar.className;
                    }
                }
                buttons.push({
                    text: text,
                    className: className,
                    action: function (e, dt, node, config) {
                        if (btnConsultar.action != undefined) {
                            btnConsultar.action();
                        }
                    }
                })
            }
        }

        if (objeto.selector == undefined || objeto.selector == "")
            throw new Error("Nombre de la tabla no ingresado en la propiedad selector.")

        let arrColumns = [];
        if (objeto.arrColumns != undefined) {
            if (typeof objeto.arrColumns !== 'object')
                throw new Error("Tipo de dato incorrecto en la propiedad arrColumns");

            arrColumns = objeto.arrColumns;
        }

        let bFilter = false;
        if (objeto.bFilter != undefined) {
            if (typeof objeto.bFilter !== 'boolean')
                throw new Error("Tipo de dato incorrecto en la propiedad bFilter");

            bFilter = objeto.bFilter;
        }

        let bPaginate = false;
        if (objeto.bPaginate != undefined) {
            if (typeof objeto.bPaginate !== 'boolean')
                throw new Error("Tipo de dato incorrecto en la propiedad bPaginate");

            bPaginate = objeto.bPaginate;
        }

        let bInfo = false;
        if (objeto.bInfo != undefined) {
            if (typeof objeto.bInfo !== 'boolean')
                throw new Error("Tipo de dato incorrecto en la propiedad bInfo");

            bInfo = objeto.bInfo;
        }

        let bAutoWidth = false;
        if (objeto.bAutoWidth != undefined) {
            if (typeof objeto.bAutoWidth !== 'boolean')
                throw new Error("Tipo de dato incorrecto en la propiedad bAutoWidth");

            bAutoWidth = objeto.bAutoWidth;
        }

        if (objeto.fnCallBack != undefined) {
            if (!objeto.fnCallBack === 'function') {
                throw new Error("Tipo de dato incorrecto en la propiedad fnCallBack");
            }
        }

        let bLengthChange = false;
        let arrLengthMenu = null

        if (objeto.bLengthChange != undefined) {
            if (typeof objeto.bLengthChange !== 'boolean')
                throw new Error("Tipo de dato incorrecto en la propiedad bAutoWidth");

            bLengthChange = objeto.bLengthChange;

            if (bLengthChange) {
                if (window.innerWidth < 1200)
                    arrLengthMenu = [5, 10, 25, 50];
            }
        }

        var config = {
            bSort: true,
            bAutoWidth: bAutoWidth,//calcula el ancho de las columnas y los ajusta según su contenido, pero para que calcule no debe de existir anchos predeterminados en las columnas.(por defecto viene en true)
            "aoColumns": arrColumns,// No hace falta las medidas para el width, automaticamente lo calcula segun el contenido y el ancho del webbrowser.
            "aaSorting": [], // la primera columna siempre viene con ordenacion asi  estblesca en false, pero con esta propiedad lo establece en false.
            //scrollX: true,
            bFilter: bFilter,
            bInfo: bInfo,
            bPaginate: bPaginate,
            bLengthChange: bLengthChange,//Oculta la opción para cambiar el números de registros a mostrar.
            "lengthMenu": arrLengthMenu,
            "language": {
                "sProcessing": "Procesando...",
                "sLengthMenu": "Mostrar _MENU_ registros",
                "sZeroRecords": "No se encontraron resultados",
                "sEmptyTable": "Ningún dato disponible en esta tabla",
                "sInfo": "Mostrando registros del _START_ al _END_ de un total de _TOTAL_ registros",
                "sInfoEmpty": "Mostrando registros del 0 al 0 de un total de 0 registros",
                "sInfoFiltered": "(filtrado de un total de _MAX_ registros)",
                "sInfoPostFix": "",
                "sSearch": "Buscar:",
                "sUrl": "",
                "sInfoThousands": ",",
                "sLoadingRecords": "Cargando...",
                "oPaginate": {
                    "sFirst": "Primero",
                    "sLast": "Último",
                    "sNext": "Siguiente",
                    "sPrevious": "Anterior"
                }
            }
        };
        $(objeto.selector).DataTable(config);

        if (objeto.callback != undefined)
            objeto.callback();

    },
    formReset: function (element) {
        let form = element;

        //Limpiamos con la ayuda de jquery.validate()
        var validator = $(form).validate();
        validator.resetForm();

        //Limpiamos lo generado por el unobtrosive
        Array.from(form.querySelectorAll("[data-valmsg-replace]")).forEach(span => {
            span.classList.remove("field-validation-error");
            span.classList.add("field-validation-valid");
            span.textContent = "";
        });
    }
}