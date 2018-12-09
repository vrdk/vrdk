using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

        private readonly IMapHelper _mapHelper;
        private readonly IViewListMapper _listMapper;

        public RequestController(IEmployeeService employeeService,
                                 IVacationService vacationRequestService,
                                 ISickLeaveService sickLeaveService,
                                 IFileManagmentService fileManagmentService,
                                 IMapHelper mapHelper,
                                 IViewListMapper listMapper)
        {
            _employeeService = employeeService;
            _vacationService = vacationRequestService;
            _sickLeaveService = sickLeaveService;
            _fileManagmentService = fileManagmentService;
            _mapHelper = mapHelper;
            _listMapper = listMapper;
        }

        [HttpGet]
        public async Task<IActionResult> RequestVacation(string id = null)
        {
            RequestVacationViewModel model = new RequestVacationViewModel();
            if (id != null)
            {
                model.EmployeeId = id;
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
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null)
            {
                var vacationRequest = _mapHelper.Map<RequestVacationViewModel, VacationRequestDTO>(model);
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
        public async Task<IActionResult> RequestSickLeave(string id = null)
        {
            RequestSickLeaveViewModel model = new RequestSickLeaveViewModel();
            if (id != null)
            {
                model.EmployeeId = id;
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
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null && model.Files.Count() <= 3)
            {
                var sickLeaveRequest = _mapHelper.Map<RequestSickLeaveViewModel, SickLeaveRequestDTO>(model);
                sickLeaveRequest.RequestStatus = RequestStatusEnum.Pending.ToString();
                sickLeaveRequest.SickLeaveId = Guid.NewGuid().ToString();
                sickLeaveRequest.CreateDate = DateTime.UtcNow.Date;
                await _sickLeaveService.CreateAsync(sickLeaveRequest);
                await _fileManagmentService.UploadSickLeaveFilesAsync(model.Files, model.EmployeeId, sickLeaveRequest.SickLeaveId);
            }

            return RedirectToAction("Profile", "Profile", new { id = model.EmployeeId });
        }
    }
}