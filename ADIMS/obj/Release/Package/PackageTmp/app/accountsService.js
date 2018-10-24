(function () {
    'use strict';

    angular
        .module('loginApp')
        .factory('Users', accountsService);

    accountsService.$inject = ['$http'];

    function accountsService($http) {
        var service = {
            login: login
        };

        return service;

        function login(model, token) {

            $http.post('/account/login', function () { })

        }
    }
})();