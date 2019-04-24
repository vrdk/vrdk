using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.Profile;
using VRdkHRMsystem.Models.SharedModels;
using VRdkHRMsystem.Models.TeamleadViewModels;

namespace VRdkHRMsystem.Controllers
{
    [Authorize(Roles = "Teamlead")]
    public class TeamleadController : Controller
    {
        private const string emptyValue = "None";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
        private readonly IResidualsService _residualsService;
        private readonly ITransactionService _transactionService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly ITimeManagementService _timeManagementService;
        private readonly ITeamService _teamService;
        private readonly IAssignmentService _assignmentService;
        private readonly INotificationService _notificationService;
        private readonly IAbsenceService _absenceService;
        private readonly IDayOffService _dayOffService;
        private readonly IWorkDayService _workDayService;
        private readonly IViewListMapper _listMapper;
        private readonly IMapHelper _mapHelper;
        private readonly IFileManagmentService _fileManagmentService;

        public TeamleadController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IEmailSender emailSender,
                                  IViewListMapper listMapper,
                                  IEmployeeService employeeService,
                                  IVacationService vacationService,
                                  IResidualsService residualsService,
                                  ITransactionService transactionService,
                                  ITimeManagementService timeManagementService,
                                  IPostService postService,
                                  ISickLeaveService sickLeaveRequestService,
                                  ITeamService teamService,
                                  IAssignmentService assignmentService,
                                  IDayOffService dayOffService,
                                  IWorkDayService workDayService,
                                  INotificationService notificationService,
                                  IAbsenceService absenceService,
                                  IMapHelper mapHelper,
                                  IFileManagmentService fileManagmentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _listMapper = listMapper;
            _employeeService = employeeService;
            _vacationService = vacationService;
            _residualsService = residualsService;
            _timeManagementService = timeManagementService;
            _transactionService = transactionService;
            _postService = postService;
            _sickLeaveService = sickLeaveRequestService;
            _teamService = teamService;
            _assignmentService = assignmentService;
            _notificationService = notificationService;
            _absenceService = absenceService;
            _workDayService = workDayService;
            _dayOffService = dayOffService;
            _mapHelper = mapHelper;
            _fileManagmentService = fileManagmentService;
        }

        #region Calendar

        [HttpGet]
        public async Task<IActionResult> Calendar(int year, int month, string teamId)
        {
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                TeamDTO team = new TeamDTO();
                CalendarViewModel model = new CalendarViewModel();
                EmployeeDTO[] employees = new EmployeeDTO[] { };
                var teams = await _teamService.GetAsync(t => t.OrganisationId == viewer.OrganisationId && (t.TeamleadId == viewer.EmployeeId || t.TeamId == viewer.TeamId));
                if (year == 0 || month == 0)
                {
                    year = DateTime.UtcNow.Year;
                    month = DateTime.UtcNow.Month;
                }

                if (teamId == null)
                {
                    if (viewer.TeamId != null)
                    {
                        team = teams.FirstOrDefault(t => t.TeamId == viewer.TeamId);
                    }
                    else
                    {
                        team = teams.FirstOrDefault(t => t.Employees.Count() != 0);
                    }
                }
                else
                {
                    team = teams.FirstOrDefault(t => t.TeamId == teamId);
                    if (team == null)
                    {
                        if (viewer.TeamId != null)
                        {
                            team = teams.FirstOrDefault(t => t.TeamId == viewer.TeamId);
                        }
                        else
                        {
                            team = teams.FirstOrDefault(t => t.Employees.Count() != 0);
                        }
                    }
                }

                if (team != null)
                {
                    if (team.TeamId == viewer.TeamId)
                    {
                        employees = await _employeeService.GetForCalendarAsync(team.TeamId, team.TeamleadId, month, year, viewer.EmployeeId);
                        model = new CalendarViewModel
                        {
                            Year = year,
                            Month = month,
                            TeamId = team.TeamId,
                            TeamName = team.Name,
                            MainMemberId = viewer.EmployeeId,
                            Teams = _listMapper.CreateTeamList(teams, team.TeamId),
                            Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                            Culture = CultureInfo.CreateSpecificCulture("ru-RU"),
                            Role = "Teamlead"
                        };

                        return View("~/Views/Profile/Calendar.cshtml", model);
                    }

                    employees = await _employeeService.GetForCalendarAsync(team.TeamId, team.TeamleadId, month, year);

                    model = new CalendarViewModel
                    {
                        Year = year,
                        Month = month,
                        TeamId = team.TeamId,
                        TeamName = team.Name,
                        MainMemberId = team.TeamleadId,
                        Teams = _listMapper.CreateTeamList(teams, team.TeamId),
                        Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                        Culture = CultureInfo.CreateSpecificCulture("ru-RU"),
                        Role = "Teamlead"
                    };

                    return View(model);
                }

                return View(new CalendarViewModel()
                {
                    Year = year,
                    Month = month,
                    Teams = _listMapper.CreateTeamList(teams, null)
                });
            }

