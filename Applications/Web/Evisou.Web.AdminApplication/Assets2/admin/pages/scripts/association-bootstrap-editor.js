/**
Association editable input.
Internally value stored as {city: "Moscow", street: "Lenina", building: "15"}

@class association
@extends abstractinput
@final
@example
<a href="#" id="association" data-type="association" data-pk="1">awesome</a>
<script>
$(function(){
    $('#association').editable({
        url: '/post',
        title: 'Enter city, street and building #',
        value: {
            city: "Moscow", 
            street: "Lenina", 
            building: "15"
        }
    });
});
</script>
**/
(function ($) {
    "use strict";
    var str = "";
   

     
    var Association = function (options) {
        this.init('association', options, Association.defaults);
    };

    //inherit from Abstract input
    $.fn.editableutils.inherit(Association, $.fn.editabletypes.abstractinput);

    $.extend(Association.prototype, {
        /**
        Renders input from tpl

        @method render() 
        **/        
        render: function() {
            this.$input = this.$tpl.find('input');
            this.$select = this.$tpl.find('select');
        },
        
        /**
        Default method to show value in element. Can be overwritten by display option.
        
        @method value2html(value, element) 
        **/
        value2html: function(value, element) {
            if(!value) {
                $(element).empty();
                return; 
            }
            var html = $('<div>').text(value.city).html() + ', ' + $('<div>').text(value.street).html() + ' st., bld. ' + $('<div>').text(value.building).html();
            $(element).html(html); 
        },
        
        /**
        Gets value from element's html
        
        @method html2value(html) 
        **/        
        html2value: function(html) {        
          /*
            you may write parsing method to get value by element's html
            e.g. "Moscow, st. Lenina, bld. 15" => {city: "Moscow", street: "Lenina", building: "15"}
            but for complex structures it's not recommended.
            Better set value directly via javascript, e.g. 
            editable({
                value: {
                    city: "Moscow", 
                    street: "Lenina", 
                    building: "15"
                }
            });
          */ 
          return null;  
        },
      
       /**
        Converts value to string. 
        It is used in internal comparing (not for sending to server).
        
        @method value2str(value)  
       **/
       value2str: function(value) {
           var str = '';
           
           if(value) {
               for(var k in value) {
                   str = str + k + ':' + value[k] + ';';  
               }
           }
           return str;
       }, 
       
       /*
        Converts string to value. Used for reading value from 'data-value' attribute.
        
        @method str2value(str)  
       */
       str2value: function (str) {
          // console.log(str);
           /*
           this is mainly for parsing value defined in data-value attribute. 
           If you will always set value by javascript, no need to overwrite it
           */
           return str;
       },                
       
       /**
        Sets value of input.
        
        @method value2input(value) 
        @param {mixed} value
       **/         
       value2input: function (value) {         
           if(!value) {
             return;
           }
           //this.$select.filter('[name="Sku"]').val(value.sku);
           //this.$select.filter('[name="PaypalApiID"]').val(value.paypalid);
           this.$input.filter('[name="ItemNumber"]').val(value.number);
           this.$input.filter('[name="ItemTitle"]').val(value.title);          
           //this.$input.filter('[name="building"]').val(value.building);
       },       
       
       /**
        Returns value of input.
        
        @method input2value() 
       **/          
       input2value: function() { 
           return {
               Sku: this.$select.filter('[name="Sku"]').val(),
               PaypalApiID: this.$select.filter('[name="PaypalApiID"]').val(),
               ItemNumber: this.$input.filter('[name="ItemNumber"]').val(),
               ItemTitle: this.$input.filter('[name="ItemTitle"]').val(),
               SellingPlace: this.$select.filter('[name="SellingPlace"]').val(),
               StorePlace: this.$select.filter('[name="StorePlace"]').val(),
           };
       },        
       
        /**
        Activates input: sets focus on the first field.
        
        @method activate() 
       **/        
       activate: function() {
           // this.$input.filter('[name="city"]').focus();
       },  
       
       /**
        Attaches handler to submit form in case of 'showbuttons=false' mode
        
        @method autosubmit() 
       **/       
       autosubmit: function() {
           this.$input.keydown(function (e) {
                if (e.which === 13) {
                    $(this).closest('form').submit();
                }
           });
       }       
    });

    $.ajax({
        type: "GET",
        cache: false,
        //data: postdata,
        url: "/Ims/Association/SkuAndPayPalJson",
        success: function (data) {
            var paypaloption = "", skuoption = "",countryoption=""; 
            if (data.data.paypal) {
                var paypalresult = data.data.paypal;
                paypaloption += '<option></option>';
                for (var i = 0; i < paypalresult.length; i++){
                    paypaloption += '<option value=' + paypalresult[i].ID + '>' + paypalresult[i].PPAccount + '</option>';                  
                }    
            }

            if (data.data.sku){
                var skuresult = data.data.sku;
                skuoption += '<option></option>';
                for (var i = 0; i < skuresult.length; i++) {
                    skuoption += '<option value=' + skuresult[i].ID + '>' + skuresult[i].Sku + '</option>';
                }
            }

            if (data.data.country) {
                var countryresult = data.data.country;
                countryoption += '<option></option>';
                for (var i = 0; i < countryresult.length; i++) {
                    countryoption += '<option value=' + countryresult[i].ShipToCountryCode + '>' + countryresult[i].ShipToCountryName + '</option>';
                }
            }

           


            //console.log(countryoption);
            str = '<div class="editable-address"><label><span>SKU: </span><select name="Sku" class="form-control input-small">' + skuoption + '</select></label></div>' +
               '<div class="editable-address"><label><span>Paypal: </span><select name="PaypalApiID" class="form-control input-small">' + paypaloption + '</select></label></div>' +
               '<div class="editable-address"><label><span>编号: </span><input type="text" name="ItemNumber" class="form-control input-small" readonly></label></div>' +
               '<div class="editable-address"><label><span>标题: </span><input type="text" name="ItemTitle" class="form-control input-small" readonly></label></div>'+
               '<div class="editable-address"><label><span>销售地点: </span><select name="SellingPlace" class="select2 form-control input-small country">' + countryoption + '</select></label></div>' +
               '<div class="editable-address"><label><span>仓库地点: </span><select name="StorePlace" class="select2 form-control input-small country">' + countryoption + '</select></label></div>'
            ;

            Association.defaults = $.extend({}, $.fn.editabletypes.abstractinput.defaults, {
                tpl: str,
                inputclass: ''
            });
            $.fn.editabletypes.association = Association;
        }
    });

    function format(state) {
        if (!state.id) return state.text; // optgroup
        return "<img class='flag' src='../../assets/global/img/flags/" + state.id.toLowerCase() + ".png'/>&nbsp;&nbsp;" + state.text;
    }

    $(".country").select2({
        placeholder: "Select",
        allowClear: true,
        formatResult: format,
        formatSelection: format,
        escapeMarkup: function (m) {
            return m;
        }
    });

}(window.jQuery));