$(document).ready(function () {
    $(".timemanagement_timepicker").timepicker({
        timeFormat: 'H:mm',
        startHour: 9,
        interval: 60
    });
    $(".timemanagement_timepicker").mask('00:00');
});

function AddTimeManagment(objAppendToId, date) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/profile/addtimemanagementrecord";
    var table = $("#" + objAppendToId);
    $.ajax({
        url: url,
        method: 'get',
        data: {
            date: date
        },
        success: function (record_template) {
            table.append(record_template);
            $(".timemanagement_timepicker").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $(".timemanagement_timepicker").mask('00:00');
           
        }
    });

    preloader.css('display', 'none');
}

function deleteTimeManagementRecord(element, id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/profile/deletetimemanagementrecord";
    if (id !== '') {
        $.ajax({
            url: url,
            data: {
                id: id
            },
            method: 'post',
            success: function () {
                var elemToDelete = $(element).parent().parent();
                elemToDelete.empty();
            }
        });
    }
    else {
        var elemToDelete = $(element).parent().parent();
        elemToDelete.empty();
    }

    preloader.css('display', 'none');
}

function submitForm(form_elem) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var form = $(form_elem);
    var data = form.serializeArray();
    if (data[2].value !== '' && data[3].value !== '' && data[4].value !== '') {
        var url = form.attr('action');
        $.ajax({
            url: url,
            data: form.serialize(),
            success: function (html_data) {
                form.replaceWith(html_data);
            }
        });
    }
    
    event.preventDefault();
    preloader.css('display', 'none');
}

function submitClosestForm(submit_element) {
    submitForm($(submit_element.closest('.timeManagementForm')));
}


