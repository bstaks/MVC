app.controller('AdminController', function ($scope, Common) {
    $scope.roles = [];
    $scope.isShow = true;
    $scope.error = false;
    // $scope.txtRoleName = '';
    //debugger;


    $scope.SearchUsers = function (urlName) {
        $scope.error = false;
        if ($scope.frm1.txtUserName.$invalid) {
            $scope.error = true;
            return;
        }
        var postValue = { 'userName': $scope.txtUserName, 'commanName': 'SearchUsers' };
        $scope.role = Common.postData(urlName, postValue).then(function (response) {
           // alert(response);
            $scope.roles = response;
            $scope.isShow = response.data.length > 0;
            console.log(response);
        }, function (response) { })
    }



    $scope.SearchRole = function (url) {
        $scope.error = false;
        if ($scope.frm1.txtRoleName.$invalid) {
            $scope.error = true;
            return;
           
        };

        var postValue = { 'UserName': $scope.txtRoleName, 'commanName': 'SearchUsers' };
        $scope.role = Common.postData(url, postValue).then(function (response) {
            $scope.roles = response;
            $scope.isShow = response.data.length > 0;
            console.log(response);
        }, function (response) { })
    };
    $scope.headerCheck = true;

    $scope.onSelect = function () {
        var isChecked = $(".header").prop("checked");

        isChecked ? $(".ace").prop("checked", true) : $(".ace").prop("checked", false);
    }

    $scope.ChildCheckbox = function () {
        var size = $(".ace[type='checkbox']").size();
        for (var i = 0; i < size; i++) {

            if ($(".ace[type='checkbox']")[i].checked == false) {
                $(".header").prop("checked", false);
                break;
            }
            $(".header").prop("checked", true);

        }
    }
     
     $scope.viewRoles = [{
        "MenuName": "Customer", "ParentMenuId": 0, "MenuType": 201, "MenuPermission": 8, "MenuId": 1,
        "ChildMenu": [{ "MenuName": "ViewCustomer", "MenuType": 202, "ParentMenuId": 1, "MenuId": 2, "MenuPermission": 7 },
        { "MenuName": "EditCustomer", "MenuType": 202, "ParentMenuId": 1, "MenuId": 3, "MenuPermission": 1 }]
    }]

    try {
        $scope.viewRoelsDisabled = isViewAndEdit;
        $scope.menuList = menuList1;
    } catch (e) {

    }

});

app.filter('ContentLength', function () {
    return function (value) {
        console.log(value)
        if(value != null)
        if (value.length > 10) {
            return value.substring(0,9) + '..'
        }
        return value;
    }
})


app.filter('ParentMenu', function () {
    return function (value, value2) {
        var newArray =[]
        angular.forEach(value, function (value) {
            if(value.MenuType == value2)
            {
                newArray.push(value);
            }
        })
        console.log(newArray);
        return newArray;
    }

})

app.filter('ChildMenu', function () {
    return function (value, value2,value4) {
        var newArray = []
        angular.forEach(value, function (value) {
            if (value.MenuType == value2  && value.ParentMenuId == value4) {
                newArray.push(value);
            }
        })
       
        return newArray;
    }

})

app.filter('ByParentId', function () {
    return function (value, value2) {
        var newArray = []
        angular.forEach(value, function (value) {
            if (value.ParentMenuId == value2) {
                newArray.push(value);
            }
        })
       
        return newArray;
    }

})



app.filter('Bitwise', function () {
    return function (value, value2) {
       
        var d = value & value2;
        //alert(d);
        console.log("d ="+d);
        return (d == value2) ?true:false;
    }
})

app.directive('roleDetails', function () {
    return {
        templateUrl: '/template/Admin/RoleDetails.html',
        restrict: 'EA',
        controller: 'AdminController'
    };
})

