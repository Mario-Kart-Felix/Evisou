var Users = function () {
    var handleUsers = function () {

        var colsObject = [
                     {
                         "aDataSort": [0],
                         "mData": "ID",
                         "mRender": function (source, type, val) {
                             return "<input type=\"checkbox\" name=\"id[]\" value=" + val[0] + ">";
                         }
                     },
                     { "aDataSort": [1] },
                     { "aDataSort": [2] },
                     { "aDataSort": [3] },
                     {
                         "aDataSort": [4],
                         "bSortable": false,
                     },
                     {
                         "aDataSort": [5],
                         "mData": "IsActive",
                         "mRender": function (source, type, val) {
                             var str = "<span class=\"label label-sm label-";
                             str += val[5] == "True" ? "success" : "danger";
                             str += "\">" + val[5] + "</span>";
                             return str;
                             // "<span class=\"label label-sm label-success\">" + val[5] + "</span>"
                         }

                     },
                     {
                         "aDataSort": [6],
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
        ];
        var url = 'http://localhost:46088/Account/User/Datatable';
        var rulesObject = {
            LoginName: {
                minlength: 2,
                required: true,
            },
            Password: {
                minlength: 2,
                required: true
            },
            Email: {
                required: true,
                email: true
            },
            Mobile: {
                required: true,
                number: true,
                minlength: 11,
                maxlength: 11
            }
        };
        Evious.init(colsObject, url, rulesObject);
    }
    var handleFilter = function () {
        $('tr.filter').hide();
        $('a.search-table').click(function () {
            $('tr.filter').fadeToggle("slow");
        })
    }

    var handleCheckForm = function () {

        var userForm = $("#mainForm");
        var error2 = $('.alert-danger', userForm);
        var success2 = $('.alert-success', userForm);

        userForm.validate({
            errorElement: 'span', //default input error message container
            errorClass: 'help-block help-block-error', // default input error message class
            focusInvalid: false, // do not focus the last invalid input
            ignore: "",  // validate all fields including form hidden input
            rules: {
                LoginName: {
                    minlength: 2,
                    required: true,
                },
                Password: {
                    minlength: 2,
                    required: true
                },
                Email: {
                    required: true,
                    email: true
                },
                Mobile: {
                    required: true,
                    number: true,
                    minlength: 11,
                    maxlength: 11
                }
            },

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
        $("#submitBtn").click(function (e) {
            // handleFormCss();
            if (!userForm.valid()) {
                e.preventDefault();
            }
        });

        $("#resetBtn").click(function (e) {
            // userForm.validate().resetForm();
        });
    }
    var handleFormCss = function () {
        $('#mainForm')[0].reset();
        $('.form-group').removeClass("has-success").removeClass("has-error");
        $('div.input-icon>i.fa').removeClass("fa-warning").removeClass("fa-check");
    };

    var handleForm = function () {

        $("a[data-modal-form-add]").click(function () {
            $('h4.modal-title').text('添加新用户');
        })

        $('#resetBtn').click(function () {
            handleFormCss();
        });
        $('#myModal').on('hide.bs.modal', function () {
            console.log($("form").serialize());
            //alert($('#active').val())
            handleFormCss();

        });

    }
    return {
        init: function () {
            handleUsers();
            //handleFilter();
            //handleCheckForm();
            // handleForm();
        },

        form: function () {
            // handleCheckBox();
            handleCheckForm();
        }
    };
}();


function bindForm(dialog) {
    $('form', dialog).submit(function () {
        $.ajax({
            url: this.action,
            type: this.method,
            beforeSend: function () {

            },
            data: $(this).serialize(),
            success: function (result) {
                if (result.success) {
                    $('#myModal').modal('hide');
                    //$('#replacetarget').load(result.url);
                    // window.location.href = result.url;
                    $('.table-x').DataTable().ajax.reload();
                    //window.load(result.url); //  Load data from the server and place the returned HTML into the matched element
                } else {
                    $('#ModalContent').html(result);
                    bindForm();
                }
            }
        });

        return false;
    });
}


