var Evious = function () {

    var handleBaseDataTable = function (colsObject, url) {
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
            src: $("#datatable_users"),
            onSuccess: function (grid) {
                // execute some code after table records loaded
            },
            onError: function (grid) {
                // execute some code on network or other general error  
            },
            loadingMessage: '载入中...',
            dataTable: {
                "aoColumns": colsObject,
                "lengthMenu": [
                    [10, 20, 50, 100, 150],
                    [10, 20, 50, 100, 150] // change per page values here 
                ],
                "pageLength": 10, // default record count per page                
                "ajax": {
                    "url": url, // ajax source
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
    };
    var handleFilter = function () {
        $('tr.filter').hide();
        $('a.search-table').click(function () {
            $('tr.filter').fadeToggle("slow");
        })
    };
    
    var handleCheckForm = function (rulesObject) {

        var userForm = $("#mainForm");
        var error2 = $('.alert-danger', userForm);
        var success2 = $('.alert-success', userForm);

        //去除验证后表单的css
        var removeFormValidateCss = function () {
            $('#mainForm')[0].reset();
            $('.form-group').removeClass("has-success").removeClass("has-error");
            $('div.input-icon>i.fa').removeClass("fa-warning").removeClass("fa-check");
        };

        

        userForm.validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block help-block-error', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",  // validate all fields including form hidden input
            rules: rulesObject,

            invalidHandler: function (event, validator) { //display error alert on form submit
                success2.hide();
                error2.show();
                //app.scrollTo(error2, -200);

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
                success2.show();
                error2.hide();
                //form[0].submit(alert('dasdas')); // submit the form
            }
        });

        //modal动作
        $('div[data-modal-form]').on('hide.bs.modal', function () {
            console.log(userForm.serialize());
            removeFormValidateCss();
        });
        $("a[data-modal-form-add]").click(function () {
            $('h4.modal-title').text('添加新用户');
        });
        
        //表单动作
        $("button[data-modal-form-submit]").click(function (e) {
            // handleFormCss();
            if (!userForm.valid()) {
                e.preventDefault();
                return false;
            } else {
                userForm.submit(function () {
                   // console.log(this);
                    $.ajax({
                        url: this.action,
                        type: this.method,
                        beforeSend: function () {

                        },
                        contentType:"application/x-www-form-urlencoded",
                        data: $(this).serialize(),
                        success: function (result) {
                            console.log(result);
                            //if (result.ReturnStatus) {
                            //    $('#Modal').modal('hide');
                            //    $('#replacetarget').load(result.url);
                            //     window.location.href = result.url;
                            //    $('.table-x').DataTable().ajax.reload();
                            //    window.load(result.url); //  Load data from the server and place the returned HTML into the matched element
                            //} else {
                            //    $('#ModalContent').html(result);
                            //    bindForm();
                            //}
                        }
                    });



                    console.log(userForm.serialize());
                    removeFormValidateCss();
                });
            }
            
        });
        $("button[data-modal-form-reset]").click(function (e) {
            
        });

        
        
    };
    return {
        init: function (colsObject, url,rulesObject) {
            handleBaseDataTable(colsObject, url);
            handleCheckForm(rulesObject)
            handleFilter();
        }
    }
}();