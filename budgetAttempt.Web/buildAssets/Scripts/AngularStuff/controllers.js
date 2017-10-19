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
    $scope.LatestCount = 10;

    //Month Flow
    $scope.flowMonth = moment();
    $scope.startFlow = moment();
    $scope.minFlow = moment().subtract(5, 'year');
    $scope.maxFlow = moment();
    $scope.monthTransactions = [];
    $scope.monthIncome = 0;
    $scope.monthExpense = 0;
    $scope.onFlowChange = function (newValue, oldValue) {$scope.getMonthTransactions();};
    $scope.getMonthTransactions = function () {
        $http.post(baseurl + "/Budget.svc/json/transactions/month", $scope.flowMonth.format('YYYY-MM'), { headers: { 'Content-Type': 'application/json', 'content': 'application/json' } }).then(function (resp) {
            $scope.monthTransactions = resp.data;
            $scope.monthIncome= resp.data.map(function (x) {
                if (x.TransactionType == 'Income')
                {
                    return x.Amount;
                }
                return 0;
            }).reduce(function (accumulator, currentValue) {
                return accumulator + currentValue;
            });
            $scope.monthExpense = resp.data.map(function (x) {
                if (x.TransactionType == 'Expense') {
                    return x.Amount;
                }
                return 0;
            }).reduce(function (accumulator, currentValue) {
                return accumulator + currentValue;
            });
        });
    }


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
        //$scope.getMonthTransactions($scope.flowMonth);
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
