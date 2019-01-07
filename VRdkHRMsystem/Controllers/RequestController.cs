using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models.Profile;
using VRdkHRMsystem.Models.RequestViewModels.SickLeave;
using VRdkHRMsystem.Models.RequestViewModels.Vacation;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private const string emptyValue = "None";
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IPostService _postService;
        private readonly IFileManagmentService _fileManagmentService;
        private readonly INotificationService _notificationService;

        private readonly IMapHelper _mapHelper;
        private readonly IViewListMapper _listMapper;

        public RequestController(IEmployeeService employeeService,
                                 IVacationService vacationRequestService,
                                 ISickLeaveService sickLeaveService,
                                 IFileManagmentService fileManagmentService,
                                 IPostService postService,
                                 INotificationService notificationService,
                                 IMapHelper mapHelper,
                                 IViewListMapper listMapper)
        {
            _employeeService = employeeService;
            _vacationService = vacationRequestService;
            _sickLeaveService = sickLeaveService;
            _postService = postService;
            _notificationService = notificationService;
            _fileManagmentService = fileManagmentService;
            _mapHelper = mapHelper;
            _listMapper = listMapper;
        }

        [HttpGet]
        public async Task<IActionResult> RequestVacation(string codeE = null)
        {
            EmployeeDTO employee;
            CompositeRequestVacationViewModel model = new CompositeRequestVacationViewModel
            {
                ProfileModel = new ProfileViewModel(),
                RequestVacationModel = new RequestVacationViewModel()
            };
            if (codeE != null)
            {
                employee = await _employeeService.GetByIdWithTeamWithResidualsAsync(codeE);
            }
            else
            {
                employee = await _employeeService.GetByEmailWithTeamWithResidualsAsync(User.Identity.Name);
            }
            if (employee != null)
            {              
                model.ProfileModel.EmployeeId = employee.EmployeeId;
                model.RequestVacationModel.EmployeeId = employee.EmployeeId;
                model.RequestVacationModel.PaidVacationResiduals = employee.EmployeeBalanceResiduals.FirstOrDefault(res => res.Name == "Paid_vacation").ResidualBalance;
                model.RequestVacationModel.UnpaidVaccationResiduals = employee.EmployeeBalanceResiduals.FirstOrDefault(res => res.Name == "Unpaid_vacation").ResidualBalance;
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
            }

            model.RequestVacationModel.VacationTypes = _listMapper.CreateVacationTypesList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RequestVacation(RequestVacationViewModel model)
        {
            var employee = await _employeeService.GetByIdWithTeamAsync(model.EmployeeId);
            if (employee != null)
            {
                var vacationRequest = _mapHelper.Map<RequestVacationViewModel, VacationRequestDTO>(model);
                var notification = new NotificationDTO
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    OrganisationId = employee.OrganisationId,
                    NotificationDate = DateTime.UtcNow,
                    EmployeeId = employee.Team != null ? employee.Team.TeamleadId : null,
                    NotificationType = NotificationTypeEnum.Vacation.ToString(),
                    IsChecked = false,
                    NotificationRange = NotificationRangeEnum.Organisation.ToString(),
                    Description = $"{employee.FirstName} {employee.LastName} запросил отпуск."
                };
                await _notificationService.CreateAsync(notification);
                if (employee.TeamId != null)
                {
                    vacationRequest.RequestStatus = RequestStatusEnum.Pending.ToString();            
                }
                else
                {
                    vacationRequest.RequestStatus = RequestStatusEnum.Proccessing.ToString();
                }      
                vacationRequest.VacationId = Guid.NewGuid().ToString();
                vacationRequest.CreateDate = DateTime.UtcNow.Date;
                await _vacationService.CreateAsync(vacationRequest);                
            }

            return RedirectToAction("Profile", "Profile", new { id = model.EmployeeId });
        }

        [HttpGet]
        public async Task<IActionResult> RequestSickLeave(string codeE = null)
        {
            EmployeeDTO employee;
            CompositeRequestSickLeaveViewModel model = new CompositeRequestSickLeaveViewModel
            {
                ProfileModel = new ProfileViewModel(),
                RequestSickLeaveModel = new RequestSickLeaveViewModel()
            };
            if (codeE != null)
            {
                employee = await _employeeService.GetByIdWithTeamWithResidualsAsync(codeE);
            }
            else
            {
                employee = await _employeeService.GetByEmailWithTeamWithResidualsAsync(User.Identity.Name);
            }
            if (employee != null)
            {
                model.ProfileModel.EmployeeId = employee.EmployeeId;
                model.RequestSickLeaveModel.EmployeeId = employee.EmployeeId;
                model.RequestSickLeaveModel.SickLeaveBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(res => res.Name == "Sick_leave").ResidualBalance;
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
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RequestSickLeave(RequestSickLeaveViewModel model)
        {
            var employee = await _employeeService.GetByIdWithTeamAsync(model.EmployeeId);
            if (employee != null)
            {
                var sickLeaveRequest = _mapHelper.Map<RequestSickLeaveViewModel, SickLeaveRequestDTO>(model);
                sickLeaveRequest.RequestStatus = RequestStatusEnum.Pending.ToString();
                sickLeaveRequest.SickLeaveId = Guid.NewGuid().ToString();
                sickLeaveRequest.CreateDate = DateTime.UtcNow.Date;
                await _sickLeaveService.CreateAsync(sickLeaveRequest);
                var notification = new NotificationDTO
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    OrganisationId = employee.OrganisationId,
                    EmployeeId = employee.Team != null ? employee.Team.TeamleadId : null,
                    NotificationDate = DateTime.UtcNow,
                    NotificationType = NotificationTypeEnum.Vacation.ToString(),
                    IsChecked = false,
                    NotificationRange = NotificationRangeEnum.Organisation.ToString(),
                    Description = $"{employee.FirstName} {employee.LastName} запросил больничный"
                };
                await _notificationService.CreateAsync(notification);
                if (model.File != null)
                {
                    await _fileManagmentService.UploadSickLeaveFileAsync(model.File, sickLeaveRequest.SickLeaveId);
                }               
            }

            return RedirectToAction("Profile", "Profile", new { id = model.EmployeeId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSickLeave(string codeS)
        {
            var sickLeaveRequest = await _sickLeaveService.GetByIdWithEmployeeWithTeamAsync(codeS);
            if (sickLeaveRequest != null)
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, UpdateSickLeaveViewModel>(sickLeaveRequest);
                model.ExistingFiles = await _fileManagmentService.GetSickLeaveFilesAsync(model.SickLeaveId);

                return View(model);
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSickLeave(UpdateSickLeaveViewModel model)
        {
            var request = await _sickLeaveService.GetByIdAsync(model.SickLeaveId);
            if (request != null)
            {
                request.Comment = model.Comment;
                await _sickLeaveService.UpdateAsync(request);
                if (model.UploadedFile != null)
                {
                    await _fileManagmentService.UploadSickLeaveFileAsync(model.UploadedFile, request.SickLeaveId);
                }
            }

            return RedirectToAction("Profile", "Profile");
        }

        public async Task<OkResult> CloseSickLeave(string codeS)
        {
            var request = await _sickLeaveService.GetByIdAsync(codeS);
            request.CloseDate = DateTime.UtcNow.Date;
            request.Duration = request.CloseDate?.DayOfYear != request.CreateDate.DayOfYear ? request.CloseDate?.DayOfYear - request.CreateDate.DayOfYear : 1;
            request.RequestStatus = RequestStatusEnum.Closed.ToString();
            await _sickLeaveService.UpdateAsync(request);
            return Ok();
        }
    }
}