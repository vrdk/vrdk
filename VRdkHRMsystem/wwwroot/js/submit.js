
 $.validator.setDefaults({
        ignore: []
    });

$.validator.unobtrusive.parse('#submit_form');

$('#submit_form').on('submit', function () {        
        if ($(this).valid()) {
            var modal = $("#request_modal");
            modal.modal('hide');
            $('#preloader').css('display', 'flex');
        }
});