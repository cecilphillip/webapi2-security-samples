(function() {

    var app = angular.module('app', []);
    app.constant('toastr', toastr);
    app.controller('HomeController', ['$scope', '$http', 'toastr', function ($scope, $http, toastr) {
     

        $scope.load = function () {
            $http.get('/api/cors/data')
                .then(function (resp) {
                 

                    toastr.success("Data Retrieved!!");
                }, function (resp) {
                    toastr.error(resp.data.Message, resp.status + ' ' + resp.statusText);
                 
                });
        };
    }]);
}());