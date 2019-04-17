function formatState(state) {
    if (!state.id) {
        return state.text;
    }
    var baseUrl = "https://vrdkstorage.blob.core.windows.net/photos/";
    var $state = $(
        '<span id="'+ 322 +'"><img src="' + baseUrl + state.element.value + '.png?"' + new Date().getTime().toString() + '" style="border-radius:50%; height:30px; width:30px;vertical-align:middle;"/> ' + state.text + '</span>'
    );
    return $state;
}

$(document).ready(function () {
    $('select#teamMembers option[selected]').each(function () {
        $('select#TeamleadId option[value="' + this.attributes.getNamedItem('value').value + '"]').attr('disabled', 'disabled');
    });     
    var teamleadId = $('#TeamleadId').val();
    $('select#teamMembers option[value="' + teamleadId + '"]').attr('disabled', 'disabled');
        $('#teamMembers').select2({
            placeholder: "Выберите cотрудников...",
        templateResult: formatState
    });
    $('#TeamleadId').select2({
        templateResult: formatState
    });
    $('#teamMembers').on('select2:select', function (selectedOption) {
        $('select#TeamleadId option[value="' + selectedOption.params.data.id + '"]').attr('disabled', 'disabled');
        $('#TeamleadId').select2('destroy').select2({
            placeholder: "Выберите cотрудников...",
            templateResult: formatState
        });
    });

    $('#teamMembers').on('select2:unselect', function (unselectedOption) {
        $('select#TeamleadId option[value="' + unselectedOption.params.data.id + '"]').removeAttr('disabled');
        $('#TeamleadId').select2('destroy').select2({
            placeholder: "Выберите cотрудников...",
            templateResult: formatState
        });     
    });

    $('#TeamleadId').on('change', function () {
        var teamleadId = $('#TeamleadId').val();
        $('select#teamMembers option[disabled="disabled"]').removeAttr('disabled');
        $('select#teamMembers option[value="' + teamleadId + '"]').attr('disabled', 'disabled');
        $('#teamMembers').select2('destroy').select2({
            placeholder: "Выберите cотрудников...",
            templateResult: formatState
        });
    });
});
