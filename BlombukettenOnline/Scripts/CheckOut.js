$(function () {
    
    $('.option-button').click(function (event) {
        if (!$(this).hasClass('btn-primary'))
        {
            $(this).toggleClass('btn-default');
            $(this).toggleClass('btn-primary');
            
            $(this).children('.glyphicon').toggleClass('glyphicon-unchecked').toggleClass('glyphicon-check');
        }

        var targetId = event.target.id;
        var group = targetId.substring(0, targetId.lastIndexOf('-'));
        group += 's';

        $('#' + group).children().each(function () {
            if ($(this).hasClass('btn-primary') && $(this).attr('id') != targetId)
            {
                $(this).toggleClass('btn-primary');
                $(this).toggleClass('btn-default');

                $(this).children('.glyphicon').toggleClass('glyphicon-check').toggleClass('glyphicon-unchecked');
            }
        });

        if (targetId == 'payment-method-store') {
            order.paymentMethod = 1;
            HidePaymentMethodPanels();
            $('#payment-options-selectstore-panel').show();
        }

        if (targetId == 'payment-method-bank') {
            order.paymentMethod = 2;
            HidePaymentMethodPanels();
            $('#payment-options-selectbank-panel').show();
        }

        if (targetId == 'payment-method-paypal') {
            order.paymentMethod = 3;
            HidePaymentMethodPanels();
            $('#payment-options-paypal-panel').show();
        }



        if (targetId == 'payment-options-selectstore-1') {
            order.storeId = 1;
        }
        if (targetId == 'payment-options-selectstore-2') {
            order.storeId = 2;
        }
        if (targetId == 'payment-options-selectstore-3') {
            order.storeId = 3;
        }


        if (targetId == 'shipping-option-1') {
            order.deliveryMethod = 1;
            HideShippingMethodPanels();
        }
        if (targetId == 'shipping-option-2') {
            order.deliveryMethod = 2;
            HideShippingMethodPanels();
            $('#shipping-option-address-panel').show();
        }
        if (targetId == 'shipping-option-3') {
            order.deliveryMethod = 3;
            HideShippingMethodPanels();
            $('#shipping-option-address-panel').show();
        }

    });

    function HidePaymentMethodPanels() {
        $('.payment-option-panel').each(function () {
            $(this).hide();
        });
    }
    function HideShippingMethodPanels() {
        $('.shipping-option-panel').each(function () {
            $(this).hide();
        });
    }

    $('#btn-confirm-order').click(function () {
        $('#order-confirmed-alert').slideToggle(300);
        //$('#btn-confirm-order').prop('disabled', true);
        
        $.removeCookie(cname, { path: '/' });
        //renderCart();
        $("#buybag").empty();
        $("#nameinput").val('');
        $("#streetinput").val('');
        $("#zipinput").val('');
        $("#commentinput").val('');
        $("#emailinput").val('');
   

    });
});