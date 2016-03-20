var Product = function () {
    var handleCheckBox = function () {
        $('.make-switch').bootstrapSwitch();
    };
    var handleProduct = function () {
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
            src: $("#datatable_product"),
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
                         "bSortable": false,
                     },
                     {
                         "aDataSort": [5],
                         "bSortable": false,
                     },
                     {
                         "aDataSort": [6],
                         "bSortable": false,
                     },
                     {
                         "aDataSort": [7],
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
    var handleFancybox = function () {
        if (!$(document).delegate.fancybox) {
            return;
        }

        if ($(document).delegate(".fancybox-button").size() > 0) {
            $(document).delegate(".fancybox-button").fancybox({
                groupAttr: 'data-rel',
                prevEffect: 'none',
                nextEffect: 'none',
                closeBtn: true,
                helpers: {
                    title: {
                        type: 'inside'
                    }
                }
            });
        }
    }
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
    var handleBatchUploadInput = function (rooturl,id) {
        $('#fileupload').find('thead').hide();
        // Initialize the jQuery File Upload widget:
        $('#fileupload').fileupload({
            disableImageResize: false,
            url: rooturl+"/UploadHandler.ashx",//
            autoUpload: false,
            disableImageResize: /Android(?!.*Chrome)|Opera/.test(window.navigator.userAgent),
            maxFileSize: 5000000,
            acceptFileTypes: /(\.|\/)(gif|jpe?g|png)$/i,
            formData: { subfolder: 'ims' }
            // Uncomment the following to send cross-domain cookies:
            //xhrFields: {withCredentials: true},                
        }).bind('fileuploadsubmit', function (e, data) {
            $('#fileupload').find('thead').show();
        });

        // Enable iframe cross-domain access via redirect option:
        $('#fileupload').fileupload(
            'option',
            'redirect',
            window.location.href.replace(
                /\/[^\/]*$/,
                '/cors/result.html?%s'
            )
        );
        // Load & display existing files:
        $('#fileupload').addClass('fileupload-processing');
        $.ajax({
            // Uncomment the following to send cross-domain cookies:
            //xhrFields: {withCredentials: true},
            url: rooturl+'/UploadHandler.ashx?productid='+id,//"@Url.StaticFile()/UploadHandler.ashx",//
            dataType: 'json',
            context: $('#fileupload')[0]
        }).always(function () {
            $(this).removeClass('fileupload-processing');
            // $('#fileupload').find('thead').show();
        }).done(function (result) {
            var i = result.files;
            if (typeof (result.files.length) != 'undefined') {
                $('#fileupload').find('thead').show();
            }
            $(this).fileupload('option', 'done')
            .call(this, $.Event('done'), { result: result });
        });

        //更改部分
        var fileUploadButtonBar = $('#fileupload').find('.fileupload-buttonbar');
        fileUploadButtonBar.find('.delete').click(function () {
            $('#fileupload').find('thead').hide();
        });
        $('.files').delegate(".btndelete", "click", function () {

            if ($('.files').find('.btndelete').length == 1) {
                $('#fileupload').find('thead').hide()
            }
        });
    }
    var handleProductCoverUpload = function (rooturl) {

        var url = rooturl+"/UploadHandler.ashx",
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

        var coverimge = $('#coverimage');

        var filename;

        $('#deleteuploaded').hide();
        $('#progress').hide();



        $('#productcoverupload').fileupload({

            url: url,//"@Url.StaticFile()/UploadHandler.ashx",
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
            previewCrop: true,
            //uploadTemplateId: null,
            //downloadTemplateId: null,
            filesContainer: $('div.files'),
            uploadTemplateId: null,
            downloadTemplateId: null,
            uploadTemplate: function (o) {
                var rows = $();
                $.each(o.files, function (index, file) {
                    $('div.files').empty();
                    var row = $('<div class="template-upload fade">' +
                        '<span class="preview"></span><div class="name"></div>' +
                        '</div>');
                    row.find('.name').text(file.name);

                    rows = row;
                });
                return rows;
            },

        }).on('fileuploadadd', function (e, data) {
            if ($("#thumbnail:has(div)").length == 0) {
                $('#uploadmsg').empty();
                //$('#progress').hide(function () {
                //    $('#progress .progress-bar').css('width', '0%');
                //});

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
                //$('#progress').hide(function () {
                //    $('#progress .progress-bar').css('width', '0%');
                //});

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
            $('#productcoverupload').prop('disabled', true);
            $.each(data.result.files, function (index, file) {

                if (file.url) {
                    var link = $('<a>')
                        .addClass('fancybox-button')
                        .prop('title', file.name)
                        //.attr('target', '_blank')
                        .prop('href', file.url)
                        .attr('data-ref', 'fancybox-button')
                        .text(file.name);
                    var deletebutton = $('#deleteuploaded');
                    $(data.context.children()[index])
                        .wrap(link);
                    $("div.name").empty();
                    link.appendTo($("div.name"));
                    deletebutton.show(function () {
                        deletebutton.attr("data-type", file.deleteType).attr("data-url", file.deleteUrl)
                    });

                    deletebutton.click(function () {

                        $.ajax({
                            xhrFields: { withCredentials: true },
                            url: file.deleteUrl,
                            type: 'DELETE',
                            success: function (response) {
                                $('#productcoverupload').prop('disabled', false);
                                $('#thumbnail').html('<img src="http://www.placehold.it/200x150/EFEFEF/AAAAAA&amp;text=no+image" alt="" />');
                                $('#progress').hide(function () {
                                    $('#progress .progress-bar').css('width', '0%');
                                });
                                deletebutton.hide();
                                coverimge.val('');
                            }
                        })
                    });
                    coverimge.val(file.url);
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
                value: "ims"
            }, {
                name: 'Filename',
                value: filename
            }];
    });
    }
    var handleFilter = function () {
        $('tr.filter').hide();
        $('a.search-table').click(function () {
            $('tr.filter').fadeToggle("slow");
        })
    }
    var handleReviews = function () {

        var grid = new Datatable();

        grid.init({
            src: $("#datatable_reviews"),
            onSuccess: function (grid) {
                // execute some code after table records loaded
            },
            onError: function (grid) {
                // execute some code on network or other general error  
            },
            loadingMessage: 'Loading...',
            dataTable: { // here you can define a typical datatable settings from http://datatables.net/usage/options 

                // Uncomment below line("dom" parameter) to fix the dropdown overflow issue in the datatable cells. The default datatable layout
                // setup uses scrollable div(table-scrollable) with overflow:auto to enable vertical scroll(see: assets/global/scripts/datatable.js). 
                // So when dropdowns used the scrollable div should be removed. 
                //"dom": "<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'<'table-group-actions pull-right'>>r>t<'row'<'col-md-8 col-sm-12'pli><'col-md-4 col-sm-12'>>",

                "lengthMenu": [
                    [10, 20, 50, 100, 150, -1],
                    [10, 20, 50, 100, 150, "All"] // change per page values here
                ],
                "pageLength": 10, // default record count per page
                "ajax": {
                    // "url": "demo/ecommerce_product_reviews.php", // ajax source
                },
                "columnDefs": [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                    'orderable': true,
                    'targets': [0]
                }],
                "order": [
                    [0, "asc"]
                ] // set first column as a default sort by asc
            }
        });
    }
    var handleHistory = function () {

        var grid = new Datatable();

        grid.init({
            src: $("#datatable_history"),
            onSuccess: function (grid) {
                // execute some code after table records loaded
            },
            onError: function (grid) {
                // execute some code on network or other general error  
            },
            loadingMessage: 'Loading...',
            dataTable: { // here you can define a typical datatable settings from http://datatables.net/usage/options 
                "lengthMenu": [
                    [10, 20, 50, 100, 150, -1],
                    [10, 20, 50, 100, 150, "All"] // change per page values here
                ],
                "pageLength": 10, // default record count per page
                "ajax": {
                    // "url": "demo/ecommerce_product_history.php", // ajax source
                },
                "columnDefs": [{ // define columns sorting options(by default all columns are sortable extept the first checkbox column)
                    'orderable': true,
                    'targets': [0]
                }],
                "order": [
                    [0, "asc"]
                ] // set first column as a default sort by asc
            }
        });
    }
    var initComponents = function () {
        //init datepickers
        $('.date-picker').datepicker({
            rtl: App.isRTL(),
            language: 'zh-CN',
            autoclose: true
        });

        //init datetimepickers
        $(".datetime-picker").datetimepicker({
            isRTL: App.isRTL(),
            autoclose: true,
            language: 'zh-CN',
            todayBtn: true,
            pickerPosition: (App.isRTL() ? "bottom-right" : "bottom-left"),
            minuteStep: 10
        });

        //init maxlength handler
        $('.maxlength-handler').maxlength({
            limitReachedClass: "label label-danger",
            alwaysShow: true,
            threshold: 5
        });
    }
    var handleSpinners = function () {
        $('.spinner').spinner();
        $('#spinner1').spinner();
        $('#spinner2').spinner({ disabled: true });
        $('#spinner3').spinner({ value: 0, min: 0, max: 10 });
        $('#spinner4').spinner({ value: 0, step: 5, min: 0, max: 200 });
    }

    return {

        //main function to initiate the module
        init: function () {
            handleProduct();
            handleDatePickers();
            handleFilter();
        },
        form: function (productId) {
            handleCheckBox();
            handleFancybox();
            handleProductCoverUpload(handleRootPath());
            handleBatchUploadInput(handleRootPath(), productId);
            handleReviews();
            handleHistory();
            initComponents();
            
        }

    };
}();
jQuery(document).ready(function () {
    Product.init();
});