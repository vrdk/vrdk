using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Absence;
using VRdkHRMsysBLL.DTOs.DayOff;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.DTOs.WorkDay;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.SharedModels.Employee;
using VRdkHRMsystem.Models.SharedModels.SickLeave;
using VRdkHRMsystem.Models.SharedModels.Vacation;
using VRdkHRMsystem.Models.TeamleadViewModels.Calendar;
using VRdkHRMsystem.Models.TeamleadViewModels.SickLeave;

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
        private readonly IMapHelper _mapHelper;
        private readonly IFileManagmentService _fileManagmentService;

        public TeamleadController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IEmailSender emailSender,
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
                        Description =  model.VacationType,
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
                        NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Ваш запрос на отпуск был подтверждён руководителем.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                        },
                   new NotificationDTO
                        {
                        NotificationId = Guid.NewGuid().ToString(),
                        OrganisationId = user.OrganisationId,
                        NotificationType = NotificationTypeEnum.SickLeave.ToString(),
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
                        NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = $"Ваш запрос на отпуск был отклонён руководителем.",
                        NotificationRange = NotificationRangeEnum.User.ToString(),
                        IsChecked = false
                    };

                    await _notificationService.CreateAsync(notification);
                }

                await _vacationService.UpdateAsync(vacationRequest);
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
                    if (teamlead.WorkEmail == User.Identity.Name)
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

                await _sickLeaveService.UpdateAsync(sickLeaveRequest);              
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
        public IActionResult ProccessCalendarDay(string codeE, string date)
        {
            var model = new CalendarDayViewModel
            {
                EmployeeId = codeE,
                Date = Convert.ToDateTime(date)
            };
            return View(model);
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
                    await _notificationService.CreateAsync(notification);
                }
                else
                {
                    var workDay = new WorkDayDTO
                    {
                        WorkDayId = Guid.NewGuid().ToString(),
                        EmployeeId = employee.EmployeeId,
                        WorkDayDate = model.Date,
                        TimeFrom = model.From.TimeOfDay,
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
                    await _notificationService.CreateAsync(notification);
                }
            }

            return View();
        }

        public async Task<OkResult> CloseSickLeave(string codeE)
        {
            //if(Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            var abs = _absenceService.GetByEmployeeIdAsync(codeE);
            if (abs == null)
            {
                var absence = new AbsenceDTO
                {
                    AbsenceId = Guid.NewGuid().ToString(),
                    EmployeeId = codeE,
                    AbsenceDate = DateTime.UtcNow
                };
                await _absenceService.CreateAsync(absence);
            }
            return Ok();
        }
    }
}