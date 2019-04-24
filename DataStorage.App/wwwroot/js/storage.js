$(document).ready(function() {
    $("#create_menu").on("click", function (e) {
        e.preventDefault();
        $(".create-item").toggle("200");
    })

    $("#tree").on("click", function (e) {
        e.preventDefault();
        $(".filetree").toggle("400");
    })

    $("#settings_menu").on("click", function (e) {
        e.preventDefault();
        $(".settings-item").toggle("400");
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
        $(this).find('.contextmenu').toggle("200");
    })

    $('.delete').on("click", function (e) {
        e.preventDefault();
        var str = $(this).parent().parent().parent().find('#hiddenblockid').val();
        $.ajax({
            type: "POST",
            data: {
                fileId: str
            },
            url: '/Document/DeleteFile'
        }).done(function () {
            //window.location = '/Document/UserStorage';
        });
    })

    $('.delete').on("click", function (e) {
        e.preventDefault();
        var parentId = $(this).parent().parent().parent().find("#hiddenblockid").val();
        console.log($(this).parent().parent().parent().find("#hiddenblockid").val());
        window.location = '/Document/DeleteFile?fileId=' + parentId;
    })

    $('.download').on("click", function (e) {
        e.preventDefault();
        if ($(this).parent().parent().parent().find("#hiddenblockfiletype").val() === 'True') {
            var parentId = $(this).parent().parent().parent().find("#hiddenblockid").val();
            console.log($(this).parent().parent().parent().find("#hiddenblockfiletype").val(), $(this).parent().parent().parent().find("#hiddenblockid").val());
            window.location = '/Document/DownloadFile?fileId=' + parentId;
        }
    })

    $('.info').on('click', function(e) {
        e.preventDefault();
        var str = $(this).parent().parent().parent().find('.detblock').toggle("200");
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