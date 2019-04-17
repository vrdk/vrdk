$(document).ready(function () {
    $("#team_select").on('change', function () {
        showPreloader('preloader');
        var teamId = $(this).val();
        var role = $("#redirectUrl").val();
        var url = "/" + role + "/calendar?teamid=" + teamId;
        window.location.href = url;
    });
    $(".calendar__block_moon").tooltipster({
        position: 'right',
        theme: 'tooltipster-light'
    });
    $(".content").on('click', '.calendar__block[type-anchor="free"]', function () {
        proccessCalendarDay(this);
    });
    $(".content").on('click', '.calendar__block[type-anchor= "dayOff"]', function () {
        proccessCalendarDay(this);
    });

    $(".content").on('click', '.calendar__block[type-anchor= "teamleadWorkDay"]', function () {
        callSelfDayProccessMenu(this);
    });

    $(".content").on('click', '.calendar__block[type-anchor="workDay"]', function () {
        callDayProccessMenuForTeamlead(this);
    });

    $(".calendar__passes").on('click', '.calendar__pass[type-anchor="absence"]', function () {
        setAbsence(this);
    });
});

function setAbsence(cellElement) {
    var toast = toastr.info('Запрос формы подтверждения прогула...');
    var el = $(cellElement);
    var url = "/teamlead/setAbsence";
    $.ajax({
        url: url,
        data: {
            id: el.attr('employee-anchor'),
            teamId: el.attr('team-anchor'),
            firstName: el.attr('name-anchor'),
            lastName: el.attr('surname-anchor')
        },
        method: 'get',
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);

            toastr.clear(toast);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });
}

function proccessCalendarDay(cellElement) {
    var toast = toastr.info('Запрос формы работы с днём...');
    var url = "/teamlead/proccesscalendarday";
    var el = $(cellElement);
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            name: el.attr('name-anchor'),
            surname: el.attr('surname-anchor'),
            teamId: el.attr('team-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#datepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#datepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#datepicker_from").mask('00:00');
            $("#datepicker_to").mask('00:00');

            toastr.clear(toast);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });
}

function callSelfDayProccessMenu(cellElement) {
    var toast = toastr.info('Запрос формы работы с рабочим днём...');
    var url = '/teamlead/selfdayproccessmenu';
    var el = $(cellElement);
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            name: el.attr('name-anchor'),
            surname: el.attr('surname-anchor'),
            teamId: el.attr('team-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);

            toastr.clear(toast);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });
}

function callDayProccessMenuForTeamlead(cellElement) {
    var toast = toastr.info('Запрос формы работы с рабочим днём...');
    var url = '/teamlead/dayproccessmenu';
    var el = $(cellElement);
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            name: el.attr('name-anchor'),
            surname: el.attr('surname-anchor'),
            teamId: el.attr('team-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);

            toastr.clear(toast);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });
}

function getEmployeesTimeManagementRecords(event_element) {
    var toast = toastr.info('Запрос распорядка дня работника...');
    var el = $(event_element);
    var url = '/teamlead/timemanagementrecords';
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);

            toastr.clear(toast);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });
}

function TeamleadTimeManagmentModal(event_element) {
    var toast = toastr.info('Запрос распорядка дня работника...');
    var el = $(event_element);
    var url = "/profile/timemanagment";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            date: el.attr('date-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#modal_place').html(modal_html);

            toastr.clear(toast);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });


}