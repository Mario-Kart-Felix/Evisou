/**
Custom module for you to write your own javascript functions
**/
var Custom = function () {

    // private functions & variables

    var myFunc = function(text) {
        alert(text);
    }
    var handleInputMasks = function () {
        $.extend($.inputmask.defaults, {
            'autounmask': true
        });

       
        $("#mask_phone").inputmask("mask", {
            "mask": "(999)-99999999"
        });
        $("#mask_mphone").inputmask("mask", {
            "mask": "999-9999-9999",
            "placeholder": "999-9999-9999"
        });
        /*$(".mask_size").inputmask("mask", {
           // "mask": "99*99*99",           
           // "placeholder": "L*W*H"
        });*/
        //specifying fn & options
        $("#mask_tin").inputmask({
            "mask": "99-9999999"
        }); //specifying options only
        $("#mask_number").inputmask({
            "mask": "9",
            "repeat": 10,
            "greedy": false
        }); // ~ mask "9" or mask "99" or ... mask "9999999999"       
    }
    // public functions
    return {

        //main function
        init: function () {
            handleInputMasks();
            //initialize here something.            
        },

        //some helper function
        doSomeStuff: function () {
            myFunc();
        }

    };

}();

/***
Usage
***/
//Custom.init();
//Custom.doSomeStuff();