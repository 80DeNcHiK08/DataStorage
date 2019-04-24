$(document).ready(function() {
    $("#dropdownMenuButton").on("click", function (e) {
        e.preventDefault();
        $(".crform").toggle("200");
    })

    $(".eventhandler").on("click", function (e) {
        e.preventDefault();
        if ($(this).parent().find("#hiddenblockfiletype").val() === 'False') {
            var parentId = $(this).parent().find("#hiddenblockid").val();
            window.location = '/Document/UserStorage?parentId=' + parentId;
        }
    })

    $('.fileblock').on("contextmenu", function (e) {
        e.preventDefault()
        if ($(this).find('.contextmenu').hasClass("invisible")) {
            $(this).find('.contextmenu').removeClass('invisible');
        } else {
            $(this).find('.contextmenu').addClass('invisible');
        }

    })

    $('.delete').on("click", function (e) {
        e.preventDefault();
        var str = $(this).parent().parent().find('#hiddenblockid').val();
        //console.log(str);
        $.ajax({
            type: "POST",
            data: {
                fileId: str
            },
            url: '/Document/DeleteFile'
        }).done(function () {
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

function sumbmit() {
    $("#uploadFile").submit();
}

;( function($, window, document, undefined)
{
	$('.inputfile').each(function()
	{
		var $input = $(this),
			$label = $input.next('label'),
			labelVal = $label.html();

		$input.on('change', function(e)
		{
			var fileName = '';

			if(this.files && this.files.length > 1)
				fileName = (this.getAttribute('data-multiple-caption' ) || '').replace('{count}', this.files.length);
			else if( e.target.value )
				fileName = e.target.value.split( '\\' ).pop();

			if(fileName)
				$label.find('span').html(fileName);
			else
				$label.html(labelVal);
		});

        $input
		.on('focus', function(){$input.addClass('has-focus');})
		.on('blur', function(){$input.removeClass('has-focus');});
	});
})(jQuery, window, document);

function deleteDocument() {
    var str = $(this).find('#hiddenblockid').val();
    $.ajax({
        type: "POST",
        data: {
            fileId: str
        },
        url: '/Document/DeleteFile'
    }).done(function () {
        window.location = '/Document/UserStorage';
    });
}