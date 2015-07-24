var EcommerceOrders = function () {

    var initPickers = function () {
        //init date pickers
        $('.date-picker').datepicker({
            rtl: App.isRTL(),
            autoclose: true
        });
    }

    var handleOrders = function() {

        var grid = new Datatable();
            grid.init({
                src: $("#datatable_orders"),
                onSuccess: function(grid) {
                    // execute some code after table records loaded
                },
                onError: function(grid) {
                    // execute some code on network or other general error  
                },
                dataTable: {  // here you can define a typical datatable settings from http://datatables.net/usage/options 
                    "aLengthMenu": [
                        [20, 50, 100, 150, -1],
                        [20, 50, 100, 150, "All"] // change per page values here
                    ], "aoColumns": [
                            {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                // "bVisible": false ,  
                                "fnRender": function (oObj) {
                                    return "<input type=\"checkbox\" name=\"id[]\" value=" + oObj.aData[0] + ">";
                                  //  return '<a href=\"/TransactionDetails/Details/' + oObj.aData[0] + '\">View</a>';
                                }
                            },
                            { "sName": "OrderTime" },
                            { "sName": "TransactionID" },
                            { "sName": "BuyerID" },
                            { "sName": "ShipToCountryCode" },
                            { "sName": "Amt", },
                            { "sName": "Agent" },
                            { "sName": "AgentPostage" },                                           
                            {
                                "sName": "ID",
                                "bSearchable": false,
                                "bSortable": false,
                                "fnRender": function (oObj) {
                                
                                    var str = "<div class=\"btn-group\">";
                                    str += "<button id=\"btnGroupVerticalDrop7\" type=\"button\" class=\"btn purple btn-sm dropdown-toggle\" data-toggle=\"dropdown\">";
                                    str += " 操作 <i class=\"fa fa-angle-down\"></i>";
                                    str += "</button>";
                                    str += "<ul class=\"dropdown-menu btn-sm pull-right\" role=\"menu\" aria-labelledby=\"btnGroupVerticalDrop7\">";
                                    str += "<li>";
                                    str += "<a class=\"ajax-order-dispatch\" id=\"" + oObj.aData[8] + "\"><i class=\"fa fa-edit\"></i> 创建发货单</a>";
                                    str += "</li>";
                                    str += "<li>";
                                    str += "<a class=\"ajax-order-submit-dispatch\" id=\"" + oObj.aData[8] + "\"><i class=\"fa fa-edit\"></i> 提审发货单</a>";
                                    str += "</li>";
                                    str += "<li>";
                                    str += "<a class=\"ajax-order-delete-dispatch\" id=\"" + oObj.aData[8] + "\"><i class=\"fa fa-edit\"></i> 删除发货单</a>";
                                    str += "</li>";
                                    str += "</ul>";
                                    str += "</div>";
                                   return str
                                }
                            },
                    ],
                    "oLanguage": {
                        //"sProcessing": "正在加载中......",
                        "sProcessing": '<img src="../../assets/admin/img/loading-spinner-grey.gif"/><span>&nbsp;&nbsp;Loading...</span>',
                        "sLengthMenu": "页<span class='seperator'>|</span>每页显示 _MENU_ 条记录",
                        "sAjaxRequestGeneralError": "请求不能完成,请检查是否联网",
                        "sZeroRecords": "对不起，查询不到相关数据！",
                        "sEmptyTable": "表中无数据存在！",
                        "sInfo": "当前显示 _START_ 到 _END_ 条，共 _TOTAL_ 条记录",
                        "sInfoFiltered": "数据表中共为 _MAX_ 条记录",
                        "sSearch": "搜索",
                        "oPaginate": {
                            "sPrevious": "上一页",
                            "sNext": "下一页",
                            "sPage": "第",
                            "sPageOf": "页,共"
                        }
                    }, //多语言配置
                    "iDisplayLength": 20, // default record count per page
                    "bServerSide": true, // server side processing
                    "sAjaxSource": "AjaxOrders", // ajax source
                    //"aaSorting": [[1, "asc"]], // set first column as a default sort by asc
                    "bSearchable": false,
                }
            });

            // handle group actionsubmit button click
            grid.getTableWrapper().on('click', '.table-group-action-submit', function(e){
                e.preventDefault();
                var action = $(".table-group-action-input", grid.getTableWrapper());
                if (action.val() != "" && grid.getSelectedRowsCount() > 0) {
                    grid.addAjaxParam("sAction", "group_action");
                    grid.addAjaxParam("sGroupActionName", action.val());
                    var records = grid.getSelectedRows();
                    for (var i in records) {
                        grid.addAjaxParam(records[i]["name"], records[i]["value"]);    
                    }
                    grid.getDataTable().fnDraw();
                    grid.clearAjaxParams();
                } else if (action.val() == "") {
                    App.alert({type: 'danger', icon: 'warning', message: 'Please select an action', container: grid.getTableWrapper(), place: 'prepend'});
                } else if (grid.getSelectedRowsCount() === 0) {
                    App.alert({type: 'danger', icon: 'warning', message: 'No record selected', container: grid.getTableWrapper(), place: 'prepend'});
                }
            });

    }

    return {

        //main function to initiate the module
        init: function () {

            initPickers();
            handleOrders();
        }

    };

}();