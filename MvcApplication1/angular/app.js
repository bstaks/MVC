var app = angular.module('myApp', ['ui.bootstrap', 'ngRoute'], function ($locationProvider) {

    $locationProvider.html5Mode(true);
});

app.service('Common', function ($http) {
    var CommonAPI = {};
    CommonAPI.postData = function (urlName, parameter) {
       // alert(urlName);
        return $http({
           
            url: urlName,
            method: 'post',
            data: parameter
        });
    };

    return CommonAPI;
    
})

app.filter("jsDate", function () {
    return function (x) {
        return new Date(parseInt(x.substr(6)));
    };
});