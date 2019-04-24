$(document).ready(function () {
    $("#submit_form").on('submit', function () {
        showLoginPreloader('preloader');
    });

    $(".back_button").on('click', function () {
        showLoginPreloader('preloader');
        window.location.href = document.referrer;
    });
});

function showLoginPreloader(preloaderId) {
    var pl = $('#' + preloaderId);
    pl.css('display', 'flex');
}
