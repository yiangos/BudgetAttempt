function Budget($scope, $filter, $window, $http, baseurl, locale) {
    $scope.newline = {};
    $scope.line = {};
    $scope.cat = {};
    moment.locale(locale);
    $scope.linedate = moment();
    $scope.line.Date = $scope.linedate.format('YYYY-MM-DD');
    $scope.line.TransactionType = "";
    $scope.line.Person = "";
    $scope.StartDate = moment();
    $scope.MinDate = moment().add(-1, 'year');
    $scope.MaxDate = moment();
    $scope.LatestCount=10;

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
    $scope.addCategory = function (cat) {
        if ($scope.ctgfrm.$valid)
        {

            $http.post(baseurl + "/Budget.svc/json/categories/create",
                $scope.cat,
                 { headers: { 'Content-Type': 'application/json' } })
            .then(function (resp) {
                $scope.ctgfrm.$setPristine();
                $scope.ctgfrm.$setUntouched();
                $scope.cat = {};
                $http.get(baseurl + "/Budget.svc/json/categories/all").then(function (resp) { $scope.categories = resp.data; });
           });
        }
    };
    $scope.init();
}
