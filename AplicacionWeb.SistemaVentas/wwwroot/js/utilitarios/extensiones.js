"use strict"
String.prototype.isnullOrEmpty = function () {
    if (this == null) {
        return true; 
    };
    if (this.trim().length == 0) {
        return true;
    };
    return false;
}
String.prototype.revertir = function () {
    return this.split("").reverse().join("");
}
String.prototype.capitalizeAll = function () {
    let arr = this.split(' ');
    let count = arr.length;
    let resultado = '';
    for (var i = 0; i < count; i++) {
        let palabra = arr[i].toLowerCase();
        let resultadoPalabra = (palabra.substring(0, 1).toUpperCase() + palabra.substring(1, palabra.length));
        resultado += i == 0 ? resultadoPalabra : (' ' + resultadoPalabra);
    }
    return resultado;
}
Array.prototype.max = function () {
    let max = Math.max.apply(null, this);
    return max == -Infinity? 0: max;
};

Array.prototype.min = function () {
    let min = Math.min.apply(null, this);
    return min == +Infinity ? 0 : min;
};