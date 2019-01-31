﻿$(document).ready(function () {
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

function requestDayOff(id, dt, tId) {
    var url = "/request/requestdayoff";
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
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}

function editDayOffRequest(id, tId) {
    var url = "/request/editdayoffrequest";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            teamId: tId
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}