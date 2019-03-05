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
                $('#preloader').css('display', 'flex');
            }
        }
        else {
            modal = $("#request_modal");
            modal.modal('hide');
            $('#preloader').css('display', 'flex');
        }
    });
   
    $("#proc_cal_day_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $.validator.unobtrusive.parse(form);
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#proc_cal_day_form").valid()) {
                $('#preloader').css('display', 'flex');
                var data = form.serializeArray();
                var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                var date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                var time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
                var time_to = data.find(x => x.name === 'TimeTo').value.split(':');
                var date_from = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
                var date_to = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
                var diff = date_to.getTime() - date_from.getTime() > 0 ? (date_to.getTime() - date_from.getTime()) / 36e5 : (date_to.getTime() - date_from.getTime()) / 36e5 + 24;
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
                        work_days_count.text(parseFloat(work_days_count.text()) + 1);
                        work_days_hours.text((parseFloat(work_days_hours.text()) + diff).toFixed(1));
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
                    day_offs_count.text(parseFloat(day_offs_count.text()) + 1);
                    $('#preloader').css('display', 'none');
                }
            });
        }
    });
   
    $("#edit_work_day_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $.validator.unobtrusive.parse(form);       
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#edit_work_day_form").valid()) {
                $('#preloader').css('display', 'flex');
                var data = form.serializeArray();
                if (data.find(x => x.name === 'result').value === 'WorkDay') {                   
                    var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                    var date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                    var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                    var time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
                    var time_to = data.find(x => x.name === 'TimeTo').value.split(':');
                    var date_from = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
                    var date_to = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);                   
                    var diff = date_to.getTime() - date_from.getTime() > 0 ? (date_to.getTime() - date_from.getTime()) / 36e5 : (date_to.getTime() - date_from.getTime()) / 36e5 + 24;
                    var current_timeFrom = current_cell.attr('timeFrom-anchor').split(':');
                    var current_timeTo = current_cell.attr('timeTo-anchor').split(':');
                    var current_dateFrom = new Date(0, 0, 0, current_timeFrom[0], current_timeFrom[1], 0, 0);
                    var current_dateTo = new Date(0, 0, 0, current_timeTo[0], current_timeTo[1], 0, 0);
                    var current_diff = current_dateTo.getTime() - current_dateFrom.getTime() > 0 ? (current_dateTo.getTime() - current_dateFrom.getTime()) / 36e5 : (current_dateTo.getTime() - current_dateFrom.getTime()) / 36e5 + 24;                                   
                    $.ajax({
                        url: form.attr('action'),
                        method: 'post',
                        data: data,
                        success: function (cal_cell_html) {
                            if (cal_cell_html) {
                                if (current_cell.hasClass('tooltipstered')) {
                                    var cell_title = current_cell.tooltipster('content');
                                }
                                current_cell.replaceWith(cal_cell_html);
                                if (cell_title) {
                                    current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + date_anchor + "']");
                                    current_cell.attr('title', cell_title);
                                    current_cell.tooltipster({
                                        position: 'right',
                                        theme: 'tooltipster-light'
                                    });
                                }                         
                                var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                                work_days_hours.text((parseFloat(work_days_hours.text()) + (diff - current_diff)).toFixed(1));
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
            date_from = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
            date_to = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
            diff = date_to.getTime() - date_from.getTime() > 0 ? (date_to.getTime() - date_from.getTime()) / 36e5 : (date_to.getTime() - date_from.getTime()) / 36e5 + 24;
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
                    day_offs_count.text(parseFloat(day_offs_count.text()) + 1);
                    work_days_count.text(parseFloat(work_days_count.text()) - 1);
                    work_days_hours.text((parseFloat(work_days_hours.text()) - diff).toFixed(1));
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

    $("#edit_day_off_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        $.validator.unobtrusive.parse(form);        
        if ($('#submit_button').attr('formnovalidate') !== 'formnovalidate') {
            if ($("#edit_day_off_form").valid()) {
                $('#preloader').css('display', 'flex');
                var data = form.serializeArray();
                if (data.find(x => x.name === 'result').value === 'WorkDay') {                 
                    var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                    var date_anchor = data.find(x => x.name === 'Date').value.split(' ')[0];
                    var time_from = data.find(x => x.name === 'TimeFrom').value.split(':');
                    var time_to = data.find(x => x.name === 'TimeTo').value.split(':');
                    var date_from = new Date(0, 0, 0, time_from[0], time_from[1], 0, 0);
                    var date_to = new Date(0, 0, 0, time_to[0], time_to[1], 0, 0);
                    var diff = date_to.getTime() - date_from.getTime() > 0 ? (date_to.getTime() - date_from.getTime()) / 36e5 : (date_to.getTime() - date_from.getTime()) / 36e5 + 24;
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
                            day_offs_count.text(parseFloat(day_offs_count.text()) - 1);
                            work_days_count.text(parseFloat(work_days_count.text()) + 1);
                            work_days_hours.text((parseFloat(work_days_hours.text()) + diff).toFixed(1));
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
                        var current_timeFrom = calendar_cell.attr('timeFrom-anchor').split(':');
                        var current_timeTo = calendar_cell.attr('timeTo-anchor').split(':');
                        var current_dateFrom = new Date(0, 0, 0, current_timeFrom[0], current_timeFrom[1], 0, 0);
                        var current_dateTo = new Date(0, 0, 0, current_timeTo[0], current_timeTo[1], 0, 0);
                        var current_diff = current_dateTo.getTime() - current_dateFrom.getTime() > 0 ? (current_dateTo.getTime() - current_dateFrom.getTime()) / 36e5 : (current_dateTo.getTime() - current_dateFrom.getTime()) / 36e5 + 24;  
                        var work_days_count = $(".calendar__workdays_daysblock[employee-anchor='" + employee_anchor + "']");
                        var work_days_hours = $(".calendar__workdays_hoursblock[employee-anchor='" + employee_anchor + "']");
                        work_days_count.text(parseFloat(work_days_count.text()) - 1);
                        work_days_hours.text((parseFloat(work_days_hours.text()) - current_diff).toFixed(1));
                    } else if (calendar_cell.attr('type-anchor') === 'dayOff') {
                        var day_offs_count = $(".calendar__chilldays_block[employee-anchor='" + employee_anchor + "']");
                        day_offs_count.text(parseFloat(day_offs_count.text()) - 1);
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
