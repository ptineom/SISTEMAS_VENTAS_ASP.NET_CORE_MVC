'use strict'
var oMain = {
    init: function () {
        axios.interceptors.request.use(function (config) {
            return config;
        }, function (error) {
            return Promise.reject(error);
        });
    }
}

document.addEventListener("DOMContentLoaded", () => {
    oMain.init();
})