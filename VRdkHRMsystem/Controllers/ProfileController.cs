﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models.Profile;
using VRdkHRMsystem.Models.Profile.Assignment;
using VRdkHRMsystem.Models.Profile.Notification;
using VRdkHRMsystem.Models.Profile.SickLeave;
using VRdkHRMsystem.Models.Profile.Vacation;
using VRdkHRMsystem.Models.SharedModels.Calendar;
using VRdkHRMsystem.Models.SharedModels.Employee;
using VRdkHRMsystem.Models.SharedModels.Team;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private const string emptyValue = "Нет";
        private readonly IEmployeeService _employeeService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IVacationService _vacationService;
        private readonly IAssignmentService _assignmentService;
        private readonly INotificationService _notificationService;
        private readonly IViewListMapper _listMapper;
        private readonly ITeamService _teamService;
        private readonly IMapHelper _mapHelper;

        public ProfileController(IEmployeeService employeeService,
                                 IPostService postService,
                                 IVacationService vacationService,
                                 ITeamService teamService,
                                 IViewListMapper listMapper,
                                 IAssignmentService assignmentService,
                                 INotificationService notificationService,
                                 ISickLeaveService sickLeaveService,
        IMapHelper mapHelper)
        {
            _employeeService = employeeService;
            _sickLeaveService = sickLeaveService;
            _notificationService = notificationService;
            _assignmentService = assignmentService;
            _teamService = teamService;
            _listMapper = listMapper;
            _vacationService = vacationService;
            _postService = postService;
            _mapHelper = mapHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Calendar(int year, int month, string teamId)
        {

            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                TeamDTO team = new TeamDTO();
                CalendarViewModel model = new CalendarViewModel();
                EmployeeDTO[] employees = new EmployeeDTO[] { };
                var teams = await _teamService.GetAsync(t => t.TeamId == viewer.TeamId);

                if (year == 0 || month == 0)
                {
                    year = DateTime.UtcNow.Year;
                    month = DateTime.UtcNow.Month;
                }

                team = teams.FirstOrDefault();

                if (team != null)
                {
                    employees = await _employeeService.GetForCalendaAsync(team.TeamId, team.TeamleadId, month, year, viewer.EmployeeId);

                    model = new CalendarViewModel
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

                }

                return View(model);
            }

            return RedirectToAction("Profile", "Profile");
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string id = null)
        {
            CompositeProfileViewModel model = new CompositeProfileViewModel()
            {
                ProfileModel = new ProfileViewModel(),
                ResidualsModel = new ProfileResidualsViewModel()
            };
            EmployeeDTO employee;
            if(id != null)
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
        public async Task<IActionResult> VacationsPage(int pageNumber = 0)
        {
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if(user != null)
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

        [HttpGet]
        public async Task<IActionResult> Notifications(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            NotificationDTO[] notifications = new NotificationDTO[] { };
            int count = 0;
            var user = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if(user != null)
            {
                if (User.IsInRole("Administrator"))
                {
                    notifications = await _notificationService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15,
                                                                            note => note.EmployeeId == user.EmployeeId 
                                                                                 || note.EmployeeId == null 
                                                                                 && note.OrganisationId == user.OrganisationId, searchKey);
                    count = await _notificationService.GetNotificationsNumber(note => note.EmployeeId == user.EmployeeId || note.EmployeeId == null && note.OrganisationId == user.OrganisationId, searchKey);
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
        public async Task<IActionResult> RedirectWithNotification(string notificationId, string notificationRange, string notificationType, bool isChecked, string role = null)
        {
            var notification = await _notificationService.GetByIdAsync(notificationId);
            if(notification != null)
            {
                
                if (!isChecked)
                {
                   await _notificationService.CheckNotificationAsync(notification.NotificationId);
                }

                if(notificationRange == NotificationRangeEnum.User.ToString())
                {
                    if(notificationType == NotificationTypeEnum.DayOff.ToString() || notificationType == NotificationTypeEnum.WorkDay.ToString())
                    {
                        return RedirectToAction("Profile", "Calendar");
                    }
                    else
                    {
                       return RedirectToAction("Profile", "Profile");
                    }                  
                }
                else
                {
                    if(notificationType == NotificationTypeEnum.Vacation.ToString())
                    {
                        return RedirectToAction("Vacations", role);
                    }
                    else if(notificationType == NotificationTypeEnum.Assignment.ToString())
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
    }
}