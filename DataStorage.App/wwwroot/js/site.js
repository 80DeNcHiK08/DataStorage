function deleteDocument() {
    var str = $(this).find('#hiddenblockid').val();
    $.ajax({
        type: "POST",
        data: {
            fileId: str
        },
        url: '/Document/DeleteFile'
    }).done(function() {
        window.location = '/Document/UserStorage';
    });
}

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

    $('.fileblock').on("contextmenu", function(e) {
        e.preventDefault()
        if($(this).find('.contextmenu').hasClass("invisible")) {
            $(this).find('.contextmenu').removeClass('invisible');
        } else {
            $(this).find('.contextmenu').addClass('invisible');
        }
        
    })

    $('.delete').on("click", function() {
        var str = $(this).parent().parent().find('#hiddenblockid').val();
        $.ajax({
        type: "POST",
        data: {
            fileId: str
        },
        url: '/Document/DeleteFile'
        }).done(function() {
            window.location = '/Document/UserStorage';
        });
    })

    $('.download').on("click", function () {
        var str = $(this).parent().parent().find('#hiddenblockid').val();
        $.ajax({
            type: "POST",
            data: {
                fileId: str
            },
            url: '/Document/DownloadFile'
        }).done(function () {
            //window.location = '/Document/UserStorage';
        });
    })
})