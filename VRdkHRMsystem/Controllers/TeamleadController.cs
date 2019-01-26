using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Absence;
using VRdkHRMsysBLL.DTOs.Assignment;
using VRdkHRMsysBLL.DTOs.DayOff;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.DTOs.WorkDay;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.SharedModels.Assignment;
using VRdkHRMsystem.Models.SharedModels.Calendar;
using VRdkHRMsystem.Models.SharedModels.Employee;
using VRdkHRMsystem.Models.SharedModels.SickLeave;
using VRdkHRMsystem.Models.SharedModels.Team;
using VRdkHRMsystem.Models.SharedModels.Vacation;
using VRdkHRMsystem.Models.SharedViewModels.Employee;
using VRdkHRMsystem.Models.TeamleadViewModels.Absence;
using VRdkHRMsystem.Models.TeamleadViewModels.Assignment;
using VRdkHRMsystem.Models.TeamleadViewModels.Calendar;

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
        [HttpGet]
        public async Task<IActionResult> Calendar(int year, int month, string teamId)
        {
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                TeamDTO team = new TeamDTO();
                CalendarViewModel model = new CalendarViewModel();
                EmployeeDTO[] employees = new EmployeeDTO[] { };
                var teams = await _teamService.GetAsync(t => t.OrganisationId == viewer.OrganisationId && t.TeamleadId == viewer.EmployeeId || t.TeamId == viewer.TeamId);
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
                }


                if (team != null)
                {
                    if(team.TeamId == viewer.TeamId)
                    {
                        employees = await _employeeService.GetForCalendaAsync(team.TeamId, team.TeamleadId, month, year, viewer.EmployeeId);
                        model = new CalendarViewModel
                        {
                            Year = year,
                            Month = month,
                            TeamId = team?.TeamId,
                            Team = team != null ? _mapHelper.Map<TeamDTO, TeamViewModel>(team) : null,
                            Teams = _listMapper.CreateTeamList(teams, team.TeamId),
                            Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                            Culture = CultureInfo.CreateSpecificCulture("ru-RU")
                        };

                        return View("~/Views/Profile/Calendar.cshtml", model);
                    }

                    employees = await _employeeService.GetForCalendaAsync(team.TeamId, team.TeamleadId, month, year);

                    model = new CalendarViewModel
                    {
                        Year = year,
                        Month = month,
                        TeamId = team?.TeamId,
                        Team = team != null ? _mapHelper.Map<TeamDTO, TeamViewModel>(team) : null,
                        Teams = _listMapper.CreateTeamList(teams, team.TeamId),
                        Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                        Culture = CultureInfo.CreateSpecificCulture("ru-RU")
                    };

                    return View(model);
                }
                else
                {
                    return View(new CalendarViewModel());
                }
            }

            return RedirectToAction("Profile", "Profile");
        }

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
        public async Task<IActionResult> ViewVacationRequest(string id)
        {
            var vacationRequest = await _vacationService.GetByIdWithEmployeeWithTeamAsync(id);
            if (vacationRequest != null && vacationRequest.ProccessedbyId != null)
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

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ProccessVacationRequest(string id)
        {
            var vacationRequest = await _vacationService.GetByIdWithEmployeeWithTeamAsync(id);
            if (vacationRequest != null && vacationRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(vacationRequest);
                model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                model.VacationType = vacationRequest.VacationType;
                var teamlead = await _employeeService.GetByIdAsync(vacationRequest.Employee.Team.TeamleadId);
                model.TeamName = vacationRequest.Employee.Team.Name;
                model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";

                return PartialView("VacationProccessModal", model);
            }

            return View();
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
                        Description = $"Ваш запрос на отпуск был подтверждён руководителем.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                        },
                   new NotificationDTO
                        {
                        NotificationId = Guid.NewGuid().ToString(),
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.Vacation.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Запрос на отпуск работника был подтверждён его руководителем.",
                        NotificationRange = NotificationRangeEnum.Organisation.ToString(),
                        IsChecked = false
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
                        Description = $"Ваш запрос на отпуск был отклонён руководителем.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                    };

                    await _notificationService.CreateAsync(notification);
                }

                await _vacationService.UpdateAsync(vacationRequest, true);
            }

            return RedirectToAction("Vacations", "Teamlead");
        }

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
        public async Task<IActionResult> Assignments(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var assignments = new AssignmentListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                assignments = await _assignmentService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, a => a.OrganisationId == employee.OrganisationId
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
                        Description = $"Ваш запрос на больничный был подтвердён.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
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
                        Description = $"Ваш запрос на больничный был отклонён.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                    };

                    await _notificationService.CreateAsync(notification);
                }

                await _sickLeaveService.UpdateAsync(sickLeaveRequest, true);
            }

            return RedirectToAction("Sickleaves", "Teamlead");
        }

        [HttpGet]
        public async Task<IActionResult> ViewVacationRequests(int pageNumber = 0, string searchKey = null)
        {
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            var requests = await _vacationService.GetPageAsync(pageNumber,
                                                                         (int)PageSizeEnum.PageSize15,
                                                                          RequestStatusEnum.Pending.ToString(),
                                                                          searchKey,
                                                                          req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                          && req.Employee.OrganisationId == employee.OrganisationId);
            var model = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(requests);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EditDayOff(string id, string date, string teamId)
        {
            var dayOff = await _dayOffService.GetByIdAsync(id);
            if (dayOff != null)
            {
                var model = new EditCalendarDayViewModel
                {
                    CalendarDayId = dayOff.DayOffId,
                    Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                    FirstName = dayOff.Employee.FirstName,
                    LastName = dayOff.Employee.LastName,
                    TeamId = teamId
                };

                return PartialView("EditDayOffModal", model);
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId });
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
                        Description = $"Ваш выходной день на {model.Date.ToString("dd.MM.yyyy")} был заменён рабочим.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                    };
                    if(dayOff.DayOffImportance != null)
                    {
                        dayOff.DayOffState = DayOffStateEnum.Requested.ToString();
                        dayOff.ProcessDate = DateTime.UtcNow;
                        await _dayOffService.UpdateAsync(dayOff);
                    }
                    else
                    {
                        await _dayOffService.DeleteAsync(dayOff.DayOffId);
                    }
                    
                    await _workDayService.CreateAsync(workDay);
                    await _notificationService.CreateAsync(notification, true);
                }
                else if(dayOff.DayOffState == DayOffStateEnum.Requested.ToString())
                {
                    dayOff.DayOffState = DayOffStateEnum.Approved.ToString();
                    dayOff.ProcessDate = DateTime.UtcNow;
                    await _dayOffService.UpdateAsync(dayOff, true);                   
                }
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId = model.TeamId, year = model.Date.Year, month = model.Date.Month });
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId = model.TeamId, year = model.Date.Year, month = model.Date.Month });
        }

        [HttpGet]
        public async Task<IActionResult> EditWorkDay(string id, string date, string teamId)
        {
            var workDay = await _workDayService.GetByIdAsync(id);
            if (workDay != null)
            {
                var parsedDate = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                var model = new EditCalendarDayViewModel
                {
                    CalendarDayId = workDay.WorkDayId,
                    Date = parsedDate,
                    TimeFrom = new DateTime(parsedDate.Year,parsedDate.Month,parsedDate.Day, workDay.TimeFrom.Hours,workDay.TimeFrom.Minutes, 0),
                    TimeTo = new DateTime(parsedDate.Year, parsedDate.Month, parsedDate.Day, workDay.TimeTo.Hours, workDay.TimeFrom.Minutes, 0),
                    FirstName = workDay.Employee.FirstName,
                    LastName = workDay.Employee.LastName,
                    TeamId = teamId
                };

                return PartialView("EditWorkDayModal", model);
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId });
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId });
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
                    if(dayOffEx != null && dayOffEx.DayOffImportance != null)
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
                            Description = $"Ваш рабочий день на {model.Date.ToString("dd.MM.yyyy")} был заменён выходным.",
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            IsChecked = false
                        };
                        await _dayOffService.UpdateAsync(dayOffEx);
                        await _notificationService.CreateAsync(notification);
                        await _workDayService.DeleteAsync(workDay.WorkDayId, true);
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
                            Description = $"Ваш рабочий день на {model.Date.ToString("dd.MM.yyyy")} был заменён выходным.",
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            IsChecked = false
                        };

                        await _workDayService.DeleteAsync(workDay.WorkDayId);
                        await _dayOffService.CreateAsync(dayOff);                  
                        await _notificationService.CreateAsync(notification, true);
                    }                                 
                }
                else
                {
                    if (workDay.TimeFrom != model.TimeFrom.TimeOfDay || workDay.TimeTo != model.TimeTo.TimeOfDay)
                    {
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
                            Description = $"Были внесены изменения в Ваш рабочий день на {model.Date.ToString("dd.MM.yyyy")}.",
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            IsChecked = false
                        };
                        await _workDayService.UpdateAsync(workDay);
                        await _notificationService.CreateAsync(notification, true);
                    }                   
                }
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId = model.TeamId, year = model.Date.Year, month = model.Date.Month });
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId = model.TeamId, year = model.Date.Year, month = model.Date.Month });
        }


        [HttpGet]
        public IActionResult ProccessCalendarDay(string id, string date, string name, string surname, string teamId)
        {
            var model = new CalendarDayViewModel
            {
                EmployeeId = id,
                Date = DateTime.ParseExact(date, "dd.MM.yyyy", CultureInfo.InvariantCulture),
                FirstName = name,
                LastName = surname,
                TeamId = teamId
            };

            return PartialView("ProccessCalendarDayModal", model);
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
                        Description = $"Подтверждение выходного дня.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                    };
                    await _dayOffService.CreateAsync(dayOff);
                    await _notificationService.CreateAsync(notification, true);
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
                        Description = $"Подтверждение рабочего дня.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                    };
                    await _workDayService.CreateAsync(workDay);
                    await _notificationService.CreateAsync(notification, true);
                }
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId = model.TeamId, year = model.Date.Year, month = model.Date.Month });
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId = model.TeamId, year = model.Date.Year, month = model.Date.Month });
        }

        [HttpGet]
        public async Task<IActionResult> SetAbsence(string id, string teamId, string firstName, string lastName)
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
                    LastName = lastName
                };

                return PartialView("AbsenceApproveModal", model);
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId });
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId });
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
                await _absenceService.CreateAsync(absence, true);
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId = model.TeamId });
            }

            return RedirectToAction("Calendar", "Teamlead", new { teamId = model.TeamId });
        }
    }
}