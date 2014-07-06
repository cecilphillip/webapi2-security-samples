(function () {

    var app = angular.module('app', []);
    app.constant('toastr', toastr);
    app.controller('HomeController', ['$scope', '$http', 'toastr', function ($scope, $http, toastr) {


        $scope.load = function () {
            $http.get('http://localhost:2524/api/cors/data', { headers: { 'X-HEADER': 'CUSTOM' } }) // use with OwinCors project
            //$http.get('http://localhost:3867/api/cors/data', { headers: { 'X-HEADER': 'CUSTOM' } })   // use with Web API Cors project
                .then(function (resp) {


                    toastr.success("Data Retrieved!!");
                }, function (resp) {
                    toastr.error('Failed to make cross domain request');

                });
        };
    }]);
}());