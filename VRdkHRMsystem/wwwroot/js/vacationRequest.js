
let connection;
let groupName;
function getGroupName() {
    let input = $('#group_name');
    let name = input.val();
    input.remove();
    return name;
}
groupName = getGroupName();
connection = new signalR.HubConnectionBuilder().withUrl("/vacationListSyncHub").build();

function sendVacationRequestToList(requestResult) {
    connection.start().then(() => {
        connection.invoke('AddVacationRequestToList', groupName, requestResult);
    }).then(() => {
        connection.stop();
    });
}
$('.content').on('submit', '#profile__request_form', function (event) {
    event.preventDefault();
    var submitButton = $('button[type="submit"]');
    submitButton.prop('disabled', true);
    var toast = toastr.info('Создание заявки...');
    var form = $(this);
    var data = form.serializeArray();
    $.ajax({
        method: "POST",
        url: form.attr('action'),
        data: data,
        success: function (vacationRequest) {
            sendVacationRequestToList(vacationRequest);
            form[0].reset();
            toastr.clear(toast);
            submitButton.prop('disabled', false);
            toastr.success('Заявка создана');
        },
        error: function () {
            toastr.clear(toast);
            submitButton.prop('disabled', false);
            toastr.error('Не удалось создать заявку');
        }
    });
});