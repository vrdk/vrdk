using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models.RequestViewModels.SickLeave;
using VRdkHRMsystem.Models.RequestViewModels.Vacation;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IFileManagmentService _fileManagmentService;
        private readonly INotificationService _notificationService;

        private readonly IMapHelper _mapHelper;
        private readonly IViewListMapper _listMapper;

        public RequestController(IEmployeeService employeeService,
                                 IVacationService vacationRequestService,
                                 ISickLeaveService sickLeaveService,
                                 IFileManagmentService fileManagmentService,
                                 INotificationService notificationService,
                                 IMapHelper mapHelper,
                                 IViewListMapper listMapper)
        {
            _employeeService = employeeService;
            _vacationService = vacationRequestService;
            _sickLeaveService = sickLeaveService;
            _notificationService = notificationService;
            _fileManagmentService = fileManagmentService;
            _mapHelper = mapHelper;
            _listMapper = listMapper;
        }

        [HttpGet]
        public async Task<IActionResult> RequestVacation(string codeE = null)
        {
            RequestVacationViewModel model = new RequestVacationViewModel();
            if (codeE != null)
            {
                model.EmployeeId = codeE;
            }
            else
            {
                var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
                model.EmployeeId = employee.EmployeeId;
            }

            model.VacationTypes = _listMapper.CreateVacationTypesList();

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
                    EmployeeId = employee.Team?.TeamleadId,
                    NotificationType = NotificationTypeEnum.Vacation.ToString(),
                    IsChecked = false,
                    Description = $"{employee.FirstName} {employee.LastName} from {employee.Team.Name} requested {NotificationTypeEnum.Vacation.ToString()}"
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
            RequestSickLeaveViewModel model = new RequestSickLeaveViewModel();
            if (codeE != null)
            {
                model.EmployeeId = codeE;
            }
            else
            {
                var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
                model.EmployeeId = employee.EmployeeId;
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
                    EmployeeId = employee.Team?.TeamleadId,
                    NotificationDate = DateTime.UtcNow,
                    NotificationType = NotificationTypeEnum.Vacation.ToString(),
                    IsChecked = false,
                    Description = $"{employee.FirstName} {employee.LastName} from {employee.Team.Name} requested {NotificationTypeEnum.SickLeave.ToString()}"
                };
                await _notificationService.CreateAsync(notification);
                if (model.Files != null)
                {
                    await _fileManagmentService.UploadSickLeaveFilesAsync(model.Files, sickLeaveRequest.SickLeaveId);
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
                if (model.UploadedFiles.Count() != 0)
                {
                    await _fileManagmentService.UploadSickLeaveFilesAsync(model.UploadedFiles, request.SickLeaveId);
                }
            }

            return RedirectToAction("Profile", "Profile");
        }

        public async Task<OkResult> CloseSickLeave(string codeS)
        {
            var request = await _sickLeaveService.GetByIdAsync(codeS);
            request.CloseDate = DateTime.UtcNow.Date;
            request.Duration = request.CloseDate?.DayOfYear - request.CreateDate.DayOfYear;
            request.RequestStatus = RequestStatusEnum.Closed.ToString();
            await _sickLeaveService.UpdateAsync(request);
            return Ok();
        }
    }
}