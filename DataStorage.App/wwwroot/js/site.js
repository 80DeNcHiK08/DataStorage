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
        //alert();
        $(this).parent().focus();
    } 

    $('.fileblock').on("click", function(e) {
        e.preventDefault();
        var str = $(this).find('#hiddenblockid').val();
        $.ajax({
            type:"POST",
            data: JSON.stringify(str),
            url: '/Document/UserStorage'
        }).done(function(res){
            alert(str);
        });
    })

    $('.fileblock').on("contextmenu", function(e) {
        e.preventDefault();
        alert("Right click (context menu)");
    })
})