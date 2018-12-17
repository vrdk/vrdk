using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Models.Profile;

namespace VRdkHRMsystem.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private const string emptyValue = "None";
        private readonly IEmployeeService _employeeService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveService;
        private readonly IMapHelper _mapHelper;

        public ProfileController(IEmployeeService employeeService,
                                 IPostService postService,
                                 ISickLeaveService sickLeaveService,
        IMapHelper mapHelper)
        {
            _employeeService = employeeService;
            _sickLeaveService = sickLeaveService;
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

       
    }
}