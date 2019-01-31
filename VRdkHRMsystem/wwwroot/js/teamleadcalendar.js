﻿$(document).ready(function () {
    $("#team_select").on('change', function () {
        showPreloader();
        var teamId = $(this).val();
        var role = $("#redirectUrl").val();
        var url = "/"+role+"/calendar?teamid=" + teamId;
        window.location.href = url;
    });
    $(".calendar__block_moon").tooltipster({
        position: 'right',
        theme: 'tooltipster-light'
    });
});

function setAbsence(id, tId, fn, ln, rl) {
    var url = "/teamlead/setAbsence";
    $.ajax({
        url: url,
        data: {
            id: id,
            teamId: tId,
            firstName: fn,
            lastName: ln,
            role: rl
        },
        method: 'get',
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}

function proccessCalendarDay(id, dt, fn, ln, tId, rl) {
    var url = "/teamlead/proccesscalendarday";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: dt,
            name: fn,
            surname: ln,
            teamId: tId,
            role : rl
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#workDay_timepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_from").mask('00:00');
            $("#workDay_timepicker_to").mask('00:00');
        }
    });
}

function editWorkDay(id, dt, tId, rl) {
    var url = "/teamlead/editworkday";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: dt,
            teamId: tId,
            role: rl
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#workDay_timepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_from").mask('00:00');
            $("#workDay_timepicker_to").mask('00:00');
        }
    });
}

function editDayOff(id, dt, tId, rl) {
    var url = "/teamlead/editdayoff";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: dt,
            teamId: tId,
            role: rl
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#workDay_timepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_from").mask('00:00');
            $("#workDay_timepicker_to").mask('00:00');
        }
    });
}