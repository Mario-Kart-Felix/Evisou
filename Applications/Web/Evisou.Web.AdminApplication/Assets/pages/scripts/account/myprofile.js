var MyProfile = function () {
    var f = '';
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
    var ajaxUpload = function (rooturl) {
        var oMyForm = new FormData();
        oMyForm.append("subfolder", "account");//
        oMyForm.append("files[]", $("#img")[0].files[0]);
        oMyForm.append("Filename", $("#img")[0].files[0].name);
        $.ajax({
            type: 'POST',
            url: rooturl+"/UploadHandler.ashx",
            data: oMyForm,//new FormData(formCol[0]),
            processData: false,
            contentType: false,
            success: function (data) {
                f = $.parseJSON(data).files[0].deleteUrl;
                $('#avatar').attr('src', $.parseJSON(data).files[0].url);
                $('#headeravatar').attr('src', $.parseJSON(data).files[0].url);
                $.cookie('headeravatar', $.parseJSON(data).files[0].url, { expires: 7, path: '/' });
                console.log($.cookie('headeravatar'));
                $('#myavatar').val($.parseJSON(data).files[0].url);
            }
        });
    };
    var handleUpload = function () {
        var oavator = $('#avatar').attr('src');
        $('.fileinput').on('change.bs.fileinput', function (e) {
            if (f == '') {
                ajaxUpload(handleRootPath())
            } else {
                $.ajax({
                    xhrFields: { withCredentials: true },
                    url: f,
                    type: 'DELETE',
                    success: function (data) {
                        f = "";
                        $('#avatar').attr('src', oavator);
                        $('#myavatar').val("");
                        ajaxUpload(handleRootPath());
                    }
                })
            }
        })

        $('.fileinput').on('clear.bs.fileinput', function (e) {
            $.ajax({
                xhrFields: { withCredentials: true },
                url: f,
                type: 'DELETE',
                success: function (data) {
                    f = "";
                    $('#avatar').attr('src', oavator);
                    $('#myavatar').val("");

                }
            })
        })

        $('.fileinput').on('reset.bs.fileinput', function (e) {
            alert('345454355345435');
        })
    };
    return {
        init: function () {
            handleUpload();
        }
    };
}();

jQuery(document).ready(function () {
    MyProfile.init();
});