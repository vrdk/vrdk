﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsystem.Interfaces;
using VRdkHRMsystem.Models;
using VRdkHRMsystem.Models.AdminViewModels;
using VRdkHRMsystem.Models.SharedModels;

namespace VRdkHRMsystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private const string emptyValue = "Нет";
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IEmployeeService _employeeService;
        private readonly IVacationService _vacationService;
        private readonly IResidualsService _residualsService;
        private readonly IAbsenceService _absenceService;
        private readonly ITransactionService _transactionService;
        private readonly IPostService _postService;
        private readonly ISickLeaveService _sickLeaveService;
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
            IAbsenceService absenceService,
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
            _absenceService = absenceService;
            _transactionService = transactionService;
            _fileManagmentService = fileManagmentService;
            _notificationService = notificationService;
            _postService = postService;
            _teamService = teamService;
            _assignmentService = assignmentService;
            _sickLeaveService = sickLeaveService;
            _emailSender = emailSender;
            _listMapper = listMapper;
            _mapHelper = mapHelper;
        }

        #region Calendar
        [HttpGet]
        public async Task<IActionResult> Calendar(int year, int month, string teamId)
        {
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                TeamDTO team;
                var teams = await _teamService.GetAsync(t => t.OrganisationId == viewer.OrganisationId);

                if (year == 0 || month == 0 || month > 12 || year > 9999)
                {
                    year = DateTime.UtcNow.Year;
                    month = DateTime.UtcNow.Month;
                }

                if (teamId == null)
                {
                    if (viewer.TeamId != null)
                    {
                        team = teams.FirstOrDefault(t => t.TeamId == viewer.TeamId);
                    }
                    else
                    {
                        team = teams.FirstOrDefault(t => t.Employees.Count() != 0);
                    }
                }
                else
                {
                    team = teams.FirstOrDefault(t => t.TeamId == teamId);
                }

                if (team != null)
                {
                    CalendarViewModel model;
                    EmployeeDTO[] employees;
                    if (team.TeamId == viewer.TeamId)
                    {
                        employees = await _employeeService.GetForCalendarAsync(team.TeamId, team.TeamleadId, month, year, viewer.EmployeeId);
                        model = new CalendarViewModel
                        {
                            Year = year,
                            Month = month,
                            TeamId = team.TeamId,
                            TeamName = team.Name,
                            MainMemberId = viewer.EmployeeId,
                            Teams = _listMapper.CreateTeamList(teams, team.TeamId),
                            Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                            Culture = CultureInfo.CreateSpecificCulture("ru-RU"),
                            Role = "Admin"
                        };

                        return View("~/Views/Profile/Calendar.cshtml", model);
                    }

                    employees = await _employeeService.GetForCalendarAsync(team.TeamId, team.TeamleadId, month, year);

                    model = new CalendarViewModel
                    {
                        Year = year,
                        Month = month,
                        TeamId = team.TeamId,
                        TeamName = team.Name,
                        MainMemberId = team.TeamleadId,
                        Teams = _listMapper.CreateTeamList(teams, team.TeamId),
                        Employees = _mapHelper.MapCollection<EmployeeDTO, CalendarEmployeeViewModel>(employees),
                        Culture = CultureInfo.CreateSpecificCulture("ru-RU"),
                        Role = "Admin"
                    };

                    if (team.TeamleadId == viewer.EmployeeId)
                    {
                        return View("~/Views/Teamlead/Calendar.cshtml", model);
                    }

                    return View(model);
                }

                var emptyModel = new CalendarViewModel()
                {
                    Year = year,
                    Month = month,
                    Teams = _listMapper.CreateTeamList(teams, null)
                };

                return View(emptyModel);
            }

            return RedirectToAction("Profile", "Profile");
        }

        #endregion

        #region Absences

        [HttpGet]
        public async Task<IActionResult> Absences(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var absences = new AbsenceListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                absences = await _absenceService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId);
                count = await _absenceService.GetAbsencesCountAsync(searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId);
            }

            var pagedAbsences = new AbsenceListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Absences = _mapHelper.MapCollection<AbsenceListUnitDTO, AbsenceViewModel>(absences)
            };

            return View(pagedAbsences);
        }

        [HttpGet]
        public async Task<IActionResult> AbsencesPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var absences = new AbsenceListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                absences = await _absenceService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId);
                count = await _absenceService.GetAbsencesCountAsync(searchKey, a => a.Employee.OrganisationId == viewer.OrganisationId);
            }

            var pagedAbsences = new AbsenceListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Absences = _mapHelper.MapCollection<AbsenceListUnitDTO, AbsenceViewModel>(absences)
            };

            return PartialView(pagedAbsences);
        }

        #endregion

        #region Teams

        [HttpGet]
        public async Task<IActionResult> EditTeam(string id)
        {
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            var team = await _teamService.GetByIdAsync(id);
            if (viewer != null && team != null)
            {
                var employees = await _employeeService.GetAsync(emp => emp.OrganisationId == viewer.OrganisationId);
                var teamMembers = team.Employees.Select(m => m.EmployeeId).ToArray();
                var model = new EditTeamViewModel
                {
                    TeamId = team.TeamId,
                    Name = team.Name,
                    Employees = _listMapper.CreateSelectedEmployeesList(employees.Where(emp => emp.TeamId == null || emp.TeamId == team.TeamId).ToArray(), teamMembers),
                    Teamleads = _listMapper.CreateSelectedEmployeesList(employees, new string[] { team.TeamleadId })
                };

                return View(model);
            }

            return RedirectToAction("Teams", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditTeam(EditTeamViewModel model)
        {
            var team = await _teamService.GetByIdAsync(model.TeamId);
            if (team != null)
            {
                var teamMembers = team.Employees.Select(e => e.EmployeeId).ToArray();
                var removedFromTeam = teamMembers.Except(model.TeamMembers).ToArray();
                var addedToTeam = model.TeamMembers.Except(teamMembers).ToArray();
                if (removedFromTeam.Count() != 0)
                {
                    await _employeeService.RemoveFromTeamAsync(removedFromTeam);
                }

                if (addedToTeam.Count() != 0)
                {
                    await _employeeService.AddToTeamAsync(addedToTeam, team.TeamId);
                }

                if (team.TeamleadId != model.TeamleadId)
                {
                    team.TeamleadId = model.TeamleadId;
                    var user = await _userManager.FindByIdAsync(model.TeamleadId);
                    await _userManager.AddToRoleAsync(user, RoleEnum.Teamlead.ToString());
                }

                team.Name = model.Name;

                await _teamService.UpdateAsync(team, true);

                return RedirectToAction("Teams", "Admin");
            }

            return RedirectToAction("Teams", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> TeamProfile(string id)
        {
            var team = await _teamService.GetByIdAsync(id);

            var model = _mapHelper.Map<TeamDTO, TeamProfileViewModel>(team);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Teams(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var teams = new TeamListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                teams = await _teamService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, t => t.OrganisationId == viewer.OrganisationId);
                count = await _teamService.GetTeamsCountAsync(searchKey, t => t.OrganisationId == viewer.OrganisationId);
            }

            var pagedTeams = new TeamListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Teams = _mapHelper.MapCollection<TeamListUnitDTO, TeamViewModel>(teams)
            };

            return View(pagedTeams);
        }

        [HttpGet]
        public async Task<IActionResult> TeamsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var teams = new TeamListUnitDTO[] { };
            var viewer = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (viewer != null)
            {
                teams = await _teamService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey, t => t.OrganisationId == viewer.OrganisationId);
                count = await _teamService.GetTeamsCountAsync(searchKey, t => t.OrganisationId == viewer.OrganisationId);
            }

            var pagedTeams = new TeamListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Teams = _mapHelper.MapCollection<TeamListUnitDTO, TeamViewModel>(teams)
            };

            return PartialView(pagedTeams);
        }

        #endregion

        #region Employees

        [HttpGet]
        public async Task<IActionResult> Employees(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var employees = new EmployeeListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                employees = await _employeeService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                emp => emp.State && emp.OrganisationId == employee.OrganisationId);
                count = await _employeeService.GetEmployeesCountAsync(searchKey, emp => emp.State && emp.OrganisationId == employee.OrganisationId);
            }

            var pagedEmployees = new EmployeeListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Employees = _mapHelper.MapCollection<EmployeeListUnitDTO, EmployeeViewModel>(employees)
            };

            return View(pagedEmployees);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeesPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var employees = new EmployeeListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                employees = await _employeeService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                emp => emp.State && emp.OrganisationId == employee.OrganisationId);
                count = await _employeeService.GetEmployeesCountAsync(searchKey, emp => emp.State && emp.OrganisationId == employee.OrganisationId);
            }

            var pagedEmployees = new EmployeeListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Employees = _mapHelper.MapCollection<EmployeeListUnitDTO, EmployeeViewModel>(employees)
            };

            return PartialView(pagedEmployees);
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
                model.States = _listMapper.CreateStateList(model.State.ToString());
                return View(model);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployee(EditEmployeeViewModel model)
        {
            var employee = await _employeeService.GetByIdWithTeamWithResidualsAsync(model.EmployeeId);
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(model.EmployeeId);
                var oldEmail = user.Email;
                user.UserName = model.WorkEmail;
                user.Email = model.WorkEmail;
                var roles = await _userManager.GetRolesAsync(user);

                if (roles.Contains("Administrator") && model.Role != "Administrator")
                {
                    await _userManager.RemoveFromRolesAsync(user, roles);
                    await _userManager.AddToRoleAsync(user, model.Role);
                    await _userManager.UpdateAsync(user);
                    await _signInManager.RefreshSignInAsync(user);
                }
                else if (!roles.Contains(model.Role))
                {
                    if (model.Role == "Administrator")
                    {
                        await _userManager.AddToRoleAsync(user, model.Role);
                        await _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        await _userManager.RemoveFromRolesAsync(user, roles);
                        await _userManager.AddToRoleAsync(user, model.Role);
                        await _userManager.UpdateAsync(user);
                    }

                    if (User.Identity.Name == oldEmail)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                    }
                }
                else
                {
                    if (User.Identity.Name == oldEmail && !(model.WorkEmail == oldEmail))
                    {
                        await _signInManager.RefreshSignInAsync(user);
                    }
                }

                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance = model.AbsenceBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance = model.AssignmentBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance = model.PaidVacationBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance = model.UnpaidVacationBalance;
                employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance = model.SickLeaveBalance;
                var newEmployee = _mapHelper.Map<EditEmployeeViewModel, EmployeeDTO>(model);
                newEmployee.TeamId = employee.Team?.TeamId;
                newEmployee.EmployeeBalanceResiduals = employee.EmployeeBalanceResiduals;
                await _employeeService.UpdateAsync(newEmployee, true);

                return Ok();
            }

            return BadRequest();
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
                States = _listMapper.CreateStateList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee(AddEmployeeViewModel model)
        {
            if (ModelState.IsValid)
            {
                var organisationId = _userManager.FindByNameAsync(User.Identity.Name).Result.OrganisationId;
                var user = new ApplicationUser { UserName = model.WorkEmail, Email = model.WorkEmail, PhoneNumber = model.PhoneNumber, Id = Guid.NewGuid().ToString(), OrganisationId = organisationId, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(user, Guid.NewGuid().ToString().ToUpper());
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
                    await _residualsService.CreateRangeAsync(residuals, true);
                    var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
                    string callbackUrl = Url.Action("SetPassword", "Account", new { id = user.Id, resetCode = passwordResetCode }, Request.Scheme);
                    await _emailSender.SendPasswordResetLink(model.WorkEmail, "", "Set password", "",
                        $"Чтобы перейти к форме изменения пароля, нажмите на ссылку: <a href='{callbackUrl}'>изменить пароль</a>");

                    await _fileManagmentService.UploadDefaultUserPhoto(employee.EmployeeId);
                }

                return RedirectToAction("Profile", "Profile");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeProfile(string id)
        {
            var employee = await _employeeService.GetByIdWithResidualsAsync(id);
            var model = new EmployeeProfileViewModel();
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(employee.EmployeeId);
                var role = await _userManager.GetRolesAsync(user);
                var posts = await _postService.GetPostsByOrganisationIdAsync(employee.OrganisationId);
                model = _mapHelper.Map<EmployeeDTO, EmployeeProfileViewModel>(employee);
                model.AbsenceBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance;
                model.AssignmentBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance;
                model.PaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance;
                model.UnpaidVacationBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance;
                model.SickLeaveBalance = employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance;
                model.Role = _listMapper.TranslateRole(role.Last());
                model.Post = posts.FirstOrDefault(p => p.PostId == employee.PostId).Name;
            }

            return View(model);
        }

        #endregion

        #region Vacations

        [HttpGet]
        public async Task<IActionResult> Vacations(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var vacations = new VacationRequestViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                vacations = await _vacationService.GetPageWithProccessingPriorityAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                             req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                             || req.RequestStatus != RequestStatusEnum.Pending.ToString()
                                                                             && req.Employee.OrganisationId == employee.OrganisationId
                                                                             && req.Employee.State);
                count = await _vacationService.GetVacationsNumberAsync(searchKey,
                                                                  req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                  || req.RequestStatus != RequestStatusEnum.Pending.ToString()
                                                                  && req.Employee.OrganisationId == employee.OrganisationId
                                                                  && req.Employee.State);
            }

            var pagedVacations = new VacationRequestListViewModel
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                SyncHubAnchor = employee.OrganisationId,
                Vacations = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(vacations)
            };

            return View(pagedVacations);
        }

        [HttpGet]
        public async Task<IActionResult> VacationsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var vacations = new VacationRequestViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                vacations = await _vacationService.GetPageWithProccessingPriorityAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                             req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                             || req.RequestStatus != RequestStatusEnum.Pending.ToString()
                                                                             && req.Employee.OrganisationId == employee.OrganisationId
                                                                             && req.Employee.State);
                count = await _vacationService.GetVacationsNumberAsync(searchKey,
                                                                  req => req.Employee.Team.TeamleadId == employee.EmployeeId
                                                                  || req.RequestStatus != RequestStatusEnum.Pending.ToString()
                                                                  && req.Employee.OrganisationId == employee.OrganisationId
                                                                  && req.Employee.State);
            }

            var pagedVacations = new VacationRequestListViewModel
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                SyncHubAnchor = employee.OrganisationId,
                Vacations = _mapHelper.MapCollection<VacationRequestViewDTO, VacationRequestViewModel>(vacations)
            };

            return PartialView(pagedVacations);
        }

        [HttpGet]
        public async Task<IActionResult> CheckVacationRequest(string id)
        {
            var vacationRequest = await _vacationService.GetByIdWithEmployeeWithTeamAsync(id);
            if (vacationRequest != null)
            {
                if (vacationRequest.RequestStatus != RequestStatusEnum.Denied.ToString() && vacationRequest.RequestStatus != RequestStatusEnum.Approved.ToString())
                {
                    var posts = await _postService.GetPostsByOrganisationIdAsync(vacationRequest.Employee.OrganisationId);
                    var model = _mapHelper.Map<VacationRequestDTO, VacationRequestProccessViewModel>(vacationRequest);
                    model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                    model.VacationType = vacationRequest.VacationType;
                    model.Post = posts.FirstOrDefault(p => p.PostId == vacationRequest.Employee.PostId).Name;
                    if (vacationRequest.Employee.Team != null)
                    {
                        model.TeamName = vacationRequest.Employee.Team.Name;
                        model.TeamleadFullName = $"{vacationRequest.Employee.Team.Teamlead.FirstName} {vacationRequest.Employee.Team.Teamlead.LastName}";
                    }
                    else
                    {
                        model.TeamName = emptyValue;
                        model.TeamleadFullName = emptyValue;
                    }

                    return PartialView("VacationProccessModal", model);
                }
                else
                {
                    var proccessor = await _employeeService.GetByIdAsync(vacationRequest.ProccessedbyId);
                    var posts = await _postService.GetPostsByOrganisationIdAsync(vacationRequest.Employee.OrganisationId);
                    var model = _mapHelper.Map<VacationRequestDTO, VacationRequestCheckViewModel>(vacationRequest);
                    model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                    model.VacationType = vacationRequest.VacationType;
                    model.Post = posts.FirstOrDefault(p => p.PostId == vacationRequest.Employee.PostId).Name;
                    if (proccessor != null)
                    {
                        model.ProccessedByName = $"{proccessor.FirstName} {proccessor.LastName}";
                    }
                    else
                    {
                        model.ProccessedByName = "Данные отсутствуют";
                    }
                    model.RequestStatus = vacationRequest.RequestStatus;
                    if (vacationRequest.Employee.Team != null)
                    {
                        model.TeamName = vacationRequest.Employee.Team.Name;
                        model.TeamleadFullName = $"{vacationRequest.Employee.Team.Teamlead.FirstName} {vacationRequest.Employee.Team.Teamlead.LastName}";
                    }
                    else
                    {
                        model.TeamName = emptyValue;
                        model.TeamleadFullName = emptyValue;
                    }

                    return PartialView("VacationViewModal", model);
                }
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> ProccessVacationRequest(VacationRequestProccessViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var vacationRequest = await _vacationService.GetByIdAsync(model.VacationId);
            if (vacationRequest != null && (vacationRequest.RequestStatus != RequestStatusEnum.Approved.ToString() && vacationRequest.RequestStatus != RequestStatusEnum.Denied.ToString()))
            {
                try
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
                            Change = vacationRequest.Duration,
                            Description = model.VacationType,
                            TransactionDate = DateTime.UtcNow,
                            TransactionType = model.VacationType
                        };
                        await _transactionService.CreateAsync(transaction);
                        vacationRequest.TransactionId = transaction.TransactionId;
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = vacationRequest.EmployeeId,
                            OrganisationId = user.OrganisationId,
                            NotificationType = NotificationTypeEnum.Vacation.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            Description = "Ваша заявка на отпуск была подтверждена.",
                            NotificationRange = NotificationRangeEnum.User.ToString()
                        };

                        await _notificationService.CreateAsync(notification, true);
                    }
                    else
                    {
                        vacationRequest.RequestStatus = RequestStatusEnum.Denied.ToString();
                        await _vacationService.UpdateAsync(vacationRequest);
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = vacationRequest.EmployeeId,
                            OrganisationId = user.OrganisationId,
                            NotificationType = NotificationTypeEnum.Vacation.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            NotificationRange = NotificationRangeEnum.User.ToString(),
                            Description = "Ваша заявка на отпуск была отклонена."
                        };

                        await _notificationService.CreateAsync(notification, true);
                    }

                    return Ok(vacationRequest);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return BadRequest(e.Message);
                }
            }

            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> ViewVacationRequest(string id)
        {
            var vacationRequest = await _vacationService.GetByIdWithEmployeeWithTeamAsync(id);
            if (vacationRequest != null)
            {
                var proccessor = await _employeeService.GetByIdAsync(vacationRequest.ProccessedbyId);
                var posts = await _postService.GetPostsByOrganisationIdAsync(vacationRequest.Employee.OrganisationId);
                var model = _mapHelper.Map<VacationRequestDTO, VacationRequestCheckViewModel>(vacationRequest);
                model.EmployeeFullName = $"{vacationRequest.Employee.FirstName} {vacationRequest.Employee.LastName}";
                model.VacationType = vacationRequest.VacationType;
                model.Post = posts.FirstOrDefault(p => p.PostId == vacationRequest.Employee.PostId).Name;
                if (proccessor != null)
                {
                    model.ProccessedByName = $"{proccessor.FirstName} {proccessor.LastName}";
                }
                else
                {
                    model.ProccessedByName = "Данные отсутствуют";
                }
                model.RequestStatus = vacationRequest.RequestStatus;
                if (vacationRequest.Employee.Team != null)
                {
                    model.TeamName = vacationRequest.Employee.Team.Name;
                    model.TeamleadFullName = $"{vacationRequest.Employee.Team.Teamlead.FirstName} {vacationRequest.Employee.Team.Teamlead.LastName}";
                }
                else
                {
                    model.TeamName = emptyValue;
                    model.TeamleadFullName = emptyValue;
                }

                return PartialView("VacationViewModal", model);
            }

            return BadRequest();
        }

        #endregion

        #region Sickleaves

        [HttpGet]
        public async Task<IActionResult> CheckSickleaveRequest(string id)
        {
            var sickLeaveRequest = await _sickLeaveService.GetByIdWithEmployeeWithTeamAsync(id);
            if (sickLeaveRequest != null)
            {
                var model = _mapHelper.Map<SickLeaveRequestDTO, SickLeaveCheckViewModel>(sickLeaveRequest);
                model.Files = await _fileManagmentService.GetSickLeaveFilesAsync(sickLeaveRequest.SickLeaveId);
                var posts = await _postService.GetPostsByOrganisationIdAsync(sickLeaveRequest.Employee.OrganisationId);
                model.EmployeeFullName = $"{sickLeaveRequest.Employee.FirstName} {sickLeaveRequest.Employee.LastName}";
                model.Post = posts.FirstOrDefault(p => p.PostId == sickLeaveRequest.Employee.PostId).Name;
                if (sickLeaveRequest.ProccessedbyId != null)
                {
                    var proccessor = await _employeeService.GetByIdAsync(sickLeaveRequest.ProccessedbyId);
                    model.ProccessedByName = $"{proccessor.FirstName} {proccessor.LastName}";
                }
                if (sickLeaveRequest.Employee.Team != null)
                {
                    model.TeamName = sickLeaveRequest.Employee.Team.Name;
                    model.TeamleadFullName = $"{sickLeaveRequest.Employee.Team.Teamlead.FirstName} {sickLeaveRequest.Employee.Team.Teamlead.LastName}";
                    if (sickLeaveRequest.Employee.Team.Teamlead.WorkEmail == User.Identity.Name && sickLeaveRequest.RequestStatus == RequestStatusEnum.Pending.ToString())
                    {
                        return PartialView("SickleaveProccessModal", model);
                    }
                }
                else
                {
                    model.TeamName = emptyValue;
                    model.TeamleadFullName = emptyValue;
                }

                if (sickLeaveRequest.Employee.Team == null && sickLeaveRequest.RequestStatus == RequestStatusEnum.Pending.ToString())
                {
                    return PartialView("SickleaveProccessModal", model);
                }

                return PartialView("SickleaveViewModal", model);
            }

            return PartialView("SickleaveViewModal");
        }

        [HttpPost]
        public async Task<IActionResult> ProccessSickLeaveRequest(SickLeaveCheckViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            var sickLeaveRequest = await _sickLeaveService.GetByIdAsync(model.SickLeaveId);
            if (sickLeaveRequest != null && sickLeaveRequest.RequestStatus.Equals(RequestStatusEnum.Pending.ToString()))
            {
                try
                {
                    sickLeaveRequest.ProccessedbyId = user.Id;
                    if (model.Result.Equals(RequestStatusEnum.Approved.ToString()))
                    {
                        sickLeaveRequest.RequestStatus = RequestStatusEnum.Approved.ToString();
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = sickLeaveRequest.EmployeeId,
                            OrganisationId = user.OrganisationId,
                            NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            Description = $"Ваш запрос на больничный был подтвердён.",
                            NotificationRange = NotificationRangeEnum.User.ToString()
                        };

                        await _notificationService.CreateAsync(notification);
                    }
                    else
                    {
                        sickLeaveRequest.RequestStatus = RequestStatusEnum.Denied.ToString();
                        var notification = new NotificationDTO
                        {
                            NotificationId = Guid.NewGuid().ToString(),
                            EmployeeId = sickLeaveRequest.EmployeeId,
                            OrganisationId = user.OrganisationId,
                            NotificationType = NotificationTypeEnum.SickLeave.ToString(),
                            NotificationDate = DateTime.UtcNow,
                            Description = $"Ваш запрос на больничный был отклонён.",
                            NotificationRange = NotificationRangeEnum.User.ToString()
                        };

                        await _notificationService.CreateAsync(notification);
                    }

                    await _sickLeaveService.UpdateAsync(sickLeaveRequest, true);
                }
                catch (DbUpdateConcurrencyException e)
                {
                    return BadRequest(e.Message);
                }
            }

            return RedirectToAction("Sickleaves", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> Sickleaves(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var sickLeaves = new SickLeaveViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                sickLeaves = await _sickLeaveService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                  req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.State);
                count = await _sickLeaveService.GetSickLeavesNumber(searchKey, req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.State);
            }

            var pagedSickLeaves = new SickLeaveListViewModel()
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                SickLeaves = _mapHelper.MapCollection<SickLeaveViewDTO, SickLeaveRequestViewModel>(sickLeaves)
            };

            return View(pagedSickLeaves);
        }

        [HttpGet]
        public async Task<IActionResult> SickleavesPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var sickLeaves = new SickLeaveViewDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                sickLeaves = await _sickLeaveService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, searchKey,
                                                                  req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.State);
                count = await _sickLeaveService.GetSickLeavesNumber(searchKey, req => req.Employee.OrganisationId == employee.OrganisationId && req.Employee.State);
            }

            var pagedSickLeaves = new SickLeaveListViewModel()
            {
                Count = count,
                PageNumber = pageNumber,
                PageSize = (int)PageSizeEnum.PageSize15,
                SickLeaves = _mapHelper.MapCollection<SickLeaveViewDTO, SickLeaveRequestViewModel>(sickLeaves)
            };

            return PartialView(pagedSickLeaves);
        }

        #endregion

        #region Assignments

        [HttpGet]
        public async Task<IActionResult> Assignments(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var assignments = new AssignmentListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                assignments = await _assignmentService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, a => a.OrganisationId == employee.OrganisationId, searchKey);
                count = await _assignmentService.GetAssignmentsCountAsync(searchKey, a => a.OrganisationId == employee.OrganisationId);
            }

            var pagedAssignments = new AssignmentListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Assignments = _mapHelper.MapCollection<AssignmentListUnitDTO, AssignmentViewModel>(assignments)
            };

            return View(pagedAssignments);
        }

        [HttpGet]
        public async Task<IActionResult> AssignmentsPage(int pageNumber = 0, string searchKey = null)
        {
            ViewData["SearchKey"] = searchKey;
            int count = 0;
            var assignments = new AssignmentListUnitDTO[] { };
            var employee = await _employeeService.GetByEmailAsync(User.Identity.Name);
            if (employee != null)
            {
                assignments = await _assignmentService.GetPageAsync(pageNumber, (int)PageSizeEnum.PageSize15, a => a.OrganisationId == employee.OrganisationId, searchKey);
                count = await _assignmentService.GetAssignmentsCountAsync(searchKey, a => a.OrganisationId == employee.OrganisationId);
            }

            var pagedAssignments = new AssignmentListViewModel()
            {
                PageNumber = pageNumber,
                Count = count,
                PageSize = (int)PageSizeEnum.PageSize15,
                Assignments = _mapHelper.MapCollection<AssignmentListUnitDTO, AssignmentViewModel>(assignments)
            };

            return PartialView(pagedAssignments);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAssignment(string id)
        {
            var assignment = await _assignmentService.GetByIdWithEmployeesAsync(id);
            if (assignment != null)
            {
                var model = _mapHelper.Map<AssignmentDTO, AssignmentViewModel>(assignment);

                return PartialView("AssignmentDeleteApproveModal", model);
            }

            return RedirectToAction("Assignments", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAssignment(AssignmentViewModel model)
        {
            var assignment = await _assignmentService.GetByIdWithEmployeesAsync(model.AssignmentId);
            if (assignment != null)
            {
                var employeeIds = assignment.AssignmentEmployee.Select(ae => ae.EmployeeId).ToArray();
                var residuals = assignment.AssignmentEmployee.Select(ae => ae.Employee.EmployeeBalanceResiduals.
                                                                                       FirstOrDefault(r => employeeIds.Any(m => m == r.EmployeeId)
                                                                                       && r.Name == ResidualTypeEnum.Assignment.ToString())).Where(r => r != null).ToArray();
                foreach (var res in residuals)
                {
                    res.ResidualBalance -= assignment.Duration;
                }

                await _residualsService.UpdateRangeAsync(residuals);

                await _assignmentService.RemoveFromAssignmentAsync(employeeIds, assignment.AssignmentId);

                await _assignmentService.DeleteAsync(assignment, true);
            }

            return RedirectToAction("Assignments", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTeam(string id)
        {
            var team = await _teamService.GetByIdAsync(id);
            if (team != null)
            {
                var model = _mapHelper.Map<TeamDTO, TeamViewModel>(team);

                return PartialView("TeamDeleteApproveModal", model);
            }

            return RedirectToAction("Teams", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTeam(TeamViewModel model)
        {
            var team = await _teamService.GetByIdAsync(model.TeamId);
            if (team != null)
            {
                await _employeeService.RemoveFromTeamAsync(team.Employees.Select(emp => emp.EmployeeId).ToArray());

                await _teamService.DeleteAsync(team, true);
            }

            return RedirectToAction("Teams", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> AddTeam(string id = null)
        {
            EmployeeDTO creator;
            if (id == null)
            {
                creator = await _employeeService.GetByEmailWithTeamAsync(User.Identity.Name);
            }
            else
            {
                creator = await _employeeService.GetByIdWithTeamAsync(id);
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

            await _employeeService.UpdateRangeAsync(employees, true);

            var user = await _userManager.FindByIdAsync(model.TeamleadId);
            await _userManager.AddToRoleAsync(user, RoleEnum.Teamlead.ToString());
            return RedirectToAction("Teams", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> EditAssignment(string id)
        {
            var assignment = await _assignmentService.GetByIdWithEmployeesAsync(id);
            if (assignment != null)
            {
                var employees = await _employeeService.GetAsync(emp => emp.OrganisationId == assignment.OrganisationId);
                var assignmentEmployees = assignment.AssignmentEmployee.Select(a => a.Employee.EmployeeId).ToArray();
                var model = new EditAssignmentViewModel
                {
                    AssignmentId = assignment.AssignmentId,
                    Name = assignment.Name,
                    Duration = assignment.Duration,
                    BeginDate = assignment.BeginDate,
                    EndDate = assignment.EndDate,
                    OrganisationId = assignment.OrganisationId,
                    Employees = _listMapper.CreateSelectedEmployeesList(employees, assignmentEmployees)
                };

                return PartialView("EditAssignmentModal", model);
            }

            return RedirectToAction("Assignments", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> EditAssignment(EditAssignmentViewModel model)
        {
            var assignment = await _assignmentService.GetByIdWithEmployeesAsync(model.AssignmentId);
            if (assignment != null)
            {
                if (model.AssignmentMembers != null && model.AssignmentMembers.Count() != 0)
                {
                    var assignmentEmployees = assignment.AssignmentEmployee.Select(a => a.Employee.EmployeeId).ToArray();
                    var removedFromAssignment = assignmentEmployees.Except(model.AssignmentMembers).ToArray();
                    var addedToAssignment = model.AssignmentMembers.Except(assignmentEmployees).ToArray();
                    if (removedFromAssignment != null && removedFromAssignment.Count() != 0)
                    {
                        var residuals = assignment.AssignmentEmployee.Select(ae => ae.Employee.EmployeeBalanceResiduals.
                                                                                         FirstOrDefault(r => removedFromAssignment.Any(m => m == r.EmployeeId)
                                                                                            && r.Name == ResidualTypeEnum.Assignment.ToString())).Where(r => r != null).ToArray();
                        foreach (var res in residuals)
                        {
                            res.ResidualBalance -= assignment.Duration;
                        }

                        await _residualsService.UpdateRangeAsync(residuals);
                        await _assignmentService.RemoveFromAssignmentAsync(removedFromAssignment, assignment.AssignmentId);
                    }

                    if (assignment.Duration != model.Duration)
                    {
                        var residuals = assignment.AssignmentEmployee.Select(ae => ae.Employee.EmployeeBalanceResiduals.
                                                                                       FirstOrDefault(r => removedFromAssignment.All(m => m != r.EmployeeId)
                                                                                                           && r.Name == ResidualTypeEnum.Assignment.ToString())).Where(r => r != null).ToArray();
                        foreach (var res in residuals)
                        {
                            res.ResidualBalance += model.Duration - assignment.Duration;
                        }

                        await _residualsService.UpdateRangeAsync(residuals);
                    }
                    assignment.Name = model.Name;
                    assignment.Duration = model.Duration;
                    assignment.BeginDate = model.BeginDate;
                    assignment.EndDate = model.EndDate;
                    if (addedToAssignment != null && addedToAssignment.Count() != 0)
                    {
                        var employees = await _employeeService.GetWithTeam(emp => addedToAssignment.Any(m => m == emp.EmployeeId));
                        var residuals = await _residualsService.GetAsync(res => addedToAssignment.Any(m => m == res.EmployeeId) && res.Name == ResidualTypeEnum.Assignment.ToString());
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
                                Description = "Вы были отмечены как участник командировки.",
                                NotificationRange = NotificationRangeEnum.User.ToString()
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
                                    Description = $"Ваш работник, {employee.FirstName} {employee.LastName}, был отмечен как участник командировки.",
                                    NotificationRange = NotificationRangeEnum.Organisation.ToString()
                                });
                            }
                        }

                        await _assignmentService.AddToAssignmentAsync(addedToAssignment, assignment.AssignmentId);
                        await _notificationService.CreateRangeAsync(notificationList.ToArray());
                    }

                    await _assignmentService.Update(assignment, true);
                }
            }

            return RedirectToAction("Assignments", "Admin");
        }

        [HttpGet]
        public async Task<IActionResult> AddAssignment(string id = null)
        {
            EmployeeDTO creator;
            if (id == null)
            {
                creator = await _employeeService.GetByEmailWithTeamAsync(User.Identity.Name);
            }
            else
            {
                creator = await _employeeService.GetByIdWithTeamAsync(id);
            }

            if (creator != null)
            {
                var employees = await _employeeService.GetAsync(emp => emp.OrganisationId == creator.OrganisationId);

                var model = new AddAssignmentViewModel
                {
                    Employees = _listMapper.CreateEmployeesList(employees),
                    OrganisationId = creator.OrganisationId
                };

                return PartialView("AddAssignmentModal", model);
            }

            return RedirectToAction("Assignments", "Admin");
        }

        [HttpPost]
        public async Task<IActionResult> AddAssignment(AddAssignmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var assignment = new AssignmentDTO
                {
                    AssignmentId = Guid.NewGuid().ToString(),
                    BeginDate = model.BeginDate,
                    EndDate = model.EndDate,
                    Duration = model.EndDate.DayOfYear - model.BeginDate.DayOfYear,
                    Name = model.Name,
                    OrganisationId = model.OrganisationId,
                    CreateDate = DateTime.UtcNow.Date
                };
                assignment.AssignmentEmployee = model.AssignmentMembers.Select(code => new AssignmentEmployeeDTO
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
                        Description = "Вы были отмечены как участник командировки.",
                        NotificationRange = NotificationRangeEnum.User.ToString()
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
                            Description = $"Ваш работник, {employee.FirstName} {employee.LastName}, был отмечен как участник командировки.",
                            NotificationRange = NotificationRangeEnum.Organisation.ToString()
                        });
                    }
                }

                await _notificationService.CreateRangeAsync(notificationList.ToArray(), true);
            }

            return RedirectToAction("Assignments", "Admin");
        }

        #endregion
    }
}