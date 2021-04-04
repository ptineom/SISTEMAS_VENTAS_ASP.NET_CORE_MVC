//$(document).ready(function () {
//    ///* Ignore validation on some elements. */
//    //var ignores = $.validator.defaults.ignore.replace(/(^|\b)(:hidden)($|\b)/gi, ':hidden:not(.val-force)').split(',');
//    //ignores.push('.val-ignore');
//    //$.validator.setDefaults({ ignore: ignores.join(',') });
//    //debugger;
//    //jQuery.validator.addMethod("country",
//    //    function (value, element, param) {
//    //        debugger;
//    //        if (value != "USA" && value != "UK" && value != "India") {
//    //            return false;
//    //        }
//    //        else {
//    //            return true;
//    //        }
//    //    });

//    //jQuery.validator.unobtrusive.adapters.addBool("country");



//})
//    //debugger;


//$.validator.addMethod('must-be-true', function (value, element, params) {
//    debugger;
//    return element.checked;
//});

//$.validator.unobtrusive.adapters.add('must-be-true', [], function (options) {
//    debugger;
//    options.rules['must-be-true'] = {};
//    options.messages['must-be-true'] = options.message;
//});

//jQuery.validator.addMethod("country",
//    function (value, element, param) {
//        if (value != "USA" && value != "UK" && value != "India") {
//            return false;
//        }
//        else {
//            return true;
//        }
//    });

//jQuery.validator.unobtrusive.adapters.addBool("country");