$('[name = result]').on('change', function () {
    var elem = $(this);
    if (elem.val() === 'DayOff') {
        $("#submit_button").attr('formnovalidate', 'formnovalidate');
        $(".timepicker-dropdown").removeClass('tripmodal__checkinput');
        $(".timepicker-dropdown").attr('readonly', 'readonly');
    }
    else {
        $("#submit_button").removeAttr('formnovalidate', 'formnovalidate');
        $(".timepicker-dropdown").addClass('tripmodal__checkinput');
        $(".timepicker-dropdown").removeAttr('readonly', 'readonly');
    }
});
$(document).ready(function () {
    $.validator.setDefaults({
        ignore: []
    });

    $("#submit_form").on('submit', function () {
        var form = $(this);
        $.validator.unobtrusive.parse(form);
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#submit_form").valid()) {
                var modal = $("#request_modal");
                modal.modal('hide');
                showPreloader('preloader');
            }
        }
        else {
            modal = $("#request_modal");
            modal.modal('hide');
            showPreloader('preloader');
        }
    });

    $("#proc_cal_day_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $.validator.unobtrusive.parse(form);        
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#proc_cal_day_form").valid()) {               
                var data = form.serializeArray();
                var employeeAnchor = data.find(x => x.name === 'EmployeeId').value;
                var dateAnchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                var toast = toastr.info('Назначение рабочего дня...', dateAnchor);
                modal = $("#request_modal");
                modal.modal('hide');
                var timeFrom = data.find(x => x.name === 'TimeFrom').value;
                var timeTo = data.find(x => x.name === 'TimeTo').value;
                var selectedWorkHours = getSelectedWorkDayDurationFromForm(timeFrom, timeTo);
                $.ajax({
                    url: form.attr('action'),
                    method: 'post',
                    data: data,
                    success: function (calCellHtml) {                        
                        var currentCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                        currentCell.replaceWith(calCellHtml);
                        var workDaysCount = $(".calendar__workdays_daysblock[employee-anchor='" + employeeAnchor + "']");
                        var workDaysHours = $(".calendar__workdays_hoursblock[employee-anchor='" + employeeAnchor + "']");
                        var newWorkHours = (parseFloat(workDaysHours.text()) + selectedWorkHours).toFixed(1);
                        workDaysCount.text(parseFloat(workDaysCount.text()) + 1);
                        if (isInt(newWorkHours)) {
                            workDaysHours.text(parseInt(newWorkHours));
                        }
                        else {
                            workDaysHours.text(newWorkHours);
                        }

                        toastr.clear(toast);
                        toastr.success('Рабочий день назначен', dateAnchor);                       
                    },
                    error: function () {
                        toastr.clear(toast);
                        toastr.error('Не удалось назначить рабочий день');
                    }
                });
            }
        }
        else {
            data = form.serializeArray();
            employeeAnchor = data.find(x => x.name === 'EmployeeId').value;
            dateAnchor = data.find(x => x.name === 'Date').value.split(' ')[0];
            toast = toastr.info('Назначение выходного дня...', dateAnchor);
            modal = $("#request_modal");
            modal.modal('hide');
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (calCellHtml) {                   
                    var currentCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                    currentCell.replaceWith(calCellHtml);
                    var dayOffsCount = $(".calendar__chilldays_block[employee-anchor='" + employeeAnchor + "']");
                    dayOffsCount.text(parseFloat(dayOffsCount.text()) + 1);

                    toastr.clear(toast);
                    toastr.success('Выходной день назначен', dateAnchor);
                },
                error: function () {
                    toastr.clear(toast);
                    toastr.error('Не удалось назначить выходной день', dateAnchor);
                }

            });
        }
    });

    $("#edit_work_day_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $.validator.unobtrusive.parse(form);
        var data = form.serializeArray();
        var employeeAnchor = data.find(x => x.name === 'EmployeeId').value;
        var dateAnchor = data.find(x => x.name === 'Date').value.split(' ')[0];
        var currentCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#edit_work_day_form").valid()) {
                var toast = toastr.info('Внесение изменений...', 'Рабочий день на ' + dateAnchor);
                modal = $("#request_modal");
                modal.modal('hide');
                var timeFrom = data.find(x => x.name === 'TimeFrom').value;
                var timeTo = data.find(x => x.name === 'TimeTo').value;
                var selectedWorkDayDuration = getSelectedWorkDayDurationFromForm(timeFrom, timeTo);
                var currentWorkDayDuration = getWorkDayDurationFromCalendarCell(currentCell);
                $.ajax({
                    url: form.attr('action'),
                    method: 'post',
                    data: data,
                    success: function (calCellHtml) {
                        if (calCellHtml) {
                            if (currentCell.hasClass('tooltipstered')) {
                                var cell_title = currentCell.tooltipster('content');
                            }
                            currentCell.replaceWith(calCellHtml);
                            if (cell_title) {
                                var newCalendarCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                                newCalendarCell.attr('title', cell_title);
                                newCalendarCell.tooltipster({
                                    position: 'right',
                                    theme: 'tooltipster-light'
                                });
                            }

                            var workDaysHours = $(".calendar__workdays_hoursblock[employee-anchor='" + employeeAnchor + "']");
                            var newWorkHours = (parseFloat(workDaysHours.text()) + (selectedWorkDayDuration - currentWorkDayDuration)).toFixed(1);
                            if (isInt(newWorkHours)) {
                                workDaysHours.text(parseInt(newWorkHours));
                            }
                            else {
                                workDaysHours.text(newWorkHours);
                            }

                            toastr.clear(toast);
                            toastr.success('Изменения внесены', 'Рабочий день на ' + dateAnchor);
                        }
                        else {
                            modal = $("#request_modal");
                            modal.modal('hide');
                        }
                    },
                    error: function () {
                        toastr.clear(toast);
                        toastr.error('Не удалось внести изменения', 'Рабочий день на ' + dateAnchor);
                    }
                });
            }
        }
        else {
            toast = toastr.info('Изменение на выходной день', 'Рабочий день на ' + dateAnchor);
            var removedWorkDayDuration = getWorkDayDurationFromCalendarCell(currentCell);
            modal = $("#request_modal");
            modal.modal('hide');
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (calCellHtml) {
                    var currentCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                    if (currentCell.hasClass('tooltipstered')) {
                        var cell_title = currentCell.tooltipster('content');
                    }
                    currentCell.replaceWith(calCellHtml);
                    if (cell_title) {
                        var newCalendarCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                        newCalendarCell.attr('title', cell_title);
                        newCalendarCell.tooltipster({
                            position: 'right',
                            theme: 'tooltipster-light'
                        });
                    }
                    var workDaysCount = $(".calendar__workdays_daysblock[employee-anchor='" + employeeAnchor + "']");
                    var workDaysHours = $(".calendar__workdays_hoursblock[employee-anchor='" + employeeAnchor + "']");
                    var dayOffsCount = $(".calendar__chilldays_block[employee-anchor='" + employeeAnchor + "']");
                    dayOffsCount.text(parseFloat(dayOffsCount.text()) + 1);
                    workDaysCount.text(parseFloat(workDaysCount.text()) - 1);
                    var newWorkHours = (parseFloat(workDaysHours.text()) - removedWorkDayDuration).toFixed(1);
                    if (isInt(newWorkHours)) {
                        workDaysHours.text(parseInt(newWorkHours));
                    }
                    else {
                        workDaysHours.text(newWorkHours);
                    }             

                    toastr.clear(toast);
                    toastr.success('Изменения внесены', 'Рабочий день на ' + dateAnchor);
                },
                error: function () {
                    toastr.clear(toast);
                    toastr.error('Не удалось внести изменения', 'Рабочий день на ' + dateAnchor);
                }
            });
        }
    });

    $("#edit_day_off_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $.validator.unobtrusive.parse(form);
        var data = form.serializeArray();
        var employeeAnchor = data.find(x => x.name === 'EmployeeId').value;
        var dateAnchor = data.find(x => x.name === 'Date').value.split(' ')[0];
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#edit_day_off_form").valid()) {               
                var toast = toastr.info('Подтверждение рабочего дня', dateAnchor);
                modal = $("#request_modal");
                modal.modal('hide');
                var timeFrom = data.find(x => x.name === 'TimeFrom').value;
                var timeTo = data.find(x => x.name === 'TimeTo').value;
                var selectedWorkDayDuration = getSelectedWorkDayDurationFromForm(timeFrom, timeTo);
                $.ajax({
                    url: form.attr('action'),
                    method: 'post',
                    data: data,
                    success: function (calCellHtml) {
                        var currentCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                        if (currentCell.hasClass('tooltipstered')) {
                            var cell_title = currentCell.tooltipster('content');
                        }
                        currentCell.replaceWith(calCellHtml);
                        if (cell_title) {
                            var newCalendarCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
                            newCalendarCell.attr('title', cell_title);
                            newCalendarCell.tooltipster({
                                position: 'right',
                                theme: 'tooltipster-light'
                            });
                        }                     
                        var workDaysCount = $(".calendar__workdays_daysblock[employee-anchor='" + employeeAnchor + "']");
                        var workDaysHours = $(".calendar__workdays_hoursblock[employee-anchor='" + employeeAnchor + "']");
                        var dayOffsCount = $(".calendar__chilldays_block[employee-anchor='" + employeeAnchor + "']");
                        dayOffsCount.text(parseFloat(dayOffsCount.text()) - 1);
                        workDaysCount.text(parseFloat(workDaysCount.text()) + 1);
                        var newWorkHours = (parseFloat(workDaysHours.text()) + selectedWorkDayDuration).toFixed(1);
                        if (isInt(newWorkHours)) {
                            workDaysHours.text(parseInt(newWorkHours));
                        }
                        else {
                            workDaysHours.text(newWorkHours);
                        }    

                        toastr.clear(toast);
                        toastr.success('Изменения внесены', 'Выходной день на ' + dateAnchor);
                    },
                    error: function () {
                        toastr.clear(toast);
                        toastr.error('Не удалось внести изменения', 'Выходной день на ' + dateAnchor);
                    }
                });
            }
        }
        else {           
            employeeAnchor = data.find(x => x.name === 'EmployeeId').value;
            dateAnchor = data.find(x => x.name === 'Date').value.split(' ')[0];
            var currentCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + dateAnchor + "']");
            if (currentCell.attr('requested')) {
                toast = toastr.info('Подтверждение выходного дня', dateAnchor);
                modal = $("#request_modal");
                modal.modal('hide');
                $.ajax({
                    url: form.attr('action'),
                    method: 'post',
                    data: data,
                    success: function (calCellHtml) {                       
                        currentCell.replaceWith(calCellHtml);
                        var dayOffsCount = $(".calendar__chilldays_block[employee-anchor='" + employeeAnchor + "']");
                        dayOffsCount.text(parseFloat(dayOffsCount.text()) + 1);

                        toastr.clear(toast);
                        toastr.success('Выходной день подтверждён', dateAnchor);
                    },
                    error: function () {
                        toastr.clear(toast);
                        toastr.success('Не удалось внести изменения', dateAnchor);
                    }
                });
            }
            else {
                closePreloader('preloader');
                modal = $("#request_modal");
                modal.modal('hide');
            }
        }
    });

    $("#absence_set_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);        
        var data = form.serializeArray();
        var employeeAnchor = data.find(x => x.name === 'EmployeeId').value;
        var date = data.find(x => x.name === 'Date').value.split('T')[0].split('-');
        var parsedDate = date[2] + "." + date[1] + "." + date[0];
        var toast = toastr.info('Подтверждение прогула', parsedDate);
        modal = $("#request_modal");
        modal.modal('hide');
        $.ajax({
            url: form.attr('action'),
            method: 'post',
            data: data,
            success: function (response) {
                if (response) {
                    var calendarCell = $(".calendar__block[employee-anchor='" + employeeAnchor + "'][date-anchor='" + parsedDate + "']");
                    var newCell = $('<div>').addClass('calendar__block calendar__block_pass');
                    calendarCell.replaceWith(newCell);
                    if (calendarCell.attr('type-anchor') === 'workDay' || calendarCell.attr('type-anchor') === 'teamleadWorkDay') {
                        var currentDifference = getWorkDayDurationFromCalendarCell(calendarCell);
                        var workDaysCount = $(".calendar__workdays_daysblock[employee-anchor='" + employeeAnchor + "']");
                        var workDaysHours = $(".calendar__workdays_hoursblock[employee-anchor='" + employeeAnchor + "']");
                        workDaysCount.text(parseFloat(workDaysCount.text()) - 1);
                        var newWorkHours = (parseFloat(workDaysHours.text()) - currentDifference).toFixed(1);
                        if (isInt(newWorkHours)) {
                            workDaysHours.text(parseInt(newWorkHours));
                        }
                        else {
                            workDaysHours.text(newWorkHours);
                        }
                    } else if (calendarCell.attr('type-anchor') === 'dayOff') {
                        var dayOffsCount = $(".calendar__chilldays_block[employee-anchor='" + employeeAnchor + "']");
                        dayOffsCount.text(parseFloat(dayOffsCount.text()) - 1);
                    }

                    var absenceCell = $(".calendar__pass[employee-anchor='" + employeeAnchor + "']");
                    absenceCell.attr('disabled', 'disabled');
                    absenceCell.css('cursor', 'not-allowed');
                    absenceCell.removeAttr('type-anchor');
                    absenceCell.text('-');

                    toastr.clear(toast);
                    toastr.success('Прогул подтверждён', parsedDate);
                }
                else {
                    toastr.clear(toast);
                    toastr.error('Не удалось внести изменениня', parsedDate);
                }
            }
        });
    });
});

