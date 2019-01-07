using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Absence;
using VRdkHRMsysBLL.DTOs.DayOff;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.DTOs.WorkDay;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Models;
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
        private readonly ISickLeaveService _sickLeaveRequestService;
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
            _sickLeaveRequestService = sickLeaveRequestService;
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
                    var residual = await _residualsService.GetByEmployeeIdAsync(vacationRequest.EmployeeId, model.VacationType);
                    vacationRequest.RequestStatus = RequestStatusEnum.Approved.ToString();
                    residual.ResidualBalance -= vacationRequest.Duration;
                    await _residualsService.UpdateAsync(residual);
                    var transaction = new TransactionDTO()
                    {
                        TransactionId = Guid.NewGuid().ToString(),
                        EmployeeId = vacationRequest.EmployeeId,
                        Change = model.Duration,
                        Description =  model.VacationType,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = model.VacationType
                    };

                    await _transactionService.CreateAsync(transaction);
                    vacationRequest.TransactionId = transaction.TransactionId;
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
        public async Task<IActionResult> ProccessSickLeaveRequest(string codeS)
        {
            var sickLeaveRequest = await _sickLeaveRequestService.GetByIdWithEmployeeWithTeamAsync(codeS);
            if (sickLeaveRequest != null && sickLeaveRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, SickLeaveRequestProccessViewModel>(sickLeaveRequest);
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

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProccessSickLeaveRequest(SickLeaveRequestProccessViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var sickLeaveRequest = await _sickLeaveRequestService.GetByIdAsync(model.SickLeaveId);
            if (sickLeaveRequest != null && sickLeaveRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                sickLeaveRequest.ProccessedbyId = user.Id;
                if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                {
                    sickLeaveRequest.RequestStatus = RequestStatusEnum.Proccessing.ToString();
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

                await _sickLeaveRequestService.UpdateAsync(sickLeaveRequest);              
            }

            return RedirectToAction("Profile", "Profile");
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