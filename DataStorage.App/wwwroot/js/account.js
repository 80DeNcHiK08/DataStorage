$(document).ready(function() {
    /*$("input[type='text']").on('focus', function(e) {
        if($("input[type='text']").valid() == false || $("input[type='password']").valid() == false) {
            $(this).css({
                'border-color':'red'
            });
            $(this).parent().find(".elemental").css({
                'font-size': '0.8em',
                'top': '-9px'
            });
        }
    });*/

    if($("input[type='text']").val() != "" || $("input[type='password']").val() != "") {
        $(this).parent().focus();
    }
})