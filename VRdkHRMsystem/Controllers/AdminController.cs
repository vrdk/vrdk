using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.Employee.AdminViewModels;
using VRdkHRMsystem.Models.Vacation.AdminViewModels;

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
        private readonly IVacationService _vacationRequestService;
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
            IVacationService vacationRequestService,
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
           
            var model = new AddEmployeeViewModel()
            {
                Posts = _listMapper.CreateOrganisationPostsList(posts),
                Roles = _listMapper.CreateRolesList()
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
            var requests = await _vacationRequestService.GetProccessingVacationRequestsAsync(employee.OrganisationId, employee.TeamId);
            var model = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(requests);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProccessVacationRequest(string id)
        {
            var request = await _vacationRequestService.GetByIdWithEmployeeWithTeamAsync(id);
            if(request != null && request.RequestStatus.Equals(RequestStatusEnum.Proccessing.ToString()))
            {
                var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(request);
                model.EmployeeFullName = $"{request.Employee.FirstName} {request.Employee.LastName}";
                model.VacationType = request.VacationType;
                if (request.Employee.Team != null)
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

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProccessVacationRequest(VacationRequestProccessViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var request = await _vacationRequestService.GetByIdAsync(model.VacationId);
            if(request != null && request.RequestStatus.Equals(RequestStatusEnum.Proccessing.ToString()))
            {             
                var transaction = new TransactionDTO()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    EmployeeId = request.EmployeeId,
                    Change = model.Duration,
                    Description = model.ProccessComment ?? model.VacationType,
                    TransactionDate = DateTime.UtcNow,
                    TransactionType = model.VacationType
                };
                await _transactionService.CreateAsync(transaction);
                request.TransactionId = transaction.TransactionId;
                request.ProccessDate = DateTime.UtcNow;
                request.ProccessedbyId = user.Id;
                if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                {
                    var residual = await _residualsService.GetByEmployeeIdAsync(request.EmployeeId, model.VacationType);
                    request.RequestStatus = RequestStatusEnum.Approved.ToString();
                    residual.ResidualBalance -= request.Duration;
                    await _residualsService.UpdateAsync(residual);
                }
                else
                {
                    request.RequestStatus = RequestStatusEnum.Denied.ToString();
                }                     
                await _vacationRequestService.UpdateAsync(request);
            }
           
            return View(model);
        }
    }
}