var price = function () {
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
                else if (typeof (str) === "string") {
                    var rxth = new RegExp("\\" + thou, "g");
                    var rxdc = new RegExp("\\" + dec, "g");
                    str = String(Number(str.replace(rxth, "").replace(rxdc, ".")) / (isNum ? 1 : Math.pow(10, mantiss)));
                }
                str = (str === '0' ? '0.0' : str).split('.');
                str[1] = str[1] || '0';
                var retval = str[0].replace(/(\d)(?=(\d\d\d)+(?!\d))/g, '$1'+thou) + dec + (str[1].length < mantiss ? (str[1] + '0').substring(0, mantiss) : str[1]);
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
                    val = str[0] + dec + (str[1].length <mantiss ? (str[1] + '0').substring(0,mantiss) : str[1]);
                }
                return parseNumber(val);
            }

            ngModel.$parsers.push(parseNumber);
            ngModel.$formatters.push(formatNumber);
        }
    };
};
var transactionInput= function () {
    return {
        restrict: 'E', /* restrict this directive to elements */
        templateUrl: "Views/LineInput.html",
        link: function (scope, elem, attrs) {

        }
    };
};
var transactionLatest= function () {
    return {
        restrict: 'E', /* restrict this directive to elements */
        templateUrl: "Views/LatestTransactions.html",
        link: function (scope, elem, attrs) {

        }
    };
};
var categoryInput= function () {
    return {
        restrict: 'E', /* restrict this directive to elements */
        templateUrl: "Views/CategoryInput.html",
        link: function (scope, elem, attrs) {

        }
    };
};

