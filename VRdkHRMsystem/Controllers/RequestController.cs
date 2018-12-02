using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models.RequestViewModels;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class RequestController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IVacationRequestService _vacationRequestService;
     
        private readonly IMapHelper _mapHelper;
        private readonly IViewListMapper _listMapper;

        public RequestController(IEmployeeService employeeService,
                                 IVacationRequestService vacationRequestService,
                                 IMapHelper mapHelper,
                                 IViewListMapper listMapper)
        {
            _employeeService = employeeService;
            _vacationRequestService = vacationRequestService;
           
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

            var types = await _vacationRequestService.GetVacationTypesAsync();
            model.VacationTypes = _listMapper.CreateVacationTypesList(types);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RequestVacation(RequestVacationViewModel model)
        {
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null)
            {
                var statuses = await _vacationRequestService.GetRequestStatusesAsync();
                var vacationTypes = await _vacationRequestService.GetVacationTypesAsync();

                var vacationRequest = _mapHelper.Map<RequestVacationViewModel, VacationRequestDTO>(model);
                if (employee.TeamId != null)
                {
                    vacationRequest.RequestStatusId = statuses.FirstOrDefault(status => status.Name.Equals(RequestStatusEnum.Pending.ToString())).RequestStatusId;
                }
                else
                {
                    vacationRequest.RequestStatusId = statuses.FirstOrDefault(status => status.Name.Equals(RequestStatusEnum.Proccessing.ToString())).RequestStatusId;
                }
                vacationRequest.VacationId = Guid.NewGuid().ToString();
                vacationRequest.CreateDate = DateTime.UtcNow.Date;
                await _vacationRequestService.CreateVacationRequestAsync(vacationRequest);
            }

            return RedirectToAction("Profile", "Profile", new { id = model.EmployeeId });
        }
    }
}