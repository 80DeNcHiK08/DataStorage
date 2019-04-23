$(document).ready(function() {
    $("#dropdownMenuButton").on("click", function(e) {
        e.preventDefault();
        $(".dropdownmenu").toggle();
    })

    $("#cee").on("click", function() {
        uploadFile();
    })
})