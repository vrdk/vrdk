$(document).ready(function () {
    $.validator.setDefaults({
        ignore: []
    });
    $.validator.unobtrusive.parse("#submit_form");
    $("#submit_form").on('submit', function () {
        if ($("#submit_form").valid()) {
            var modal = $("#request_modal");
            modal.modal('hide');
            $('#preloader').css('display', 'flex');
        }      
    });
});
