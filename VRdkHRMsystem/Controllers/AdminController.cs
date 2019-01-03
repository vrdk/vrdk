﻿using System;
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
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
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
            IVacationService vacationService,
            IResidualsService residualsService,
            ISickLeaveService sickLeaveService,
            ITransactionService transactionService,
            IFileManagmentService fileManagmentService,
            INotificationService notificationService,
            ITeamService teamService,
            IAssignmentService assignmentService,
            IEmailSender emailSender,
            IViewListMapper listMapper,
            IMapHelper mapHelper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeService = employeeService;
            _residualsService = residualsService;
            _vacationService = vacationService;
            _transactionService = transactionService;
            _fileManagmentService = fileManagmentService;
            _notificationService = notificationService;
            _postService = postService;
            _teamService = teamService;
            _assignmentService = assignmentService;
            _sickLeaveRequestService = sickLeaveService;
            _emailSender = emailSender;
            _listMapper = listMapper;
            _mapHelper = mapHelper;
        }

        [HttpGet]
        public async Task<IActionResult> EditEmployee(string id)
        {
            var employee = await _employeeService.GetByIdWithResidualsAsync(id);
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(employee.EmployeeId);
                var role = await _userManager.GetRolesAsync(user);
                var posts = await _postService.GetPostsByOrganisationIdAsync(employee.OrganisationId);
                var model = _mapHelper.Map<EmployeeDTO, EditEmployeeViewModel>(employee);
                model.AbsenceBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance;
                model.AssignmentBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance;
                model.PaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance;
                model.UnpaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance;
                model.SickLeaveBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance;
                model.Posts = _listMapper.CreateOrganisationPostsList(posts, employee.PostId);
                model.Roles = _listMapper.CreateRolesList(role[0]);
                model.States = _listMapper.CreateStateList(model.State);
                return View(model);
            }

            return View("AddEmployee");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel model)
        {
            var employee = await _employeeService.GetByIdWithResidualsAsync(model.EmployeeId);
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

                var roles = await _userManager.GetRolesAsync(user);
                if (!roles.Contains(model.Role))
                {
                   if ((!roles.Contains(RoleEnum.Administrator.ToString())
                   && model.Role == RoleEnum.Administrator.ToString())
                   || (!roles.Contains(RoleEnum.Administrator.ToString())
                   && !roles.Contains(RoleEnum.Teamlead.ToString())
                   && model.Role == RoleEnum.Teamlead.ToString())
                   )
                   {
                        await _userManager.AddToRoleAsync(user, model.Role.ToString());
                   }
                   else
                   {
                        await _userManager.RemoveFromRolesAsync(user, roles);
                        await _userManager.AddToRoleAsync(user, model.Role);
                   }
                }                          
                await _userManager.UpdateAsync(user);                
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance = model.AbsenceBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance = model.AssignmentBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance = model.PaidVacationBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance = model.UnpaidVacationBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance = model.SickLeaveBalance;
                var newEmployee = _mapHelper.Map<EditEmployeeViewModel, EmployeeDTO>(model);
                await _employeeService.UpdateAsync(newEmployee);
                await _residualsService.UpdateRangeAsync(employee.EmployeeBalanceResiduals);
            }

            return RedirectToAction("Profile","Profile");
        }

        [HttpGet]
        public async Task<IActionResult> AddEmployee()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var posts = await _postService.GetPostsByOrganisationIdAsync(user.OrganisationId);

            var model = new AddEmployeeViewModel()
            {
                Posts = _listMapper.CreateOrganisationPostsList(posts),
                Roles = _listMapper.CreateRolesList(),
                States = _listMapper.CreateStateList(true)
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
                   var residuals = new BalanceResidualsDTO[]
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
                      },
                      new BalanceResidualsDTO
                      {
                      ResidualId = Guid.NewGuid().ToString(),
                      EmployeeId = employee.EmployeeId,
                      Name = ResidualTypeEnum.Assignment.ToString(),
                      ResidualBalance = model.AssignmentBalance
                      }
                    };
                    await _employeeService.CreateAsync(employee);
                    await _residualsService.CreateRangeAsync(residuals);
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
            var requests = await _vacationService.GetForProccessAsync(req
                                                        => (req.Employee.Team.TeamleadId == employee.EmployeeId
                                                        || req.RequestStatus.Equals(RequestStatusEnum.Proccessing.ToString()))
                                                        && req.Employee.OrganisationId == employee.OrganisationId);
            var model = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(requests);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ProccessVacationRequest(string codeV)
        {
            var vacationRequest = await _vacationService.GetByIdWithEmployeeWithTeamAsync(codeV);
            if (vacationRequest != null)
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
            var vacationRequest = await _vacationService.GetByIdAsync(model.VacationId);
            if (vacationRequest != null)
            {
                vacationRequest.ProccessDate = DateTime.UtcNow;
                vacationRequest.ProccessedbyId = user.Id;
                if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                {
                    vacationRequest.RequestStatus = RequestStatusEnum.Approved.ToString();
                    await _vacationService.UpdateAsync(vacationRequest);
                    var residual = await _residualsService.GetByEmployeeIdAsync(vacationRequest.EmployeeId, model.VacationType);                
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
                    await _vacationService.UpdateAsync(vacationRequest);
                }               
                var notification = new NotificationDTO
                {
                    NotificationId = Guid.NewGuid().ToString(),
                    EmployeeId = vacationRequest.EmployeeId,
                    OrganisationId = user.OrganisationId,
                    NotificationType = NotificationTypeEnum.Vacation.ToString(),
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

            var employees = await _employeeService.GetAsync(emp => emp.OrganisationId.Equals(creator.OrganisationId));

            var model = new AddTeamViewModel
            {
                Employees = _listMapper.CreateEmployeesList(employees.Where(emp => emp.TeamId == null).ToArray()),
                Teamleads = _listMapper.CreateEmployeesList(employees),
                OrganisationId = creator.OrganisationId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddTeam(AddTeamViewModel model)
        {
            var employees = await _employeeService.GetAsync(emp => model.TeamMembers.Contains(emp.EmployeeId));
            var team = new TeamDTO
            {
                TeamId = Guid.NewGuid().ToString(),
                OrganisationId = model.OrganisationId,
                TeamleadId = model.TeamleadId,
                Name = model.Name
            };
            await _teamService.CreateAsync(team);
            foreach (var employee in employees)
            {
                employee.TeamId = team.TeamId;
            }

            await _employeeService.UpdateRangeAsync(employees);

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

            var employees = await _employeeService.GetAsync(emp => emp.OrganisationId.Equals(creator.OrganisationId));

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
                Duration = model.EndDate.DayOfYear - model.BeginDate.DayOfYear,
                Name = model.Name,
                OrganisationId = model.OrganisationId
            };
            assignment.Employees = model.AssignmentMembers.Select(code => new AssignmentEmployeeDTO
            {
                AssignmentId = assignment.AssignmentId,
                EmployeeId = code,
                RowId = Guid.NewGuid().ToString()
            }).ToArray();

            await _assignmentService.CreateAsync(assignment);
            var employees = await _employeeService.GetWithTeam(emp => model.AssignmentMembers.Any(m => m == emp.EmployeeId));
            var residuals = await _residualsService.GetAsync(res => model.AssignmentMembers.Any(m => m == res.EmployeeId) && res.Name == ResidualTypeEnum.Assignment.ToString());
            foreach (var res in residuals)
            {
                res.ResidualBalance += assignment.Duration;
            }

            await _residualsService.UpdateRangeAsync(residuals);
            List<NotificationDTO> notificationList = new List<NotificationDTO>();
            foreach (var employee in employees)
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
                if (employee.TeamId != null)
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