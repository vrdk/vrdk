$(document).ready(function () {
    $("#team_select").on('change', function () {
        var teamId = $(this).val();
        var url = "/admin/calendar?teamid=" + teamId;
        window.location.href = url;
    });
});

function setAbsence(id, tId,fn,ln) {
    var url = "/teamlead/setAbsence?id=" + id + "&teamId=" + tId + "&firstName=" + fn + "&lastName=" + ln;
    $.ajax({
        url: url,
        method: 'get',
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}
