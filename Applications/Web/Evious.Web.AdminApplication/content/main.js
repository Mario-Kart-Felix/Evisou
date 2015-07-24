$(function () {

    
    var $createDestroy = $('#switch-create-destroy');



    // initialize all the inputs
    $('input[type="checkbox"], input[type="radio"]:not("#switch-create-destroy, #switch-modal")').bootstrapSwitch();



    $('[data-switch-create-destroy]').on('click', function () {
        var isSwitch = $createDestroy.data('bootstrap-switch');

        $createDestroy.bootstrapSwitch(isSwitch ? 'destroy' : null);
        $(this).button(isSwitch ? 'reset' : 'destroy');
    });

    $('#modal-switch');
});