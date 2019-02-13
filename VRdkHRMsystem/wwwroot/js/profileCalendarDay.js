﻿$(document).ready(function () {
    $.validator.setDefaults({
        ignore: []
    });
    $.validator.unobtrusive.parse("#proc_day_off_form");

    $("#proc_day_off_form").on('submit', function () {
        event.preventDefault();
        var form = $(this);
        if ($("#proc_day_off_form").valid()) {
            $('#preloader').css('display', 'flex');
            var data = form.serializeArray();
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (day_off) {
                    var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                    var date = data.find(x => x.name === 'DayOffDate').value.split('T')[0].split('-');
                    var parsedDate = date[2] + "." + date[1] + "." + date[0];
                    var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                    current_cell.addClass('calendar__block_dayoff' + day_off["dayOffImportance"] + ' calendar__block_moon');
                    current_cell.attr('title', day_off["comment"] === null ? "Нет комментария" : day_off["comment"]);
                    current_cell.tooltipster({
                        position: 'right',
                        theme: 'tooltipster-light'
                    });
                    modal = $("#request_modal");
                    modal.modal('hide');
                    $('#preloader').css('display', 'none');
                }
            });
        }
    });

    $.validator.unobtrusive.parse("#edit_day_off_form");

    $("#edit_day_off_form").on('submit', function () {
        event.preventDefault();
        var form = $(this);
        if ($("#edit_day_off_form").valid()) {
            $('#preloader').css('display', 'flex');
            var data = form.serializeArray();
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (day_off) {
                    if (day_off === false) {
                        var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                        var date = data.find(x => x.name === 'DayOffDate').value.split('T')[0].split('-');
                        var parsedDate = date[2] + "." + date[1] + "." + date[0];
                        var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                        current_cell.removeClass('calendar__block_dayoffgreen calendar__block_dayoffyellow calendar__block_dayoffred calendar__block_moon');
                        current_cell.tooltipster('destroy');
                        modal = $("#request_modal");
                        modal.modal('hide');
                        $('#preloader').css('display', 'none');
                    }
                    else {
                        employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                        date = data.find(x => x.name === 'DayOffDate').value.split('T')[0].split('-');
                        parsedDate = date[2] + "." + date[1] + "." + date[0];
                        current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                        current_cell.removeClass('calendar__block_dayoffgreen calendar__block_dayoffyellow calendar__block_dayoffred');
                        current_cell.addClass('calendar__block_dayoff' + day_off["dayOffImportance"] + ' calendar__block_moon');
                        if (current_cell.hasClass('tooltipstered')){
                            current_cell.tooltipster('content', day_off["comment"] === null ? "Нет комментария" : day_off["comment"]);
                        }
                        else {
                            current_cell.attr('title', day_off["comment"] === null ? "Нет комментария" : day_off["comment"]);
                            current_cell.tooltipster({
                                position: 'right',
                                theme: 'tooltipster-light'
                            });
                        }                      
                        modal = $("#request_modal");
                        modal.modal('hide');
                        $('#preloader').css('display', 'none');
                    }
                }
            });
        }
    });

});