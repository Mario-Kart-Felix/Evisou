angular.module('EvisouApp').controller('DashboardController', function($rootScope, $scope, $http, $timeout) {
    $scope.$on('$viewContentLoaded', function () {
        //$http.get("http://localhost:46088/api/account/auth").success(function (data) {
        //    $scope.menugroup = data.AdminMenuGroups;
        //    // $('ul.page-sidebar-menu>li:eq(1)').addClass("active");
        //});
        // initialize core components
        App.initAjax();

    });

    // set sidebar closed and body solid layout mode
    $rootScope.settings.layout.pageContentWhite = true;
    $rootScope.settings.layout.pageBodySolid = false;
    $rootScope.settings.layout.pageSidebarClosed = false;
});