/// <reference path="area/controllers/account/user/table-ajax.js" />
/// <reference path="area/controllers/account/user/table-ajax.js" />
/***
Metronic AngularJS App Main Script
***/

/* Metronic App */
var EvisouApp = angular.module("EvisouApp", [
    "ui.router",
    "ui.bootstrap", 
    "oc.lazyLoad",
    //'ngResource',
    'toggle-switch',
    'toggle-checker',
    'blockUI',
    "ngSanitize"
    //'cgBusy',
    //'datatables',
   // 'datatables.bootstrap',
    

]); 

/* Configure ocLazyLoader(refer: https://github.com/ocombe/ocLazyLoad) */
EvisouApp.config(['$ocLazyLoadProvider', function($ocLazyLoadProvider) {
    $ocLazyLoadProvider.config({
        // global configs go here
    });
}]);


EvisouApp.config(function (blockUIConfig) {
    blockUIConfig.autoBlock = false;
    // Change the default overlay message
    blockUIConfig.message = '载入中...';

    // Change the default delay to 100ms before the blocking is visible
    blockUIConfig.delay = 0;

    //blockUIConfig.cssClass = 'block-ui page-loading';

    blockUIConfig.templateUrl = 'block-ui-overlay.html';


});


EvisouApp.config(function ($httpProvider) {
    $httpProvider.defaults.headers.common['X-Requested-With'] = 'XMLHttpRequest';
    $httpProvider.defaults.withCredentials = true;
});
/********************************************
 BEGIN: BREAKING CHANGE in AngularJS v1.3.x:
*********************************************/
/**
`$controller` will no longer look for controllers on `window`.
The old behavior of looking on `window` for controllers was originally intended
for use in examples, demos, and toy apps. We found that allowing global controller
functions encouraged poor practices, so we resolved to disable this behavior by
default.

To migrate, register your controllers with modules rather than exposing them
as globals:

Before:

```javascript
function MyController() {
  // ...
}
```

After:

```javascript
angular.module('myApp', []).controller('MyController', [function() {
  // ...
}]);

Although it's not recommended, you can re-enable the old behavior like this:

```javascript
angular.module('myModule').config(['$controllerProvider', function($controllerProvider) {
  // this option might be handy for migrating old apps, but please don't use it
  // in new ones!
  $controllerProvider.allowGlobals();
}]);
**/

//AngularJS v1.3.x workaround for old style controller declarition in HTML
EvisouApp.config(['$controllerProvider', function($controllerProvider) {
  // this option might be handy for migrating old apps, but please don't use it
  // in new ones!
  $controllerProvider.allowGlobals();
}]);

/********************************************
 END: BREAKING CHANGE in AngularJS v1.3.x:
*********************************************/

/* Setup global settings */
EvisouApp.factory('settings', ['$rootScope', '$http', function ($rootScope, $http) {
    // supported languages
    var settings = {
        layout: {
            pageSidebarClosed: false, // sidebar menu state
            pageContentWhite: true, // set page content layout
            pageBodySolid: false, // solid body color state
            pageAutoScrollOnLoad: 1000 // auto scroll to top on page load
        },
        assetsPath: '../assets',
        globalPath: '../assets/global',
        layoutPath: '../assets/layouts/layout',
    };

    $rootScope.settings = settings;

    //Load Sidebar Menu
    $http.get("/api/account/auth").success(function (data) {
        $rootScope.menugroup = data.AdminMenuGroups;
    });

    return settings;
}]);

/*Setup Factory*/
EvisouApp.factory('MyDate', function () {
    var myDate = new Object();
    myDate.time = function () { return new Date; };
    return myDate;
});

/*Setup Service*/
EvisouApp.service('DateService', function (MyDate) {
    this.time = function () {
        //console.log(MyDate);
        return MyDate.time();
    }
    
});

