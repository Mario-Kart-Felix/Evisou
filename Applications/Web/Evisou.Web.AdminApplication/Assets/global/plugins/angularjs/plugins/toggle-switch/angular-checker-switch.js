// JavaScript source code
(function () {
    var module = angular.module('toggle-checker', ['ng']);

    module.provider('checkerSwitchConfig', [function () {
        var self = this;
        this.$get = function () {
            return {};
        };
    }]);

    module.directive('checkerSwitch', ['checkerSwitchConfig', function (checkerSwitchConfig) {
        return {
            restrict: 'EA',
            replace: true,
            require: 'ngModel',
            scope: {

            },
            template: '<div class="checker">'+
                        '<span ng-class="{\'\': !model, \'checked\': model}" >' +
                        
                        '</span>' +
                       '</div>',
            compile: function (element, attrs) {
                return this.link;
            },
            link: function (scope, element, attrs, ngModelCtrl) {
                var KEY_SPACE = 32;
                element.on('click', function () {
                    scope.$apply(scope.toggle);
                });
                element.on('keydown', function (e) {
                    var key = e.which ? e.which : e.keyCode;
                    if (key === KEY_SPACE) {
                        scope.$apply(scope.toggle);
                        $event.preventDefault();
                    }
                });

                ngModelCtrl.$formatters.push(function (modelValue) {
                    return modelValue;
                });

                ngModelCtrl.$parsers.push(function (viewValue) {
                    return viewValue;
                });

                ngModelCtrl.$viewChangeListeners.push(function () {
                    scope.$eval(attrs.ngChange);
                });

                ngModelCtrl.$render = function () {
                    scope.model = ngModelCtrl.$viewValue;
                };
                scope.toggle = function toggle() {
                    if (!scope.disabled) {
                        scope.model = !scope.model;
                        ngModelCtrl.$setViewValue(scope.model);
                    }
                };
            }
        };
    }])
})();