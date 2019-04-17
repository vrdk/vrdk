$(document).ready(function () {
    $.validator.setDefaults({
        ignore: []
    });
    $.validator.unobtrusive.parse("#proc_day_off_form");
    
    $("#proc_day_off_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        if ($("#proc_day_off_form").valid()) {
            var data = form.serializeArray();
            var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
            var date = data.find(x => x.name === 'DayOffDate').value.split('T')[0].split('-');
            var parsedDate = date[2] + "." + date[1] + "." + date[0];
            toastr.info('Подтверждение желаемого выходного', parsedDate);   
            modal = $("#request_modal");
            modal.modal('hide');
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (day_off) {                    
                    var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                    current_cell.addClass('calendar__block_dayoff' + day_off["dayOffImportance"] + ' calendar__block_moon');
                    current_cell.attr('title', day_off["comment"] === null ? "Нет комментария" : day_off["comment"]);
                    current_cell.tooltipster({
                        position: 'right',
                        theme: 'tooltipster-light'
                    });         

                    toastr.success('Желаемый выходной подтверждён', parsedDate);   
                },
                error: function () {
                    toastr.error('Произошла ошибка', parsedDate);
                }
            });
        }
    });

    $.validator.unobtrusive.parse("#edit_day_off_form");

    $("#edit_day_off_form").on('submit', function (event) {
        event.preventDefault();
        var form = $(this);
        if ($("#edit_day_off_form").valid()) {           
            var data = form.serializeArray();
            var employee_anchor = data.find(x => x.name === 'EmployeeId').value;
            var date = data.find(x => x.name === 'DayOffDate').value.split('T')[0].split('-');
            var parsedDate = date[2] + "." + date[1] + "." + date[0];
            toastr.info('Внесение изменений', parsedDate);  
            modal = $("#request_modal");
            modal.modal('hide');
            $.ajax({
                url: form.attr('action'),
                method: 'post',
                data: data,
                success: function (day_off) {
                    if (day_off === false) {                     
                        var current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                        current_cell.removeClass('calendar__block_dayoffgreen calendar__block_dayoffyellow calendar__block_dayoffred calendar__block_moon');
                        current_cell.tooltipster('destroy');
                        toastr.success('Изменения внесены', parsedDate);
                    }
                    else {
                        employee_anchor = data.find(x => x.name === 'EmployeeId').value;
                        date = data.find(x => x.name === 'DayOffDate').value.split('T')[0].split('-');
                        parsedDate = date[2] + "." + date[1] + "." + date[0];
                        current_cell = $(".calendar__block[employee-anchor='" + employee_anchor + "'][date-anchor='" + parsedDate + "']");
                        current_cell.removeClass('calendar__block_dayoffgreen calendar__block_dayoffyellow calendar__block_dayoffred');
                        current_cell.addClass('calendar__block_dayoff' + day_off["dayOffImportance"] + ' calendar__block_moon');
                        if (current_cell.hasClass('tooltipstered')) {
                            current_cell.tooltipster('content', day_off["comment"] === null ? "Нет комментария" : day_off["comment"]);
                        }
                        else {
                            current_cell.attr('title', day_off["comment"] === null ? "Нет комментария" : day_off["comment"]);
                            current_cell.tooltipster({
                                position: 'right',
                                theme: 'tooltipster-light'
                            });
                        }

                        toastr.success('Изменения внесены', parsedDate);
                    }
                },
                error: function () {
                    toastr.error('Произошла ошибка', parsedDate);
                }
            });
        }
    });

});