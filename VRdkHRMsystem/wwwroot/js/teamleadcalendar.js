$(document).ready(function () {
    $("#team_select").on('change', function () {
        showPreloader();
        var teamId = $(this).val();
        var role = $("#redirectUrl").val();
        var url = "/"+role+"/calendar?teamid=" + teamId;
        window.location.href = url;
    });
    $(".calendar__block_moon").tooltipster({
        position: 'right',
        theme: 'tooltipster-light'
    });

    $(".calendar__contentblock").on('click', '.calendar__block[type-anchor="free"]', function () {
        proccessCalendarDay(this);
    });  
    $(".calendar__contentblock").on('click', '.calendar__block[type-anchor= "dayOff"]', function () {
        proccessCalendarDay(this);
    });   
    
    $(".calendar__contentblock").on('click', '.calendar__block[type-anchor= "teamleadWorkDay"]', function () {
        callSelfDayProccessMenu(this);
    });

    $(".calendar__contentblock").on('click', '.calendar__block[type-anchor="workDay"]', function () {
        callDayProccessMenuForTeamlead(this);
    });

    $(".calendar__passes").on('click', '.calendar__pass[type-anchor="absence"]', function () {
       setAbsence(this);
    });
});

function setAbsence(cellElement) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var el = $(cellElement);  
    var url = "/teamlead/setAbsence";
    $.ajax({
        url: url,
        data: {
            id: el.attr('employee-anchor'),
            teamId: el.attr('team-anchor'),
            firstName: el.attr('name-anchor'),
            lastName: el.attr('surname-anchor'),
            role: el.attr('role-anchor')
        },
        method: 'get',
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function proccessCalendarDay(cellElement) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/teamlead/proccesscalendarday";
    var el = $(cellElement);    
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            name: el.attr('name-anchor'),
            surname: el.attr('surname-anchor'),
            teamId: el.attr('team-anchor'),
            role: el.attr('role-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#datepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#datepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#datepicker_from").mask('00:00');
            $("#datepicker_to").mask('00:00');
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function editWorkDay(id, dt, tId, rl) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/teamlead/editworkday";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: dt,
            teamId: tId,
            role: rl
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#workDay_timepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_from").mask('00:00');
            $("#workDay_timepicker_to").mask('00:00');
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function editDayOff(id, dt, tId, rl) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = "/teamlead/editdayoff";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: id,
            date: dt,
            teamId: tId,
            role: rl
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            $("#workDay_timepicker_from").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_to").timepicker({
                timeFormat: 'H:mm',
                startHour: 9,
                interval: 60
            });
            $("#workDay_timepicker_from").mask('00:00');
            $("#workDay_timepicker_to").mask('00:00');
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function callSelfDayProccessMenu(cellElement) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = '/teamlead/selfdayproccessmenu';
    var el = $(cellElement);    
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            name: el.attr('name-anchor'),
            surname: el.attr('surname-anchor'),
            teamId: el.attr('team-anchor'),
            role: el.attr('role-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function callDayProccessMenuForTeamlead(cellElement) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var url = '/teamlead/dayproccessmenu';
    var el = $(cellElement);     
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor'),
            name: el.attr('name-anchor'),
            surname: el.attr('surname-anchor'),
            teamId: el.attr('team-anchor'),
            role: el.attr('role-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });
}

function getEmployeesTimeManagementRecords(event_element) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var el = $(event_element);
    var url = '/teamlead/timemanagementrecords';
    $.ajax({
        url: url,
        method: 'get',
        data: {
            id: el.attr('employee-anchor'),
            date: el.attr('date-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#request_modal').modal();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });  
}

function TeamleadTimeManagmentModal(event_element) {
    var preloader = $("#preloader");
    preloader.css('display', 'flex');
    var el = $(event_element);
    var url = "/profile/timemanagment";
    $.ajax({
        url: url,
        method: 'get',
        data: {
            date: el.attr('date-anchor')
        },
        success: function (modal_html) {
            $('#modal_place').empty();
            $('#modal_place').html(modal_html);
            preloader.css('display', 'none');
        },
        error: function () {
            preloader.css('display', 'none');
        }
    });


}