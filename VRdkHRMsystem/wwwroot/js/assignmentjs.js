function formatState(state) {
    if (!state.id) {
        return state.text;
    }
    var baseUrl = "https://vrdkstorage.blob.core.windows.net/photos/";
    var $state = $(
        '<span><img src="' + baseUrl + state.element.value + '.png" style="border-radius:50%; height:30px; width:30px;vertical-align:middle;"/> ' + state.text + '</span>'
    );
    return $state;
};
function addAssignment() {
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
            $("#tripMembers").rules("add", {
                required: true
            });
            $("#assignmentaddmodal_datepicker_from").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_to").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"],
                firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_from").mask('00.00.0000');
            $("#assignmentaddmodal_datepicker_to").mask('00.00.0000');
        }
    });
}
function editAssignment(id) {
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
            $("#tripMembers").rules("add", {
                required: true
            });
            $("#assignmentaddmodal_datepicker_from").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_to").datepicker({
                dayNamesMin: ["Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб"],
                dateFormat: 'dd.mm.yy',
                monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"], firstDay: 1
            });
            $("#assignmentaddmodal_datepicker_from").mask('00.00.0000');
            $("#assignmentaddmodal_datepicker_to").mask('00.00.0000');

        }
    });
}
function checkAssignment(id) {
    url = "/teamlead/checkassignment?id=" + id;
    $.ajax({
        type: "Get",
        url: url,
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
        }
    });
}