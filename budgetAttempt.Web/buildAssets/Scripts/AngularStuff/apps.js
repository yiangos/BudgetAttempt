var budgetApp = angular.module('budgetApp', ['moment-picker']);
if (location.hostname === "localhost" || location.hostname === "127.0.0.1")
{
    budgetApp.value("baseurl", "http://localhost:64432");
}
else
{
    budgetApp.value("baseurl", "//"+location.hostname);
}
budgetApp.value("locale", "el");

budgetApp.directive('price',price);
budgetApp.directive('transactionInput',transactionInput);
budgetApp.directive('transactionLatest',transactionLatest);
budgetApp.directive('categoryInput', categoryInput);
budgetApp.directive('monthTransactions', monthTransactions);
budgetApp.controller('budgetcontroller', ['$scope', '$filter', '$window', '$http','baseurl', Budget]);

budgetApp.controller('navigationcontroller', ['$scope', function($scope){}]);

