$(document).ready(function () {
    $("#team_select").on('change', function () {
        showPreloader();
        var teamId = $(this).val();
        var role = $("#redirectUrl").val();
        var url = "/" + role + "/calendar?teamid=" + teamId;
        window.location.href = url;
    });
    $(".calendar__block_moon").tooltipster({
        position: 'right',
        theme: 'tooltipster-light'
    });

    $('.calendar__block[type-anchor="dayOff"]').on('click', function () {
        proccessDayOff(this);
    });

    $('.calendar__block[type-anchor="workDay"]').on('click', function () {
        callDayProccessMenu(this);
    });
});


function proccessDayOff(cellElement) {  
    var el = $(cellElement);
    toastr.info('Запрос формы для запроса выходного дня...', el.attr('date-anchor'));
    var url = "/request/proccesscalendarday";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            teamId: el.attr('team-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });
}

function timeManagmentModal(cellElement) { 
    var el = $(cellElement);
    toastr.info('Запрос формы распорядка дня...', el.attr('date-anchor'));
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
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });


}

function callDayProccessMenu(cellElement) {
    var el = $(cellElement);
    toastr.info('Запрос формы работы с днём...', el.attr('date-anchor'));
    var url = '/profile/dayproccessmenu';
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            teamId: el.attr('team-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);          
        },
        error: function () {
            toastr.error('Произошла ошибка');
        }
    });

 
}