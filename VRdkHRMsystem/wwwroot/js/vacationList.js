let isBlurred = false;
let signalRConnected = false;
let connection;
let blurTimestamp;
let groupAnchor = $('[group-anchor]').attr('group-anchor');
$('[group-anchor]').remove();
$(window).on('blur', function () {
    isBlurred = true;
    blurTimestamp = new Date();
});
$(window).on('focus', function () {
    var now = new Date();
    if ((now - blurTimestamp) / 6e4 > 5 && isBlurred && !signalRConnected) {
        showPreloader();
        location.reload();
    }
});
connection = new signalR.HubConnectionBuilder().withUrl("/vacationListSyncHub").build();
connection.start({ waitForPageLoad: false }).then(() => {
    signalRConnected = true;
    connection.invoke('AddToGroup', groupAnchor);
}).catch(function (err) {
    toastr.error('Не удалось синхронизировать страницу');
    console.error(err.toString());
});
connection.on('VacationRequestProccessed', function (proccessedVacationRequest) {
    changeStatus(proccessedVacationRequest);
    if (proccessedVacationRequest.requestStatus === 'Approved') {
        changeBalance(proccessedVacationRequest);
    }
});
connection.on('AddVacationRequestToList', function (vacationRequest) {
    
});
connection.onclose(() => {
    signalRConnected = false;
    setTimeout(() => {
        connection.start().then(() => {
            signalRConnected = true;
            connection.invoke('AddToGroup', groupAnchor);
        }).catch(function (err) {
            toastr.error('Не удалось синхронизировать страницу');
            console.error(err.toString());
        });
    }, 5000);
});
$('.modal-dialog').on('submit', '#vac__submit_form', function (event) {
    event.preventDefault();
    let toast = toastr.info('Обработка заявки...');
    let form = $(this);
    let data = form.serialize();
    modal = $("#request_modal");
    modal.modal('hide');
    $.ajax({
        url: form.attr('action'),
        method: 'post',
        data: data,
        success: function (vacRequest) {
            changeStatus(vacRequest);
            if (vacRequest.requestStatus === 'Approved') {
                changeBalance(vacRequest);
            }
            if (signalRConnected) {
                connection.invoke('SyncVacationLists', groupAnchor, vacRequest).catch(function (err) {
                    return console.error(err.toString());
                });
            }   
            toastr.clear(toast);
            toastr.success('Заявка обработана');
        },
        error: function () {
            toastr.clear(toast);
            toastr.error('Не удалось обработать заявку');
        }
    });
});
function changeStatus(vacationRequest) {
    let status = $('div[request-anchor="' + vacationRequest.vacationId + '"] div.vaclist__info div[class*="vacation_status_"]');
    if (status.length !== 0) {     
        status.removeClass().addClass('vacation_status_' + vacationRequest.requestStatus);
        return true;
    }
    else {
        return false;
    }
}
function changeBalance(vacationRequest) {
    let relatedRequests = $('p[employee-anchor="' + vacationRequest.employeeId + '"][type-anchor="' + vacationRequest.vacationType + '"]');
    $.each(relatedRequests, function (index, obj) {
        let newBalance = parseInt(obj.dataset.balance - vacationRequest.duration);
        obj.innerText = newBalance;
        obj.dataset.balance = newBalance;
    });
}