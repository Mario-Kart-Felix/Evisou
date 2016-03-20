/* Setup general page controller */
'use strict';
angular.module('EvisouApp', ['datatables', 'datatables.bootstrap'])
    .controller('UserController', UserCtrl)
    .directive('datatableWrapper', datatableWrapper)
   .directive('customElement', customElement)
   .directive('remoteUserexist', UserExist)
   
   
function UserCtrl2($scope, dataGridWrapperService, DTColumnBuilder, DateService) {
    $scope.dtOptions = dataGridWrapperService.options2();
  
}
function UserCtrl($rootScope, $compile, $scope,$http, $resource, DTOptionsBuilder, DTColumnBuilder,DateService,accountsService) {
        $scope.$on('$viewContentLoaded', function () {
            // initialize core components
            App.initAjax();

            // set default layout mode
            $rootScope.settings.layout.pageContentWhite = true;
            $rootScope.settings.layout.pageBodySolid = false;
            $rootScope.settings.layout.pageSidebarClosed = false;


        });
        $scope.time = DateService.time();
        console.log(DateService);

        //accountsService.getUsers(success(), error());
        //function success() {
        //    console.log('sucess');
        //}
        //function error() {
        //    console.log('error');
        //}

        var titleHtml = '<input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)">';
        $scope.selected = {};
        $scope.selectAll = false;
        $scope.toggleAll = toggleAll;
        $scope.toggleOne = toggleOne;
        function toggleAll(selectAll, selectedItems) {
            angular.element('.table-actions-wrapped').find('span').text('');
            var i = 0;
            for (var id in selectedItems) {
                i++;
                if (selectedItems.hasOwnProperty(id)) {
                    selectedItems[id] = selectAll;
                   
                }
            }
            if (selectAll){
                angular.element('.table-actions-wrapped').find('span').prepend('选择了' + i + '条记录')
            }
        };

        function toggleOne(selectedItems) {
            angular.element('.table-actions-wrapped').find('span').text('');
            var j = 0;
            
            //当少于总选项，逐个选j
            for (var val in selectedItems) {
                if (selectedItems[val]) {
                    j++;
                }
            };
            
           var i = 0;
            for (var id in selectedItems) {
               
                if (selectedItems.hasOwnProperty(id)) {
                    i++; 
                    if (!selectedItems[id]) {
                        $scope.selectAll = false;
                        angular.element('.table-actions-wrapped').find('span').prepend('选择了' + j + '条记录')
                        return;
                    }
                }
            }
           //当等于总选项i
            $scope.selectAll = true;
            angular.element('.table-actions-wrapped').find('span').prepend('选择了' + i + '条记录')
        }

        $scope.dtOptions = DTOptionsBuilder.newOptions()
                                           .withOption('ajax',
                                           function (data, callback, settings) {
                                                $http.get('http://localhost:46088/api/Account/user').success(function (res) {
                                                    // map your server's response to the DataTables format and pass it to
                                                    // DataTables' callback
                                                    callback({
                                                        recordsTotal: 120,
                                                        recordsFiltered: 120,
                                                        data: res.Users
                                                    });
                                                    //vm.notifications = res;
                                                });
                                            })
                                           .withDataProp('data')
                                           .withOption('processing', true)
                                           .withOption('serverSide', true)
                                           .withOption('language', {
                                              "metronicGroupActions": "选择了_TOTAL_条记录 :  ",
                                              "metronicAjaxRequestGeneralError": "不能完成请求,请检查网络连接",
                                              "infoFiltered": " 从 _MAX_ 记录中过滤",
                                              // data tables spesific
                                              "lengthMenu": "<span class='seperator'>页|</span>每页 _MENU_ 条记录",
                                              "info": "<span class='seperator'>|</span>一共有 _TOTAL_ 条记录",
                                              "infoEmpty": "找不到相关记录",
                                              "emptyTable": "表中没有数据",
                                              "zeroRecords": "没有找到配备的记录",
                                              "paginate": {
                                                  "previous": "上一页",
                                                  "next": "下一页",
                                                  "last": "最有一页",
                                                  "first": "第一页",
                                                  "page": "第",
                                                  "pageOf": "页，共"
                                              }
                                          })
                                           .withOption("orderCellsTop", true)
                                           .withOption("columnDefs", [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                                               'orderable': false,
                                               'targets': [0]
                                           }])
                                           .withOption("autoWidth", false)// disable fixed width and enable fluid table
                                           .withOption("processing", false) // enable/disable display message box on record load
                                          // .withOption("serverSide", true)// enable/disable server side ajax loading
                                           .withOption("lengthMenu", [
                                                       [10, 20, 50, 100, 150],
                                                       [10, 20, 50, 100, 150] // change per page values here 
                                           ])
                                           .withOption("order", [
                                                       [1, "asc"]
                                           ])
                                           .withOption('createdRow', function (row, data, dataIndex) {
                                               // Recompiling so we can bind Angular directive to the DT
                                               $compile(angular.element(row).contents())($scope);
                                           })
                                           .withOption('headerCallback', function (header) {
                                               if (!$scope.headerCompiled) {
                                                   // Use this headerCompiled field to only compile header once
                                                   $scope.headerCompiled = true;
                                                   $compile(angular.element(header).contents())($scope);
                                               }
                                           })
                                           .withDOM("<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'<'custom-element'>>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>")
                                           .withBootstrap()
                                           .withBootstrapOptions({
                                               pagination: {
                                                   classes: {
                                                       ul: 'pagination pagination-sm'
                                                   }
                                               }
                                           })
                                           .withPaginationType('bootstrap_extended')
                                           .withDisplayLength(5);
        
        
        $scope.dtColumns = [
            DTColumnBuilder.newColumn(null).withTitle(titleHtml).notSortable()
                .renderWith(function (data, type, full, meta) {
                    //console.log(full);
                    $scope.selected[full.ID] = false;
                    return '<input type="checkbox"  ng-model="selected[' + data.ID + ']" ng-click="toggleOne(selected)">';
                }),
            DTColumnBuilder.newColumn('LoginName').withTitle('登录名'),
            DTColumnBuilder.newColumn('Email').withTitle('邮箱'),
            DTColumnBuilder.newColumn('Mobile').withTitle('电话'),
            DTColumnBuilder.newColumn('Mobile').withTitle('角色'),
            DTColumnBuilder.newColumn('IsActive').withTitle('激活'),
            DTColumnBuilder.newColumn('IsActive').withTitle('搜索'),
            //DTColumnBuilder.newColumn('lastName').withTitle('Last name').notVisible()
        ];
        
        $scope.wrappedAction = function () {
            //alert('dsadadsadad');
            console.log($scope.selected);
          //  $http().success().error();
        }

    
        
    }

