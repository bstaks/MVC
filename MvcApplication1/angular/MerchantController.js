app.controller('MerchantController', function ($scope, $http) {
    $scope.data = [];
    $scope.GetMerchant = function () {
        $http({
            url: urlName,
            method: 'Post',
            params: { 'MerchantName': $scope.MerchantName }
        }).then(function success(response) {
            $scope.data = response.data;
        }, function error() {
        });
    }
})