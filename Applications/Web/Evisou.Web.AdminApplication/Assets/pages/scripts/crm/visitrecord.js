var VisitRecord = function () {
    var handleVisitRecord = function () {
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
            src: $("#datatable_visitrecord"),
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
                     { "aDataSort": [4] },
                     { "aDataSort": [5] },
                     { "aDataSort": [6] },
                     { "aDataSort": [7] },
                     { "aDataSort": [8] },
                     { "aDataSort": [9] },
                     { "aDataSort": [10] },
                     { "aDataSort": [11] },
                     { "aDataSort": [12] },
                     { "aDataSort": [13] },
                     {
                         "aDataSort": [14],
                         "mData": "ID",
                         "bSortable": false,
                         "mRender": function (source, type, val) {
                             var str = " <a class=\"btn btn-xs purple thickbox\" title=\'编辑用户资料\' data-modal href=\"Edit/" + val[0] + "\">";
                             str += "<i class=\"fa fa-edit\"></i>";
                             str += "编辑";
                             str += "</a>";
                             return str
                         }
                     },
                ],
                "lengthMenu": [
                    [10, 20, 50, 100, 150],
                    [10, 20, 50, 100, 150] // change per page values here 
                ],
                "pageLength": 10, // default record count per page                
                "ajax": {
                    "url": "Datatable", // ajax source
                },
                "order": [
                    [1, "asc"]
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
    var handleFilter = function () {
        $('tr.filter').hide();
        $('a.search-table').click(function () {
            $('tr.filter').fadeToggle("slow");
        })
    }

     var handleCombox = function () {

        $("#CityId").change(function () {
            $("#areas").load("GetArea")
        });

        $("#CustomerId").combobox();

    };
     var handleCheckForm = function () {
            var form = $("#mainForm");
            var error = $('.alert-danger', form);
            var success = $('.alert-success', form);

            form.validate({
                errorElement: 'span', //default input error message container
                errorClass: 'help-block help-block-error', // default input error message class
                focusInvalid: false, // do not focus the last invalid input
                ignore: "",  // validate all fields including form hidden input
                rules: {
                    Name: {
                        minlength: 2,
                        required: true,
                    },
                    BirthDate: {
                        minlength: 2,
                        required: true,
                    },
                    Email: {
                        minlength: 2,
                        required: true,
                    },
                    Tel: {
                        minlength: 11,
                        maxlength:11,
                        required: true,
                    },
                    Position: {
                        minlength: 1,
                        required: true,
                    }

                },

                invalidHandler: function (event, validator) { //display error alert on form submit
                    success.hide();
                    error.show();
                    Metronic.scrollTo(error, -200);

                },

                errorPlacement: function (error, element) { // render error placement for each input type
                    var icon = $(element).parent('.input-icon').children('i');
                    icon.removeClass('fa-check').addClass("fa-warning");
                    icon.attr("data-original-title", error.text()).tooltip({ 'container': 'body' });
                },

                highlight: function (element) { // hightlight error inputs
                    $(element)
                        .closest('.form-group').removeClass("has-success").addClass('has-error'); // set error class to the control group
                },

                unhighlight: function (element) { // revert the change done by hightlight

                },

                success: function (label, element) {
                    var icon = $(element).parent('.input-icon').children('i');
                    $(element).closest('.form-group').removeClass('has-error').addClass('has-success'); // set success class to the control group
                    icon.removeClass("fa-warning").addClass("fa-check");
                },

                submitHandler: function (form) {
                    success.show();
                    error.hide();
                    //form[0].submit(alert('dasdas')); // submit the form
                }
            });


            $("#submitbutton").click(function (e) {
                if (!form.valid()) {
                    e.preventDefault();
                }

            });
        };
     var handleDatePickers = function () {

         if (jQuery().datepicker) {
             $('.date-picker').datepicker({
                 language: 'zh-CN',
                 rtl: Metronic.isRTL(),
                 orientation: "left",
                 autoclose: true,
                 dateFormat: 'dd日mm月yy年'
             });
             //$('body').removeClass("modal-open"); // fix bug when inline picker is used in modal
         }

         /* Workaround to restrict daterange past date select: http://stackoverflow.com/questions/11933173/how-to-restrict-the-selectable-date-ranges-in-bootstrap-datepicker */
     }
    return {

        //main function to initiate the module
        init: function () {
            handleVisitRecord();
            handleFilter();
        },
        form:function(){
            handleCombox();
            handleCheckForm();
            handleDatePickers();
        }
    };
}();
jQuery(document).ready(function () {
    VisitRecord.init();
});