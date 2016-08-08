/* Setup general page controller */
'use strict';
angular.module('EvisouApp', ['datatables', 'datatables.bootstrap'])
    .controller('UserController', UserCtrl)
    .directive('remoteUserexist', UserExist)
    .directive('uibPagination', PaginationCtrl)
    




function UserCtrl($rootScope, $compile, $scope, $http,$uibModal, $log, filterFilter, DateService, accountsService, dataGridService) {
    $scope.$on('$viewContentLoaded', function () {
        // initialize core components
        App.initAjax();

        // set default layout mode
        $rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;


    });
    $scope.time = DateService.time();
    
  //  console.log($scope.totalItems);
    dataGridService.initializeTableHeaders();
    dataGridService.addHeader("登录名", "LoginName", true);
    dataGridService.addHeader("邮箱", "Email", true);
    dataGridService.addHeader("电话", "Mobile", true);
    dataGridService.addHeader("角色", "Role", false);
    dataGridService.addHeader("激活", "IsActive", true);

    $scope.tableHeaders = dataGridService.setTableHeaders();
    $scope.defaultSort = dataGridService.setDefaultSort("登录名");
    $scope.changeSorting = function (column, sorting) {
        if (sorting) {
            console.log(dataGridService.getSortExpression());
            dataGridService.changeSorting(column, $scope.defaultSort, $scope.tableHeaders);
            $scope.defaultSort = dataGridService.getSort();
            $scope.SortDirection = dataGridService.getSortDirection();
            $scope.SortExpression = dataGridService.getSortExpression();
            $scope.getUsers();
        } else {
            return false;
        }
        
    };
    $scope.setSortIndicator = function (column, sorting) {
        return dataGridService.setSortIndicator(column, $scope.defaultSort, sorting);
    };

    $scope.currentPage = 1;
    $scope.pagesizes = [
                         { size: 2 },
                         { size: 5 },
                         { size: 10 },
                         { size: 20 },
                         { size: 50 },
                         { size: 100 },
                         { size: 150 }];
    $scope.PageSize = $scope.pagesizes[0];

    /*Search Action Begin*/
    /*show filter start*/
    $scope.filter = true;
    $scope.toggleFilter = function (filter) {
        $scope.filter=dataGridService.showToggleFilter(filter);
    };
    /*show filter end*/

    /*search form start*/
    $scope.searchUsers = function () {
        $scope.getUsers();
    };
    $scope.resetSearchFieds = function () {
        $scope.LoginName = '';
        $scope.IsActive = '';
        $scope.getUsers();
    }
    $scope.isActiveData = [
                { value: true, name: '是' },
                { value: false, name: '否' },
    ];
    /*search form end*/
    /*Search Action End*/

    /*custom action start*/
    $scope.closeAlert = function (index) {
        $scope.alerts.splice(index, 1);
    };
    $scope.customActionData = [
                { value: 'active', name: '激活' },
                { value: 'freeze', name: '冻结' },
                { value: 'delete', name: '删除' },
    ];
    $scope.customAction = function () {
        //var selectedUser = filterFilter($scope.users, { selected: true });
        //if (selectedUser.length == 0) {
       if ($scope.userSelection.length == 0) {
            $scope.alerts = new Array();
            if ($scope.customActionModel == null) {
                $scope.alerts.push({ type: 'danger', msg: ' 请选择动作' });
            } else {
                $scope.alerts.push({ type: 'danger', msg: ' 没有选择的记录' });
            }
        } else {
            $scope.alerts = new Array();
           
            $scope.CustomActionType = 'GROUP_ACTION';
            $scope.CustomActionName = $scope.customActionModel.value;
            $scope.getUsers();
            

            console.log($scope.customActionModel.value)
            //$http().success().error(function () {
            //    $scope.alerts.push({ type: 'danger', msg: ' 不能完成请求,请检查网络连接' });
            //});
            //console.log('dsadasdasdasdasdasdasdasdsads');
        }

    };
    $scope.customDeleteAction = function () {
        console.log($scope.userSelection);
        if ($scope.userSelection.length == 0) {
            $scope.alerts = new Array();
            $scope.alerts.push({ type: 'danger', msg: ' 没有选择的记录' });
        } else {
            $scope.alerts = new Array();
            $scope.alerts.push({ type: 'success', msg: '删除记录成功' });
            $scope.CustomActionType = 'DELETE';
            $scope.getUsers();
        }
    }
    /*custom action end*/

    /*page set begin*/
    
   
    
    
    /*page set end*/
   

    /*users object start*/
    $scope.createUserInquiryObject = function () {
        var userInquiry = new Object();
        userInquiry.LoginName = $scope.LoginName;
        userInquiry.IsActive = (typeof ($scope.IsActive) == 'undefined') ? null: $scope.IsActive.value;
        userInquiry.CurrentPageNumber = $scope.currentPage;
        userInquiry.SortExpression = $scope.SortExpression;
        userInquiry.SortDirection = $scope.SortDirection;
        userInquiry.PageSize = $scope.PageSize.size;
        userInquiry.CustomActionType = $scope.CustomActionType;
        userInquiry.CustomActionName = $scope.CustomActionName;
        userInquiry.IDs = $scope.userSelection;
        //console.log(userInquiry);
        return userInquiry;
    };
    $scope.usersInquiryCompleted = function (response, status) {
        //console.log(response);
        $scope.users = response.Users;
        $scope.totalItems = response.TotalRecords;
        $scope.userSelection = new Array();
        $scope.$watch('users|filter:{selected:true}', function (nv) {
            $scope.userSelection = nv.map(function (user) {
                return user.ID;
            });
        }, true);
    };
    $scope.usersInquiryError = function (response, status) {
        $scope.alerts = new Array();
        $scope.alerts.push({ type: 'danger', msg: response.Message });
    };
    $scope.getUsers = function () {
        var userInquiry = $scope.createUserInquiryObject();
        accountsService.getUsers(userInquiry, $scope.usersInquiryCompleted, $scope.usersInquiryError);
    };
    $scope.getUsers();
    
    
    /*users object end*/
    $scope.pageSizeChange = function () {
        $scope.getUsers();
    };
    $scope.checkOne = function () {
        var selectedUser = filterFilter($scope.users, { selected: true });
        if (selectedUser.length == $scope.users.length) 
            $scope.selectAll = true;
        else 
            $scope.selectAll = false;
        angular.element('.table-group-actions').find('span').text(selectedUser.length != 0 ? '选择了' + selectedUser.length + '条记录' : '');
    };
    $scope.checkAll = function () {
       
        console.log($scope.selectAll);
        angular.forEach($scope.users, function (obj) {
            obj.selected = $scope.selectAll;
        });
        angular.element('.table-group-actions').find('span').text($scope.selectAll ? '选择了' + $scope.users.length + '条记录' : '');

    };



 
   
    $scope.open = function (id) {
        
        var modalInstance = $uibModal.open(
        {
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            backdrop: 'static',
            //resolve: resolveObj,
            resolve:
                {
                    
                    id: function () {
                        return id;
                    }
                }
        });
        modalInstance.result.then(function (aa) {
            $scope.getUsers();
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
}


function PaginationCtrl($timeout, $compile) {
    return {
        require: 'ngModel',
        replace: true,
        link: function (scope, element) {
            //console.log(scope.filter);
            $timeout(function () {
                $compile(element.find('.pagination-panel-input'))(scope);
               
                scope.pageChanged = function () {
                    scope.getUsers();
                    console.log('Page changed to: ' + scope.currentPage);
                };
                element.bind('keyup', function () {
                    scope.getUsers();
                });
            }, 0, false);
        }
    };
}

function ModalInstanceCtrl($scope, filterFilter, $http, $uibModalInstance, id, DateService, accountsService) {
    $scope.time2 = DateService.time();
    
    if (typeof (id) != 'undefined') {
        $scope.title = "编辑新用户ID";
        $scope.userInqueryCompleted = function (response, status) {
            $scope.LoginName = response.User.LoginName;
            $scope.Email = response.User.Email;
            $scope.Mobile = parseInt(response.User.Mobile);
            $scope.IsActive = response.User.IsActive;

            $http({
                method: 'get',
                url: 'http://localhost:46088/api/account/role',

            }).success(function (data, status, headers, config) {
                $scope.Roles = data.Roles;
                var roleNames = response.User.Roles.split(',');
                for (var i = 0; i < data.Roles.length;i++){
                    for (var j = 0; j < roleNames.length; j++){
                        if (data.Roles[i].Name == roleNames[j]){
                            data.Roles[i].selected = true;
                        }
                    }
                }
                    
              
                $scope.$watch('Roles|filter:{selected:true}', function (nv) {
                     nv.map(function (Role) {
                        return Role.ID;
                    });
                }, true);  
            }).error(function (data, status, headers, config) {

            });
        };
        $scope.userInquiryError = function (response, status) {
            $scope.alerts = new Array();
            $scope.alerts.push({ type: 'danger', msg: response.Message });
        };
        $scope.getUser = function (id) {
            var getUser = new Object();
            getUser.UserID = id;
            accountsService.getUser(getUser, $scope.userInqueryCompleted, $scope.userInquiryError);
        }
        $scope.getUser(id);
        
    } else {
        $scope.title = "添加新用户";
        $scope.Password = '1111111';
        $http({
            method: 'get',
            url: 'http://localhost:46088/api/account/role',

        }).success(function (data, status, headers, config) {
            $scope.Roles = data.Roles;
            $scope.roleSelection = new Array();
            $scope.$watch('Roles|filter:{selected:true}', function (nv) {
                $scope.roleSelection = nv.map(function (Role) {
                    return Role.ID;
                });
            }, true);
        }).error(function (data, status, headers, config) {

        });

    }
    
    $scope.registerUserCompleted = function (response) {
        $uibModalInstance.close($scope);
       // window.location = "/applicationMasterPage.html#/Customers/CustomerInquiry";
    }
    $scope.registerUserError = function (response, status) {
        $scope.alerts = new Array();
        $scope.alerts.push({ type: 'danger', msg: response.Message });
        $scope.alerts.push({ type: 'danger', msg: response.ReturnMessage });
        console.log(response);
    };
    $scope.registerUser = function () {
        var user = $scope.createUser();
        accountsService.registerUser(user, $scope.registerUserCompleted, $scope.registerUserError);
    }

    
    $scope.createUser = function () {

        var user = new Object();
        user.LoginName = $scope.LoginName;
        user.Password = $scope.Password;
        user.Email = $scope.Email;
        user.Mobile = $scope.Mobile;
        user.IsActive = $scope.IsActive;
        user.IDs = $scope.roleSelection;
        return user;

    };

    $scope.ok = function () {
        if (typeof (id) != 'undefined') {

        } else {
           $scope.registerUser();
        }
    };
    $scope.cancel = function () {
        $uibModalInstance.dismiss('cancel');
    };
}
function UserExist($http) {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            elm.bind('keyup', function () {
                console.log(ctrl.$modelValue);
                // $http({ method: 'GET', url: 'http://localhost:46088/api/account/user?LoginName='  }).
                $http({ method: 'GET', url: '/api/account/user?CurrentPageNumber=1&PageSize=1&LoginName=' + ctrl.$modelValue }).
                success(function (data, status, headers, config) {
                    if (data.Users != 0) {
                    //if (false) {
                        var str = '用户存在';
                        ctrl.$setValidity('LoginName', false);
                        scope.flg = function () {
                            scope.msg = ctrl.$viewValue.length < attrs.minlength ? attrs.valRequired : str;
                            return false;
                        }
                    } else {
                        if (ctrl.$viewValue.length < attrs.minlength) {
                            console.log(attrs.valRequired);
                            ctrl.$setValidity('LoginName', false);
                            scope.flg = function () {
                                scope.msg = attrs.valRequired;
                                return false;
                            }

                        } else {
                            scope.flg = function () {
                                ctrl.$setValidity('LoginName', true);
                                return true;
                            };
                        }


                    }
                }).
                error(function (data, status, headers, config) {
                    ctrl.$setValidity('LoginName', false);
                    var str = 'http错误：' + status;
                    scope.flg = function () {
                        scope.msg = ctrl.$viewValue.length < attrs.minlength ? str + '；' + attrs.valRequired : str;
                        return false;
                    }
                });
            });
        }
    }
}

