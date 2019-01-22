$(document).ready(function () {
    $("#team_select").on('change', function () {
        var teamId = $(this).val();
        var url = "/admin/calendar?teamid=" + teamId;
        window.location.href = url;
    });
});