function getWorkDayDurationFromCalendarCell(calendarDayObj) {
    var currentTimeFrom = calendarDayObj.attr('timeFrom-anchor').split(':');
    var currentTimeTo = calendarDayObj.attr('timeTo-anchor').split(':');
    var currentDateFrom = new Date(0, 0, 0, currentTimeFrom[0], currentTimeFrom[1], 0, 0);
    var currentDateTo = new Date(0, 0, 0, currentTimeTo[0], currentTimeTo[1], 0, 0);
    return currentDateTo.getTime() - currentDateFrom.getTime() > 0 ? (currentDateTo.getTime() - currentDateFrom.getTime()) / 36e5 : (currentDateTo.getTime() - currentDateFrom.getTime()) / 36e5 + 24;
}

function getSelectedWorkDayDurationFromForm(timeFromString, timeToString) {
    var timeFrom = timeFromString.split(':');
    var timeTo = timeToString.split(':');
    var dateFrom = new Date(0, 0, 0, timeFrom[0], timeFrom[1], 0, 0);
    var dateTo = new Date(0, 0, 0, timeTo[0], timeTo[1], 0, 0);
    return dateTo.getTime() - dateFrom.getTime() > 0 ? (dateTo.getTime() - dateFrom.getTime()) / 36e5 : (dateTo.getTime() - dateFrom.getTime()) / 36e5 + 24;
}

function isInt(n) {
    return n % 1 === 0;
}