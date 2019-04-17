$(document).ready(function () {
    $("#submit_form").on('submit', function () {
        showLoginPreloader('preloader');
    });
});

function showLoginPreloader(preloaderId) {
    var pl = $('#' + preloaderId);
    pl.css('display', 'flex');
}