//EvisouApp.service('ajaxService', ['$http', 'blockUI', function ($http, blockUI) {
EvisouApp.service('ajaxService', ['$http', 'blockUI', function ($http,blockUI) {

    this.AjaxPost = function (data, route, successFunction, errorFunction) {
       // blockUI.start();
        setTimeout(function () {
            $http.post(route, data).success(function (response, status, headers, config) {
                //blockUI.stop();
                successFunction(response, status);
            }).error(function (response) {
               // blockUI.stop();
                if (response.IsAuthenicated == false) {
                    //window.location = "/index.html";
                }
                errorFunction(response);
            });
        }, 0);

    }

    this.AjaxPostWithNoAuthenication = function (data, route, successFunction, errorFunction) {
       // blockUI.start();
        setTimeout(function () {
            $http.post(route, data).success(function (response, status, headers, config) {
               // blockUI.stop();
                successFunction(response, status);
            }).error(function (response) {
              //  blockUI.stop();
                errorFunction(response);
            });
        }, 0);

    }

    this.AjaxGet = function (route, successFunction, errorFunction) {
      //  blockUI.start();
        setTimeout(function () {
            $http({ method: 'GET', url: route }).success(function (response, status, headers, config) {
               // blockUI.stop();
                successFunction(response, status);
            }).error(function (response) {
               // blockUI.stop();
                if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(response);
            });
        }, 0);

    }

    this.AjaxGetWithData = function (data, route, successFunction, errorFunction) {
        //var BlockUI = blockUI.instances.get('BlockUI');
        blockUI.start();
        setTimeout(function () {
            $http({ method: 'GET', url: route, params: data }).success(function (response, status, headers, config) {
                blockUI.stop();
                successFunction(response, status);
            }).error(function (response) {
                blockUI.stop();
                if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(response);
            });
        }, 0);

    }

    this.AjaxGetWithNoBlock = function (data, route, successFunction, errorFunction) {
        setTimeout(function () {
            $http({ method: 'GET', url: route, params: data }).success(function (response, status, headers, config) {
                successFunction(response, status);
            }).error(function (response) {;
                if (response.IsAuthenicated == false) { window.location = "/index.html"; }
                errorFunction(response);
            });
        }, 0);

    }

}])


EvisouApp.service('dataGridService', function () {
    var dataGrid = new Object();

    dataGrid.tableHeaders = [];
    dataGrid.sortExpression = "";
    dataGrid.sortDirection = "";
    dataGrid.sort = "";
    //dataGrid.sorting = "";

    this.initializeTableHeaders = function () {
        dataGrid = new Object();
        dataGrid.tableHeaders = [];
    };

    this.addHeader = function (label, expression,sorting) {
        var tableHeader = new Object();
        tableHeader.label = label;
        tableHeader.sortExpression = expression;
        tableHeader.sorting = sorting;
        dataGrid.tableHeaders.push(tableHeader);
    };

    this.setTableHeaders = function () {
        return dataGrid.tableHeaders;
    }

    this.changeSorting = function (columnSelected, currentSort, tableHeaders) {

        dataGrid = new Object();

        dataGrid.sortExpression = "";

        var sort = currentSort;
        if (sort.column == columnSelected) {
            sort.descending = !sort.descending;
        } else {
            sort.column = columnSelected;
            sort.descending = false;
        }

        for (var i = 0; i < tableHeaders.length; i++) {
            if (sort.column == tableHeaders[i].label) {
                dataGrid.sortExpression = tableHeaders[i].sortExpression;
                break;
            }
        }

        if (sort.descending == true)
            dataGrid.sortDirection = "DESC";
        else
            dataGrid.sortDirection = "ASC";

        dataGrid.sort = sort;

    }

    this.getSort = function (columnSelected, currentSort, tableHeaders) {
        return dataGrid.sort;
    };

    this.getSortDirection = function () {
        return dataGrid.sortDirection;
    };

    this.getSortExpression = function () {
        return dataGrid.sortExpression;
    };

    this.setDefaultSort = function (defaultSort) {
        var sort = {
            column: defaultSort,
            descending: true
        }
        return sort;
    };

    this.setSortIndicator = function (column, defaultSort, sorting) {
        var flg = column == defaultSort.column;
        var descFlg = defaultSort.descending;
        var sortIndicator = new Array();
        if (sorting)
        {
            sortIndicator.push({ 'sorting': true });
            if (flg) {
                if (descFlg)
                    sortIndicator.push({ 'sorting_desc': flg });
                else
                    sortIndicator.push({ 'sorting_asc': flg });
            }
        }
        return sortIndicator;
       
    };

    this.showToggleFilter = function (filter) {
        filter = filter === false ? true : false;
        return filter
    }
});

