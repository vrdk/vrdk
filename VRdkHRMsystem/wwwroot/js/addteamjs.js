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

$(document).ready(function () {
    $('#teamMembers').select2({
        placeholder: "Выберите cотрудников...",
        templateResult: formatState
    });
    $('.emplits__empselect').select2({
        templateResult: formatState
    });
});