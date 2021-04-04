var oMovie = {
    init: function () {
        //Debe ser marcado
        $.validator.addMethod('debe-ser-marcado', function (value, element, params) {
            return element.checked;
        });

        $.validator.unobtrusive.adapters.add('debe-ser-marcado', [], function (options) {
            options.rules['debe-ser-marcado'] = {};
            options.messages['debe-ser-marcado'] = options.message;
        });

        //Solo algunos paises
        $.validator.addMethod('solo-algunos-paises', function (value, element, params) {
            value = value.toUpperCase()
            if (value == "PERU" || value == "CHILE" || value == "CUBA") {
                return true;
            }
            else {
                return false;
            }
        });

        $.validator.unobtrusive.adapters.add('solo-algunos-paises', [], function (options) {
            options.rules['solo-algunos-paises'] = {};
            options.messages['solo-algunos-paises'] = options.message;
        });

        //Documento segun edad
        $.validator.addMethod('documento-segun-edad', function (value, element, params) {
            var edad = params[0].value
            var edadEvaluar = params[1], documento = value.toUpperCase();

            if (edad >= edadEvaluar && documento != "RUC")
                return false;

            return true;
        });

        $.validator.unobtrusive.adapters.add('documento-segun-edad', ['edadevaluar'], function (options) {
            //La edad que ingrese el usuario en este elemento
            let element = options.form.querySelector('input#Age'); 
            //Parámetro que se vealuará con la edad ingresada en el elemento
            let edadEvaluar = parseInt(options.params['edadevaluar']);

            options.rules['documento-segun-edad'] = [element, edadEvaluar];
            options.messages['documento-segun-edad'] = options.message;
        });

        document.getElementById('btnGuardar').addEventListener('click', () => {
            if ($("#form-contact").valid()) {
                let Username = document.getElementById('Username');
                let AcceptedPrivacyPolicy = document.getElementById('AcceptedPrivacyPolicy');
                let Age = document.getElementById('Age');
                let Document = document.getElementById('Document');
                let Note = document.getElementById('Note');
                let Country = document.getElementById('Country');

                let parameters = {
                    Username: Username.value,
                    AcceptedPrivacyPolicy: AcceptedPrivacyPolicy.checked,
                    Age: Age.value,
                    Document: Document.value,
                    Note: Note.value,
                    Country: Country.value
                };

                axios.post("/Movie/Register", parameters).then((response) => {
                    alert(response.data.Message);
                }).catch((error) => {
                    oAlerta.alerta({
                        title: error.response.data.Message,
                        type: "warning"
                    });
                }).finally(() => { });
            }
        })
    }
}

document.addEventListener('DOMContentLoaded', oMovie.init);