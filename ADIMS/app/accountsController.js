(function () {
    'use strict';

    angular
        .module('loginApp')
        .controller('accountsController', accountsController);

    accountsController.$inject = ['$scope'];

    function accountsController($scope) {
        
    }
    //login controller
    angular
        .module('loginApp')
        .controller('loginController', loginController);

    loginController.$inject = ['$scope','Users'];

    function loginController($scope,Users) {

        $scope.user = {};
        
        $scope.sendForm = function () {
            var a = Users.login(JSON.stringify($scope.user), $scope.antiForgeryToken);
            console.log($scope.user);
            console.log($scope.antiForgeryToken);
            console.log(a);
        }
    }

    //register controller
    angular
        .module('loginApp')
        .controller('registerContoller', registerController);

    registerController.$inject = ['$scope'];

    function registerController($scope) {

    }

    //forget password controller
    angular
        .module('loginApp')
        .controller('forgotPassword', forgotPassword);

    forgotPassword.$inject = ['$scope'];

    function forgotPassword($scope) {

    }
})();
