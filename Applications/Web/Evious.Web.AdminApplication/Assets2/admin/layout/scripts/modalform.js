// modalform.js


$(function () {
    $.fn.modal.defaults.spinner = $.fn.modalmanager.defaults.spinner =
              '<div class="loading-spinner" style="width: 200px; margin-left: -100px;">' +
                '<div class="progress progress-striped active">' +
                  '<div class="progress-bar" style="width: 100%;"></div>' +
                '</div>' +
              '</div>';
   // $.fn.modalmanager.defaults.resize = true;

    $('#Modal').on('hidden', function () {
        if ($("#ajax-modal").hasClass("container")) {
            $("#ajax-modal").removeClass("container");
        }

    })



    $.ajaxSetup({ cache: false });
    $(document).delegate("a[data-modal]", "click", function () {
        if ($("#Modal").hasClass("container")) {
            $("#Modal").removeClass("container");
        }
        $('body').modalmanager('loading');
        $('#ModalContent').load(this.href, function () {
            //$('#Modal').modal({
            //    /*backdrop: 'static',*/
            //    keyboard: true
            //}, 'show');
            $('#Modal').modal();
            bindForm(this);
        });
        return false;
    });
});

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
                    $('#Modal').modal('hide');
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