EvisouApp.service('accountsService', ['ajaxService', function (ajaxService) {
    this.registerUser = function (user, successFunction, errorFunction) {
        ajaxService.AjaxPost(user, "/api/account/user", successFunction, errorFunction);
    };

    this.login = function (user, successFunction, errorFunction) {
        ajaxService.AjaxPostWithNoAuthenication(user, "/api/accounts/Login", successFunction, errorFunction);
    };

    this.getUser = function (userID,successFunction, errorFunction) {
        ajaxService.AjaxGetWithData(userID,"/api/account/user", successFunction, errorFunction);
    };

    this.getUsers = function (user,successFunction, errorFunction) {
        ajaxService.AjaxGetWithData(user, "/api/account/user", successFunction, errorFunction);
    };

    this.updateUser = function (user, successFunction, errorFunction) {
        ajaxService.AjaxPost(user, "/api/accounts/UpdateUser", successFunction, errorFunction);
    };
}]);
/* Setup App Main Controller */
EvisouApp.controller('AppController', ['$scope', '$rootScope', '$http', function ($scope, $rootScope, $http) {
    $scope.$on('$viewContentLoaded', function () {
        if ($("ul.page-sidebar-menu>li.active>ul.sub-menu").length == 0) {
           $('#breadcrumb').hide();
        } else {
            $('#breadcrumb').show();
        }
        var menu = $("ul.page-sidebar-menu>li.active>a>span.title");
        var submenu = $("ul.sub-menu>li.active>a>span.title");
        $('ul.page-breadcrumb>li:eq(1)>a').text(menu.text());
        $('ul.page-breadcrumb>li:eq(2)>a').text(submenu.text());
        $('h3.page-title>small').text(submenu.parent('a.nav-link').attr('title'));
        $('h3.page-title>span').text(submenu.text());
        
        //App.initComponents(); // init core components
       // Layout.init(); //  Init entire layout(header, footer, sidebar, etc) on page load if the partials included in server side instead of loading with ng-include directive 
    });
}]);


/***
Layout Partials.
By default the partials are loaded through AngularJS ng-include directive. In case they loaded in server side(e.g: PHP include function) then below partial 
initialization can be disabled and Layout.init() should be called on page load complete as explained above.
***/

/* Setup Layout Part - Header */
EvisouApp.controller('HeaderController', ['$scope', function($scope) {
    $scope.$on('$includeContentLoaded', function() {
        Layout.initHeader(); // init header
    });
}]);

/* Setup Layout Part - Sidebar */
EvisouApp.controller('SidebarController', ['$scope','$http', function($scope, $http) {
    $scope.$on('$includeContentLoaded', function() {
        // init sidebar
        Layout.initSidebar();
    });
}]);

/* Setup Layout Part - Quick Sidebar */
EvisouApp.controller('QuickSidebarController', ['$scope', function($scope) {    
    $scope.$on('$includeContentLoaded', function() {
       setTimeout(function(){
            QuickSidebar.init(); // init quick sidebar        
        }, 2000)
    });
}]);

/* Setup Layout Part - Theme Panel */
EvisouApp.controller('ThemePanelController', ['$scope', function($scope) {    
    $scope.$on('$includeContentLoaded', function() {
        Demo.init(); // init theme panel
    });
}]);

/* Setup Layout Part - Footer */
EvisouApp.controller('FooterController', ['$scope', function($scope) {
    $scope.$on('$includeContentLoaded', function() {
        Layout.initFooter(); // init footer
    });
}]);

/* Setup App Login Controller */
EvisouApp.controller('LoginController', ['$scope', '$rootScope', function ($scope, $rootScope) {
    $scope.$on('$viewContentLoaded', function () {
        Login.init();
    });
}])

