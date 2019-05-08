using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models.Profile;
using VRdkHRMsystem.Models.RequestViewModels;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private const string EmptyValue = "None";
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IPostService _postService;
        private readonly ITransactionService _transactionService;
        private readonly IFileManagmentService _fileManagmentService;
        private readonly INotificationService _notificationService;
        private readonly IDayOffService _dayOffService;
        private readonly IMapHelper _mapHelper;
        private readonly IViewListMapper _listMapper;

        public RequestController(IEmployeeService employeeService,
                                 IVacationService vacationRequestService,
                                 ISickLeaveService sickLeaveService,
                                 ITransactionService transactionService,
                                 IFileManagmentService fileManagmentService,
                                 IPostService postService,
                                 IDayOffService dayOffService,
                                 INotificationService notificationService,
                                 IMapHelper mapHelper,
                                 IViewListMapper listMapper)
        {
            _employeeService = employeeService;
            _vacationService = vacationRequestService;
            _sickLeaveService = sickLeaveService;
            _postService = postService;
            _dayOffService = dayOffService;
            _transactionService = transactionService;
            _notificationService = notificationService;
            _fileManagmentService = fileManagmentService;
            _mapHelper = mapHelper;
            _listMapper = listMapper;
        }

        [HttpPost]
        public async Task<IActionResult> EditDayOffRequest(EditDayOffRequestViewModel model)
        {
            var dayOff = await _dayOffService.GetByIdAsync(model.DayOffId);
            if (dayOff != null)
            {
                if (model.DayOffImportance == "delete")
                {
                   await _dayOffService.DeleteAsync(dayOff.DayOffId, true);

                    return Json(false);
                }
                else
                {
                    dayOff.DayOffImportance = model.DayOffImportance;
                    dayOff.Comment = model.Comment;
                    await _dayOffService.UpdateAsync(dayOff, true);
                }
                dayOff.Employee = null;
                return Json(dayOff);
            }

            if (User.IsInRole("Administrator"))
            {
                return RedirectToAction("Calendar", "Admin", new { teamId = model.TeamId });
            }
            else if (User.IsInRole("Teamlead"))
            {
                return RedirectToAction("Calendar", "Teamlead", new { teamId = model.TeamId });
            }
            else
            {
                return RedirectToAction("Calendar", "Profile", new { teamId = model.TeamId });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ProccessCalendarDay(string id, string date, string teamId)
        {
            var dayOffDate = Convert.ToDateTime(date);
            var dayOff = await _dayOffService.GetByDateAsync(dayOffDate, id);
            if(dayOff == null)
            {
                var model = new RequestDayOffViewModel
                {
                    EmployeeId = id,
                    DayOffDate = Convert.ToDateTime(date),
                    TeamId = teamId
                };

                return PartialView("RequestDayOffModal", model);
            }
            else
            {
                var model = new EditDayOffRequestViewModel
                {
                    EmployeeId = dayOff.EmployeeId,
                    DayOffDate = dayOff.DayOffDate,
                    Comment = dayOff.Comment,
                    DayOffId = dayOff.DayOffId,
                    DayOffImportance = dayOff.DayOffImportance,
                    TeamId = teamId
                };

                return PartialView("EditDayOffRequestModal", model);
            }                   
        }

        [HttpPost]
        public async Task<IActionResult> RequestDayOff(RequestDayOffViewModel model)
        {
            var employee = await _employeeService.GetByIdWithTeamAsync(model.EmployeeId);
            if (employee != null)
            {
                var dayOff = new DayOffDTO
                {
                    DayOffId = Guid.NewGuid().ToString(),
                    EmployeeId = employee.EmployeeId,
                    DayOffDate = model.DayOffDate,
                    DayOffImportance = model.DayOffImportance,
                    DayOffState = DayOffStateEnum.Requested.ToString(),
                    Comment = model.Comment,
                    ProcessDate = DateTime.UtcNow
                };
                var notification = new NotificationDTO
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    EmployeeId = employee.Team?.TeamleadId,
                    OrganisationId = employee.OrganisationId,
                    NotificationType = NotificationTypeEnum.DayOff.ToString(),
                    NotificationDate = DateTime.UtcNow,
                    Description = $"{employee.FirstName} {employee.LastName} выбрал желаемый выходной.",
                    RelatedDate = model.DayOffDate,
                    RelatedTeamId = employee.TeamId,
                    NotificationRange = NotificationRangeEnum.User.ToString()
                };
                await _dayOffService.CreateAsync(dayOff);
                await _notificationService.CreateAsync(notification, true);

                return Json(dayOff);
            }

            return Json(false);
        }

        [HttpGet]
        public async Task<IActionResult> RequestVacation(string id = null)
        {
            EmployeeDTO employee;
            CompositeRequestVacationViewModel model = new CompositeRequestVacationViewModel
            {
                ProfileModel = new ProfileViewModel(),
                RequestVacationModel = new RequestVacationViewModel()
            };
            if (id != null)
            {
                employee = await _employeeService.GetByIdWithTeamWithResidualsAsync(id);
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
                    model.RequestVacationModel.TeamId = employee.TeamId;
                }
                else
                {
                    model.ProfileModel.Team = EmptyValue;
                    model.ProfileModel.Teamlead = EmptyValue;
                    model.RequestVacationModel.OrganisationId = employee.OrganisationId;
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
                    EmployeeId = employee.Team?.TeamleadId,
                    NotificationType = NotificationTypeEnum.Vacation.ToString(),
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
                vacationRequest.CreateDate = DateTime.UtcNow;
                await _vacationService.CreateAsync(vacationRequest, true);

                return Ok(vacationRequest);
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> RequestSickLeave(string id = null)
        {
            EmployeeDTO employee;
            CompositeRequestSickLeaveViewModel model = new CompositeRequestSickLeaveViewModel
            {
                ProfileModel = new ProfileViewModel(),
                RequestSickLeaveModel = new RequestSickLeaveViewModel()
            };
            if (id != null)
            {
                employee = await _employeeService.GetByIdWithTeamWithResidualsAsync(id);
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
                    model.ProfileModel.Team = EmptyValue;
                    model.ProfileModel.Teamlead = EmptyValue;
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
                    EmployeeId = employee.Team?.TeamleadId,
                    NotificationDate = DateTime.UtcNow,
                    NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                    NotificationRange = NotificationRangeEnum.Organisation.ToString(),
                    Description = $"{employee.FirstName} {employee.LastName} запросил больничный"
                };

                await _notificationService.CreateAsync(notification, true);

                if (model.File != null)
                {
                    await _fileManagmentService.UploadSickLeaveFileAsync(model.File, sickLeaveRequest.SickLeaveId);
                }
            }

            return RedirectToAction("Profile", "Profile", new { id = model.EmployeeId });
        }

        [HttpGet]
        public async Task<IActionResult> UpdateSickLeave(string id)
        {
            var sickLeaveRequest = await _sickLeaveService.GetByIdWithEmployeeWithTeamAsync(id);
            if (sickLeaveRequest != null)
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, UpdateSickLeaveViewModel>(sickLeaveRequest);
                model.ExistingFiles = await _fileManagmentService.GetSickLeaveFilesAsync(model.SickLeaveId);

                return PartialView("UpdateSickleaveModal", model);
            }

            return PartialView("UpdateSickleaveModal");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSickLeave(UpdateSickLeaveViewModel model)
        {
            var request = await _sickLeaveService.GetByIdAsync(model.SickLeaveId);
            if (request != null)
            {
                request.Comment = model.Comment;
                await _sickLeaveService.UpdateAsync(request, true);
                if (model.File != null)
                {
                    await _fileManagmentService.UploadSickLeaveFileAsync(model.File, request.SickLeaveId);
                }
            }

            return RedirectToAction("Profile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> CloseSickleave(string id)
        {
            var sickLiveRequest = await _sickLeaveService.GetByIdWithEmployeeWithTeamAsync(id);
            if (sickLiveRequest != null)
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, CloseSickLeaveViewModel>(sickLiveRequest);
                model.Files = await _fileManagmentService.GetSickLeaveFilesAsync(sickLiveRequest.SickLeaveId);

                return PartialView("CloseSickleaveModal", model);
            }

            return PartialView("CloseSickleaveModal");
        }

        [HttpPost]
        public async Task<ActionResult> CloseSickLeave(CloseSickLeaveViewModel model)
        {
            var request = await _sickLeaveService.GetByIdWithEmployeeWithResidualsAsync(model.SickLeaveId);
            if (request != null && request.RequestStatus == RequestStatusEnum.Approved.ToString())
            {
                request.CloseDate = DateTime.UtcNow.Date;
                var c = (int)(DateTime.UtcNow.Date - request.CreateDate).TotalDays;
                request.Duration = (int)(DateTime.UtcNow.Date - request.CreateDate).TotalDays != 0 ? (int)(DateTime.UtcNow.Date - request.CreateDate).TotalDays : 1;
                request.RequestStatus = RequestStatusEnum.Closed.ToString();
                request.Employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance += request.Duration.Value;
                var transaction = new TransactionDTO()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    EmployeeId = request.EmployeeId,
                    Change = request.Duration.Value,
                    Description = "Sickleave close",
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = "Sickleave close"
                };
                await _transactionService.CreateAsync(transaction);
                await _sickLeaveService.UpdateAsync(request, true);
            }

            return RedirectToAction("Profile", "Profile");
        }
    }
}