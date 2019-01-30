$(document).ready(function () {
    $("#submit_form").on('submit', function () {
        $('#preloader').css('display', 'flex');
    });
});
function closePreloader(preloader) {
    setTimeout(function () {
        $(preloader).hide();
    }, 1000);
}