using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.SharedViewModels.Vacation;
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
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationRequestService;
        private readonly IResidualsService _residualsService;
        private readonly ITransactionService _transactionService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveRequestService;
        private readonly ITeamService _teamService;
        private readonly IAssignmentService _assignmentService;
        private readonly INotificationService _notificationService;
        private readonly IMapHelper _mapHelper;
        private readonly IFileManagmentService _fileManagmentService;

        public TeamleadController(UserManager<ApplicationUser> userManager,
                                  SignInManager<ApplicationUser> signInManager,
                                  IEmailSender emailSender,
                                  ILogger logger,
                                  IEmployeeService employeeService,
                                  IVacationService vacationRequestService,
                                  IResidualsService residualsService,
                                  ITransactionService transactionService,
                                  IPostService postService,
                                  ISickLeaveService sickLeaveRequestService,
                                  ITeamService teamService,
                                  IAssignmentService assignmentService,
                                  INotificationService notificationService,
                                  IMapHelper mapHelper,
                                  IFileManagmentService fileManagmentService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
            _employeeService = employeeService;
            _vacationRequestService = vacationRequestService;
            _residualsService = residualsService;
            _transactionService = transactionService;
            _postService = postService;
            _sickLeaveRequestService = sickLeaveRequestService;
            _teamService = teamService;
            _assignmentService = assignmentService;
            _notificationService = notificationService;
            _mapHelper = mapHelper;
            _fileManagmentService = fileManagmentService;
        }

        [HttpGet]
        public async Task<IActionResult> ProccessVacationRequest(string codeV)
        {
            var vacationRequest = await _vacationRequestService.GetByIdWithEmployeeWithTeamAsync(codeV);
            if (vacationRequest != null && vacationRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(vacationRequest);
                model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                model.VacationType = vacationRequest.VacationType;
                var teamlead = await _employeeService.GetByIdAsync(vacationRequest.Employee.Team.TeamleadId);
                model.TeamName = vacationRequest.Employee.Team.Name;
                model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProccessVacationRequest(VacationRequestProccessViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var vacationRequest = await _vacationRequestService.GetByIdAsync(model.VacationId);
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
                        Description = model.ProccessComment ?? model.VacationType,
                        TransactionDate = DateTime.UtcNow,
                        TransactionType = model.VacationType
                    };
                    await _transactionService.CreateAsync(transaction);
                    vacationRequest.TransactionId = transaction.TransactionId;
                }
                else
                {
                    vacationRequest.RequestStatus = RequestStatusEnum.Denied.ToString();
                }
                await _vacationRequestService.UpdateAsync(vacationRequest);
                var notification = new NotificationDTO
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    EmployeeId = vacationRequest.EmployeeId,
                    OrganisationId = user.OrganisationId,
                    NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                    NotificationDate = DateTime.UtcNow,
                    Description = $"Your vacation request was proccessed",
                    IsChecked = false
                };
                await _notificationService.CreateAsync(notification);
            }

            return RedirectToAction("Profile", "Profile");
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
                    sickLeaveRequest.RequestStatus = RequestStatusEnum.Approved.ToString();
                }
                else
                {
                    sickLeaveRequest.RequestStatus = RequestStatusEnum.Denied.ToString();
                }

                await _sickLeaveRequestService.UpdateAsync(sickLeaveRequest);
            }

            var notification = new NotificationDTO
            {
                NotificationId = Guid.NewGuid().ToString(),
                EmployeeId = sickLeaveRequest.EmployeeId,
                OrganisationId = user.OrganisationId,
                NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                NotificationDate = DateTime.UtcNow,
                Description = $"Your sick leave request was proccessed",
                IsChecked = false
            };

            await _notificationService.CreateAsync(notification);

            return RedirectToAction("Profile", "Profile");
        }
    }
}