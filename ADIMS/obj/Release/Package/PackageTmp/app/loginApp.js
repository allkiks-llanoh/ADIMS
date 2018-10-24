(function () {
    'use strict';

    angular.module('loginApp', [
        // Angular modules 
        'ngRoute'

        // Custom modules 

        // 3rd Party Modules

    ]);

    //configure routes
    angular.module('loginApp').config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {

        $routeProvider
            .when('/login', {
                templateUrl: '../templates/login',
                controller: 'loginController'
            })

            .when('Account/TestLogin', {
                templateUrl: 'templates/login',
                controller: 'loginController'
            })

            .when('register', {
                templateUrl: '../templates/register',
                controller: 'registerController'
            })

            .when('forgot-password', {
                templateUrl: '../templates/forgotpassword',
                controller: 'forgotController'
            });
        
    }]);
})(angular);