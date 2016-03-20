/* Setup general page controller */
'use strict';
angular.module('EvisouApp')
    .controller('UserController', 
    function ($rootScope, $compile, $scope, $resource, DTOptionsBuilder, DTColumnBuilder) {
    $scope.$on('$viewContentLoaded', function() {   
    	// initialize core components
    	App.initAjax();
    	
    	// set default layout mode
    	$rootScope.settings.layout.pageContentWhite = true;
        $rootScope.settings.layout.pageBodySolid = false;
        $rootScope.settings.layout.pageSidebarClosed = false;
       
        
    });
    $scope.selected = {};
    $scope.selectAll = false;
    $scope.toggleAll = toggleAll;
    $scope.toggleOne = toggleOne;
    function toggleAll(selectAll, selectedItems) {
        for (var id in selectedItems) {
            if (selectedItems.hasOwnProperty(id)) {
                selectedItems[id] = selectAll;
            }
        }
    }
    function toggleOne(selectedItems) {
        for (var id in selectedItems) {
            if (selectedItems.hasOwnProperty(id)) {
                if (!selectedItems[id]) {
                    $scope.selectAll = false;
                    return;
                }
            }
        }
        $scope.selectAll = true;
    }

    var titleHtml = '<input type="checkbox" ng-model="selectAll" ng-click="toggleAll(selectAll, selected)">';
    $scope.dtOptions = DTOptionsBuilder
        .fromSource('http://localhost:46088/Assets/global/plugins/angularjs/plugins/angular-datatables/data.json')
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
            TableTools: {
                classes: {
                    container: 'btn-group',
                    buttons: {
                        normal: 'btn btn-danger'
                    }
                }
            },
            ColVis: {
                classes: {
                    masterButton: 'btn btn-primary'
                }
            },
            pagination: {
                classes: {
                    ul: 'pagination pagination-sm'
                }
            }
        })
        .withPaginationType('bootstrap_extended')
        .withDisplayLength(2);
    $scope.dtColumns = [
        DTColumnBuilder.newColumn(null).withTitle(titleHtml).notSortable()
            .renderWith(function (data, type, full, meta) {
                $scope.selected[full.id] = false;
                return '<input type="checkbox"  ng-model="selected[' + data.id + ']" ng-click="toggleOne(selected)">';
            }),
        DTColumnBuilder.newColumn('id').withTitle('ID'),
        DTColumnBuilder.newColumn('firstName').withTitle('牌子'),
        DTColumnBuilder.newColumn('lastName').withTitle('产品')
    ];

    var vm = this;
    vm.title = '你好这个controller as的绑定演示';
    $scope.count = 0;
    
    
   //select row end






    $scope.toggleFilter = true;
    $scope.toggle = function () {
        $scope.toggleFilter = $scope.toggleFilter === false ? true : false;
    }
    $scope.changeTitle = function () {
       // $('h4.modal-title').text('添加新用户');
        $scope.title = '添加新用户';
        $scope.Password = '111111';
       
       // $scope.IsActive = true;
        //{ loginName: '', Password: '111111', Email: '', Mobile: '', IsActive: true }
    };
    $scope.btnSubmit = function () {
        var user = $scope.createUser();
        console.log(user);
        //$scope.mainForm.$setPristine();
    };
    $scope.setOriginalValues = function () {
        $scope.user = { loginName: '', Password: '111111', Email: '', Mobile: '', IsActive: false };


        $scope.OriginalProductCode = $scope.ProductCode;
        $scope.OriginalDescription = $scope.Description;
        $scope.OriginalUnitPrice = $scope.UnitPrice;
        $scope.OriginalUnitOfMeasure = $scope.UnitOfMeasure;
    }
    $scope.clearValidationErrors = function () {
        $scope.mainForm.$setPristine();
    };
    $scope.createUser = function () {

        var user = new Object();

        user.LoginName = $scope.loginName;
        user.Password = $scope.Password;
        user.Email = $scope.Email;
        user.Mobile = $scope.Mobile;
        user.IsActive = $scope.IsActive;
        
        return user;

    };
    $scope.btnReset = function () {
        $scope.clearValidationErrors();
    };

    })

    .directive('datatableWrapper',
    function ($timeout, $compile) {
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
        
        
    })
    .directive('customElement',
    function () {
       
        return {
            restrict: 'C',
            replace: true,
            scope: true,
            template:
            '<div class="table-actions-wrapper1"><span></span>' +
                '<select class="table-group-action-input form-control input-inline input-small input-sm" name="status">' +
                    '<option value="">选择...</option>' +
                    '<option value="active">激活</option>' +
                    '<option value="freeze">冻结</option>' +
                    '<option value="delete">删除</option>' +
                '</select>' +
                '<button class="btn btn-sm yellow table-group-action-submit"><i class="fa fa-check"></i> 提交</button>',
        };
    })
    .directive("addbuttonsbutton",
    function () {
        return {
            restrict: "E",
            template: "<button addbuttons>Click to add sdfsfsbuttons</button>"
        }
    })
    .directive("addbuttons",
    function ($compile) {
        return function (scope, element, attrs) {
            element.bind("click", function () {
                scope.count++;
                angular.element(document.getElementById('space-for-buttons'))
                    .append($compile("<div><button class='btn btn-default' data-alert=" + scope.count + ">Show alert #" + scope.count + "</button></div>")(scope));
            });
        };
    })
    .directive("alert",
    function () {
        return function (scope, element, attrs) {
            element.bind("click", function () {
                console.log(attrs);
                alert("This is alert #" + attrs.alert);
            });
        };
    })
    
    