/* Setup Rounting For All Pages */
EvisouApp.config(['$stateProvider', '$urlRouterProvider', function($stateProvider, $urlRouterProvider) {
    // Redirect any unmatched url
    //$urlRouterProvider.otherwise("/todo");
    $urlRouterProvider.otherwise("/Account/Auth/Index");


    $stateProvider

        // Login
        .state('login', {
            url: "/login",
            templateUrl: "angularjs/views/login.html",
            data: { pageTitle: 'Login' },
            controller: "LoginController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before a LINK element with this ID. Dynamic CSS files must be loaded between core and theme css files
                        files: [
                            '../assets/global/plugins/select2/css/select2.min.css',
                            '../assets/global/plugins/select2/css/select2-bootstrap.min.css',
                            '../assets/pages/css/login.min.css',
                            '../assets/global/plugins/jquery-validation/js/jquery.validate.min.js',
                            '../assets/global/plugins/jquery-validation/js/additional-methods.min.js',
                            '../assets/global/plugins/select2/js/select2.full.min.js',

                            
                            'angularjs/js/controllers/LoginController.js',
                        ]
                    });
                }]
            }
        })

        // Dashboard
        .state('dashboard', {
            url: "/Account/Auth/Index",
            templateUrl: "angularjs/views/dashboard.html",
            data: {pageTitle: 'Admin Dashboard Template'},
            controller: "DashboardController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before a LINK element with this ID. Dynamic CSS files must be loaded between core and theme css files
                        files: [
                            '../assets/global/plugins/morris/morris.css',                            
                            '../assets/global/plugins/morris/morris.min.js',
                            '../assets/global/plugins/morris/raphael-min.js',                            
                            '../assets/global/plugins/jquery.sparkline.min.js',

                            '../assets/pages/scripts/dashboard.min.js',
                            'angularjs/js/controllers/DashboardController.js',
                        ] 
                    });
                }]
            }
        })

        //.state("auth", {
        //    url: "/Account/User/MyProfile",
        //    templateUrl: "angularjs/views/profile/help.html",
        //    data: { pageTitle: 'User Help' }
        //})


        .state('myprofile', {
            url: "/Account/User/MyProfile",
            templateUrl: "angularjs/views/profile/help.html",
            data: { pageTitle: '用户简介' },
           // controller: "DashboardController",
            //resolve: {
            //    deps: ['$ocLazyLoad', function ($ocLazyLoad) {
            //        return $ocLazyLoad.load({
            //            name: 'EvisouApp',
            //            insertBefore: '#ng_load_plugins_before', // load the above css files before a LINK element with this ID. Dynamic CSS files must be loaded between core and theme css files
            //            files: [
            //                '../assets/global/plugins/morris/morris.css',
            //                '../assets/global/plugins/morris/morris.min.js',
            //                '../assets/global/plugins/morris/raphael-min.js',
            //                '../assets/global/plugins/jquery.sparkline.min.js',

            //                '../assets/pages/scripts/dashboard.min.js',
            //                'angularjs/js/controllers/DashboardController.js',
            //            ]
            //        });
            //    }]
            //}
        })

         .state('user', {
             url: "/Account/User/Index",
             templateUrl: "angularjs/views/area/account/user/user.html",
             data: { pageTitle: '用户管理' },
             controller: "UserController",
             controllerAs: 'user',
             resolve: {
                 deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                     return $ocLazyLoad.load({
                         name: 'EvisouApp',
                         insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                         files: [
                             //'../assets/global/plugins/datatables/datatables.min.css',
                             
                             '../assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css',
                             '../assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css',
                             
                             '../assets/global/plugins/jquery-validation/js/jquery.validate.min.js',
                             '../assets/global/scripts/datatable.js',
                             
                             '../assets/global/plugins/datatables/datatables.all.min.js',
                             '../assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js',
                             
                             '../Assets/global/plugins/angularjs/plugins/angular-datatables/angular-datatables.min.js',
                             '../Assets/global/plugins/angularjs/plugins/angular-datatables/plugins/bootstrap/angular-datatables.bootstrap.min.js',

                             '../Assets/global/plugins/angularjs/plugins/angular-datatables/plugins/datatables-columnfilter/js/dataTables.columnFilter.js',
                             '../Assets/global/plugins/angularjs/plugins/angular-datatables/plugins/columnfilter/angular-datatables.columnfilter.min.js',


                            '../Assets/global/plugins/angularjs/plugins/angular-datatables/plugins/light-columnfilter/dataTables.lightColumnFilter.min.js',
                            '../Assets/global/plugins/angularjs/plugins/angular-datatables/plugins/light-columnfilter/angular-datatables.light-columnfilter.min.js',

                            // 'angularjs/js/area/controllers/account/user/test.js',
                            // 'angularjs/js/area/controllers/account/user/user.js',
                             'angularjs/js/area/controllers/account/user/UserController.js'
                         ]
                     });
                 }]
             }
         })


        .state('role', {
            url: "/Account/Role/Index",
            templateUrl: "angularjs/views/form_tools.html",
            data: { pageTitle: 'Form Tools' },
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '../assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css',
                            '../assets/global/plugins/bootstrap-markdown/css/bootstrap-markdown.min.css',
                            '../assets/global/plugins/typeahead/typeahead.css',

                            '../assets/global/plugins/fuelux/js/spinner.min.js',
                            '../assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',
                            '../assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js',
                            '../assets/global/plugins/jquery.input-ip-address-control-1.0.min.js',
                            '../assets/global/plugins/bootstrap-pwstrength/pwstrength-bootstrap.min.js',
                            '../assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js',
                            '../assets/global/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js',
                            '../assets/global/plugins/bootstrap-touchspin/bootstrap.touchspin.js',
                            '../assets/global/plugins/typeahead/handlebars.min.js',
                            '../assets/global/plugins/typeahead/typeahead.bundle.min.js',
                            '../assets/pages/scripts/components-form-tools-2.min.js',

                            'angularjs/js/controllers/GeneralPageController.js'
                        ]
                    }]);
                }]
            }
        })

        // Dashboard
        .state('dashboard2', {
            url: "/dashboard.html",
            templateUrl: "angularjs/views/dashboard.html",
            data: { pageTitle: 'Admin Dashboard Template' },
            controller: "DashboardController",
            resolve: {
                deps: ['$ocLazyLoad', function ($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before a LINK element with this ID. Dynamic CSS files must be loaded between core and theme css files
                        files: [
                            '../assets/global/plugins/morris/morris.css',
                            '../assets/global/plugins/morris/morris.min.js',
                            '../assets/global/plugins/morris/raphael-min.js',
                            '../assets/global/plugins/jquery.sparkline.min.js',

                            '../assets/pages/scripts/dashboard.min.js',
                            'angularjs/js/controllers/DashboardController.js',
                        ]
                    });
                }]
            }
        })

        // AngularJS plugins
        .state('fileupload', {
            url: "/file_upload",
            templateUrl: "angularjs/views/file_upload.html",
            data: {pageTitle: 'AngularJS File Upload'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'angularFileUpload',
                        files: [
                            '../assets/global/plugins/angularjs/plugins/angular-file-upload/angular-file-upload.min.js',
                        ] 
                    }, {
                        name: 'EvisouApp',
                        files: [
                            'angularjs/js/controllers/GeneralPageController.js'
                        ]
                    }]);
                }]
            }
        })

        // UI Select
        .state('uiselect', {
            url: "/ui_select.html",
            templateUrl: "angularjs/views/ui_select.html",
            data: {pageTitle: 'AngularJS Ui Select'},
            controller: "UISelectController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'ui.select',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/angularjs/plugins/ui-select/select.min.css',
                            '../assets/global/plugins/angularjs/plugins/ui-select/select.min.js'
                        ] 
                    }, {
                        name: 'EvisouApp',
                        files: [
                            'angularjs/js/controllers/UISelectController.js'
                        ] 
                    }]);
                }]
            }
        })

        // UI Bootstrap
        .state('uibootstrap', {
            url: "/ui_bootstrap.html",
            templateUrl: "angularjs/views/ui_bootstrap.html",
            data: {pageTitle: 'AngularJS UI Bootstrap'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'EvisouApp',
                        files: [
                            'angularjs/js/controllers/GeneralPageController.js'
                        ] 
                    }]);
                }] 
            }
        })

        // Tree View
        .state('tree', {
            url: "/tree",
            templateUrl: "angularjs/views/tree.html",
            data: {pageTitle: 'jQuery Tree View'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/jstree/dist/themes/default/style.min.css',

                            '../assets/global/plugins/jstree/dist/jstree.min.js',
                            '../assets/pages/scripts/ui-tree.min.js',
                            'angularjs/js/controllers/GeneralPageController.js'
                        ] 
                    }]);
                }] 
            }
        })     

        // Form Tools
        .state('formtools', {
            url: "/form-tools",
            templateUrl: "angularjs/views/form_tools.html",
            data: {pageTitle: 'Form Tools'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '../assets/global/plugins/bootstrap-switch/css/bootstrap-switch.min.css',
                            '../assets/global/plugins/bootstrap-markdown/css/bootstrap-markdown.min.css',
                            '../assets/global/plugins/typeahead/typeahead.css',

                            '../assets/global/plugins/fuelux/js/spinner.min.js',
                            '../assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',
                            '../assets/global/plugins/jquery-inputmask/jquery.inputmask.bundle.min.js',
                            '../assets/global/plugins/jquery.input-ip-address-control-1.0.min.js',
                            '../assets/global/plugins/bootstrap-pwstrength/pwstrength-bootstrap.min.js',
                            '../assets/global/plugins/bootstrap-switch/js/bootstrap-switch.min.js',
                            '../assets/global/plugins/bootstrap-maxlength/bootstrap-maxlength.min.js',
                            '../assets/global/plugins/bootstrap-touchspin/bootstrap.touchspin.js',
                            '../assets/global/plugins/typeahead/handlebars.min.js',
                            '../assets/global/plugins/typeahead/typeahead.bundle.min.js',
                            '../assets/pages/scripts/components-form-tools-2.min.js',

                            'angularjs/js/controllers/GeneralPageController.js'
                        ] 
                    }]);
                }] 
            }
        })        

        // Date & Time Pickers
        .state('pickers', {
            url: "/pickers",
            templateUrl: "angularjs/views/pickers.html",
            data: {pageTitle: 'Date & Time Pickers'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/clockface/css/clockface.css',
                            '../assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css',
                            '../assets/global/plugins/bootstrap-timepicker/css/bootstrap-timepicker.min.css',
                            '../assets/global/plugins/bootstrap-colorpicker/css/colorpicker.css',
                            '../assets/global/plugins/bootstrap-daterangepicker/daterangepicker-bs3.css',
                            '../assets/global/plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.min.css',

                            '../assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js',
                            '../assets/global/plugins/bootstrap-timepicker/js/bootstrap-timepicker.min.js',
                            '../assets/global/plugins/clockface/js/clockface.js',
                            '../assets/global/plugins/moment.min.js',
                            '../assets/global/plugins/bootstrap-daterangepicker/daterangepicker.js',
                            '../assets/global/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.js',
                            '../assets/global/plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.min.js',

                            '../assets/pages/scripts/components-date-time-pickers.min.js',

                            'angularjs/js/controllers/GeneralPageController.js'
                        ] 
                    }]);
                }] 
            }
        })

        // Custom Dropdowns
        .state('dropdowns', {
            url: "/dropdowns",
            templateUrl: "angularjs/views/dropdowns.html",
            data: {pageTitle: 'Custom Dropdowns'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load([{
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/bootstrap-select/css/bootstrap-select.min.css',
                            '../assets/global/plugins/select2/css/select2.min.css',
                            '../assets/global/plugins/select2/css/select2-bootstrap.min.css',

                            '../assets/global/plugins/bootstrap-select/js/bootstrap-select.min.js',
                            '../assets/global/plugins/select2/js/select2.full.min.js',

                            '../assets/pages/scripts/components-bootstrap-select.min.js',
                            '../assets/pages/scripts/components-select2.min.js',

                            'angularjs/js/controllers/GeneralPageController.js'
                        ] 
                    }]);
                }] 
            }
        }) 

        // Advanced Datatables
        .state('datatablesAdvanced', {
            url: "/datatables/managed.html",
            templateUrl: "angularjs/views/datatables/managed.html",
            data: {pageTitle: 'Advanced Datatables'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [                             
                            '../assets/global/plugins/datatables/datatables.min.css', 
                            '../assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css',

                            '../assets/global/plugins/datatables/datatables.all.min.js',

                            '../assets/pages/scripts/table-datatables-managed.min.js',

                            'angularjs/js/controllers/GeneralPageController.js'
                        ]
                    });
                }]
            }
        })

        // Ajax Datetables
        .state('datatablesAjax', {
            url: "/datatables/ajax.html",
            templateUrl: "angularjs/views/datatables/ajax.html",
            data: {pageTitle: 'Ajax Datatables'},
            controller: "GeneralPageController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'EvisouApp',
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/datatables/datatables.min.css', 
                            '../assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css',
                            '../assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css',

                            '../assets/global/plugins/datatables/datatables.all.min.js',
                            '../assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js',
                            '../assets/global/scripts/datatable.js',

                            'angularjs/js/scripts/table-ajax.js',
                            'angularjs/js/controllers/GeneralPageController.js'
                        ]
                    });
                }]
            }
        })

        // User Profile
        .state("profile", {
            url: "/profile",
            templateUrl: "angularjs/views/profile/main.html",
            data: {pageTitle: 'User Profile'},
            controller: "UserProfileController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load({
                        name: 'EvisouApp',  
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.css',
                            '../assets/pages/css/profile.css',
                            
                            '../assets/global/plugins/jquery.sparkline.min.js',
                            '../assets/global/plugins/bootstrap-fileinput/bootstrap-fileinput.js',

                            '../assets/pages/scripts/profile.min.js',

                            'angularjs/js/controllers/UserProfileController.js'
                        ]                    
                    });
                }]
            }
        })

        // User Profile Dashboard
        .state("profile.dashboard", {
            url: "/dashboard",
            templateUrl: "angularjs/views/profile/dashboard.html",
            data: {pageTitle: 'User Profile'}
        })

        // User Profile Account
        .state("profile.account", {
            url: "/account",
            templateUrl: "angularjs/views/profile/account.html",
            data: {pageTitle: 'User Account'}
        })

        // User Profile Help
        .state("profile.help", {
            url: "/help",
            templateUrl: "angularjs/views/profile/help.html",
            data: {pageTitle: 'User Help'}      
        })

        // Todo
        .state('todo', {
            url: "/todo",
            templateUrl: "angularjs/views/todo.html",
            data: {pageTitle: 'Todo'},
            controller: "TodoController",
            resolve: {
                deps: ['$ocLazyLoad', function($ocLazyLoad) {
                    return $ocLazyLoad.load({ 
                        name: 'EvisouApp',  
                        insertBefore: '#ng_load_plugins_before', // load the above css files before '#ng_load_plugins_before'
                        files: [
                            '../assets/global/plugins/bootstrap-datepicker/css/bootstrap-datepicker3.min.css',
                            '../assets/apps/css/todo-2.css',
                            '../assets/global/plugins/select2/css/select2.min.css',
                            '../assets/global/plugins/select2/css/select2-bootstrap.min.css',

                            '../assets/global/plugins/select2/js/select2.full.min.js',
                            
                            '../assets/global/plugins/bootstrap-datepicker/js/bootstrap-datepicker.min.js',

                            '../assets/apps/scripts/todo-2.min.js',

                            'angularjs/js/controllers/TodoController.js'  
                        ]                    
                    });
                }]
            }
        })


       .state("auth", {
           url: "/help",
           templateUrl: "angularjs/views/profile/help.html",
           data: { pageTitle: 'User Help' }
       })
}]);

/* Init global settings and run the app */
EvisouApp.run(["$rootScope", "settings", "$state", function($rootScope, settings, $state) {
    $rootScope.$state = $state; // state to be accessed from view
    $rootScope.$settings = settings; // state to be accessed from view
}]);