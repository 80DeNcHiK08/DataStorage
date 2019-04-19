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