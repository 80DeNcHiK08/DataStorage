$(document).ready(function () {
    var cursize = $("#CurentSize").val();
    var totalsize = $("#StorageSize").val();
    document.getElementById('CurrentSizeoutput').innerHTML = (parseInt(cursize)/8000000).toFixed(2).toString() + " mb";
    document.getElementById('StorageSizeoutput').innerHTML = (parseInt(totalsize)/8000000).toFixed(2).toString() + " mb";
    setPercentage((parseInt(cursize)/parseInt(totalsize) * 100).toFixed(1));

    $("#dropzoneForm").dropzone({
        url: "/Document/CreateFile",
        paramName: "uploadedFile",
        uploadMultiple: true,
        parallelUploads: 2,
        maxFilesize: 30,
        addRemoveLinks: true,
        clickable: false,
        previewsContainer: ".dz-documenthandler",
        createImageThumbnails: false,
        params: {
            'parentId': GetParentId()
        },
        init: function() {
            this.on("dragstart", function(e) {
                $(this).css({
                    "border-color":"blue"
                })
            })
        }
    });

    $("#create_menu").on("click", function (e) {
        e.preventDefault();
        $("#dropdown-create").slideToggle("200");
    })

    $("#settings_menu").on("click", function (e) {
        e.preventDefault();
        $("#dropdown-settings").slideToggle("200");
    })

    $(".oppensortdropdown").on("click", function(e) {
        e.preventDefault();
        $(".sortmenu").slideToggle("fast");
    })

    $(".block").on("contextmenu", function(e) {
        e.preventDefault();
        $(this).find(".documentoptions").slideToggle("100", function() {
            if($(this).parent().hasClass("folderblock"))
            {
                if($(this).parent().hasClass("increase"))
                {
                    $(this).parent().removeClass("increase")
                } else {
                    $(this).parent().addClass("increase")
                }
            }
        });
    })

    $(".buttonmenu").on("click", function(e) {
        e.preventDefault();
        $(this).parent().find(".documentoptions").slideToggle("100", function() {
            if($(this).parent().hasClass("folderblock"))
            {
                if($(this).parent().hasClass("increase"))
                {
                    $(this).parent().removeClass("increase")
                } else {
                    $(this).parent().addClass("increase")
                }
            }
        });
    })

    $("#sortbyname").on("click", function(e) {
        e.preventDefault();
        parentId = GetParentId();
        window.location = '/Document/UserStorage?parentId='+ parentId + '?name=true'; 
    })

    $("#sortbylength").on("click", function(e) {
        e.preventDefault();
        parentId = GetParentId();
        $.ajax ({
            type: "POST",
            url: "/Document/UserStorage",
            data: {length : true},
            success: function(e) {
                //window.location = '/Document/UserStorage?fileId='+id;
            }
        })
    })

    $(".delete").on("click", function(e) {
        e.preventDefault();
        var id = $(this).parent().parent().parent().find("#hiddendocid").val();
        Delete(id);
    })

    $(".download").on("click", function(e) {
        e.preventDefault();
        var id = $(this).parent().parent().parent().find("#hiddendocid").val();
        Download(id);
    })

    $(".folderblock").find(".eventhandler").on("click", function(e) {
        e.preventDefault();
        var parentId = $(this).parent().find("#hiddendocid").val();
        window.location = '/Document/UserStorage?parentId=' + parentId;
    })
})

function GetParentId() {
    var parentId = sessionStorage.getItem("parentId");
    $("#parentId").val(parentId);
    return parentId;
}

function GetOwnerId() {
    var ownerId = sessionStorage.getItem("ownerId");
    $("#ownerId").val(ownerId);
    return ownerId;
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

function Delete(id) {
    $.ajax ({
        type: "POST",
        url: "/Document/DeleteFile",
        data: {fileId : id},
        success: function(e) {
            
        }
    })
    $("#" + id).remove();
}

function Download(id) {
    $.ajax ({
        type: "GET",
        url: "/Document/DownloadFile",
        data: {fileId : id},
        success: function(e) {
            window.location = '/Document/DownloadFile?fileId='+id;
        }
    })
}

function setPercentage(value) {
    var newWidth = parseInt($("#percentageContainer").width() * value / 100.0);
    console.log("total : " + $("#percentageContainer").width());
    console.log("new : " + newWidth);
    $("#percentageValue").animate({
        width: newWidth + "px"
    }, 500);
}