$(document).ready(function () {
    $('.profile__tab').on('click.initial', function () {
        getinitialprofilerequests(0, this.id);
    });
    $('.profile__tab_active').trigger('click.initial');
    $('.profile__tabs').on('click', '.pagginate', function () {
        getprofilerequests(this);
    });

});

function getinitialprofilerequests(page_number, data_type) {
    url = "/profile/" + data_type + "spage";
    $.ajax({
        type: "Get",
        url: url,
        data: {
            pageNumber: page_number
        },
        success: function (requests_data) {
            $(".profile__tabs_" + data_type).html(requests_data);
            $("#" + data_type).off('click.initial');
        }
    }
    );
}
function getprofilerequests(element) {
    var target = $(element);
    var page_number = target.attr('page-anchor');
    var data_type = target.attr('type-anchor');
    url = "/profile/" + data_type + "spage";
    $('#paggination_menu[type-anchor= "' + data_type + '"]').css('display', 'none');
    $('#page_load_gif[type-anchor="' + data_type + '"]').css('display', 'flex');
    $.ajax({
        type: "Get",
        url: url,
        data: {
            pageNumber: page_number
        },
        success: function (data) {
            $(".profile__tabs_" + data_type).html(data);
        }
    }
    );
}