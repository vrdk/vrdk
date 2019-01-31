$('[name = result]').on('change', function () {
    var elem = $(this);
    if (elem.val() === 'DayOff') {
        $("#submit_button").attr('formnovalidate', 'formnovalidate');
        $(".timepicker-dropdown").removeClass('tripmodal__checkinput');
        $(".timepicker-dropdown").attr('readonly', 'readonly');
    }
    else {
        $("#submit_button").removeAttr('formnovalidate', 'formnovalidate');
        $(".timepicker-dropdown").addClass('tripmodal__checkinput');
        $(".timepicker-dropdown").removeAttr('readonly', 'readonly');

    }
});
$(document).ready(function () {
    $.validator.setDefaults({
        ignore: []
    });
    $.validator.unobtrusive.parse("#submit_form");
    $("#submit_form").on('submit', function () {
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#submit_form").valid()) {
                var modal = $("#request_modal");
                modal.modal('hide');
                $('#preloader').css('display', 'flex');
            }
        }
        else {
            modal = $("#request_modal");
            modal.modal('hide');
            $('#preloader').css('display', 'flex');
        }
    });
});
