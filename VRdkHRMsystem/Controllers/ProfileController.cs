﻿using System;
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

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private const string emptyValue = "Нет";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmployeeService _employeeService;
        private readonly ITimeManagementService _timeManagementService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IVacationService _vacationService;
        private readonly IAbsenceService _absenceService;
        private readonly IAssignmentService _assignmentService;
        private readonly INotificationService _notificationService;
        private readonly IViewListMapper _listMapper;
        private readonly ITeamService _teamService;
        private readonly IMapHelper _mapHelper;

        public ProfileController(IEmployeeService employeeService,
                                 UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 IPostService postService,
                                 IVacationService vacationService,
                                 ITimeManagementService timeManagementService,
                                 IAbsenceService absenceService,
                                 ITeamService teamService,
                                 IViewListMapper listMapper,
                                 IAssignmentService assignmentService,
                                 INotificationService notificationService,
                                 ISickLeaveService sickLeaveService,
        IMapHelper mapHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeService = employeeService;
            _sickLeaveService = sickLeaveService;
            _notificationService = notificationService;
            _timeManagementService = timeManagementService;
            _absenceService = absenceService;
            _assignmentService = assignmentService;
            _teamService = teamService;
            _listMapper = listMapper;
            _vacationService = vacationService;
            _postService = postService;
            _mapHelper = mapHelper;
        }

        #region Calendar

        [HttpGet]
        public async Task<IActionResult> Calendar(int year, int month, string teamId)
        {
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                var teams = await _teamService.GetAsync(t => t.TeamId == viewer.TeamId);

                if (year == 0 || month == 0)
                {
                    year = DateTime.UtcNow.Year;
                    month = DateTime.UtcNow.Month;
                }

                var team = teams.FirstOrDefault();

                if (team != null)
                {
                    var employees = await _employeeService.GetForCalendarAsync(team.TeamId, team.TeamleadId, month, year, viewer.EmployeeId);

                    var model = new CalendarViewModel
                    {
                        Year = year,
                        Month = month,
                        TeamId = team.TeamId,
                        TeamName = team.Name,
                        MainMemberId = viewer.EmployeeId,
                        Teams = _listMapper.CreateTeamList(teams, teamId),
                        Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                        Culture = CultureInfo.CreateSpecificCulture("ru-RU"),
                        Role = "Profile"
                    };

                    return View(model);

                }
                else
                {
                    return View(new CalendarViewModel()
                    {
                        Year = year,
                        Month = month,
                        Teams = _listMapper.CreateTeamList(teams, null)
                    });
                }
            }

            return RedirectToAction("Profile", "Profile");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTimeManagementRecord(string id)
        {
            var record = await _timeManagementService.GetByIdAsync(id);

            if (record != null)
            {
                await _timeManagementService.DeleteAsync(record, true);

                return Json(true);
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> CreateTimeManagementRecord(TimeManagementRecordViewModel model)
        {
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null)
            {
                if (model.TimeManagementRecordId == null)
                {
                    var record = new TimeManagementRecordDTO
                    {
                        TimeManagementRecordId = Guid.NewGuid().ToString(),
                        Description = model.Description,
                        EmployeeId = employee.EmployeeId,
                        TimeFrom = model.TimeFrom,
                        TimeTo = model.TimeTo,
                        ProccessDate = DateTime.UtcNow,
                        RecordDate = model.RecordDate
                    };

                    await _timeManagementService.CreateAsync(record, true);

                    model.TimeManagementRecordId = record.TimeManagementRecordId;

                    return PartialView("TimeManagementRecord", model);
                }
                else
                {
                    var record = await _timeManagementService.GetByIdAsync(model.TimeManagementRecordId);
                    record.TimeFrom = model.TimeFrom;
                    record.TimeTo = model.TimeTo;
                    record.Description = model.Description;
                    record.ProccessDate = DateTime.UtcNow;
                    await _timeManagementService.UpdateAsync(record, true);

                    return PartialView("TimeManagementRecord", model);
                }

            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> AddTimeManagementRecord(string date)
        {
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                var recordDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);

                var model = new TimeManagementRecordViewModel
                {
                    EmployeeId = employee.EmployeeId,
                    RecordDate = recordDate
                };
                return PartialView("TimeManagementRecord", model);
            }

            return PartialView("TimeManagementRecord");
        }

        [HttpGet]
        public IActionResult DayProccessMenu(string id, string date, string teamId)
        {
            var model = new DayProccessMenuViewModel
            {
                EmployeeId = id,
                Date = date,
                TeamId = teamId
            };

            return PartialView("DayProccessMenuModal", model);
        }

        [HttpGet]
        public async Task<IActionResult> TimeManagment(string date)
        {
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                var recordsDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var records = await _timeManagementService.GetAsync(r => r.EmployeeId == employee.EmployeeId && r.RecordDate.Date == recordsDate.Date);

                var model = new TimeManagementProccessViewModel
                {
                    Date = recordsDate,
                    EmployeeId = employee.EmployeeId,
                    Records = _mapHelper.MapCollection<TimeManagementRecordDTO, TimeManagementRecordViewModel>(records.OrderBy(r => r.ProccessDate).ToArray())
                };

                return PartialView("TimeManagmentModal", model);
            }

            return PartialView("TimeManagmentModal");
        }

        #endregion

        #region Profile

        [HttpGet]
        public async Task<IActionResult> Profile(string id = null)
        {
            CompositeProfileViewModel model = new CompositeProfileViewModel()
            {
                ProfileModel = new ProfileViewModel(),
                ResidualsModel = new ProfileResidualsViewModel()
            };
            EmployeeDTO employee;
            if (id != null)
            {
                employee = await _employeeService.GetByIdWithTeamWithResidualsAsync(id);
            }
            else
            {
                employee = await _employeeService.GetByEmailWithTeamWithResidualsAsync(User.Identity.Name);
            }

            var posts = await _postService.GetPostsByOrganisationIdAsync(employee.OrganisationId);
            model.ProfileModel = _mapHelper.Map<EmployeeDTO, ProfileViewModel>(employee);
            model.ProfileModel.Post = posts.FirstOrDefault(post => post.PostId.Equals(employee.PostId)).Name;
            if (employee.Team != null)
            {
                var teamlead = await _employeeService.GetByIdAsync(employee.Team.TeamleadId);
                model.ProfileModel.Team = employee.Team.Name;
                model.ProfileModel.Teamlead = $"{teamlead.FirstName} {teamlead.LastName}";
            }
            else
            {
                model.ProfileModel.Team = emptyValue;
                model.ProfileModel.Teamlead = emptyValue;
            }
            model.ResidualsModel.EmployeeId = employee.EmployeeId;
            model.ResidualsModel.AbsenceBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance;
            model.ResidualsModel.AssignmentBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance;
            model.ResidualsModel.PaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance;
            model.ResidualsModel.UnpaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance;
            model.ResidualsModel.SickLeaveBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance;

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditProfile(string id)
        {
            var employee = await _employeeService.GetByIdWithResidualsAsync(id);
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(employee.EmployeeId);
                var model = _mapHelper.Map<EmployeeDTO, EditProfileViewModel>(employee);              
                return View(model);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileViewModel model)
        {
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(model.EmployeeId);
                var oldEmail = user.Email;
                user.UserName = model.WorkEmail;
                user.Email = model.WorkEmail;
                if (model.WorkEmail != oldEmail)
                {
                    await _signInManager.RefreshSignInAsync(user);
                    await _userManager.UpdateAsync(user);
                }

                employee.FirstName = model.FirstName;
                employee.LastName = model.LastName;
                employee.WorkEmail = model.WorkEmail;
                employee.PersonalEmail = model.PersonalEmail;
                employee.PhoneNumber = model.PhoneNumber;
                await _employeeService.UpdateAsync(employee, true);

                return Ok();
            }

            return BadRequest();
        }

        #endregion

        #region RequestsPages

        [HttpGet]
        public async Task<IActionResult> VacationsPage(int pageNumber = 0)
        {
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var requests = await _vacationService.GetProfilePageAsync((int)PageSizeEnum.PageSize5, user.EmployeeId, pageNumber);
                int count = await _vacationService.GetVacationsNumberAsync(null, req => req.EmployeeId == user.EmployeeId);
                var model = new ProfileVacationListViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = (int)PageSizeEnum.PageSize5,
                    Count = count,
                    Vacations = _mapHelper.MapCollection<VacationRequestDTO, ProfileVacationsViewModel>(requests)
                };

                return PartialView(model);
            }

            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> AbsencesPage(int pageNumber = 0)
        {
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var absences = await _absenceService.GetProfilePageAsync((int)PageSizeEnum.PageSize4, user.EmployeeId, pageNumber);
                int count = await _absenceService.GetAbsencesCountAsync(null, req => req.EmployeeId == user.EmployeeId);
                var model = new ProfileAbsencesListViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = (int)PageSizeEnum.PageSize4,
                    Count = count,
                    Absences = _mapHelper.MapCollection<AbsenceDTO, ProfileAbsencesViewModel>(absences)
                };

                return PartialView(model);
            }

            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> SickleavesPage(int pageNumber = 0)
        {
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var requests = await _sickLeaveService.GetProfilePageAsync((int)PageSizeEnum.PageSize5, user.EmployeeId, pageNumber);
                int count = await _sickLeaveService.GetSickLeavesNumber(null, req => req.EmployeeId == user.EmployeeId);
                var model = new ProfileSickLeaveListViewModel
                {
                    EmployeeId = user.EmployeeId,
                    PageNumber = pageNumber,
                    PageSize = (int)PageSizeEnum.PageSize5,
                    Count = count,
                    SickLeaves = _mapHelper.MapCollection<SickLeaveRequestDTO, ProfileSickLeavesViewModel>(requests)
                };

                return PartialView(model);
            }

            return PartialView();
        }

        [HttpGet]
        public async Task<IActionResult> AssignmentsPage(int pageNumber = 0)
        {
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                var requests = await _assignmentService.GetProfilePageAsync((int)PageSizeEnum.PageSize5, user.EmployeeId, pageNumber);
                int count = await _assignmentService.GetProfileAssignmentsCountAsync(a => a.EmployeeId == user.EmployeeId);
                var model = new ProfileAssignmentListViewModel
                {
                    PageNumber = pageNumber,
                    PageSize = (int)PageSizeEnum.PageSize5,
                    Count = count,
                    Assignments = requests.Select(a => new ProfileAssignmentsViewModel
                    {
                        BeginDate = a.Assignment.BeginDate,
                        EndDate = a.Assignment.EndDate,
                        Duration = a.Assignment.Duration,
                        Name = a.Assignment.Name
                    }).ToArray()
                };

                return PartialView(model);
            }

            return PartialView();
        }

        #endregion

        #region Notifications

        [HttpGet]
        public async Task<IActionResult> Notifications(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            NotificationDTO[] notifications = new NotificationDTO[] { };
            int count = 0;
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                if (User.IsInRole("Administrator"))
                {
                    notifications = await _notificationService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                                            note => (note.EmployeeId == user.EmployeeId
                                                                                 || note.EmployeeId == null)
                                                                                 && note.OrganisationId == user.OrganisationId, searchKey);
                    count = await _notificationService.GetNotificationsNumber(note => (note.EmployeeId == user.EmployeeId || note.EmployeeId == null) && note.OrganisationId == user.OrganisationId, searchKey);
                }
                else
                {
                    notifications = await _notificationService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                  note => note.EmployeeId == user.EmployeeId && note.OrganisationId == user.OrganisationId, searchKey);
                    count = await _notificationService.GetNotificationsNumber(note => note.EmployeeId == user.EmployeeId && note.OrganisationId == user.OrganisationId, searchKey);
                }
            }
            var pagedNotifications = new NotificationListViewModel
            {
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                Count = count,
                Notifications = _mapHelper.MapCollection<NotificationDTO, NotificationViewModel>(notifications)
            };

            return View(pagedNotifications);
        }

        [HttpGet]
        public async Task<IActionResult> NotificationsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            NotificationDTO[] notifications = new NotificationDTO[] { };
            int count = 0;
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (user != null)
            {
                if (User.IsInRole("Administrator"))
                {
                    notifications = await _notificationService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                                            note => (note.EmployeeId == user.EmployeeId
                                                                                 || note.EmployeeId == null)
                                                                                 && note.OrganisationId == user.OrganisationId, searchKey);
                    count = await _notificationService.GetNotificationsNumber(note => (note.EmployeeId == user.EmployeeId || note.EmployeeId == null) && note.OrganisationId == user.OrganisationId, searchKey);
                }
                else
                {
                    notifications = await _notificationService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                  note => note.EmployeeId == user.EmployeeId && note.OrganisationId == user.OrganisationId, searchKey);
                    count = await _notificationService.GetNotificationsNumber(note => note.EmployeeId == user.EmployeeId && note.OrganisationId == user.OrganisationId, searchKey);
                }
            }
            var pagedNotifications = new NotificationListViewModel
            {
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                Count = count,
                Notifications = _mapHelper.MapCollection<NotificationDTO, NotificationViewModel>(notifications)
            };

            return PartialView(pagedNotifications);
        }


        [HttpGet]
        public async Task<IActionResult> RedirectWithNotification(string notificationId, string notificationRange, string notificationType, string role = null)
        {
            var notification = await _notificationService.GetByIdAsync(notificationId);
            if (notification != null)
            {
                await _notificationService.DeleteAsync(notification.NotificationId, true);

                if (notificationRange == NotificationRangeEnum.User.ToString())
                {
                    if (notificationType == NotificationTypeEnum.DayOff.ToString() || notificationType == NotificationTypeEnum.WorkDay.ToString())
                    {

                            return RedirectToAction("Calendar", role, new {year = notification.RelatedDate.Year, month = notification.RelatedDate.Month, teamId = notification.RelatedTeamId});
                    }
                    else
                    {
                        TempData["ActiveTab"] = notificationType.ToLower();
                        return RedirectToAction("Profile", "Profile");
                    }
                }
                else
                {
                    if (notificationType == NotificationTypeEnum.Vacation.ToString())
                    {
                        
                        return RedirectToAction("Vacations", role);
                    }
                    else if (notificationType == NotificationTypeEnum.Assignment.ToString())
                    {
                        
                        return RedirectToAction("Assignments", role);
                    }
                    else if (notificationType == NotificationTypeEnum.Absence.ToString())
                    {
                        
                        return RedirectToAction("Absences", role);
                    }
                    else if (notificationType == NotificationTypeEnum.SickLeave.ToString())
                    {
                        
                        return RedirectToAction("SickLeaves", role);
                    }
                }
            }

            return RedirectToAction("Profile", "Profile");
        }

        [HttpGet]
        public async Task<bool> CheckNotificationsNuvelty(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null)
            {
                var notifications = new NotificationDTO[] { };
                if (User.IsInRole("Administrator"))
                {
                    notifications = await _notificationService.GetAsync(n => n.Employee.WorkEmail == userEmail || n.EmployeeId == null && n.OrganisationId == user.OrganisationId);
                }
                else
                {
                    notifications = await _notificationService.GetAsync(n => n.Employee.WorkEmail == userEmail);
                }

                return notifications.Length != 0;
            }

            return false;
        }

        #endregion
    }
}