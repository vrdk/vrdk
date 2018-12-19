using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Models.Profile;
using VRdkHRMsystem.Models.Profile.Assignment;
using VRdkHRMsystem.Models.Profile.SickLeave;
using VRdkHRMsystem.Models.Profile.Vacation;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private const string emptyValue = "None";
        private readonly IEmployeeService _employeeService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IVacationService _vacationService;
        private readonly IAssignmentService _assignmentService;
        private readonly IMapHelper _mapHelper;

        public ProfileController(IEmployeeService employeeService,
                                 IPostService postService,
                                 IVacationService vacationService,
                                 IAssignmentService assignmentService,
                                 ISickLeaveService sickLeaveService,
        IMapHelper mapHelper)
        {
            _employeeService = employeeService;
            _sickLeaveService = sickLeaveService;
            _assignmentService = assignmentService;
            _vacationService = vacationService;
            _postService = postService;
            _mapHelper = mapHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Profile(string id = null)
        {
            ProfileViewModel model;
            EmployeeDTO employee;
            if(id != null)
            {
                employee = await _employeeService.GetByIdWithTeamAsync(id);               
            }
            else
            {
                employee = await _employeeService.GetByEmailWithTeamAsync(User.Identity.Name);              
            }
            var posts = await _postService.GetPostsByOrganisationIdAsync(employee.OrganisationId);
            model = _mapHelper.Map<EmployeeDTO, ProfileViewModel>(employee);
            model.Post = posts.FirstOrDefault(post => post.PostId.Equals(employee.PostId)).Name;
            if (employee.Team != null)
            {
                var teamlead = await _employeeService.GetByIdAsync(employee.Team.TeamleadId);
                model.Team = employee.Team.Name;
                model.Teamlead = $"{teamlead.FirstName} {teamlead.LastName}";
            }
            else
            {
                model.Team = emptyValue;
                model.Teamlead = emptyValue;
            }
            
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Vacations(string codeE)
        {
            var requests = await  _vacationService.GetAsync(req => req.EmployeeId == codeE);
            var model = _mapHelper.MapCollection<VacationRequestDTO, ProfileVacationsViewModel>(requests);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> SickLeaves(string codeE)
        {
            var requests = await _sickLeaveService.GetAsync(req => req.EmployeeId == codeE);
            var model = _mapHelper.MapCollection<SickLeaveRequestDTO, ProfileSickLeavesViewModel>(requests);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Assignments(string codeE)
        {
            var assignments = await _assignmentService.GetByEmployeeIdAsync(codeE);
            var model = assignments.Select(a     => new ProfileAssignmentsViewModel
            {
                BeginDate = a.Assignment.BeginDate,
                EndDate = a.Assignment.EndDate,
                Duration = a.Assignment.Duration,
                Name = a.Assignment.Name
            }).ToArray();
            return View(model);
        }
    }
}