            return RedirectToAction("Profile", "Profile");
        }


        [HttpPost]
        public async Task<IActionResult> EditDayOff(EditCalendarDayViewModel model)
        {
            var dayOff = await _dayOffService.GetByIdAsync(model.CalendarDayId);
            if (dayOff != null)
            {
                if (model.Result == CalendarDayTypeEnum.WorkDay.ToString())
                {
                    var workDay = new WorkDayDTO
                    {
                        WorkDayId = Guid.NewGuid().ToString(),
                        EmployeeId = dayOff.Employee.EmployeeId,
                        WorkDayDate = model.Date,
                        TimeFrom = model.TimeFrom.TimeOfDay,
                        TimeTo = model.TimeTo.TimeOfDay,
                        ProcessDate = DateTime.UtcNow
                    };
                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = dayOff.Employee.EmployeeId,
                        OrganisationId = dayOff.Employee.OrganisationId,
                        NotificationType = NotificationTypeEnum.WorkDay.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        RelatedDate = model.Date,
                        RelatedTeamId = model.TeamId
                    };
                    if (dayOff.DayOffImportance != null)
                    {
                        notification.Description = $"Подтверждение рабочего дня на {workDay.WorkDayDate.ToLocalTime().ToString("dd.MM.yyyy")}";
                        dayOff.DayOffState = DayOffStateEnum.Requested.ToString();
                        dayOff.ProcessDate = DateTime.UtcNow;
                        await _dayOffService.UpdateAsync(dayOff);
                    }
                    else
                    {
                        notification.Description = $"Ваш выходной день на {dayOff.DayOffDate.ToLocalTime().ToString("dd.MM.yyyy")} был заменён рабочим днём";
                        await _dayOffService.DeleteAsync(dayOff.DayOffId);
                    }

                    await _workDayService.CreateAsync(workDay);
                    await _notificationService.CreateAsync(notification, true);

                    var cellModel = new CalendarWorkDayCellViewModel
                    {
                        EmployeeId = workDay.EmployeeId,
                        Date = workDay.WorkDayDate,
                        FirstName = dayOff.Employee.FirstName,
                        LastName = dayOff.Employee.LastName,
                        TeamId = dayOff.Employee.TeamId,
                        TimeFrom = new DateTime(1, 1, 1, workDay.TimeFrom.Hours, workDay.TimeFrom.Minutes, 0),
                        TimeTo = new DateTime(1, 1, 1, workDay.TimeTo.Hours, workDay.TimeTo.Minutes, 0),
                        DayOffImportance = dayOff.DayOffImportance
                    };

                    return PartialView("CalendarWorkDayCell", cellModel);
                }
                else if (dayOff.DayOffState == DayOffStateEnum.Requested.ToString())
                {
                    dayOff.DayOffState = DayOffStateEnum.Approved.ToString();
                    dayOff.ProcessDate = DateTime.UtcNow;
                    await _dayOffService.UpdateAsync(dayOff, true);

                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = dayOff.Employee.EmployeeId,
                        OrganisationId = dayOff.Employee.OrganisationId,
                        NotificationType = NotificationTypeEnum.DayOff.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Подтверждение выходного дня на {model.Date.ToLocalTime().ToString("dd.MM.yyyy")}",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        RelatedDate = model.Date,
                        RelatedTeamId = model.TeamId
                    };

                    await _notificationService.CreateAsync(notification, true);

                    var cellModel = new CalendarDayOffCellViewModel
                    {
                        EmployeeId = dayOff.EmployeeId,
                        Date = dayOff.DayOffDate,
                        FirstName = dayOff.Employee.FirstName,
                        LastName = dayOff.Employee.LastName,
                        TeamId = dayOff.Employee.TeamId,
                        Comment = dayOff.Comment,
                        DayOffImportance = dayOff.DayOffImportance,
                        DayOffState = dayOff.DayOffState
                    };

                    return PartialView("CalendarDayOffCell", cellModel);
                }
            }

            return Json(false);
        }

        [HttpPost]
        public async Task<IActionResult> EditWorkDay(EditCalendarDayViewModel model)
        {
            var workDay = await _workDayService.GetByIdAsync(model.CalendarDayId);
            if (workDay != null)
            {
                if (model.Result == CalendarDayTypeEnum.DayOff.ToString())
                {
                    var dayOffEx = await _dayOffService.GetByDateAsync(workDay.WorkDayDate, workDay.EmployeeId);
                    if (dayOffEx != null && dayOffEx.DayOffImportance != null)
                    {
                        dayOffEx.DayOffState = DayOffStateEnum.Approved.ToString();
                        dayOffEx.ProcessDate = DateTime.UtcNow;
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = workDay.Employee.EmployeeId,
                            OrganisationId = workDay.Employee.OrganisationId,
                            NotificationType = NotificationTypeEnum.DayOff.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            Description = $"Ваш рабочий день на {model.Date.ToLocalTime().ToString("dd.MM.yyyy")} был заменён выходным",
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            RelatedDate = model.Date,
                            RelatedTeamId = model.TeamId
                        };
                        await _dayOffService.UpdateAsync(dayOffEx);
                        await _notificationService.CreateAsync(notification);
                        await _workDayService.DeleteAsync(workDay.WorkDayId, true);

                        var cellModel = new CalendarDayOffCellViewModel
                        {
                            EmployeeId = dayOffEx.EmployeeId,
                            Date = dayOffEx.DayOffDate,
                            FirstName = workDay.Employee.FirstName,
                            LastName = workDay.Employee.LastName,
                            TeamId = workDay.Employee.TeamId,
                            Comment = dayOffEx.Comment,
                            DayOffImportance = dayOffEx.DayOffImportance,
                            DayOffState = dayOffEx.DayOffState
                        };

                        return PartialView("CalendarDayOffCell", cellModel);
                    }
                    else
                    {
                        var dayOff = new DayOffDTO
                        {
                            DayOffId = Guid.NewGuid().ToString(),
                            EmployeeId = workDay.Employee.EmployeeId,
                            DayOffDate = model.Date,
                            DayOffState = DayOffStateEnum.Approved.ToString(),
                            ProcessDate = DateTime.UtcNow
                        };
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = workDay.Employee.EmployeeId,
                            OrganisationId = workDay.Employee.OrganisationId,
                            NotificationType = NotificationTypeEnum.DayOff.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            Description = $"Ваш рабочий день на {model.Date.ToLocalTime().ToString("dd.MM.yyyy")} был заменён выходным",
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            RelatedDate = model.Date,
                            RelatedTeamId = model.TeamId
                        };

                        await _workDayService.DeleteAsync(workDay.WorkDayId);
                        await _dayOffService.CreateAsync(dayOff);
                        await _notificationService.CreateAsync(notification, true);

                        var cellModel = new CalendarDayOffCellViewModel
                        {
                            EmployeeId = dayOff.EmployeeId,
                            Date = dayOff.DayOffDate,
                            FirstName = workDay.Employee.FirstName,
                            LastName = workDay.Employee.LastName,
                            TeamId = workDay.Employee.TeamId,
                            Comment = dayOff.Comment,
                            DayOffImportance = dayOff.DayOffImportance,
                            DayOffState = dayOff.DayOffState
                        };

                        return PartialView("CalendarDayOffCell", cellModel);
                    }
                }
                else
                {
                    if (workDay.TimeFrom != model.TimeFrom.TimeOfDay || workDay.TimeTo != model.TimeTo.TimeOfDay)
                    {
                        var dayOffEx = await _dayOffService.GetByDateAsync(workDay.WorkDayDate, workDay.EmployeeId);
                        workDay.TimeFrom = model.TimeFrom.TimeOfDay;
                        workDay.TimeTo = model.TimeTo.TimeOfDay;
                        workDay.ProcessDate = DateTime.UtcNow;
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = workDay.Employee.EmployeeId,
                            OrganisationId = workDay.Employee.OrganisationId,
                            NotificationType = NotificationTypeEnum.WorkDay.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            Description = $"Были внесены изменения в Ваш рабочий день на {model.Date.ToLocalTime().ToString("dd.MM.yyyy")}",
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            RelatedDate = model.Date,
                            RelatedTeamId = model.TeamId
                        };
                        await _workDayService.UpdateAsync(workDay);
                        await _notificationService.CreateAsync(notification, true);

                        var cellModel = new CalendarWorkDayCellViewModel
                        {
                            EmployeeId = workDay.EmployeeId,
                            Date = workDay.WorkDayDate,
                            FirstName = workDay.Employee.FirstName,
                            LastName = workDay.Employee.LastName,
                            TeamId = workDay.Employee.TeamId,
                            TimeFrom = new DateTime(1, 1, 1, workDay.TimeFrom.Hours, workDay.TimeFrom.Minutes, 0),
                            TimeTo = new DateTime(1, 1, 1, workDay.TimeTo.Hours, workDay.TimeTo.Minutes, 0),
                            DayOffImportance = dayOffEx?.DayOffImportance
                        };

                        return PartialView("CalendarWorkDayCell", cellModel);
                    }
                    else
                    {
                        return Json(false);
                    }
                }
            }

            return Json(false);
        }


        [HttpGet]
        public async Task<IActionResult> ProccessCalendarDay(string id, string date, string name, string surname, string teamId)
        {
            var day = Convert.ToDateTime(date);
            var dayOff = await _dayOffService.GetByDateAsync(day, id);
            var workDay = await _workDayService.GetByDateAsync(day, id);
            if (workDay != null)
            {
                var model = new EditCalendarDayViewModel
                {
                    CalendarDayId = workDay.WorkDayId,
                    Date = day,
                    TimeFrom = new DateTime(day.Year, day.Month, day.Day, workDay.TimeFrom.Hours, workDay.TimeFrom.Minutes, 0),
                    TimeTo = new DateTime(day.Year, day.Month, day.Day, workDay.TimeTo.Hours, workDay.TimeTo.Minutes, 0),
                    FirstName = name,
                    LastName = surname,
                    TeamId = teamId,
                    EmployeeId = id
                };

                return PartialView("EditWorkDayModal", model);
            }
            else if (dayOff != null)
            {
                var model = new EditCalendarDayViewModel
                {
                    CalendarDayId = dayOff.DayOffId,
                    Date = day,
                    FirstName = name,
                    LastName = surname,
                    TeamId = teamId,
                    EmployeeId = id,
                };

                return PartialView("EditDayOffModal", model);

            }
            else
            {
                var model = new CalendarDayViewModel
                {
                    EmployeeId = id,
                    Date = day,
                    FirstName = name,
                    LastName = surname,
                    TeamId = teamId,
                };

                return PartialView("ProccessCalendarDayModal", model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProccessCalendarDay(CalendarDayViewModel model)
        {
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null)
            {
                if (model.Result == CalendarDayTypeEnum.DayOff.ToString())
                {
                    var dayOff = new DayOffDTO
                    {
                        DayOffId = Guid.NewGuid().ToString(),
                        EmployeeId = employee.EmployeeId,
                        DayOffDate = model.Date,
                        DayOffState = DayOffStateEnum.Approved.ToString(),
                        ProcessDate = DateTime.UtcNow
                    };
                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = employee.EmployeeId,
                        OrganisationId = employee.OrganisationId,
                        NotificationType = NotificationTypeEnum.DayOff.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Подтверждение выходного дня на {model.Date.ToLocalTime().ToString("dd.MM.yyyy")}",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        RelatedDate = model.Date,
                        RelatedTeamId = model.TeamId
                    };
                    await _dayOffService.CreateAsync(dayOff);
                    await _notificationService.CreateAsync(notification, true);

                    var cellModel = new CalendarDayOffCellViewModel
                    {
                        EmployeeId = dayOff.EmployeeId,
                        Date = dayOff.DayOffDate,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        TeamId = employee.TeamId,
                        DayOffState = dayOff.DayOffState,
                        DayOffImportance = dayOff.DayOffImportance,

                    };

                    return PartialView("CalendarDayOffCell", cellModel);
                }
                else
                {
                    var workDay = new WorkDayDTO
                    {
                        WorkDayId = Guid.NewGuid().ToString(),
                        EmployeeId = employee.EmployeeId,
                        WorkDayDate = model.Date,
                        TimeFrom = model.TimeFrom.TimeOfDay,
                        TimeTo = model.TimeTo.TimeOfDay,
                        ProcessDate = DateTime.UtcNow
                    };
                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = employee.EmployeeId,
                        OrganisationId = employee.OrganisationId,
                        NotificationType = NotificationTypeEnum.WorkDay.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Подтверждение рабочего дня на {model.Date.ToLocalTime().ToString("dd.MM.yyyy")}",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        RelatedDate = model.Date,
                        RelatedTeamId = model.TeamId
                    };
                    await _workDayService.CreateAsync(workDay);
                    await _notificationService.CreateAsync(notification, true);

                    var cellModel = new CalendarWorkDayCellViewModel
                    {
                        EmployeeId = workDay.EmployeeId,
                        Date = workDay.WorkDayDate,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName,
                        TeamId = employee.TeamId,
                        TimeFrom = new DateTime(1, 1, 1, workDay.TimeFrom.Hours, workDay.TimeFrom.Minutes, 0),
                        TimeTo = new DateTime(1, 1, 1, workDay.TimeTo.Hours, workDay.TimeTo.Minutes, 0)
                    };

                    return PartialView("CalendarWorkDayCell", cellModel);
                }
            }

            return Json(false);
        }

        [HttpGet]
        public IActionResult DayProccessMenu(string id, string date, string name, string surname, string teamId)
        {
            var model = new DayProccessMenuViewModel
            {
                EmployeeId = id,
                Date = date,
                TeamId = teamId,
                Name = name,
                Surname = surname,
            };

            return PartialView("DayProccessMenuModal", model);
        }

        [HttpGet]
        public IActionResult SelfDayProccessMenu(string id, string date, string name, string surname, string teamId)
        {
            var model = new DayProccessMenuViewModel
            {
                EmployeeId = id,
                Date = date,
                TeamId = teamId,
                Name = name,
                Surname = surname,
            };

            return PartialView("SelfDayProccessMenuModal", model);
        }


        [HttpGet]
        public async Task<IActionResult> TimeManagementRecords(string id, string date)
        {
            var recordsDate = Convert.ToDateTime(date);
            var employee = await _employeeService.GetByIdAsync(id);
            var records = await _timeManagementService.GetAsync(r => r.EmployeeId == id && r.RecordDate.Date == recordsDate.Date);
            if (employee != null)
            {
                var model = new TimeManagementListViewModel
                {
                    EmployeeFullName = $"{employee.FirstName} {employee.LastName}",
                    Date = recordsDate,
                    Records = _mapHelper.MapCollection<TimeManagementRecordDTO, TimeManagementListUnitViewModel>(records.OrderBy(r => r.RecordDate).ToArray())
                };

                return PartialView("TimeManagementModal", model);
            }

            return Json(false);
        }

        #endregion

        #region Absences
        [HttpGet]
        public async Task<IActionResult> Absences(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var absences = new AbsenceListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                absences = await _absenceService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId && a.Employee.Team.TeamleadId == viewer.EmployeeId);
                count = await _absenceService.GetAbsencesCountAsync(searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId && a.Employee.Team.TeamleadId == viewer.EmployeeId);
            }

            var pagedAbsences = new AbsenceListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Absences = _mapHelper.MapCollection<AbsenceListUnitDTO, AbsenceViewModel>(absences)
            };

            return View(pagedAbsences);
        }

        [HttpGet]
        public async Task<IActionResult> AbsencesPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var absences = new AbsenceListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                absences = await _absenceService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId && a.Employee.Team.TeamleadId == viewer.EmployeeId);
                count = await _absenceService.GetAbsencesCountAsync(searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId && a.Employee.Team.TeamleadId == viewer.EmployeeId);
            }

            var pagedAbsences = new AbsenceListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Absences = _mapHelper.MapCollection<AbsenceListUnitDTO, AbsenceViewModel>(absences)
            };

            return PartialView(pagedAbsences);
        }

        [HttpGet]
        public async Task<IActionResult> SetAbsence(string id, string teamId, string firstName, string lastName, string role)
        {
            var abs = await _absenceService.GetTodayByEmployeeIdAsync(id);
            if (abs == null)
            {

                var model = new AbsenceApproveViewModel
                {
                    TeamId = teamId,
                    EmployeeId = id,
                    Date = DateTime.UtcNow,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = role
                };

                return PartialView("AbsenceApproveModal", model);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> SetAbsence(AbsenceApproveViewModel model)
        {
            var abs = await _absenceService.GetTodayByEmployeeIdAsync(model.EmployeeId);
            if (abs == null)
            {
                var absence = new AbsenceDTO
                {
                    AbsenceId = Guid.NewGuid().ToString(),
                    EmployeeId = model.EmployeeId,
                    AbsenceDate = model.Date
                };
                var residual = await _residualsService.GetByEmployeeIdAsync(model.EmployeeId, ResidualTypeEnum.Absence.ToString());
                residual.ResidualBalance += 1;
                await _residualsService.UpdateAsync(residual);
                await _absenceService.CreateAsync(absence, true);
                return Json(true);
            }

            return Json(false);
        }

        #endregion

        #region Teams

        [HttpGet]
        public async Task<IActionResult> TeamProfile(string id)
        {
            var team = await _teamService.GetByIdAsync(id);

            var model = _mapHelper.Map<TeamDTO, TeamProfileViewModel>(team);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Teams(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var teams = new TeamListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                teams = await _teamService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, t => t.OrganisationId == viewer.OrganisationId && t.TeamleadId == viewer.EmployeeId);
                count = await _teamService.GetTeamsCountAsync(searchKey, t => t.OrganisationId == viewer.OrganisationId && t.TeamleadId == viewer.EmployeeId);
            }

            var pagedTeams = new TeamListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Teams = _mapHelper.MapCollection<TeamListUnitDTO, TeamViewModel>(teams)
            };

            return View(pagedTeams);
        }

        [HttpGet]
        public async Task<IActionResult> TeamsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var teams = new TeamListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                teams = await _teamService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, t => t.OrganisationId == viewer.OrganisationId && t.TeamleadId == viewer.EmployeeId);
                count = await _teamService.GetTeamsCountAsync(searchKey, t => t.OrganisationId == viewer.OrganisationId && t.TeamleadId == viewer.EmployeeId);
            }

            var pagedTeams = new TeamListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Teams = _mapHelper.MapCollection<TeamListUnitDTO, TeamViewModel>(teams)
            };

            return PartialView(pagedTeams);
        }

        #endregion

        #region Employees

        [HttpGet]
        public async Task<IActionResult> EmployeeProfile(string id)
        {
            var employee = await _employeeService.GetByIdWithResidualsAsync(id);
            var model = new EmployeeProfileViewModel();
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(employee.EmployeeId);
                var role = await _userManager.GetRolesAsync(user);
                var posts = await _postService.GetPostsByOrganisationIdAsync(employee.OrganisationId);
                model = _mapHelper.Map<EmployeeDTO, EmployeeProfileViewModel>(employee);
                model.AbsenceBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance;
                model.AssignmentBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance;
                model.PaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance;
                model.UnpaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance;
                model.SickLeaveBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance;
                model.Role = role.First();
                model.Post = posts.FirstOrDefault(p => p.PostId == employee.PostId).Name;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Employees(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var employees = new EmployeeListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                employees = await _employeeService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                emp => emp.State && emp.OrganisationId == employee.OrganisationId && emp.Team.TeamleadId == employee.EmployeeId);
                count = await _employeeService.GetEmployeesCountAsync(searchKey, emp => emp.State && emp.OrganisationId == employee.OrganisationId && emp.Team.TeamleadId == employee.EmployeeId);
            }

            var pagedEmployees = new EmployeeListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Employees = _mapHelper.MapCollection<EmployeeListUnitDTO, EmployeeViewModel>(employees)
            };

            return View(pagedEmployees);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var employees = new EmployeeListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                employees = await _employeeService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                emp => emp.State && emp.OrganisationId == employee.OrganisationId && emp.Team.TeamleadId == employee.EmployeeId);
                count = await _employeeService.GetEmployeesCountAsync(searchKey, emp => emp.State && emp.OrganisationId == employee.OrganisationId && emp.Team.TeamleadId == employee.EmployeeId);
            }

            var pagedEmployees = new EmployeeListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Employees = _mapHelper.MapCollection<EmployeeListUnitDTO, EmployeeViewModel>(employees)
            };

            return PartialView(pagedEmployees);
        }

        #endregion

        #region Vacations

        [HttpGet]
        public async Task<IActionResult> Vacations(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var vacations = new VacationRequestViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                vacations = await _vacationService.GetPageAsync(pageNumber,
                                                                          (int)PageSizeEnum.PageSize15,
                                                                          RequestStatusEnum.Pending.ToString(),
                                                                          searchKey,
                                                                          req => (req.Employee.Team.TeamleadId == employee.EmployeeId)
                                                                          && req.RequestStatus != RequestStatusEnum.Proccessing.ToString()
                                                                          && req.Employee.OrganisationId == employee.OrganisationId);
                count = await _vacationService.GetVacationsNumberAsync(searchKey,
                                                                  req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                  && req.RequestStatus != RequestStatusEnum.Proccessing.ToString()
                                                                  && req.Employee.OrganisationId == employee.OrganisationId);
            }

            var pagedVacations = new VacationRequestListViewModel
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                Vacations = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(vacations)
            };

            return View(pagedVacations);
        }

        [HttpGet]
        public async Task<IActionResult> VacationsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var vacations = new VacationRequestViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                vacations = await _vacationService.GetPageAsync(pageNumber,
                                                                          (int)PageSizeEnum.PageSize15,
                                                                          RequestStatusEnum.Pending.ToString(),
                                                                          searchKey,
                                                                          req => (req.Employee.Team.TeamleadId == employee.EmployeeId)
                                                                          && req.RequestStatus != RequestStatusEnum.Proccessing.ToString()
                                                                          && req.Employee.OrganisationId == employee.OrganisationId);
                count = await _vacationService.GetVacationsNumberAsync(searchKey,
                                                                  req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                  && req.RequestStatus != RequestStatusEnum.Proccessing.ToString()
                                                                  && req.Employee.OrganisationId == employee.OrganisationId);
            }

            var pagedVacations = new VacationRequestListViewModel
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                Vacations = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(vacations)
            };

            return PartialView(pagedVacations);
        }

       
        [HttpGet]
        public async Task<IActionResult> CheckVacationRequest(string id)
        {
            var vacationRequest = await _vacationService.GetByIdWithEmployeeWithTeamAsync(id);
            if (vacationRequest != null)
            {
                if(vacationRequest.RequestStatus == RequestStatusEnum.Pending.ToString())
                {
                    var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(vacationRequest);
                    model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                    model.VacationType = vacationRequest.VacationType;
                    if (vacationRequest.Employee.Team != null)
                    {
                        var teamlead = await _employeeService.GetByIdAsync(vacationRequest.Employee.Team.TeamleadId);
                        model.TeamName = vacationRequest.Employee.Team.Name;
                        model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";
                    }
                    else
                    {
                        model.TeamName = emptyValue;
                        model.TeamleadFullName = emptyValue;
                    }

                    return PartialView("VacationProccessModal", model);
                }
                else
                {
                    var proccessor = await _employeeService.GetByIdAsync(vacationRequest.ProccessedbyId);
                    var posts = await _postService.GetPostsByOrganisationIdAsync(vacationRequest.Employee.OrganisationId);
                    var model = _mapHelper.Map<VacationRequestDTO, VacationRequestCheckViewModel>(vacationRequest);
                    model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                    model.VacationType = vacationRequest.VacationType;
                    model.Post = posts.FirstOrDefault(p => p.PostId == vacationRequest.Employee.PostId).Name;
                    model.ProccessedByName = $"{proccessor.FirstName} {proccessor.LastName}";
                    model.RequestStatus = vacationRequest.RequestStatus;
                    if (vacationRequest.Employee.Team != null)
                    {
                        var teamlead = await _employeeService.GetByIdAsync(vacationRequest.Employee.Team.TeamleadId);
                        model.TeamName = vacationRequest.Employee.Team.Name;
                        model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";
                    }
                    else
                    {
                        model.TeamName = emptyValue;
                        model.TeamleadFullName = emptyValue;
                    }

                    return PartialView("VacationViewModal", model);
                }
            }
            
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ProccessVacationRequest(VacationRequestProccessViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var vacationRequest = await _vacationService.GetByIdAsync(model.VacationId);
            if (vacationRequest != null && vacationRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                vacationRequest.ProccessDate = DateTime.UtcNow;
                vacationRequest.ProccessedbyId = user.Id;
                if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                {
                    vacationRequest.RequestStatus = RequestStatusEnum.Proccessing.ToString();
                    var transaction = new TransactionDTO()
                    {
                        TransactionId = Guid.NewGuid().ToString(),
                        EmployeeId = vacationRequest.EmployeeId,
                        Change = model.Duration,
                        Description = model.VacationType,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = model.VacationType
                    };

                    var notifications = new NotificationDTO[]
                    {
                    new NotificationDTO
                        {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = vacationRequest.EmployeeId,
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.Vacation.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Ваш запрос на отпуск был подтверждён руководителем",
                        NotificationRange = NotificationRangeEnum.User.ToString()
                        },
                   new NotificationDTO
                        {
                        NotificationId = Guid.NewGuid().ToString(),
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.Vacation.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Запрос на отпуск работника был подтверждён его руководителем",
                        NotificationRange = NotificationRangeEnum.Organisation.ToString()
                        }
                    };

                    await _notificationService.CreateRangeAsync(notifications);
                }
                else
                {
                    vacationRequest.RequestStatus = RequestStatusEnum.Denied.ToString();
                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = vacationRequest.EmployeeId,
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.Vacation.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Ваш запрос на отпуск был отклонён руководителем",
                        NotificationRange = NotificationRangeEnum.User.ToString()
                    };

                    await _notificationService.CreateAsync(notification);
                }

                await _vacationService.UpdateAsync(vacationRequest, true);
            }

            return RedirectToAction("Vacations", "Teamlead");
        }

        #endregion

        #region Sickleaves

        [HttpGet]
        public async Task<IActionResult> Sickleaves(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var sickLeaves = new SickLeaveViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                sickLeaves = await _sickLeaveService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, null, searchKey,
                                                                  req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.Team.TeamleadId == employee.EmployeeId && req.Employee.State);
                count = await _sickLeaveService.GetSickLeavesNumber(searchKey, req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.Team.TeamleadId == employee.EmployeeId && req.Employee.State);
            }

            var pagedSickLeaves = new SickLeaveListViewModel()
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                SickLeaves = _mapHelper.MapCollection<SickLeaveViewDTO, SickLeaveRequestViewModel>(sickLeaves)
            };

            return View(pagedSickLeaves);
        }

        [HttpGet]
        public async Task<IActionResult> SickleavesPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var sickLeaves = new SickLeaveViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                sickLeaves = await _sickLeaveService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, null, searchKey,
                                                                  req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.Team.TeamleadId == employee.EmployeeId && req.Employee.State);
                count = await _sickLeaveService.GetSickLeavesNumber(searchKey, req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.Team.TeamleadId == employee.EmployeeId && req.Employee.State);
            }

            var pagedSickLeaves = new SickLeaveListViewModel()
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                SickLeaves = _mapHelper.MapCollection<SickLeaveViewDTO, SickLeaveRequestViewModel>(sickLeaves)
            };

            return PartialView(pagedSickLeaves);
        }

        [HttpGet]
        public async Task<IActionResult> CheckSickleaveRequest(string id)
        {
            var sickLiveRequest = await _sickLeaveService.GetByIdWithEmployeeWithTeamAsync(id);
            if (sickLiveRequest != null)
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, SickLeaveCheckViewModel>(sickLiveRequest);
                model.Files = await _fileManagmentService.GetSickLeaveFilesAsync(sickLiveRequest.SickLeaveId);
                var posts = await _postService.GetPostsByOrganisationIdAsync(sickLiveRequest.Employee.OrganisationId);
                model.EmployeeFullName = $"{sickLiveRequest.Employee.FirstName} {sickLiveRequest.Employee.LastName}";
                model.Post = posts.FirstOrDefault(p => p.PostId == sickLiveRequest.Employee.PostId).Name;
                if (sickLiveRequest.Employee.Team != null)
                {
                    var teamlead = await _employeeService.GetByIdAsync(sickLiveRequest.Employee.Team.TeamleadId);
                    model.TeamName = sickLiveRequest.Employee.Team.Name;
                    model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";
                    if (teamlead.WorkEmail == User.Identity.Name && sickLiveRequest.RequestStatus == RequestStatusEnum.Pending.ToString())
                    {

                        return PartialView("SickleaveProccessModal", model);
                    }
                }
                else
                {
                    model.TeamName = emptyValue;
                    model.TeamleadFullName = emptyValue;
                }

                if (sickLiveRequest.ProccessedbyId != null)
                {
                    var proccessor = await _employeeService.GetByIdAsync(sickLiveRequest.ProccessedbyId);
                    model.ProccessedByName = $"{proccessor.FirstName} {proccessor.LastName}";
                }

                return PartialView("SickleaveViewModal", model);
            }

            return PartialView("SickleaveViewModal");
        }

        [HttpGet]
        public async Task<IActionResult> ProccessSickLeaveRequest(string id)
        {
            var sickLeaveRequest = await _sickLeaveService.GetByIdWithEmployeeWithTeamAsync(id);
            if (sickLeaveRequest != null && sickLeaveRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, SickLeaveCheckViewModel>(sickLeaveRequest);
                model.Files = await _fileManagmentService.GetSickLeaveFilesAsync(model.SickLeaveId);
                model.EmployeeFullName = $"{sickLeaveRequest.Employee.FirstName} {sickLeaveRequest.Employee.LastName}";
                if (sickLeaveRequest.Employee.Team != null)
                {
                    var teamlead = await _employeeService.GetByIdAsync(sickLeaveRequest.Employee.Team.TeamleadId);
                    model.TeamName = sickLeaveRequest.Employee.Team.Name;
                    model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";
                }
                else
                {
                    model.TeamName = emptyValue;
                    model.TeamleadFullName = emptyValue;
                }

                return PartialView("SickleaveProccessModal", model);
            }

            return PartialView("SickleaveProccessModal");
        }

        [HttpPost]
        public async Task<IActionResult> ProccessSickLeaveRequest(SickLeaveCheckViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var sickLeaveRequest = await _sickLeaveService.GetByIdAsync(model.SickLeaveId);
            if (sickLeaveRequest != null && sickLeaveRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                sickLeaveRequest.ProccessedbyId = user.Id;
                if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                {
                    sickLeaveRequest.RequestStatus = RequestStatusEnum.Approved.ToString();
                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = sickLeaveRequest.EmployeeId,
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Ваш запрос на больничный был подтверждён",
                        NotificationRange = NotificationRangeEnum.User.ToString()
                    };

                    await _notificationService.CreateAsync(notification);
                }
                else
                {
                    sickLeaveRequest.RequestStatus = RequestStatusEnum.Denied.ToString();
                    var notification = new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = sickLeaveRequest.EmployeeId,
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Ваш запрос на больничный был отклонён",
                        NotificationRange = NotificationRangeEnum.User.ToString()
                    };

                    await _notificationService.CreateAsync(notification);
                }

                await _sickLeaveService.UpdateAsync(sickLeaveRequest, true);
            }

            return RedirectToAction("Sickleaves", "Teamlead");
        }

        #endregion

        #region Assignments

        [HttpGet]
        public async Task<IActionResult> Assignments(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var assignments = new AssignmentListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                assignments = await _assignmentService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                                    a => a.OrganisationId == employee.OrganisationId
                                                                    && a.AssignmentEmployee.Any(ae => ae.Employee.Team.TeamleadId == employee.EmployeeId), searchKey);

                count = await _assignmentService.GetAssignmentsCountAsync(searchKey, a => a.OrganisationId == employee.OrganisationId && a.AssignmentEmployee.Any(ae => ae.Employee.Team.TeamleadId == employee.EmployeeId));
            }

            var pagedAssignments = new AssignmentListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Assignments = _mapHelper.MapCollection<AssignmentListUnitDTO, AssignmentViewModel>(assignments)
            };

            return View(pagedAssignments);
        }

        [HttpGet]
        public async Task<IActionResult> AssignmentsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var assignments = new AssignmentListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                assignments = await _assignmentService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                                    a => a.OrganisationId == employee.OrganisationId
                                                                    && a.AssignmentEmployee.Any(ae => ae.Employee.Team.TeamleadId == employee.EmployeeId), searchKey);

                count = await _assignmentService.GetAssignmentsCountAsync(searchKey, a => a.OrganisationId == employee.OrganisationId && a.AssignmentEmployee.Any(ae => ae.Employee.Team.TeamleadId == employee.EmployeeId));
            }

            var pagedAssignments = new AssignmentListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Assignments = _mapHelper.MapCollection<AssignmentListUnitDTO, AssignmentViewModel>(assignments)
            };

            return PartialView(pagedAssignments);
        }

        [HttpGet]
        public async Task<IActionResult> CheckAssignment(string id)
        {
            var assignment = await _assignmentService.GetByIdWithEmployeesAsync(id);
            if (assignment != null)
            {
                var assignmentEmployees = assignment.AssignmentEmployee.Select(a => a.Employee.EmployeeId).ToArray();
                var model = new AssignmentCheckViewModel
                {
                    Name = assignment.Name,
                    BeginDate = assignment.BeginDate,
                    EndDate = assignment.EndDate,
                    AssignmentMembers = assignment.AssignmentEmployee.Select(ae => new EmployeeAssignmentViewModel
                    {
                        EmployeeId = ae.Employee.EmployeeId,
                        FirstName = ae.Employee.FirstName,
                        LastName = ae.Employee.LastName
                    }).ToArray()
                };

                return PartialView("AssignmentViewModal", model);
            }

            return RedirectToAction("Assignments", "Teamlead");
        }

        #endregion
    }
}