app.controller('CustomerController', function ($scope, $http, $location) {
    $scope.data = [];

    $scope.sortReverse = false;
    $scope.sortType = '';
    $scope.totalPage = 0;
    $scope.itemPerPage = 10;
    $scope.currentPage = 1;
    $scope.filteredFriends = [];
    $scope.GetCustomer = function GetCustomer(urlName) {
        var customerName = document.getElementById('txtCustomerName').value;
        var config = {
            "customerName": customerName
        };
        //$http.get(urlName, config).then(function (response) {
        //    $scope.data = response.data;
        //}, function (err) { });
        $http({ url: urlName, params: { "customerName": customerName }, method: 'get' }).then(function (response) {
            $scope.data = response.data;
            $scope.totalPage = $scope.data.length;
            $scope.filteredFriends.push($scope.data);
        }, function (err) { });

        //$.ajax({
        //    url:urlName,

        //    method: 'get',
        //    data: { "customerName": customerName },
        //    success: function (response) {
        //        $scope.data = response;
        //    },
        //    error: function (err) {
        //        alert(err);
        //    }
        //})
    }

    $scope.$watch('currentPage + itemsPerPage', function () {
        var begin = (($scope.currentPage - 1) * $scope.itemsPerPage),
          end = begin + $scope.itemsPerPage;

        //  $scope.data = $scope.filteredFriends.slice(begin, end);
        // alert($scope.data);
    });
    // link function call method
    $scope.GetCustomerDetails = function () {
        // alert($location.url())

        $http({ url: url, method: 'GET', params: { "CusotmerId": id } }).then(function success(response) {
            $scope.CustomerDetails = [];
            $scope.CustomerDetails = response;
            //  alert($scope.CustomerDetails);
        }, function error() { })
        $scope.CustomerName = id;
        //alert($scope.CustomerName);

    }
});
app.controller('TodoController', function ($scope, $http) {
    $scope.filteredTodos = []
   , $scope.currentPage = 1
   , $scope.numPerPage = 10
   , $scope.maxSize = 5,
    $scope.data = [];

    $scope.makeTodos = function () {
        $scope.todos = [];
        for (i = 1; i <= 1000; i++) {
            $scope.todos.push({ text: 'todo ' + i, done: false });
        }
    };

    $scope.RedirectUrl = function (urlName) {
        document.location.href = urlName;
    }

    $scope.GetCustomer = function (urlName) {

        var customerName = document.getElementById('txtCustomerName').value;
        var config = {
            "customerName": customerName
        };
        //$http.get(urlName, config).then(function (response) {
        //    $scope.data = response.data;
        //}, function (err) { });
        $http({ url: urlName, params: { "customerName": customerName }, method: 'get' }).then(function (response) {
            $scope.data = response.data;
            $scope.totalPage = $scope.data.length;
            //$scope.filteredFriends.push($scope.data);
            $scope.$watch('currentPage + numPerPage', function () {
                var begin = (($scope.currentPage - 1) * $scope.numPerPage)
                , end = begin + $scope.numPerPage;

                $scope.filteredTodos = $scope.data.slice(begin, end);
            });
        }, function (err) { });
    }


    // $scope.makeTodos();


});



app.controller('paginationController', function ($scope, $controller) {

    $controller("AdminController", { $scope: $scope })
    $scope.pageSize = 10;
    $scope.TotalData = 300;
    $scope.pageIndex = 1;


    $scope.GetPager = function GetPagination(totalItem, currentPage, pageSize) {
        currentPage = currentPage || 1;
        var totalPages = Math.ceil(totalItem / pageSize);
        var startPage, endPage;
        if (totalPages <= 10) {
            startPage = 1;
            endPage = totalItem;
        } else {
            if (totalPages <= 6) {
                startPage = 1;
                endPage = 10;
            }
            else if (currentPage + 4 >= totalPages) {
                startPage = totalPages - 9;
                endPage = totalPages;
            } else {
                startPage = currentPage - 5;
                endPage = currentPage = 4;
            }
        }
        var startIndex = (currentPage - 1) * pageSize;
        var endIndex = startIndex + pageSize;
        var pages = _.range(startIndex, endPage + 1);

        var vm = {};
        $scope.SetPage = function SetPage(pageNumber) {
            $scope.items = $scope.$scope.data;

            if (pageNumber < 1) {
                return;
            }
            vm.pager = $scope.GetPager(item.length, pageNumber);
            //vm.itemsSlice = items.slice()


        }
    }
})

app.directive('customerDetails', function () {
    return {
        templateUrl: '/Template/Customer/ViewCustomer.html',
        restrict: 'EA',
        controller: 'CustomerController',
        link: function ($scope, $location) {
            $scope.GetCustomerDetails();
        },
        transclude: true
    };
})