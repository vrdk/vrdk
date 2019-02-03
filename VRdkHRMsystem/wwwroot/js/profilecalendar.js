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
});

function proccessProfileCalendarDay(id, dt, tId) {
    var url = "/request/proccesscalendarday";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: dt,
            teamId: tId
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#modal_place').html(modal_html);
        }
    });
}

function timeManagmentModal(date) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/profile/timemanagment";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            date: date
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#modal_place').html(modal_html);
        }
    });

    preloader.css('display', 'none');
}

function callDayProccessMenu(id, date, teamId) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = '/profile/dayproccessmenu';
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: date,
            teamId: teamId
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });

    preloader.css('display', 'none');
}