function datatableWrapper($timeout, $compile) {
    return {
        restrict: 'E',
        transclude: true,
        replace: true,
        scope: true,
        template: '<ng-transclude></ng-transclude>',

        link: link
    };
    function link(scope, element) {
        // Using $timeout service as a "hack" to trigger the callback function once everything is rendered
        $timeout(function () {
            $compile(element.find('.custom-element'))(scope);
        }, 0, false);
    }
}

function customElement(){
    return {
        restrict: 'C',
        template:
        '<div class="table-actions-wrapped"><span></span>' +
            '<select class="table-group-action-input form-control input-inline input-small input-sm" name="status">' +
                '<option value="">选择...</option>' +
                '<option value="active">激活</option>' +
                '<option value="freeze">冻结</option>' +
                '<option value="delete">删除</option>' +
            '</select>' +
            '<button class="btn btn-sm yellow table-group-action-submit" ng-click="wrappedAction()"><i class="fa fa-check"></i> 提交</button>',
    };
}

function ModalDemoCtrl($scope, $uibModal, $log) {
    
    
    $scope.open = function (size) {
        
        var modalInstance = $uibModal.open(
        {
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: size,
            backdrop: 'static',
            resolve:
            {
                items: function () {
                    
                }
            }
        });
        modalInstance.result.then(function (selectedItem) {
           
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
}

function ModalInstanceCtrl($scope,filterFilter, $http,$modalInstance, items) {
    $scope.items = items;
    $scope.title = "添加新用户";
    $scope.Password = '1111111';
    
    $http({
        method: 'get',
        url: 'http://localhost:46088/api/account/role',

    }).success(function (data, status, headers, config) {
       
        $scope.RoleIds = data.Roles;
        
        $scope.selection = [];

        $scope.selectedRoleIds = function selectedRoleIds() {
            return filterFilter($scope.RoleIds, { selected: true });
        };

        $scope.$watch('RoleIds|filter:{selected:true}', function (nv) {
           
            $scope.selection = nv.map(function (RoleId) {
                return RoleId.ID;
            });
        }, true);
        
    }).error(function (data, status, headers, config) {

    });

   
    //RoleIds checkbox begin
    //$scope.RoleIds = [
    //    { ID: 1, selected: true },
    //    { ID: 2, selected: false },
    //    { ID: 3, selected: true },
    //];

    
    //RoleIds checkbox end

    $scope.createUser = function () {

        var user = new Object();

        user.LoginName = $scope.LoginName;
        user.Password = $scope.Password;
        user.Email = $scope.Email;
        user.Mobile = $scope.Mobile;
        user.IsActive = $scope.IsActive;
        user.RoleIds = $scope.selection;
        return user;

     };


    $scope.ok = function () {
        console.log($scope);
       // alert('dsadasda');
        var user = $scope.createUser();
        console.log(user);
        $scope.user = user;
       // console.log(mainForm);
      //  $modalInstance.close($scope.selected.item);
        
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}

function UserExist($http) {
    return {
        require: 'ngModel',
        link: function (scope, elm, attrs, ctrl) {
            elm.bind('keyup', function () {
                $http({ method: 'GET', url: 'http://localhost:46088/api/Account/user/7' }).
			    success(function (data, status, headers, config) {
			        //if (parseInt(data) != 0) {
                    if(false){
			            var str = '用户存在';
			            console.log('dsadsadada');
			            ctrl.$setValidity('LoginName', false);
                        
			            scope.flg = function () {
			                scope.msg = ctrl.$viewValue.length < attrs.minlength ?  attrs.valRequired : str;
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
