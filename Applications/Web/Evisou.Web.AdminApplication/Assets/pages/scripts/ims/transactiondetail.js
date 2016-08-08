var Transaction = function () {
    var handleTransaction = function () {
        var grid = new Datatable();
        $(".delete").click(function () {
            var message = "你确定要删除勾选的记录吗?";
            if ($(this).attr("message"))
                message = $(this).attr("message") + "，" + message;
            if (confirm(message))
                if (grid.getSelectedRowsCount() > 0) {
                    grid.setAjaxParam("customActionType", "delete");
                    grid.setAjaxParam("id", grid.getSelectedRows());
                    grid.getDataTable().ajax.reload();
                    grid.clearAjaxParams();
                } else {
                    alert("还没有勾选记录")
                }

        });
        grid.init({
            src: $("#datatable_transaction"),
            onSuccess: function (grid) {
                // execute some code after table records loaded
            },
            onError: function (grid) {
                // execute some code on network or other general error  
            },
            loadingMessage: '载入中...',
            dataTable: { // here you can define a typical datatable settings from http://datatables.net/usage/options 

                // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
                // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/scripts/datatable.js). 
                // So when dropdowns used the scrollable div should be removed. 
                //"dom": "<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r>t<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>",
                /*"oLanguage": {//语言国际化
				    "sUrl": "http://cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Chinese.json"
				},*/

                "aoColumns": [
                     {
                         "aDataSort": [0],
                         "mData": "ID",
                         "mRender": function (source, type, val) {
                             return "<input type=\"checkbox\" name=\"id[]\" value=" + val[0] + ">";
                             //  return '<a href=\"/TransactionDetails/Details/' + oObj.aData[0] + '\">View</a>';
                         }
                     },
                     { "aDataSort": [1] },
                     { "aDataSort": [2] },
                     { "aDataSort": [3] },
                     {
                         "aDataSort": [4],

                     },
                     {
                         "aDataSort": [5],

                     },
                     {
                         "aDataSort": [6],

                     },
                     {
                         "aDataSort": [7],

                     },
                     {
                         "aDataSort": [8],
                         "mData": "ID",
                         "bSortable": false,
                         "mRender": function (source, type, val) {
                             //var str = " <a class=\"btn btn-xs purple thickbox\" title=\'编辑供应商\' data-modal href=\"Edit/"+val[0] + "\">";
                             //str+="<i class=\"fa fa-edit\"></i>";
                             //str+="编辑";
                             //str += "</a>";
                             //return str

                             var str = "<div class=\"btn-group\">";
                             str += "<button id=\"btnGroupVerticalDrop7\" type=\"button\" class=\"btn purple btn-sm dropdown-toggle\" data-toggle=\"dropdown\">";
                             str += " 操作 <i class=\"fa fa-angle-down\"></i>";
                             str += "</button>";
                             str += "<ul class=\"dropdown-menu btn-sm pull-right\" role=\"menu\" aria-labelledby=\"btnGroupVerticalDrop7\">";
                             str += "<li>";
                             str += "<a class=\"ajax-order-dispatch\" data-modal=\" \" id=\"" + val[0] + "\" href=\"/Ims/TransactionDetail/Dispatch/" + val[0] + "\"><i class=\"fa fa-edit\"></i> 创建发货单</a>";
                             str += "</li>";
                             str += "<li>";
                             str += "<a class=\"ajax-order-submit-dispatch\" id=\"" + val[0] + "\"><i class=\"fa fa-edit\"></i> 提审发货单</a>";
                             str += "</li>";
                             str += "<li>";
                             str += "<a class=\"ajax-order-delete-dispatch\" id=\"" + val[0] + "\"><i class=\"fa fa-edit\"></i> 删除发货单</a>";
                             str += "</li>";
                             str += "</ul>";
                             str += "</div>";
                             return str


                         }
                     },
                ],
                // "sDom": 'T<"clear">lrtip',
                //"sDom": "T<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r><'table-scrollable't><'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>",
                //"oTableTools": {
                //    "sSwfPath": "../../assets/global/plugins/datatables/extensions/TableTools/swf/copy_csv_xls_pdf.swf",
                //    "aButtons": [{
                //        "sExtends": "pdf",
                //        "sButtonText": "PDF"
                //    }, {
                //        "sExtends": "csv",
                //        "sButtonText": "CSV"
                //    }, {
                //        "sExtends": "xls",
                //        "sButtonText": "Excel"
                //    }, {
                //        "sExtends": "print",
                //        "sButtonText": "打印",
                //        "sInfo": 'Please press "CTR+P" to print or "ESC" to quit',
                //        "sMessage": "Generated by DataTables"
                //    }]
                //},
                "lengthMenu": [
                    [10, 20, 50, 100, 150],
                    [10, 20, 50, 100, 150] // change per page values here 
                ],
                "pageLength": 10, // default record count per page                
                "ajax": {
                    "url": "Datatable", // ajax source
                },
                "order": [
                    [1, "desc"]
                ] // set first column as a default sort by asc
            }
        });

        // handle group actionsubmit button click
        grid.getTableWrapper().on('click', '.table-group-action-submit', function (e) {
            e.preventDefault();
            var action = $(".table-group-action-input", grid.getTableWrapper());
            if (action.val() != "" && grid.getSelectedRowsCount() > 0) {
                grid.setAjaxParam("customActionType", "group_action");
                grid.setAjaxParam("customActionName", action.val());
                grid.setAjaxParam("id", grid.getSelectedRows());
                grid.getDataTable().ajax.reload();
                grid.clearAjaxParams();

            } else if (action.val() == "") {
                App.alert({
                    type: 'danger',
                    icon: 'warning',
                    message: '请选择动作',
                    container: grid.getTableWrapper(),
                    place: 'prepend'
                });
            } else if (grid.getSelectedRowsCount() === 0) {
                App.alert({
                    type: 'danger',
                    icon: 'warning',
                    message: '没有选择的记录',
                    container: grid.getTableWrapper(),
                    place: 'prepend'
                });
            }
        });
    }
    var handleDatePickers = function () {

        if (jQuery().datepicker) {
            $('.date-picker').datepicker({
                language: 'zh-CN',
                rtl: App.isRTL(),
                orientation: "left",
                autoclose: true,

            });
            //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
        }

        /* Workaround to restrict daterange past date select: http://stackoverflow.com/questions/11933173/how-to-restrict-the-selectable-date-ranges-in-bootstrap-datepicker */
    }

    var handleFilter = function () {
        $('tr.filter').hide();
        $('a.search-table').click(function () {
            $('tr.filter').fadeToggle("slow");
        })
    }
    var handleDateRangePickers = function () {



        //$('#defaultrange_modal').daterangepicker({
        //    opens: (App.isRTL() ? 'left' : 'right'),
        //    format: 'MM/DD/YYYY',
        //    separator: ' to ',
        //    startDate: moment().subtract('days', 29),
        //    endDate: moment(),
        //    minDate: '01/01/2009',
        //    maxDate: '12/31/2020',
        //    ranges: {
        //        '今天': [moment(), moment()],
        //        '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
        //        '前七天': [moment().subtract('days', 6), moment()],
        //        '前30天': [moment().subtract('days', 29), moment()],
        //        '这个月': [moment().startOf('month'), moment().endOf('month')],
        //        '前个月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
        //    },
        //    buttonClasses: ['btn'],
        //    applyClass: 'green',
        //    cancelClass: 'default',
        //    locale: {
        //        applyLabel: '应用',
        //        cancelLabel: '取消',
        //        fromLabel: '从',
        //        toLabel: '到',
        //        customRangeLabel: '自定义时间',
        //        daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
        //        monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
        //        firstDay: 1
        //    }
        //},
        //    function (start, end) {
        //        $('#defaultrange_modal input').val(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
        //    }
        //);

   
        //if (!jQuery().daterangepicker) {
        //    return;
        //}

        $('#rangepicker').daterangepicker({
            opens: 'left',//(App.isRTL() ? 'left' : 'right'),
            // format: 'MM/DD/YYYY',
            separator: ' to ',
            startDate: moment().subtract('days', 29),
            endDate: moment(),
            minDate: '01/01/2009',
            maxDate: '12/31/2020',
            showDropdowns: true,
            showWeekNumbers: true,
            timePicker: false,
            timePickerIncrement: 1,
            timePicker12Hour: true,
            ranges: {
                '今天': [moment(), moment()],
                '昨天': [moment().subtract('days', 1), moment().subtract('days', 1)],
                '前七天': [moment().subtract('days', 6), moment()],
                '前30天': [moment().subtract('days', 29), moment()],
                '这个月': [moment().startOf('month'), moment().endOf('month')],
                '前个月': [moment().subtract('month', 1).startOf('month'), moment().subtract('month', 1).endOf('month')]
            },
            buttonClasses: ['btn'],
            applyClass: 'green',
            cancelClass: 'default',
            locale: {
                applyLabel: '应用',
                cancelLabel: '取消',
                fromLabel: '从',
                toLabel: '到',
                customRangeLabel: '自定义时间',
                daysOfWeek: ['日', '一', '二', '三', '四', '五', '六'],
                monthNames: ['一月', '二月', '三月', '四月', '五月', '六月', '七月', '八月', '九月', '十月', '十一月', '十二月'],
                firstDay: 1
            }
        },
            function (start, end) {
                $('#rangepicker input').val(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
              
            }
        );


        //Set the initial state of the picker label
        $('#reportrange span').html(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        //$('#RangeDate').val(moment().subtract('days', 29).format('MMMM D, YYYY') + ' - ' + moment().format('MMMM D, YYYY'));
        //$('#reportrange span').html(moment().subtract('days', 29) + ' - ' + moment());
        // $('#RangeDate').val(moment().subtract('days', 29) + ' - ' + moment());
        // this is very important fix when daterangepicker is used in modal. in modal when daterange picker is opened and mouse clicked anywhere bootstrap modal removes the modal-open class from the body element.
        // so the below code will fix this issue.
        $('#reportrange').on('click', function () {
            if ($('#Modal').is(":visible") && $('body').hasClass("modal-open") == false) {
                $('body').addClass("modal-open");
            }
        });
    };
    var handleRootPath = function () {
        //获取当前网址，如： http://localhost:8083/uimcardprj/share/meun.aspx
        var curWwwPath = window.document.location.href;
        //获取主机地址之后的目录，如： uimcardprj/share/meun.jsp
        var pathName = window.document.location.pathname;
        var pos = curWwwPath.indexOf(pathName);
        //获取主机地址，如： http://localhost:8083
        var localhostPath = curWwwPath.substring(0, pos);
        //获取带"/"的项目名，如：/uimcardprj
        var projectName = pathName.substring(0, pathName.substr(1).indexOf('/') + 1);
        //return (localhostPaht + projectName);
        return (localhostPath);
    }

    var text = $('#submitbutton').text();
    //设定倒数秒数
    var st = 60;
    var handleShowTime = function (rootUrl, uniqueId) {
        st -= 1;
        var tt = setTimeout(handleShowTime(rootUrl, uniqueId), 1000);
        $('#submitbutton').text(text + "(" + st + "秒)");
        if (st == 0) {
            clearTimeout(tt);
            $('#submitbutton').text(text);
            $('#submitbutton').attr('disabled', "true");
            $('#cancel').attr('disabled', "true");
            $.post(rootUrl + "/Ims/TransactionDetail/StartLongRunningProcess",
                {
                    "PaypalApi": $("#PaypalApi").val(),
                    "RangeDate": $('#rangepicker input').val(), id: uniqueId
                },
                function () {
                    $('#progress').show();
                    handleStatus(rootUrl, uniqueId);
                    st = 60;
                });
        }
        //每秒执行一次,showTime()


    }
    //var uniqueId = "@Guid.NewGuid().ToString()";
    var t;
    var handleStatus = function (rootUrl, uniqueId) {
        var url = rootUrl + "/ims/transactiondetail/getcurrentprogress/" + uniqueId;//"@Url.StaticFile(" / Ims / TransactionDetail / GetCurrentProgress / ")" + uniqueId;
        var get = $.get(url, function (result) {
            t = window.setTimeout(handleStatus(rootUrl, uniqueId), 3000)
            //t = window.setTimeout("getStatus()", 3000);
            $('#RangeDate').attr('readonly', 'readonly');
            $('#PaypalApi').attr('readonly', 'readonly');
            $('#submitbutton').attr('disabled', "true");
            $('#cancel').attr('disabled', "true");
            var data = result.data;
            if (data.percentage == 0) {
                $('#txtprocesss').text('准备中....');
                $("#progress").attr("data-percent", "准备中....");
            } else if (data.percentage == 100) {
                var startdate = moment($('#rangepicker input').val().split('-')[0]).format('YYYY-MM-DD');
                var enddate = moment(data.enddate).format('YYYY-MM-DD');
                if (startdate == enddate) {
                    window.location.href = rootUrl + "/Ims/TransactionDetail/Index"; //'@Url.StaticFile("/Ims/TransactionDetail/Index")';
                } else {
                    clearTimeout(t);
                    $('.table-x').DataTable().ajax.reload();
                    var start = $('#rangepicker input').val().split('-')[0];
                    var end = moment(data.enddate).format('MMMM D, YYYY');//$('#rangepicker input').val().split('-')[1];
                    var datarange = start + '-' + end;
                    $('#rangepicker input').val(datarange);
                    $('#txtprocesss').text(' ');
                    $("#progressbar").attr("style", "width:0%")
                    $('#submitbutton').removeAttr("disabled");
                    $('#cancel').removeAttr("disabled");
                    handleShowTime(rootUrl, uniqueId);
                    // clearTimeout(tt);
                }
            } else {
                $('#txtprocesss').text("共有:" + data.total + "条," + "正在处理第" + data.number + "条," + "已经同步" + data.percentage + "%," + "最终同步日期:" + data.enddate);
                $("#progressbar").attr("style", "width:" + data.percentage + "%")
            }
        });
    }
    var handleButton = function (rootUrl, uniqueId) {
        $('#submitbutton').click(function () {
            
            $.post(rootUrl+"/Ims/TransactionDetail/StartLongRunningProcess",
                        {
                            "PaypalApi": $("#PaypalApi").val(),
                            "RangeDate": $('#rangepicker input').val(), id: uniqueId
                        },
                   function () {
                       $('#progress').show();
                       handleStatus(rootUrl, uniqueId);
                   });
        })

        $('#cancel').click(function () {
            clearTimeout(t);
        });
    }

    var handleDeleteExpress = function () {
        $("#express").select2('data', null);
        $("#express option").each(function () {
            $("#express option").remove();
        });
        $("#express").select2();
        //console.log($("#express").select2());
    }

    var handleEditables = function () {
        //set editable mode based on URL parameter
        if (App.getURLParameter('mode') == 'inline') {
            $.fn.editable.defaults.mode = 'inline';
            $('#inline').attr("checked", true);
            jQuery.uniform.update('#inline');
        } else {
            $('#inline').attr("checked", false);
            jQuery.uniform.update('#inline');
        }

           
        // console.log($(".country"));
        //global settings 
        $.fn.editable.defaults.inputclass = 'form-control';
        // $.fn.editable.defaults.url = '/post';

           
        //$('.association').editable({
        //        url: '/Ims/Association/Create',
        //        success: function () {
        //            $('#PaymentItem').load('/Ims/TransactionDetail/PaymentItemList/@Model.ID');
        //            //alert('sadsadadsa');
        //        },
        //        display: function (value) {
        //            if (!value) {
        //                $(this).html('请匹配');
        //                return;
        //            }                   
        //        }
        //    });
    }

    var handleExpress = function (rootUrl, id) {

        $('#typegroup').hide();
        $('#warehousegroup').hide();
        $('#expressgroup').hide();
        $('#expressdetail').hide();
        var dispatchorder = {};
        dispatchorder.transactiondetailid = id;

        $('#agent').select2({
            placeholder: "选择物流代理",
            allowClear: true,
            language: "zh-CN",
        }).on("change", function (data) {
            $('#showagent').text($('#agent').select2('data').text);                   
            $("#type").select2('data');
            $('typegroup').hide(function () {
                handleDeleteExpress();
            });
            $('#warehousegroup').hide();
            $('#expressgroup').hide();
            switch ($('#agent').select2('data')[0].id) {
                case "1":
                    //console.log('sadsadada' + $('#agent').select2('data')[0].id);
                    $('#type').find('option[value=1]').removeAttr('disabled');
                    $("#typegroup").show();
                    dispatchorder.agent = $('#agent').select2('data')[0].id;

                    $('#ck1_packing').show();
                    $('#ck1_weight').show();
                    break;

                case "2":
                   // console.log('11111xxxxx' + $('#agent').select2('data')[0].id);
                    $('#type').find('option[value=1]').attr('disabled', 'disabled');
                    $("#typegroup").show();
                    dispatchorder.agent = $('#agent').select2('data')[0].id;
                    $('#ck1_packing').hide();
                    $('#ck1_weight').hide();
                    break;
                            
                default:
                    $("#typegroup").hide();
                    $('#warehousegroup').hide();
                    $('#expressgroup').hide();
                    break;
            }
        });

        $('#type').select2({
            placeholder: "发货类型",
            allowClear: true,
            language: "zh-CN",
        }).on("change", function (e) {
            $("#warehouse").select2('data', null);
            handleDeleteExpress();
            if ($('#type').select2('data')[0].id) {
                        
                $('#showtype').text($('#type').select2('data').text);
                dispatchorder.type = $('#type').select2('data')[0].id;
                switch ($('#type').select2('data')[0].id) {
                    case "1":
                        $('#warehousegroup').show(function () {
                            $.getJSON(rootUrl+"/Ims/TransactionDetail/GetWarehouseList/?agentid=" + dispatchorder.agent, null,
                                function (data) {
                                    $("<option value=''></option>").appendTo($('#warehouse'));
                                    $.each(data, function (i, n) {
                                        $("<option value=" + n.Code + ">" + n.Code + '---' + n.Name + "</option>").appendTo($('#warehouse'));
                                    });
                                });
                        });
                        $('#expressdetail').hide();
                                
                        break;
                    case "2":
                        $("#expressdetail").show(function () {

                        });
                        $('#expressgroup').show(
                            function () {
                                $.ajax({
                                    type: "GET",
                                    cache: false,
                                    delay: 250,
                                    url: rootUrl+"/Ims/TransactionDetail/GetExpressList/?agentid=" + dispatchorder.agent,
                                    beforeSend: function () {
                                        //$('#express').select2({
                                        //    placeholder: "代理",
                                        //});
                                    },                                           
                                    success: function (data) {
                                        $("<option value=''></option>").appendTo($('#express'));
                                        $.each(data, function (i, n) {
                                            $("<option value=" + n.Code + ">" + n.Code + '---' + n.Name + "</option>").appendTo($('#express'));
                                        });
                                    }
                                });
                            }
                           );
                        $('#expressdetail').show();
                        $('#warehousegroup').hide();
                        break;
                }
            } else {
                $('#warehousegroup').hide();
                $('#expressgroup').hide(function () {
                    handleDeleteExpress();
                });
            }
        });

        $('#warehouse').select2({
            placeholder: "选择仓库",
            allowClear: true,
            language: "zh-CN",
        }).on("change", function (e) {
            $('#showwarehouse').text($('#warehouse').select2('data').text);
            handleDeleteExpress();
            
            if ($('#warehouse').select2('data')[0].id) {
                $("#express").select2('data', null);
                dispatchorder.warehouse = $('#warehouse').select2('data')[0].id;
                $.getJSON(rootUrl+"/Ims/TransactionDetail/GetOutboundExpressList/?agentid=" + dispatchorder.agent + "&warehouse=" + $('#warehouse').select2('data')[0].id,
                    null,
                    function (data) {
                        console.log($('#express').select2('data'));
                        $('#express').select2('data', null)
                        $("<option value=''></option>").appendTo($('#express'));
                        $.each(data, function (i, n) {
                            $("<option value=" + n.Code + ">" + n.Code + '---' + n.Name + "</option>").appendTo($('#express'));
                        });
                    });
                $('#expressgroup').show();
            } else {
                $('#expressgroup').hide();
            }
        });

        $('#express').select2({
            placeholder: "快递方式",
            allowClear: true,
        }).on("change", function (e) {
            $('#showexpress').text($('#express').select2('data').text);
            dispatchorder.express = $('#express').select2('data')[0].id;
        });

                
        var form = $('#submit_form');
        var error = $('.alert-danger', form);
        var success = $('.alert-success', form);

        form.validate({
            doNotHideMessage: true, //this option enables to show the error/success messages on tab switch.
            errorElement: 'span', //default input error message container
            errorClass: 'help-block help-block-error', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            rules: {
                agent: {
                    required: true
                },
                type: {
                    required: true
                },
                express: {
                    required: true
                },
            },

            //messages: { // custom messages for radio buttons and checkboxes
            //    'payment[]': {
            //        required: "Please select at least one option",
            //        minlength: jQuery.validator.format("Please select at least one option")
            //    }
            //},

            errorPlacement: function (error, element) { // render error placement for each input type
                       
            },

            invalidHandler: function (event, validator) { //display error alert on form submit
                success.hide();
                error.show();
                App.scrollTo(error, -200);
            },

            highlight: function (element) { // hightlight error inputs
                $(element)
                    .closest('.form-group').removeClass('has-success').addClass('has-error'); // set error class to the control group
            },

            unhighlight: function (element) { // revert the change done by hightlight
                $(element)
                    .closest('.form-group').removeClass('has-error'); // set error class to the control group
            },

                    

            submitHandler: function (form) {
                success.show();
                error.hide();
                //add here some ajax code to submit your form or just call form.submit() if you want to submit the form without ajax
            }

        });

        var handleTitle = function (tab, navigation, index) {
            var total = navigation.find('li').length;
            var current = index + 1;
            // set wizard title
            $('.step-title', $('#form_wizard_1')).text('Step ' + (index + 1) + ' of ' + total);
            // set done steps
            jQuery('li', $('#form_wizard_1')).removeClass("done");
            var li_list = navigation.find('li');
            for (var i = 0; i < index; i++) {
                jQuery(li_list[i]).addClass("done");
            }

            if (current == 1) {
                $('#form_wizard_1').find('.button-previous').hide();
            } else {
                $('#form_wizard_1').find('.button-previous').show();
            }

            if (current >= total) {
                $('#form_wizard_1').find('.button-next').hide();
                $('#form_wizard_1').find('.button-submit').show();
                        
            } else {
                $('#form_wizard_1').find('.button-next').show();
                $('#form_wizard_1').find('.button-submit').hide();
            }
            App.scrollTo($('.page-title'));
        }

        // default form wizard
        $('#form_wizard_1').bootstrapWizard({
            'nextSelector': '.button-next',
            'previousSelector': '.button-previous',
            onTabClick: function (tab, navigation, index, clickedIndex) {
                return false;
                        
                success.hide();
                error.hide();
                if (form.valid() == false) {
                    return false;
                }
                handleTitle(tab, navigation, clickedIndex);
                       
            },
            onNext: function (tab, navigation, index) {
                success.hide();
                error.hide();

                if (form.valid() == false) {
                    return false;
                }

                handleTitle(tab, navigation, index);
            },
            onPrevious: function (tab, navigation, index) {
                success.hide();
                error.hide();

                handleTitle(tab, navigation, index);
            },
            onTabShow: function (tab, navigation, index) {
                var total = navigation.find('li').length;
                var current = index + 1;
                var $percent = (current / total) * 100;
                $('#form_wizard_1').find('.progress-bar').css({
                    width: $percent + '%'
                });
            }
        });

        $('#form_wizard_1').find('.button-previous').hide();
        $('#form_wizard_1 .button-submit').click(function () {
            var postdata = {};
            switch (dispatchorder.agent) {
                case "1"://ck1                                        
                    var postdata = {
                        "Express": dispatchorder.express,
                        "Agent": dispatchorder.agent,
                        "ID": dispatchorder.transactiondetailid,
                        "Type": dispatchorder.type,
                        "Warehouse": dispatchorder.warehouse,
                        "goodsDescription": $("#goodsDescription").val(),
                        "goodsQuantity": $("#goodsQuantity").val(),
                        "goodsDeclareWorth": $("#goodsDeclareWorth").val(),
                        "detailDescriptionCN": $("#detailDescriptionCN").val(),
                        "goodsWeight": $("#goodsWeight").val(),
                        "size": $("#size").val(),
                        "Length": $("#length").val(),
                        "Width": $("#width").val(),
                        "Height": $("#height").val()
                    };
                    break;

                case "2"://sfc
                    var postdata = {
                        "Express": dispatchorder.express,
                        "Agent": dispatchorder.agent,
                        "ID": dispatchorder.transactiondetailid,
                        "Type": dispatchorder.type,
                        "Warehouse": dispatchorder.warehouse,
                        "goodsDescription": $("#goodsDescription").val(),
                        "goodsQuantity": $("#goodsQuantity").val(),
                        "goodsDeclareWorth": $("#goodsDeclareWorth").val(),
                        "detailDescriptionCN": $("#detailDescriptionCN").val(),
                        "goodsWeight": $("#goodsWeight").val()

                    };
                    break;
            }

            $.ajax({
                type: "POST",
                cache: false,
                data: postdata,
                url: rootUrl+"/Ims/TransactionDetail/Dispatch", 
                success: function (data) {
                    if (data.Message) {
                        alert(data.Message);
                    }
                    window.location.href=rootUrl+"/Ims/TransactionDetail/Index"
                }
        });

        // alert('Finished! Hope you like it :)');
    }).hide();

    }


    return {

        //main function to initiate the module
        init: function () {
            handleFilter();
            handleDatePickers();
            handleTransaction();
        },
        sync: function (uniqueId) {
            handleDateRangePickers();
            handleButton(handleRootPath(), uniqueId);
           // handleStatus(handleRootPath(), uniqueId);
           // handleShowTime(handleRootPath(), uniqueId);
        },
        dispatch:function(id){
            handleEditables();
            handleExpress(handleRootPath(),id)
        }

    };
}();

jQuery(document).ready(function () {
    Transaction.init();
});