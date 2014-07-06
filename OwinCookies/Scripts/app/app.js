(function () {
    var app = angular.module('app', []);

    app.config(['$httpProvider', function ($httpProvider) {
        // angular's $http service does not set this header by default
        $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
    }]);

    app.constant('toastr', toastr);

    app.controller('HomeController', ['$scope', '$http', 'toastr', function ($scope, $http, toastr) {
        $scope.showData = false;

        $scope.load = function () {
            $http.get('/api/secure/data')
                .then(function (resp) {
                    $scope.data = resp.data;
                    $scope.showData = true;

                    toastr.success("Data Retrieved!!");
                }, function (resp) {
                    toastr.error(resp.data.Message, resp.status + ' ' + resp.statusText);
                    $scope.showData = false;
                });
        };
    }]);
}());