//(function () {
//    'use strict';

//    angular
//        .module('app')
//        .controller('sweetalert2', sweetalert2);

//    sweetalert2.$inject = ['$location'];

//    function sweetalert2($location) {
//        /* jshint validthis:true */
//        var vm = this;
//        vm.title = 'sweetalert2';

//        activate();

//        function activate() { }
//    }
//})();

window.SweetAlert = {
    showAlert: (title, message, icon) => {
        Swal.fire(title, message, icon);
    },
    showConfirm: (title, message, icon, confirmButtonText) => {
        return Swal.fire({
            title: title,
            text: message,
            icon: icon,
            showCancelButton: true,
            confirmButtonText: confirmButtonText
        }).then((result) => {
            return result.isConfirmed;
        });
    }
};