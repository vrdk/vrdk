﻿function formatState(state) {
    if (!state.id) {
        return state.text;  
    }
    var baseUrl = "https://vrdkstorage.blob.core.windows.net/photos/";
    var $state = $(
        '<span><img src="' + baseUrl + state.element.value + '.png?"' + new Date().getTime().toString() + '" style="border-radius:50%; height:30px; width:30px;vertical-align:middle;"/> ' + state.text + '</span>'
    );
    return $state;
};
function addAssignment() {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    url = "/admin/addassignment";
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#tripMembers").select2({
                placeholder: "Выберите cотрудников...",
                templateResult: formatState
            }); 
            $("#from_datepicker").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                firstDay: 1
            });
            $("#to_datepicker").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                firstDay: 1
            });
            $("#from_datepicker").mask('00.00.0000');
            $("#to_datepicker").mask('00.00.0000');
            $(".date_picker_diff").on('change', function () {
                var input = $(this);
                var date = parseDate(input.val());
                input.val(date);
                CalcDiff('from_datepicker', 'to_datepicker');
            });
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}
function editAssignment(id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    url = "/admin/editassignment?id=" + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#tripMembers").select2({
                placeholder: "Выберите cотрудников...",
                templateResult: formatState
            });
            $("#from_datepicker").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1
            });
            $("#to_datepicker").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1
            });
            $("#from_datepicker").mask('00.00.0000');
            $("#to_datepicker").mask('00.00.0000');
            $(".date_picker_diff").on('change', function () {
                var input = $(this);
                var date = parseDate(input.val());
                input.val(date);
                CalcDiff('from_datepicker', 'to_datepicker');
            });
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}
function checkAssignment(id) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    url = "/teamlead/checkassignment?id=" + id;
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