function ModalDemoCtrl($scope, $uibModal, $log) {
    $scope.items = ['item1', 'item2', 'item3'];
    $scope.open = function (size) {
        var modalInstance = $uibModal.open(
        {
            templateUrl: 'myModalContent.html',
            controller: 'ModalInstanceCtrl',
            size: size,
            resolve:
            {
                items: function () {
                    return $scope.items;
                }
            }
        });
        modalInstance.result.then(function (selectedItem) {
            $scope.selected = selectedItem;
        }, function () {
            $log.info('Modal dismissed at: ' + new Date());
        });
    };
}

function ModalInstanceCtrl($scope, $modalInstance, items) {
    $scope.items = items;
    $scope.selected = {
        item: $scope.items[0]
    };
    $scope.ok = function () {
        $modalInstance.close($scope.selected.item);
    };
    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    };
}

function WithOptionsCtrl(DTOptionsBuilder, DTColumnDefBuilder) {
    var vm = this;
    vm.dtOptions = DTOptionsBuilder.newOptions()
        .withPaginationType('full_numbers')
        .withDisplayLength(2)
        .withDOM('pitrfl');
    vm.dtColumnDefs = [
        DTColumnDefBuilder.newColumnDef(0),
        DTColumnDefBuilder.newColumnDef(1).notVisible(),
        DTColumnDefBuilder.newColumnDef(2).notSortable()
    ];
}




angular.module('EvisouApp')
    .controller('CustomElementCtrl', CustomElementCtrl)
    .directive('datatableWrapper2', datatableWrapper2)
    .directive('customElement2', customElement2);

function CustomElementCtrl(DTOptionsBuilder, DTColumnBuilder) {
    var vm = this;
    vm.dtOptions = DTOptionsBuilder.fromSource('http://localhost:46088/Assets/global/plugins/angularjs/plugins/angular-datatables/data.json')
        // Add your custom button in the DOM
       .withDOM("<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'<'custom-element2'>>>r><'table-responsive't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>")
    vm.dtColumns = [
        DTColumnBuilder.newColumn('id').withTitle('ID'),
        DTColumnBuilder.newColumn('firstName').withTitle('First name'),
        DTColumnBuilder.newColumn('lastName').withTitle('Last name').notVisible()
    ];
}

/**
 * This wrapper is only used to compile your custom element
 */
function datatableWrapper2($timeout, $compile) {
    console.log('sdadsadasad');
    return {
        restrict: 'E',
        transclude: true,
        template: '<ng-transclude></ng-transclude>',
        link: link
    };

    function link(scope, element) {
        // Using $timeout service as a "hack" to trigger the callback function once everything is rendered
        $timeout(function () {
            // Compiling so that angular knows the button has a directive
            $compile(element.find('.custom-element2'))(scope);
        }, 0, false);
    }
}

/**
 * Your custom element
 */
function customElement2() {
    console.log('4353535353');
    return {
        restrict: 'C',
        template: '<h1>My custom elemenvvvvvvt</h1>'
    };
}