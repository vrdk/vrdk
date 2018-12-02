using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Role;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.AdminViewModels;

namespace VRdkHRMsystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private const string emptyValue = "None";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IVacationRequestService _vacationRequestService;
        private readonly IResidualsService _residualsService;
        private readonly ITransactionService _transactionService;
        private readonly IPostService _postService;
        private readonly IViewListMapper _listMapper;
        private readonly IMapHelper _mapHelper;

        public AdminController(
           UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPostService postService,
            IEmployeeService employeeService,
            IVacationRequestService vacationRequestService,
            IResidualsService residualsService,
            ITransactionService transactionService,
            IEmailSender emailSender,
            ILogger<AccountController> logger,
            IViewListMapper listMapper,
            IMapHelper mapHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeService = employeeService;
            _residualsService = residualsService;
            _vacationRequestService = vacationRequestService;
            _transactionService = transactionService;
            _postService = postService;
            _emailSender = emailSender;
            _logger = logger;
            _listMapper = listMapper;
            _mapHelper = mapHelper;
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            var employee = await _employeeService.GetByIdAsync(id);
            if (employee != null)
            {
                var model = _mapHelper.Map<EmployeeDTO, EditEmployeeViewModel>(employee);
                return View(model);
            }

            return View("AddEmployee");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel model)
        {
            var employee = await _employeeService.GetByIdAsync(model.EmployeeId);
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(model.EmployeeId);
                var oldEmail = user.Email;
                user.UserName = model.WorkEmail;
                user.Email = model.WorkEmail;
                if (User.Identity.Name.Equals(model.WorkEmail) && !model.WorkEmail.Equals(oldEmail))
                {
                    await _signInManager.RefreshSignInAsync(user);
                }
                await _userManager.UpdateAsync(user);
                var newEmployee = _mapHelper.Map<EditEmployeeViewModel, EmployeeDTO>(model);
                await _employeeService.UpdateAsync(newEmployee);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var posts = await _postService.GetPostsByOrganisationIdAsync(user.OrganisationId);
            var roles = new RoleDTO[] {
                new RoleDTO
                {
                    Name=RoleEnum.Administrator.ToString()
                },
                new RoleDTO
                {
                    Name=RoleEnum.Teamlead.ToString()
                },
                new RoleDTO
                {
                    Name=RoleEnum.Employee.ToString()
                }
            };
            var model = new AddEmployeeViewModel()
            {
                Posts = _listMapper.CreateOrganisationPostsList(posts),
                Roles = _listMapper.CreateRolesList(roles)
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel model)
        {
            if (ModelState.IsValid) 
            {
                var organisationId = _userManager.FindByNameAsync(User.Identity.Name).Result.OrganisationId;
                var user = new ApplicationUser { UserName = model.WorkEmail, Email = model.WorkEmail, PhoneNumber = model.PhoneNumber, Id = Guid.NewGuid().ToString(), OrganisationId = organisationId };
                var result = await _userManager.CreateAsync(user, "123asdQ!");
                if (result.Succeeded)
                {
                    var employee = _mapHelper.Map<AddEmployeeViewModel, EmployeeDTO>(model);
                    employee.EmployeeId = user.Id;
                    employee.OrganisationId = organisationId;
                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _employeeService.CreateAsync(employee);

                    var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string callbackUrl = Url.Action("SetPasswordAndConfrimEmail", "Account", new { id = user.Id, resetCode = passwordResetCode, confirmCode = emailConfirmationCode }, Request.Scheme);
                    await _emailSender.SendPasswordResetLink(model.WorkEmail,"", "Set password","",
                      $"Please set your password by clicking here: <a href='{callbackUrl}'>link</a>");
                }

                return RedirectToAction("Profile", "Profile");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ViewVacationRequests()
        {
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            var model = await _vacationRequestService.GetProccessingVacationRequestsAsync(employee.OrganisationId, employee.TeamId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProccessVacationRequest(string id)
        {
            var request = await _vacationRequestService.GetByIdWithEmployeeWithTeamAsync(id);
            var vacationTypes = await _vacationRequestService.GetVacationTypesAsync();
            var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(request);
            model.EmployeeFullName = $"{request.Employee.FirstName} {request.Employee.LastName}";
            model.VacationType = vacationTypes.FirstOrDefault(type=>type.VacationTypeId.Equals(request.VacationTypeId)).Name;
            if(request.Employee.Team != null)
            {
                var teamlead = await _employeeService.GetByIdAsync(request.Employee.Team.TeamleadId);
                model.TeamName = request.Employee.Team.Name;
                model.TeamleadFullName = $"{teamlead.FirstName} {teamlead.LastName}";
            }
            else
            {
                model.TeamName = emptyValue;
                model.TeamleadFullName = emptyValue;
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProccessVacationRequest(VacationRequestProccessViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var request = await _vacationRequestService.GetByIdAsync(model.VacationId);
            var status = await _vacationRequestService.GetRequestStatusByNameAsync(RequestStatusEnum.Proccessing.ToString());
            if(request != null && request.RequestStatusId.Equals(status.RequestStatusId))
            {
                var residual = await _residualsService.GetByEmployeeIdAsync(request.EmployeeId, model.VacationType);
                var transactionTypes = await _transactionService.GetTransactionTypesAsync();
                var requestStatuses = await _vacationRequestService.GetRequestStatusesAsync();
                var transaction = new TransactionDTO()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    EmployeeId = request.EmployeeId,
                    Change = model.Duration,
                    Description = model.ProccessComment ?? model.VacationType,
                    TransactionDate = DateTime.UtcNow,
                    TransactionTypeId = transactionTypes.FirstOrDefault(type => type.Name.Equals(model.VacationType)).TransactionTypeId
                };
                request.TransactionId = transaction.TransactionId;
                request.ProcessDate = DateTime.UtcNow;
                request.ProcessedbyId = user.Id;
                if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                {
                    request.RequestStatusId = requestStatuses.FirstOrDefault(st => st.Name.Equals(RequestStatusEnum.Approved.ToString())).RequestStatusId;
                    residual.ResidualBalance -= request.Duration;
                }
                else
                {
                    request.RequestStatusId = requestStatuses.FirstOrDefault(st => st.Name.Equals(RequestStatusEnum.Denied.ToString())).RequestStatusId;
                }

                await _transactionService.CreateAsync(transaction);
                await _residualsService.UpdateAsync(residual);
                await _vacationRequestService.UpdateAsync(request);
            }
           
            return View(model);
        }
    }
}