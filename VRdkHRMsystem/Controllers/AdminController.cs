using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VRdkHRMsysBLL.DTOs.Assignment;
using VRdkHRMsysBLL.DTOs.BalanceResiduals;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.DTOs.Transaction;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.AdminViewModels.Assignment;
using VRdkHRMsystem.Models.AdminViewModels.Employee;
using VRdkHRMsystem.Models.AdminViewModels.Team;
using VRdkHRMsystem.Models.SharedViewModels.Employee;
using VRdkHRMsystem.Models.SharedViewModels.Vacation;

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
        private readonly ISickLeaveService _sickLeaveRequestService;
        private readonly ITeamService _teamService;
        private readonly IAssignmentService _assignmentService;
        private readonly IViewListMapper _listMapper;
        private readonly INotificationService _notificationService;
        private readonly IMapHelper _mapHelper;
        private readonly IFileManagmentService _fileManagmentService;

        public AdminController(
           UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IPostService postService,
            IEmployeeService employeeService,
            IVacationService vacationRequestService,
            IResidualsService residualsService,
            ISickLeaveService sickLeaveService,
            ITransactionService transactionService,
            IFileManagmentService fileManagmentService,
            INotificationService notificationService,
            ITeamService teamService,
            IAssignmentService assignmentService,
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
            _fileManagmentService = fileManagmentService;
            _notificationService = notificationService;
            _postService = postService;
            _teamService = teamService;
            _assignmentService = assignmentService;
            _sickLeaveRequestService = sickLeaveService;
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
                    await _userManager.AddToRoleAsync(user, model.Role);
                    var employee = _mapHelper.Map<AddEmployeeViewModel, EmployeeDTO>(model);
                    employee.EmployeeId = user.Id;
                    employee.OrganisationId = organisationId;
                    employee.EmployeeBalanceResiduals = new BalanceResidualsDTO[]
                    {
                      new BalanceResidualsDTO
                      {
                      ResidualId = Guid.NewGuid().ToString(),
                      EmployeeId = employee.EmployeeId,
                      Name = ResidualTypeEnum.Unpaid_vacation.ToString(),
                      ResidualBalance = model.UnpaidVacationBalance
                      },
                      new BalanceResidualsDTO
                      {
                      ResidualId = Guid.NewGuid().ToString(),
                      EmployeeId = employee.EmployeeId,
                      Name = ResidualTypeEnum.Paid_vacation.ToString(),
                      ResidualBalance = model.PaidVacationBalance
                      },
                      new BalanceResidualsDTO
                      {
                      ResidualId = Guid.NewGuid().ToString(),
                      EmployeeId = employee.EmployeeId,
                      Name = ResidualTypeEnum.Sick_leave.ToString(),
                      ResidualBalance = model.SickLeaveBalance
                      },
                      new BalanceResidualsDTO
                      {
                      ResidualId = Guid.NewGuid().ToString(),
                      EmployeeId = employee.EmployeeId,
                      Name = ResidualTypeEnum.Absence.ToString(),
                      ResidualBalance = model.AbsenceBalance
                      }
                    };
                    await _employeeService.CreateAsync(employee);

                    var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string callbackUrl = Url.Action("SetPasswordAndConfrimEmail", "Account", new { id = user.Id, resetCode = passwordResetCode, confirmCode = emailConfirmationCode }, Request.Scheme);
                    await _emailSender.SendPasswordResetLink(model.WorkEmail, "", "Set password", "",
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
        public async Task<IActionResult> ProccessVacationRequest(string codeV)
        {
            var vacationRequest = await _vacationRequestService.GetByIdWithEmployeeWithTeamAsync(codeV);
            if (vacationRequest != null && vacationRequest.RequestStatus.Equals(RequestStatusEnum.Proccessing.ToString()))
            {
                var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(vacationRequest);
                model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                model.VacationType = vacationRequest.VacationType;
                if (vacationRequest.Employee.Team != null)
                {
                    var teamlead = await _employeeService.GetByIdAsync(vacationRequest.Employee.Team.TeamleadId);
                    model.TeamName = vacationRequest.Employee.Team.Name;
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
            var vacationRequest = await _vacationRequestService.GetByIdAsync(model.VacationId);
            if (vacationRequest != null && vacationRequest.RequestStatus.Equals(RequestStatusEnum.Proccessing.ToString()))
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
        public async Task<IActionResult> AddTeam(string codeE = null)
        {
            EmployeeDTO creator;
            if (codeE == null)
            {
                creator = await _employeeService.GetByEmailWithTeamAsync(User.Identity.Name);
            }
            else
            {
                creator = await _employeeService.GetByIdWithTeamAsync(codeE);
            }

            var employees = await _employeeService.Get(emp => emp.OrganisationId.Equals(creator.OrganisationId));

            var model = new AddTeamViewModel
            {
                Employees = _mapHelper.MapCollection<EmployeeDTO, EmployeeViewModel>(employees.Where(emp=>emp.TeamId == null).ToArray()),
                Teamleads = _listMapper.CreateEmployeesList(employees),
                OrganisationId = creator.OrganisationId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeam(AddTeamViewModel model)
        {
            var employees = await _employeeService.Get(emp => model.TeamMembers.Contains(emp.EmployeeId));
            var team = new TeamDTO
            {
                TeamId = Guid.NewGuid().ToString(),
                OrganisationId = model.OrganisationId,
                TeamleadId = model.TeamleadId,
                Name = model.Name
            };
            await _teamService.CreateAsync(team);
            foreach(var employee in employees)
            {
                employee.TeamId = team.TeamId;
            }

            await _employeeService.UpdateRange(employees);

            var user = await _userManager.FindByIdAsync(model.TeamleadId);
            await _userManager.AddToRoleAsync(user, RoleEnum.Teamlead.ToString());
            return RedirectToAction("Profile", "Profile");
        }

        [HttpGet]
        public async Task<IActionResult> AddAssignment(string codeE = null)
        {
            EmployeeDTO creator;
            if (codeE == null)
            {
                creator = await _employeeService.GetByEmailWithTeamAsync(User.Identity.Name);
            }
            else
            {
                creator = await _employeeService.GetByIdWithTeamAsync(codeE);
            }

            var employees = await _employeeService.Get(emp => emp.OrganisationId.Equals(creator.OrganisationId));

            var model = new AddAssignmentViewModel
            {
                Employees = _mapHelper.MapCollection<EmployeeDTO, EmployeeViewModel>(employees).ToArray(),
                OrganisationId = creator.OrganisationId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentViewModel model)
        {
            var assignment = new AssignmentDTO
            {
                AssignmentId = Guid.NewGuid().ToString(),
                BeginDate = model.BeginDate,
                EndDate = model.EndDate,
                Duration = model.EndDate.DayOfYear - model.EndDate.DayOfYear,
                Name = model.Name,
                OrganisationId = model.OrganisationId          
            };
            assignment.Employees = model.AssignmentMembers.Select(code => new AssignmentEmployeeDTO
            {
                AssignmentId = assignment.AssignmentId,
                EmployeeId =  code,
                RowId = Guid.NewGuid().ToString()
            }).ToArray();
            await _assignmentService.CreateAsync(assignment);
            var employees = await _employeeService.GetWithTeam(emp => model.AssignmentMembers.Any(m => m == emp.EmployeeId));
            List<NotificationDTO> notificationList = new List<NotificationDTO>();
            foreach(var employee in employees)
            {
                notificationList.Add(new NotificationDTO
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    EmployeeId = employee.EmployeeId,
                    OrganisationId = employee.OrganisationId,
                    NotificationType = NotificationTypeEnum.Assignment.ToString(),
                    NotificationDate = DateTime.UtcNow,
                    Description = "You have been added to assignment",
                    IsChecked = false                   
                });
                if(employee.TeamId != null)
                {
                    notificationList.Add(new NotificationDTO
                    {
                        NotificationId = Guid.NewGuid().ToString(),
                        EmployeeId = employee.Team.TeamleadId,
                        OrganisationId = employee.OrganisationId,
                        NotificationType = NotificationTypeEnum.Assignment.ToString(),
                        NotificationDate = DateTime.UtcNow,
                        Description = "Your employee have been added to assignment",
                        IsChecked = false
                    });
                }
            }

            await _notificationService.CreateRangeAsync(notificationList.ToArray());

            return RedirectToAction("Profile", "Profile");
        }
    }
}