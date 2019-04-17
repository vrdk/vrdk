$(document).ready(function () {
    $(".timemanagement_timepicker").timepicker({
        timeFormat: 'H:mm',
        startHour: 9,
        interval: 60,
        change: function () {
            $(this).change();
        }
    });
    $(".timemanagement_timepicker").mask('00:00');
    $(".form_input").on('keydown change', function () {
        var formNumber = $(this).attr('form-number');
        $('img[form-number=' + formNumber + ']').css('display', 'flex');
    });
});

function AddTimeManagment(objAppendToId, date) {
    toastr.info('Добавление...');
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
        },
        error: function () {
            toastr.info('Произошла ошибка');
        }
    });


}

function deleteTimeManagementRecord(element, id) {
    toastr.info('Удаление деятельности');
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
                toastr.success('Деятельность удалена');
            },
            error: function () {
                toastr.error('Произошла ошибка');
            }
        });
    }
    else {
        var elemToDelete = $(element).parent().parent();
        elemToDelete.empty();
        toastr.success('Деятельность удалена');
    }


}

function submitForm(form_elem) {
    
    var form = $(form_elem);
    var data = form.serializeArray();
    if (data[2].value !== '' && data[3].value !== '' && data[4].value !== '') {
        toastr.info('Добавление деятельности в распорядок дня');
        var url = form.attr('action');
        $.ajax({
            url: url,
            data: form.serialize(),
            success: function (html_data) {
                form.replaceWith(html_data);
                $(".timemanagement_timepicker").timepicker({
                    timeFormat: 'H:mm',
                    startHour: 9,
                    interval: 60
                });
                $(".timemanagement_timepicker").mask('00:00');
                toastr.success('Деятельность добавлена');
            },
            error: function () {
                toastr.error('Произошла ошибка');
            }
        });
    }
    else {
        toastr.error('Заполните все поля');
    }
    event.preventDefault();
}

function submitClosestForm(submit_element) {
    submitForm($(submit_element.closest('.timeManagementForm')));
}

