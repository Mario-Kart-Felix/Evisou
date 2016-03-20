var Channel = function () {
    var handleChannel = function () {
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
            src: $("#datatable_channel"),
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
                     {
                         "aDataSort": [3],
                         "mData": "IsActive",
                         "mRender": function (source, type, val) {
                             var str = "<span class=\"label label-sm label-";
                             str += val[3] == "True" ? "success" : "danger";
                             str += "\">" + val[3] + "</span>";
                             return str;
                             // "<span class=\"label label-sm label-success\">" + val[5] + "</span>"
                         }

                     },
                     {
                         "aDataSort": [4],
                         "mData": "ID",
                         "bSortable": false,
                         "mRender": function (source, type, val) {
                             var str = " <a class=\"btn btn-xs purple thickbox\" title=\'编辑文章频道\' data-modal href=\"Edit/" + val[0] + "\">";
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
    var handleFancyBox = function () {
        $(".fancybox-button").fancybox({
            'titleShow': true,   //是否显示标题
            'showCloseButton': true,
        });
    };
    var handleCheckBox = function () {
        $('.switch-chekcbox').bootstrapSwitch();
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
                Desc: {
                    minlength: 2,
                    required: true,
                }
            },
            invalidHandler: function (event, validator) { //display error alert on form submit
                success.hide();
                error.show();
               // Metronic.scrollTo(error, -200);
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
    var handleUploadInput = function (rootUrl) {
        var url = rootUrl + "/UploadHandler.ashx";
        var coverimage = $('#coverimage');
        var deletebutton = $('#deleteuploaded');
        var filename;
        var ajaxDelete = function (type, url) {
            $.ajax({
                xhrFields: { withCredentials: true },
                url: url,
                type: type,
                success: function (response) {
                    $('#fileupload').prop('disabled', false);
                    $('#thumbnail').html('<img src="http://www.placehold.it/200x150/EFEFEF/AAAAAA&amp;text=no+image" alt="" />');
                    $('#progress').hide(function () {
                        $('#progress .progress-bar').css('width', '0%');
                    });
                    deletebutton.hide();
                    coverimage.val('');
                }
            })
        }
        if (coverimage.val() != '') {
            var deleteUrl = rootUrl + "/UploadHandler.ashx?f=" + coverimage.val();
           
            deletebutton.show(function () {
               deletebutton.attr("data-type", "DELETE").attr("data-url", deleteUrl)
            });
            
            //deletebutton.click(ajaxDelete('DELETE', deleteUrl));
            deletebutton.click(function () {
                $.ajax({
                    xhrFields: { withCredentials: true },
                    url: deleteUrl,
                    type: 'DELETE',
                    success: function (response) {
                        $('#fileupload').prop('disabled', false);
                        $('#thumbnail').html('<img src="http://www.placehold.it/200x150/EFEFEF/AAAAAA&amp;text=no+image" alt="" />');
                        $('#progress').hide(function () {
                            $('#progress .progress-bar').css('width', '0%');
                        });
                        deletebutton.hide();
                        coverimage.val('');
                    }
                })
            });
          
        } else {
            uploadButton = $('<button/>')
            .addClass('btn btn-primary')
            .prop('disabled', true)
            .prop('type', 'button')
            .text('处理中...')
            .on('click', function () {
                var $this = $(this),
                    data = $this.data();
                $this
                    .off('click')
                    .text('终止')
                    .on('click', function () {
                        $this.remove();
                        data.abort();
                    });
                data.submit().always(function (data) {
                    $this.remove();
                });
            });

            $('#deleteuploaded').hide();
            $('#progress').hide();
            $('#fileupload').fileupload({
                url: url,
                dataType: 'json',
                autoUpload: false,
                maxNumberOfFiles: 1,
                limitConcurrentUploads: 1,
                acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
                maxFileSize: 5000000, // 5 MB           
                disableImageResize: /Android(?!.*Chrome)|Opera/
                    .test(window.navigator.userAgent),
                previewMaxWidth: 180,
                previewMaxHeight: 140,
                previewCrop: true
            }).on('fileuploadadd', function (e, data) {
                if ($("#thumbnail:has(div)").length == 0) {
                    $('#uploadmsg').empty();
                    $('#progress').hide(function () {
                        $('#progress .progress-bar').css('width', '0%');
                    });

                    $('#thumbnail').empty();
                    data.context = $('<div/>').appendTo('#thumbnail');
                    //$('#uploadtext').text('更改');
                    $.each(data.files, function (index, file) {
                        var node = $('<p/>').append($('<span/>').text(file.name));
                        if (!index) {
                            $('#uploadmsg').append(uploadButton.clone(true).data(data));
                            filename = file.name;
                        }
                        node.appendTo(data.context);
                    });
                } else {
                    $('#uploadmsg').empty();
                    $('#progress').hide(function () {
                        $('#progress .progress-bar').css('width', '0%');
                    });

                    $('#thumbnail').empty();
                    data.context = $('<div/>').appendTo('#thumbnail');
                    $.each(data.files, function (index, file) {
                        var node = $('<p/>').append($('<span id="filename"/>').text(file.name));
                        if (!index) {
                            $('#uploadmsg').append(uploadButton.clone(true).data(data));
                            filename = file.name;
                        }
                        node.appendTo(data.context);
                    });
                }

            }).on('fileuploadprocessalways', function (e, data) {
                var index = data.index,
                    file = data.files[index],
                    node = $(data.context.children()[index]);
                if (file.preview) {
                    node
                        .prepend('<br>')
                        .prepend(file.preview);
                }
                if (file.error) {
                    $('#uploadmsg')
                        .append('<br>')
                        .append($('<span class="text-danger"/>').text(file.error));
                }
                if (index + 1 === data.files.length) {
                    $('#uploadmsg').find('button')
                       // .text('上传')
                        .html('<i class="fa fa-upload"></i>')
                        .prop('disabled', !!data.files.error);
                }
                $('#progress').show();
            }).on('fileuploadprogressall', function (e, data) {
                $('#progress').show();
                var progress = parseInt(data.loaded / data.total * 100, 10);
                $('#progress .progress-bar').css(
                    'width',
                    progress + '%'
                );
            }).on('fileuploaddone', function (e, data) {
                $('#fileupload').prop('disabled', true);
                $.each(data.result.files, function (index, file) {
                    if (file.url) {
                        //console.log(file.deleteType);
                        var link = $('<a>')
                            .addClass('fancybox-button')
                            .prop('title', file.name)
                            //.attr('target', '_blank')
                            .prop('href', file.url)
                            .attr('data-ref', 'fancybox-button')
                            .text(file.name);

                        $(data.context.children()[index])
                            .wrap(link);

                        deletebutton.show(function () {
                            deletebutton.attr("data-type", file.deleteType).attr("data-url", file.deleteUrl)
                        });

                        deletebutton.click(function () {
                            $.ajax({
                                xhrFields: { withCredentials: true },
                                url: file.deleteUrl,
                                type: 'DELETE',
                                success: function (response) {
                                    $('#fileupload').prop('disabled', false);
                                    $('#thumbnail').html('<img src="http://www.placehold.it/200x150/EFEFEF/AAAAAA&amp;text=no+image" alt="" />');
                                    $('#progress').hide(function () {
                                        $('#progress .progress-bar').css('width', '0%');
                                    });
                                    deletebutton.hide();
                                    coverimage.val('');
                                }
                            })
                        });
                        //deletebutton.click(ajaxDelete('DELETE', file.deleteUrl));
                        coverimage.val(file.url);
                        //.append(deleteButton.clone(true));

                    } else if (file.error) {
                        var error = $('<span class="text-danger"/>').text(file.error);
                        //$(data.context.children()[index])
                        $('#uploadmsg')
                            .append('<br>')
                            .append(error);
                    }
                });
            }).on('fileuploadfail', function (e, data) {
                $.each(data.files, function (index) {
                    var error = $('<span class="text-danger"/>').text('文件上传失败.');
                    //$(data.context.children()[index])
                    $('#uploadmsg')
                        .append('<br>')
                        .append(error);
                });
            }).prop('disabled', !$.support.fileInput)
            .parent().addClass($.support.fileInput ? undefined : 'disabled')//;
        .bind('fileuploadsubmit', function (e, data) {
            data.formData =
                [{
                    name: "subfolder",
                    value: "cms"
                }, {
                    name: 'Filename',
                    value: filename
                }];
        });
        }
        
        
    }

    return {
        //main function to initiate the module
        init: function () {
            handleChannel();
            handleFilter();
        },
        form: function () {
            handleFancyBox();
            handleCheckBox();
            handleCheckForm();
            handleUploadInput(handleRootPath());
        }
    };
}();

jQuery(document).ready(function () {
    Channel.init();
});