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
    $.validator.unobtrusive.parse("#submit_form");
    $("#submit_form").on('submit', function () {
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#submit_form").valid()) {
                var modal = $("#request_modal");
                modal.modal('hide');
                $('#preloader').css('display', 'flex');
            }
        }
        else {
            modal = $("#request_modal");
            modal.modal('hide');
            $('#preloader').css('display', 'flex');
        }
    });

    $.validator.unobtrusive.parse("#proc_cal_day_form");
    $("#proc_cal_day_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#proc_cal_day_form").valid()) {
                $('#preloader').css('display', 'flex');
                var data = form.serializeArray();
                var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                var date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                var time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
                var time_to = data.find(x => x.name === 'TimeTo').value.split(':');
                var dt1 = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
                var dt2 = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
                var diff = Math.abs(dt2.getHours() - dt1.getHours());
                $.ajax({
                    url: form.attr('action'),
                    method: 'post',
                    data: data,
                    success: function (cal_cell_html) {
                        modal = $("#request_modal");
                        modal.modal('hide');
                        var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                        current_cell.replaceWith(cal_cell_html);
                        var work_days_count = $(".calendar__workdays_daysblock[employee-anchor='" + employee_anchor + "']");
                        var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                        work_days_count.text(parseInt(work_days_count.text()) + 1);
                        work_days_hours.text(parseInt(work_days_hours.text()) + parseInt(diff));
                        $('#preloader').css('display', 'none');
                    }
                });
            }
        }
        else {
            $('#preloader').css('display', 'flex');
            data = form.serializeArray();
            employee_anchor = data.find(x => x.name === 'EmployeeId').value;
            date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (cal_cell_html) {
                    modal = $("#request_modal");
                    modal.modal('hide');
                    var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                    current_cell.replaceWith(cal_cell_html);
                    var day_offs_count = $(".calendar__chilldays_block[employee-anchor='" + employee_anchor + "']");
                    day_offs_count.text(parseInt(day_offs_count.text()) + 1);
                    $('#preloader').css('display', 'none');
                }
            });
        }
    });

    $.validator.unobtrusive.parse("#edit_work_day_form");

    $("#edit_work_day_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#edit_work_day_form").valid()) {
                $('#preloader').css('display', 'flex');
                var data = form.serializeArray();
                if (data.find(x => x.name === 'result').value === 'WorkDay') {
                    var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                    var date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                    var time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
                    var time_to = data.find(x => x.name === 'TimeTo').value.split(':');
                    var dt1 = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
                    var dt2 = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
                    var diff = Math.abs(dt2.getHours() - dt1.getHours());
                    var durationText = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']").text().split('-');
                    var currentDuration = parseInt(durationText[1]) - parseInt(durationText[0]);
                    $.ajax({
                        url: form.attr('action'),
                        method: 'post',
                        data: data,
                        success: function (cal_cell_html) {
                            if (cal_cell_html) {
                                var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                                current_cell.replaceWith(cal_cell_html);
                                var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                                work_days_hours.text(parseInt(work_days_hours.text()) + (parseInt(diff) - currentDuration));
                                modal = $("#request_modal");
                                modal.modal('hide');
                                $('#preloader').css('display', 'none');
                            }
                            else {
                                modal = $("#request_modal");
                                modal.modal('hide');
                                $('#preloader').css('display', 'none');
                            }
                        },
                        error: function () {
                            modal = $("#request_modal");
                            modal.modal('hide');
                            $('#preloader').css('display', 'none');
                        }
                    });
                }
            }
        }
        else {
            $('#preloader').css('display', 'flex');
            data = form.serializeArray();
            employee_anchor = data.find(x => x.name === 'EmployeeId').value;
            date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
            time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
            time_to = data.find(x => x.name === 'TimeTo').value.split(':');
            dt1 = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
            dt2 = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
            diff = Math.abs(dt2.getHours() - dt1.getHours());
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (cal_cell_html) {
                    var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                    current_cell.replaceWith(cal_cell_html);
                    var work_days_count = $(".calendar__workdays_daysblock[employee-anchor='" + employee_anchor + "']");
                    var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                    var day_offs_count = $(".calendar__chilldays_block[employee-anchor='" + employee_anchor + "']");
                    day_offs_count.text(parseInt(day_offs_count.text()) + 1);
                    work_days_count.text(parseInt(work_days_count.text()) - 1);
                    work_days_hours.text(parseInt(work_days_hours.text()) - parseInt(diff));
                    modal = $("#request_modal");
                    modal.modal('hide');
                    $('#preloader').css('display', 'none');
                },
                error: function () {
                    modal = $("#request_modal");
                    modal.modal('hide');
                    $('#preloader').css('display', 'none');
                }
            });
        }
    });

    $.validator.unobtrusive.parse("#edit_work_day_form");

    $("#edit_day_off_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#edit_day_off_form").valid()) {
                $('#preloader').css('display', 'flex');
                var data = form.serializeArray();
                if (data.find(x => x.name === 'result').value === 'WorkDay') {                 
                    var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                    var date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                    var time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
                    var time_to = data.find(x => x.name === 'TimeTo').value.split(':');
                    var dt1 = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
                    var dt2 = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
                    var diff = Math.abs(dt2.getHours() - dt1.getHours());
                    $.ajax({
                        url: form.attr('action'),
                        method: 'post',
                        data: data,
                        success: function (cal_cell_html) {
                            var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                            current_cell.replaceWith(cal_cell_html);
                            var work_days_count = $(".calendar__workdays_daysblock[employee-anchor='" + employee_anchor + "']");
                            var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                            var day_offs_count = $(".calendar__chilldays_block[employee-anchor='" + employee_anchor + "']");
                            day_offs_count.text(parseInt(day_offs_count.text()) - 1);
                            work_days_count.text(parseInt(work_days_count.text()) + 1);
                            work_days_hours.text(parseInt(work_days_hours.text()) + parseInt(diff));
                            modal = $("#request_modal");
                            modal.modal('hide');
                            $('#preloader').css('display', 'none');
                        }
                    });
                }
            }
        }
        else {
            modal = $("#request_modal");
            modal.modal('hide');
        }
    });

    $("#absence_set_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $('#preloader').css('display', 'flex');
        var data = form.serializeArray();
        var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
        var date = data.find(x => x.name === 'Date').value.split('T')[0].split('-');
        var parsedDate = date[2] + "." + date[1] + "." + date[0];
        $.ajax({
            url: form.attr('action'),
            method: 'post',
            data: data,
            success: function (response) {
                if (response) {
                    var calendar_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                    var new_cell = $('<div>').addClass('calendar__block calendar__block_pass');
                    calendar_cell.replaceWith(new_cell);
                    if (calendar_cell.attr('type-anchor') === 'workDay') {
                        var durationText = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']").text().split('-');
                        var currentDuration = parseInt(durationText[1]) - parseInt(durationText[0]);
                        var work_days_count = $(".calendar__workdays_daysblock[employee-anchor='" + employee_anchor + "']");
                        var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                        work_days_count.text(parseInt(work_days_count.text()) - 1);
                        work_days_hours.text(parseInt(work_days_hours.text()) - parseInt(currentDuration));
                    } else if (calendar_cell.attr('type-anchor') === 'dayOff') {
                        var day_offs_count = $(".calendar__chilldays_block[employee-anchor='" + employee_anchor + "']");
                        day_offs_count.text(parseInt(day_offs_count.text()) - 1);
                    }
                    var absence_cell = $(".calendar__pass[employee-anchor='" + employee_anchor + "']");                   
                    absence_cell.attr('disabled', 'disabled');
                    absence_cell.css('cursor', 'not-allowed');
                    absence_cell.removeAttr('type-anchor');
                    absence_cell.text('-');
                    modal = $("#request_modal");
                    modal.modal('hide');
                    $('#preloader').css('display', 'none');
                }
                else {
                    $('#preloader').css('display', 'none');
                }
            }
        });
    });
});
