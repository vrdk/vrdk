$(document).ready(function () {
    $("#submit_form").on('submit', function () {
        showPreloader();
    });
});

function closePreloader(preloader) {
    setTimeout(function () {
        $(preloader).hide();
    }, 1000);
}
function showPreloader() {
    var pl = $('#preloader');
    pl.css('display', 'flex');
}
