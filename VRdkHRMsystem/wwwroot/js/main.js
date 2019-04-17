$(document).ready(function () { $(function () { $("#vacrequestmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#vacrequestmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripaddmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripaddmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripeditmodal__from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#tripeditmodal__to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__birthDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#from_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#to_datepicker").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__hireDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }), $("#empprofileedit__datepicker__dismissalDate").datepicker({ dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"], dateFormat: 'dd.mm.yy', monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1 }); }), window.onload = function () { !function () { let e = document.getElementsByClassName("profile__tab"), a = document.getElementsByClassName("profile__tab_post"); for (let t = 0; t < e.length; t++)e[t].addEventListener("click", function (i) { i.preventDefault(); for (let t = 0; t < e.length; t++)e[t].classList.remove("profile__tab_active"), a[t].classList.remove("profile__tab_postactive"); e[t].classList.add("profile__tab_active"), a[t].classList.add("profile__tab_postactive"); }); }(); }; });
$(document).ready(function () {
    $("#empprofileedit__datepicker__birthDate").mask('00.00.0000');
    $("#empprofileedit__datepicker__hireDate").mask('00.00.0000');
    $("#empprofileedit__datepicker__dismissalDate").mask('00.00.0000');
    $("#from_datepicker").mask('00.00.0000');
    $("#to_datepicker").mask('00.00.0000');
    $("#phone").mask('(000) 000-0000');
    $("#File").on('change', function () {
        var inputFile = $(this);
        var check_button = $('#check_photo');
        if (inputFile.val() === "") {
            $("#fileUploadText").html("Добавить фото");
            if (check_button) {
                check_button.css('display', 'none');
            }
        }
        else {
            $("#fileUploadText").html("Фото загружено");
            if (check_button) {
                check_button.css('display', 'flex');
            }
        }
    });
    $("#Photo").on('focus', function () {
        $(this).val("");
    });
    $("a").on('click', function () {
        showPreloader();
    });
    $(".submit_button").on('click', function () {
        showPreloader();
    });
    $(".back_button").on('click', function () {
        showPreloader();
        window.location.href = document.referrer;
    });
    $('.content').on('change', '.hasDatepicker', function () {
        var input = $(this);
        var date = parseDate(input.val());
        input.val(date);
    });
    $(".date_picker_diff").on('change', function () {
        var input = $(this);
        var date = parseDate(input.val());
        input.val(date);
        CalcDiff('from_datepicker', 'to_datepicker');
    });
    $(".date_picker").on('change', function () {
        var input = $(this);
        var date = parseDate(input.val());
        input.val(date);
    });
    $('.content').on('click', '.list_paggination', function () {
        var target = $(this);
        var action = target.attr('action-anchor');
        var page = target.attr('pageNumber-anchor');
        var searchKey = target.attr('searchKey-anchor');
        $('#paggination_list').css('display', 'none');
        $('#paggination_list_gif').css('display', 'flex');
        $.ajax({
            url: action,
            method: 'get',
            data: {
                pageNumber: page,
                searchKey: searchKey
            },
            success: function (page_html) {
                $('.listsection').replaceWith(page_html);
            },
            error: function () {
                toastr.warning('pagination error');
            }
        });
    });
    $('.content').on('click', 'img.vacation__check_popup', function () {
        checkVacationRequest(this);
    });
    $('.content').on('click', 'img.sickleave__check_popup', function () {
        checkSickleaveRequest(this);
    });
    $('#list_form').on('submit', function (event) {
        showPreloader();
        event.preventDefault();
        var form = $(this);
        var data = form.serializeArray();
        $.ajax({
            url: form.attr('action'),
            method: 'get',
            data: data,
            success: function (search_result_html) {
                $('.listsection').replaceWith(search_result_html);
                closePreloader();
            },
            error: function () {
                alert('search error');
                closePreloader();
            }
        });
    });
    $("#notes-new").on('change', function () {
        var icon = $(this);
        var url = "/profile/checknotificationsnuvelty";
        $.ajax({
            url: url,
            data: {
                userEmail: icon.attr('user-notifications-anchor')
            },
            method: 'get',
            success: function (result) {
                if (result) {
                    icon.css('display', 'inline-block');
                }
            }
        });
    });
    $("#notes-new").trigger('change');
    $("#check_photo").on('click', function () {
        viewPhoto(this);
    });    
    $('.content').on('submit', '#profile__request_form', function (event) {
        event.preventDefault();
        var toast = toastr.info('Создание заявки...');
        var form = $(this);
        var data = form.serializeArray();
        $.ajax({
            method: "POST",
            url: form.attr('action'),
            data: data,
            success: function () {
                toastr.clear(toast);
                toastr.success('Заявка создана');
            },
            error: function () {
                toastr.clear(toast);
                toastr.error('Не удалось создать заявку');
            }
        });
    });
    $('body').on('submit', '#profile__edit_form', function (event) {
        event.preventDefault();
        var form = $(this);
        var toast = toastr.info('Внесение изменений...');
        var data = form.serializeArray();
        $.ajax({
            method: "POST",
            url: form.attr('action'),
            data: data,
            success: function () {
                toastr.clear(toast);
                toastr.success('Изменения внесены');
            },
            error: function () {
                toastr.clear(toast);
                toastr.error('Не удалось внести изменения');
            }
        });
    });

    ligthMenuItem();
});
function ligthMenuItem() {
    var menu = $('.navigation__link');
    var url = location.href;
    url = url.split('/');
    if (url.length > 4) {
        for (var i = 0; i <= menu.length - 1; i++) {
            if (url[url.length - 1].toLowerCase().indexOf(menu[i].id) >= 0) {
                menu[i].classList.add('navigation__link_active');
            }
            else {
                menu[i].classList.remove('navigation__link_active');
            }
        }
    }
}

function parseDate(date) {
    if (date) {
        var today = new Date();
        var regex = /^\d+$/;
        var isValid = true;
        var stringDate = date.split('.');
        var month = stringDate[1];
        if (month && regex.test(month)) {
            var intMonth = parseInt(month);

            if (intMonth > 12) {
                month = 12;
            } else {
                if (intMonth <= 0) {
                    month = 1;
                }
            }
        } else if (month) {
            month = today.getMonth();
        } else {
            isValid = false;
        }
        var year = stringDate[2];
        if (year && regex.test(year)) {
            if (parseInt(year) < 100 || parseInt(year) > 3000) {
                year = today.getFullYear();
            }
        }
        else if (year) {
            year = today.getFullYear();
        }
        if (!year) {
            isValid = false;
        }
        var day = stringDate[0];
        if (day && regex.test(day)) {
            if (isValid) {
                var currDate = new Date(parseInt(year), parseInt(month + 1), 0);
                if (day > currDate.getDate() || day <= 0) {
                    day = currDate.getDate();
                }
            }
        }
        else if (day) {
            day = today.getDate();
        }
        else {
            isValid = false;
        }

        if (isValid) {
            return day + '.' + month + '.' + year;
        }
        else {
            var corrcetMonth = parseInt(today.getMonth()) + 1;
            return today.getDate() + '.' + corrcetMonth + '.' + today.getFullYear();
        }
    }
}

function GetDiff(fromId, toId) {
    var d1 = document.getElementById(toId).value;
    var endDate = new Date(d1.split('.')[2], d1.split('.')[1] - 1, d1.split('.')[0]);
    var d2 = document.getElementById(fromId).value;
    var beginDate = new Date(d2.split('.')[2], d2.split('.')[1] - 1, d2.split('.')[0]);
    var timeDiff = endDate.getTime() - beginDate.getTime();
    return Math.ceil(timeDiff / (1000 * 3600 * 24));
}

function CalcDiff(fromId, toId) {
    if (document.getElementById(fromId).value && document.getElementById(toId).value) {
        document.getElementById("Duration").value = GetDiff(fromId, toId);
    }
}

function changeUserPhoto(input) {

    if (input.files && input.files[0]) {
        var reader = new FileReader();
        reader.onload = function (e) {
            $('#user_photo').attr('src', e.target.result);
        };
        reader.readAsDataURL(input.files[0]);
    }
}

function checkVacationRequest(requestAnchorHolder) {
    var toast = toastr.info('Запрос данных о заявке...');
    var target = $(requestAnchorHolder);
    var action = 'checkvacationrequest';
    $.ajax({
        type: "GET",
        url: action,
        data: {
            id: target.attr('request-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            toastr.clear(toast);
        },
        error: function () {
            toastr.clear(toast);
            toastr.error('Не удалось получить данные');
        }
    });
}

function checkSickleaveRequest(requestAnchorHolder) {
    var toast = toastr.info('Запрос данных о заявке...');
    var target = $(requestAnchorHolder);
    var action = 'checkSickleaveRequest';
    $.ajax({
        type: "get",
        url: action,
        data: {
            id: target.attr('request-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            toastr.clear(toast);
        },
        error: function () {
            toastr.clear(toast);
            toastr.error('Не удалось получить данные');
        }
    });
}
function ShowRequestPopup(url, id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');

    url = url + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });

}


function proccessSickleave(method, id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');

    url = "/request/" + method + "sickleave?id=" + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });


}

function showPreloader() {
    var pl = $('#preloader');
    pl.css('display', 'flex');
}

function closePreloader() {
    $('#preloader').css('display', 'none');
}

function manageMenu() {
    var menu = $(".header__burger");
    if (menu.css('visibility') === 'hidden') {
        menu.css('visibility', 'visible');
        menu.css('opacity', '1');
        $(document.body).on('click', function () {
            manageMenu();
        });
    }
    else {
        menu.css('visibility', 'hidden');
        menu.css('opacity', '0');
        $(document.body).off('click');
    }
}

function deleteTeam(id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/admin/deleteteam";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function viewPhoto(el) {
    var target = $(el);
    var input = document.getElementById(target.attr('file-anchor'));
    var file = input.files[0];
    if (file) {
        $("body").append("<div class='popup'>" +
            "<div class='popup_bg'></div>" +
            "<img id='sickmessage_photo' src='' class='popup_img' />" +
            "</div>");
        $(".popup").fadeIn(400);
        setTimeout(function () {
            $(".popup_bg").click(function () {
                $(".popup").fadeOut(400);
                setTimeout(function () {
                    $(".popup").remove();
                }, 400);
            });
        }, 400);
        var fr = new FileReader();
        fr.onloadend = function () {
            document.getElementById('sickmessage_photo').src = fr.result;
        };
        fr.readAsDataURL(file);
    }
}