'use strict'
var oCompra = {
    intervalId: 0,
    modeloAbono: null,
    init: function () {
        oConfigControls.inicializarDatePicker(".date-picker");

        document.getElementById('txtIgv').addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));

        let txtSerie = document.getElementById('txtSerie');
        txtSerie.addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtNumero"));

        let txtNumero = document.getElementById('txtNumero');
        txtNumero.addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtFecEmi"));
        txtNumero.addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));

        let txtFecEmi = document.getElementById('txtFecEmi');
        txtFecEmi.addEventListener('keyup', (e) => oHelper.teclaEnter(e, "txtFecVen"));

        let txtNumDoc = document.getElementById('txtNumDoc');
        txtNumDoc.addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));
        txtNumDoc.addEventListener('keyup', (e) => {
            if (e.key == "Enter")
                oCompra.obtenerProveedorPorDocumento();
        });

        document.getElementById('cboTipDoc').addEventListener('change', (e) => {
            let cbo = e.target;
            oCompra.inicializarDatosProveedor(cbo);
        });

        document.getElementById('cboTipCom').addEventListener('change', (e) => {
            let cbo = e.target;

            if (cbo.value == "-1")
                return;

            let nomTipCom = cbo.options[cbo.selectedIndex].text;
            let cboTipDoc = document.getElementById('cboTipDoc');

            //Habilitamos solo los documentos según el comprobante seleccionado.
            Array.from(cboTipDoc.querySelectorAll("option")).forEach(opt => {
                opt.disabled = true;

                if (opt.value != "-1") {
                    //Si es factura entonces se habilitaran solo los documento con el flagRuc.
                    if (nomTipCom.slice(0, 3).toUpperCase() == "FAC") {
                        let flgRuc = opt.getAttribute("data-flgruc") == "True" ? true : false;
                        if (flgRuc) {
                            opt.disabled = false;
                            opt.selected = true;
                        }
                    } else {
                        opt.disabled = false;
                    }
                } else {
                    opt.disabled = false;
                }
            });

            if (nomTipCom.slice(0, 3).toUpperCase() == "FAC" && cboTipDoc.value != "-1") {
                let flgRuc = cboTipDoc.options[cboTipDoc.selectedIndex].getAttribute("data-flgruc") == "True" ? true : false;
                //Si es ruc(es una ayuda), inicializamos los controles del proveedor. 
                if (flgRuc)
                    oCompra.inicializarDatosProveedor(cboTipDoc);
                else
                    oCompra.limpiarProveedor(true);
             };

            cboTipDoc.focus();
        });

        document.getElementById('btnBusPorNum').addEventListener('click', oCompra.obtenerProveedorPorDocumento);
        document.getElementById('btnBusPorCodBar').addEventListener('click', oCompra.obtenerArticuloPorCodigoBarra);

        let txtCodBar = document.getElementById('txtCodBar');
        txtCodBar.addEventListener('input', (e) => {
            if (e.target.value.length > 0) {
                document.getElementById('btnLimCodBar').style.display = "block";
            } else {
                document.getElementById('btnLimCodBar').style.display = "none";
            }
        });
        txtCodBar.addEventListener('keyup', (e) => {
            if (e.key == "Enter")
                oCompra.obtenerArticuloPorCodigoBarra();
        });
        txtCodBar.addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));

        document.getElementById('btnLimCodBar').addEventListener('click', (e) => {
            e.currentTarget.style.display = "none";
            let txtCodBar = document.getElementById('txtCodBar');
            txtCodBar.value = "";
            txtCodBar.focus();
        });

        $("#tblDetalle").find("tbody").on("change", "td select", (e) => {
            let cbo = e.target;
            let row = cbo.parentElement.parentElement;
            let cells = row.cells;

            let nroFactor = "";
            if (cbo.value != "-1")
                nroFactor = cbo.options[cbo.selectedIndex].getAttribute('data-nroFactor');

            cells[3].textContent = nroFactor;
            cells[5].children[0].focus();

        });

        $("#tblDetalle").find("tbody").on("keypress", "td input[type=text], td input[type=number]", (e) => {
            let input = e.target;
            let td = input.parentElement;
            if (td.cellIndex == 5 || td.cellIndex == 7) {
                oHelper.numerosDecimales(e);
            } else if (td.cellIndex == 6) {
                oHelper.soloNumerosEnteros(e);
            }
        });

        $("#tblDetalle").find("tbody").on("keyup", "td input[type=number], td input[type=text]", (e) => {
            let input = e.target;
            let td = input.parentElement;
            let cells = td.parentElement.cells;

            if (td.cellIndex == 5 && e.key == "Enter") {
                cells[6].children[0].focus();
            } else if (td.cellIndex == 6 && e.key == "Enter") {
                cells[7].children[0].focus();
            }
        });

        $("#tblDetalle").find("tbody").on("input", "td input[type=number], td input[type=text]", (e) => {
            let input = e.target;
            let td = input.parentElement;
            let cells = td.parentElement.cells;

            let precio = cells[4].children[0].value == '' ? 0 : parseFloat(cells[4].children[0].value);
            let cantidad = cells[5].children[0].value == '' ? 0 : parseFloat(cells[5].children[0].value);
            let descuento = cells[6].children[0].value == '' ? 0 : parseFloat(cells[6].children[0].value);
            let importe = cells[7].children[0].value == '' ? 0 : parseFloat(cells[7].children[0].value);

            //Editando el precio
            if (td.cellIndex == 4) {
                precio = new Decimal(precio);
                let precioBruto = precio.times(cantidad);
                let montoDscto = precioBruto.times((descuento / 100));
                cells[7].children[0].value = precioBruto.minus(montoDscto).toNumber().toFixed(2);

                //Editando el importe
            } else if (td.cellIndex == 7) {
                let importeSinDscto = 0;
                let porcMaximo = new Decimal(100);
                let porcVendido = porcMaximo.minus(descuento).toNumber();

                if (porcVendido > 0) {
                    //Hallamos el importe sin descuento con la regla de 3 simple.
                    importeSinDscto = new Decimal(porcMaximo.times(importe).dividedBy(porcVendido).toNumber());

                    //Hallamos el precio UM
                    if (cantidad > 0)
                        cells[4].children[0].value = importeSinDscto.dividedBy(cantidad).toNumber().toFixed(2);
                    else
                        cells[4].children[0].value = 0;

                } else {
                    cells[7].children[0].value = 0;
                }

                //Editando la cantidad o descuento
            } else {
                //Controlamos para que no se ingrese mayor a 100
                if (td.cellIndex == 6) {
                    let dscto = input.value;
                    if (!!dscto) {
                        //Esto es solo porque a veces estando el numero 100 y apretamos una tecla numerica
                        if (dscto.length == 4) {
                            dscto = dscto.slice(0, 3);
                        } else {
                            if (dscto > 100)
                                dscto = dscto.slice(0, 2);
                        }
                        descuento = dscto;
                        input.value = descuento == 0 ? '' : descuento;
                    }
                }

                if (precio > 0) {
                    precio = new Decimal(precio);
                    let precioBruto = precio.times(cantidad);
                    let montoDscto = precioBruto.times((descuento / 100));
                    cells[7].children[0].value = precioBruto.minus(montoDscto).toNumber().toFixed(2);
                } else {
                    let importeSinDscto = 0;
                    let porcMaximo = new Decimal(100);
                    let porcVendido = porcMaximo.minus(descuento).toNumber();

                    if (porcVendido > 0) {
                        //Hallamos el importe sin descuento con la regla de 3 simple.
                        importeSinDscto = new Decimal(porcMaximo.times(importe).dividedBy(porcVendido).toNumber());

                        //Hallamos el precio UM
                        if (cantidad > 0)
                            cells[4].children[0].value = importeSinDscto.dividedBy(cantidad).toNumber().toFixed(2);
                        else
                            cells[4].children[0].value = 0;

                    } else {
                        cells[7].children[0].value = 0;
                    }
                }
            }
            oCompra.calcularTotales();
        });

        $("#tblDetalle").find("tbody").on("click", "td button", (e) => {
            let button = e.currentTarget;
            let row = button.parentElement.parentElement;
            document.getElementById("tblDetalle").deleteRow(row.rowIndex);

            oCompra.calcularTotales();
        });

        let txtTasDes = document.getElementById('txtTasDes');
        txtTasDes.addEventListener('keypress', (e) => oHelper.soloNumerosEnteros(e));
        txtTasDes.addEventListener('input', (e) => {
            let input = e.target;
            //Controlamos para que no se ingrese mayor a 100
            let dscto = input.value;
            if (!!dscto) {
                //Esto es solo porque a veces estando el numero 100 y apretamos una tecla numerica
                if (dscto.length == 4) {
                    dscto = dscto.slice(0, 3);
                } else {
                    if (dscto > 100)
                        dscto = dscto.slice(0, 2);
                }
                input.value = dscto == 0 ? '' : dscto;
            }
            oCompra.calcularTotales();
        });

        document.getElementById('cboForPag').addEventListener('change', (e) => oCompra.seleccionarFormaPago(e));

        document.getElementById('btnEditarAbono').addEventListener('click', () => {
            let flgEditar = true;
            oModalAbono.show(oCompra.modeloAbono, flgEditar).then(response => {
                oCompra.bindingAbono(response);
            }).catch((editar) => { });
        });

        oCompra.limpiarTotales();

        oCompra.limpiarAbono();

        //Configuración modal consultar proveedor


        document.getElementById('btnConPro').addEventListener('click', (e) => {
            oModalConsultarProveedor.show().then(response => {
                document.getElementById('hddIdPro').value = response.idProveedor;
                document.getElementById('txtRazSoc').value = response.nomProveedor;
                document.getElementById('cboTipDoc').value = response.idTipoDocumento;
                document.getElementById('txtNumDoc').value = response.numDocumento;

                ////Nombre del modal
                //let modal = document.getElementById('modalConsultarProveedor');

                //let myModal = new bootstrap.Modal(modal, {});
                //myModal.hide();

                //
                document.getElementById('btnBusPorNum').disabled = false;
                let txtNumDoc = document.getElementById('txtNumDoc');
                txtNumDoc.disabled = false;
                txtNumDoc.maxLength = cbo.options[cbo.selectedIndex].getAttribute("data-maxDigitos");
                txtNumDoc.focus();
            }).catch(error => {
        
            });
         });
    },
    limpiarTotales: function () {
        document.getElementById('lblSubTot').textContent = "0.00";
        document.getElementById('txtTasDes').textContent = "";
        document.getElementById('lblTotDes').textContent = "0.00";
        document.getElementById('lblTotIgv').textContent = "0.00";
        document.getElementById('lblTotal').textContent = "0.00";
        document.getElementById('lblTotRed').textContent = "0.00";
        document.getElementById('lblTotPag').textContent = `${oCompra.getSgnMoneda()} 0.00`;
    },
    getSgnMoneda: function () {
        let cboMoneda = document.getElementById('cboMoneda');
        return cboMoneda.options[cboMoneda.selectedIndex].getAttribute("data-sgnMoneda");
    },
    getIgv: function () {
        return (parseFloat(document.getElementById('txtIgv').value) / 100);
    },
    calcularTotales: function () {
        let table = document.getElementById('tblDetalle');
        let tbody = table.getElementsByTagName('tbody')[0];
        let rows = tbody.rows;
        let count = rows.length;

        let sumTotal = 0, subTotal = 0, tasDes = 0, monDes = 0, monIgv = 0, total = 0, totalPagar = 0, decimalRestara = 0, bruto = 0;
        let igv = oCompra.getIgv();

        //Obtenemos el total del detalle
        for (var i = 0; i < count; i++) {
            let importe = rows[i].cells[7].children[0].value == "" ? 0 : parseFloat(rows[i].cells[7].children[0].value);
            sumTotal += importe;
        }

        tasDes = document.getElementById('txtTasDes').value == '' ? 0 : parseFloat(document.getElementById('txtTasDes').value);

        //Hallamos los demas totales
        if (sumTotal > 0) {
            //Para un cálculo mas perfecto se redondeará el subtotal en 2 decimales
            subTotal = parseFloat((sumTotal / (1 + igv)).toFixed(2));

            if (tasDes > 0) {
                monDes = (subTotal * (tasDes / 100));
                bruto = (subTotal - monDes);
                monIgv = (bruto * igv);
                total = (bruto + monIgv);
            } else {
                monIgv = (subTotal * igv);
                total = sumTotal;
            }

            totalPagar = ((parseInt(total.toFixed(2) * 10)) / 10);
            decimalRestara = (parseFloat(total.toFixed(2)) - totalPagar);
        }

        document.getElementById('lblSubTot').textContent = oHelper.formatoMiles(subTotal, 2);
        document.getElementById('lblTotDes').textContent = oHelper.formatoMiles(monDes, 2);
        document.getElementById('lblTotIgv').textContent = oHelper.formatoMiles(monIgv, 2);
        document.getElementById('lblTotal').textContent = oHelper.formatoMiles(total, 2);
        document.getElementById('lblTotRed').textContent = decimalRestara.toFixed(2);
        document.getElementById('lblTotPag').textContent = oHelper.formatoMoneda(oCompra.getSgnMoneda(), totalPagar, 2);

        if (subTotal == 0)
            document.getElementById('txtTasDes').value = "";
    },
    obtenerProveedorPorDocumento: function () {
        let cboTipDoc = document.getElementById('cboTipDoc');
        let txtNumDoc = document.getElementById('txtNumDoc');

        if (cboTipDoc.value == '-1') {
            oAlerta.alerta({
                title: "Debe de seleccionar el tipo de documento",
                type: "warning"
            });
            return;
        }
        if (txtNumDoc.value == '') {
            oAlerta.alerta({
                title: "Debe de ingresar el N° de documento",
                type: "warning"
            });
            return;
        }

        oHelper.showLoading("#cardProveedor");

        const parameters = `${cboTipDoc.value}/${txtNumDoc.value}`;

        axios.get(`/Proveedor/GetByDocument/${parameters}`).then(response => {
            const modelo = response.data.Data;

            document.getElementById('hddIdPro').value = modelo.IdProveedor;
            document.getElementById('cboTipDoc').value = modelo.IdTipoDocumento;
            document.getElementById('txtNumDoc').value = modelo.NroDocumento;
            document.getElementById('txtRazSoc').value = modelo.NomProveedor;
        }).catch((error) => {
            oAlerta.alerta({
                title: error.response.data.Message,
                type: "warning"
            });
        }).finally(() => oHelper.hideLoading());

    },
    obtenerArticuloPorCodigoBarra: function () {
        let txtCodBar = document.getElementById('txtCodBar');
        if (txtCodBar.value == "")
            oAlerta.alerta({
                title: "Debe de ingresar el código de barra",
                type: "warning"
            })

        oHelper.showLoading();
        axios.get(`/Articulo/GetByBarcode/${txtCodBar.value}`).then((response) => {
            let result = response.data;
            if (!result.Resultado)
                return;

            let articulo = result.Data;
            let modelo = {
                idArticulo: articulo.IdArticulo,
                codigo: articulo.Codigo,
                descripcion: articulo.NomArticulo,
                jsonListaUm: articulo.ListaUm
            };

            //Agregamos el artículo encontrado al detalle.
            oCompra.agregarArticulo(modelo).then(() => {
                document.getElementById('btnLimCodBar').click();
            }).catch(error => {
                oAlerta.alerta({
                    title: error,
                    type: "warning",
                    closeAutomatic: true
                });
                document.getElementById('btnLimCodBar').click();
            });
        }).catch((error) => {
            oAlerta.alerta({
                title: error.response.data.Message,
                type: "warning"
            });
        }).finally(() => oHelper.hideLoading());
    },
    agregarArticulo: function (modelo) {
        let table = document.getElementById('tblDetalle');
        let tbody = table.getElementsByTagName('tbody')[0];
        let tr = document.createElement('tr');

        return new Promise((resolve, reject) => {
            //Validamos que aún no se haya ingresado.
            let error = '';
            let arrId = tbody.querySelectorAll('tr td:nth-child(10)');
            let count = arrId.length;
            if (count > 0) {
                for (var i = 0; i < count; i++) {
                    if (arrId[i].textContent == modelo.idArticulo) {
                        error = 'Artículo ya ingresado.';
                        break;
                    }
                }
            }
            if (error != '')
                return reject(error);

            //Construímos las UM del articulo.
            let option = '<option value=-1>---Seleccione---</option>';
            modelo.jsonListaUm.forEach(x => {
                option += `<option value="${x.IdUm}" data-nroFactor="${x.NroFactor}">${x.NomUm}</option>`
            });

            let cboUm = `<select class="form-select form-select-sm">${option}</select>`;
            let txtPrecio = '<input class="form-control form-control-sm text-end" type="text" style="width:80px"/>';
            let txtCantidad = '<input class="form-control form-control-sm text-end" type="text" value=1 style="width:80px"/>';
            let txtDescuento = '<input class="form-control form-control-sm text-end" type="number" min="0" max="100" step="5" style="width:80px"/>';
            let txtImporte = '<input class="form-control form-control-sm text-end" type="text" style="width:100px"/>';

            let td = `<td>${modelo.codigo}</td>
                    <td>${modelo.descripcion.toString().capitalizeAll()}</td>
                    <td>${cboUm}</td>
                    <td class='text-end'></td>
                    <td class='text-end'>${txtPrecio}</td>
                    <td>${txtCantidad}</td>
                    <td>${txtDescuento}</td>
                    <td>${txtImporte}</td>
                    <td><button type="button" class="btn btn-danger btn-sm" ><i class="bi bi-trash-fill"></i></button></td>
                    <td class='d-none'>${modelo.idArticulo}</td>`;

            tr.innerHTML = td;
            tbody.appendChild(tr);

            //Todo OK
            resolve();
        });

    },
    inicializarDatosProveedor: function (cbo) {
        oCompra.limpiarProveedor(false);

        let txtNumDoc = document.getElementById('txtNumDoc');
        if (cbo.value == "-1") {
            txtNumDoc.disabled = true;
            document.getElementById('btnBusPorNum').disabled = true;
        } else {
            document.getElementById('btnBusPorNum').disabled = false;
            txtNumDoc.disabled = false;
            txtNumDoc.maxLength = cbo.options[cbo.selectedIndex].getAttribute("data-maxDigitos");
            txtNumDoc.focus();
        }
    },
    limpiarProveedor: function (bLimpiarTipDoc) {
        if (!!bLimpiarTipDoc)
            document.getElementById('cboTipDoc').value = "-1";

        document.getElementById('hddIdPro').value = "";
        document.getElementById('txtNumDoc').value = "";
        document.getElementById('txtRazSoc').value = "";
    },
    seleccionarFormaPago: function (e) {
        let cbo = e.target;
        let flgEvaluaCredito = cbo.options[cbo.selectedIndex].getAttribute("data-flgEvaluaCredito") == "True" ? true : false;
        if (flgEvaluaCredito) {
            if (document.getElementById('lblTotal').textContent == 0) {
                oAlerta.alerta({
                    title: "Debe de existir un monto total de la compra para realizar el abono",
                    type: "warning",
                    closeAutomatic: true
                });
                document.getElementById('cboForPag').value = "-1";
                return;
            }; txtFecVen

            let fechaVencimiento = document.getElementById('txtFecVen');
            if (fechaVencimiento.value == "") {
                oAlerta.alerta({
                    title: "Debe de seleccionar la fecha de vencimiento si la forma de pago es a crédito",
                    type: "warning"
                });
                document.getElementById('cboForPag').value = "-1";
                return;
            } else {
                let fechaActual = moment(new Date()).format("DD/MM/YYYY");
                fechaActual = moment(fechaActual, "DD/MM/YYYY");
                //Si la fecha es menor a la fecha actual retorna false
                if (moment(fechaVencimiento.value, "DD/MM/YYYY").isBefore(fechaActual)) {
                    oAlerta.alerta({
                        title: "La fecha de vencimiento no puede ser menor a la fecha actual.",
                        type: "warning"
                    });
                    document.getElementById('cboForPag').value = "-1";
                    return;
                }
            };

            let modelo = {
                total: document.getElementById('lblTotPag').textContent,
                abono: 0,
                saldo: 0,
                fechaEmision: document.getElementById('txtFecEmi').value,
                fechaVencimiento: document.getElementById('txtFecVen').value
            }
            let flgEditar = false;

            oModalAbono.show(modelo, flgEditar).then(response => {
                oCompra.bindingAbono(response);
            }).catch((editar) => {
                if (!editar) {
                    oCompra.limpiarAbono();
                    document.getElementById('cboForPag').value = "-1";
                }
            });
        } else {
            oCompra.limpiarAbono();
        }
    },
    bindingAbono: function (modelo) {
        let sgnMoneda = oCompra.getSgnMoneda();

        oCompra.modeloAbono = {
            total: document.getElementById('lblTotPag').textContent,
            abono: modelo.abono,
            saldo: modelo.saldo,
            fechaCancelacion: modelo.fechaCancelacion
        };

        document.getElementById('txtAbono').value = oHelper.formatoMoneda(sgnMoneda, modelo.abono, 2);
        document.getElementById('txtSaldo').value = oHelper.formatoMoneda(sgnMoneda, modelo.saldo, 2);
        document.getElementById('panelAbono').style.display = "block";
    },
    limpiarAbono: function () {
        document.getElementById('panelAbono').style.display = "none";
        document.getElementById('txtAbono').value = "";
        document.getElementById('txtSaldo').value = "";
        oCompra.modeloAbono = null;
    }
}

document.addEventListener('DOMContentLoaded', oCompra.init);