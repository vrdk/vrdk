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
$.validator.setDefaults({
    ignore: []
});
$.validator.unobtrusive.parse("#modalForm");
