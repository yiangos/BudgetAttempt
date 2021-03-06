﻿angular.module('budgetApp', ['moment-picker'])
.directive('price', [function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            attrs.$set('ngTrim', "false");
            thou = attrs.thousep || ',';
            dec = attrs.decsep || '.';
            mantiss = Number(attrs.mantiss || 2);
            var formatter = function (str, isNum) {
                if (typeof(str) === "undefined")
                {
                    str = String(Number(0) / (isNum ? 1 : Math.pow(10,mantiss)));
                }
                else if (typeof (str) === "number") {
                    str = String(str / (isNum ? 1 : Math.pow(10, mantiss)));
                }
                else if (typeof(str) === "string") {
                    str = String(Number(str.replace(/\./g, "").replace(/\,/g, ".")) / (isNum ? 1 : Math.pow(10, mantiss)));
                }
                str = (str === '0' ? '0.0' : str).split('.');
                str[1] = str[1] || '0';
                var retval = str[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1.') + ',' + (str[1].length < mantiss ? (str[1] + '0').substring(0, mantiss) : str[1]);
                return retval;
            }
            var updateView = function (val) {
                scope.$applyAsync(function () {
                    ngModel.$setViewValue(val || '');
                    ngModel.$render();
                });
            }
            var parseNumber = function (val) {
                var modelString = formatter(ngModel.$modelValue, true);
                var sign = {
                    pos: /[+]/.test(val),
                    neg: /[-]/.test(val)
                }
                sign.has = sign.pos || sign.neg;
                sign.both = sign.pos && sign.neg;
                var newVal;
                if (!val || sign.has && val.length === 1 || ngModel.$modelValue && Number(val) === 0) {
                    newVal = (!val || ngModel.$modelValue && Number() === 0 ? '' : val);
                    if (ngModel.$modelValue !== newVal)
                        updateView(newVal);

                    return '';
                }
                else {
                    var valString = String(val || '');
                    var newSign = (sign.both && ngModel.$modelValue >= 0 || !sign.both && sign.neg ? '-' : '');
                    newVal = valString.replace(/[^0-9]/g, '');
                    var viewVal = newSign + formatter(angular.copy(newVal));

                    if (modelString !== valString)
                        updateView(viewVal);

                    return (Number(newSign + newVal) / Math.pow(10, mantiss)) || 0;
                }
            }
            var formatNumber = function (val) {
                if (val) {
                    var str = String(val).split('.');
                    str[1] = str[1] || '0';
                    val = str[0] + ',' + (str[1].length <mantiss ? (str[1] + '0').substring(0,mantiss) : str[1]);
                }
                return parseNumber(val);
            }

            ngModel.$parsers.push(parseNumber);
            ngModel.$formatters.push(formatNumber);
        }
    };
}])
.directive("transactionInput", function () {
    return {
        restrict: 'E', /* restrict this directive to elements */
        templateUrl: "/HTML/Views/LineInput.html",
        link: function (scope, elem, attrs) {

        }
    };
})
.directive("transactionLatest", function () {
    return {
        restrict: 'E', /* restrict this directive to elements */
        templateUrl: "/HTML/Views/LatestTransactions.html",
        link: function (scope, elem, attrs) {

        }
    };
})
.directive("categoryInput", function () {
    return {
        restrict: 'E', /* restrict this directive to elements */
        templateUrl: "/HTML/Views/CategoryInput.html",
        link: function (scope, elem, attrs) {

        }
    };
})

.controller('budgetcontroller', ['$scope', '$filter', '$window', '$http', function ($scope, $filter, $window, $http) {
    $scope.newline = {};
    $scope.line = {};
    $scope.linedate = moment();
    $scope.line.Date = $scope.linedate.format('YYYY-MM-DD');
    $scope.line.TransactionType = "";
    $scope.line.Person = "";
    $scope.StartDate = moment();
    $scope.MinDate = moment().add(-1, 'year');
    $scope.MaxDate = moment();
    $scope.LatestCount=15;

    $scope.categories = [];
    $scope.persons = [];
    $scope.latestTransactions=[];
    $scope.getLatestTransactions=function(){
        $http.post(baseurl + "/Budget.svc/json/transactions/latest",$scope.LatestCount,{ headers: { 'Content-Type': 'application/json' } }).then(function (resp) { $scope.latestTransactions = resp.data; });
    };


    $scope.init = function () {
        $http.get(baseurl + "/Budget.svc/json/persons/all").then(function (resp) { $scope.persons = resp.data; });
        $http.get(baseurl + "/Budget.svc/json/categories/all").then(function (resp) { $scope.categories = resp.data; });
        $scope.getLatestTransactions();
    }
    $scope.onDateChange = function (newValue, oldValue) {
        $scope.line.Date = $scope.linedate.format('YYYY-MM-DD');
    };


    $scope.addLine = function (line) {
        if ($scope.linefrm.$valid)
        {

            $http.post(baseurl + "/Budget.svc/json/transactions/add",
                $scope.line,
                 { headers: { 'Content-Type': 'application/json' } })
            .then(function (resp) {
                $scope.linefrm.$setPristine();
                $scope.linefrm.$setUntouched();
                $scope.line = {};
                $scope.linedate = moment();
                $scope.line.Date = $scope.linedate.format('YYYY-MM-DD');
                $scope.line.TransactionType = "";
                $scope.line.Person = "";
                $scope.getLatestTransactions();
           });
        }
    };
    $scope.init();
}]);


