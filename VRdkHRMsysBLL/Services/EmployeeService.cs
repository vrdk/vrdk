using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IPostRepository _postsRepository;
        private readonly IMapHelper _mapHelper;

        public EmployeeService(IEmployeeRepository employeeRepository,
                               IPostRepository postRepository,
                               IMapHelper mapHelper)
        {
            _employeeRepository = employeeRepository;
            _postsRepository = postRepository;
            _mapHelper = mapHelper;
        }

        public async Task<EmployeeDTO[]> GetAsync(Expression<Func<Employee, bool>> condition = null)
        {
            var employees = await _employeeRepository.GetAsync(condition);
            return _mapHelper.MapCollection<Employee, EmployeeDTO>(employees);
        }

        public async Task<EmployeeDTO[]> GetForCalendaAsync(string teamId, string teamleadId, int month, int year, string personalId = null)
        {
            var employees = await _employeeRepository.GetForCalendarAsync(teamId, teamleadId, month, year, personalId);
            return _mapHelper.MapCollection<Employee, EmployeeDTO>(employees);
        }

        public async Task<EmployeeDTO[]> GetWithTeam(Expression<Func<Employee, bool>> condition = null)
        {
            var employees = await _employeeRepository.GetWithTeamAsync(condition);
            return _mapHelper.MapCollection<Employee, EmployeeDTO>(employees);
        }

        public async Task<int> GetEmployeesCountAsync(string searchKey = null, Expression<Func<Employee, bool>> condition = null)
        {
            return await _employeeRepository.GetEmployeesCount(condition, searchKey);
        }

        public async Task<EmployeeListUnitDTO[]> GetPageAsync(int pageNumber,int pageSize, string searchKey, Expression<Func<Employee, bool>> condition = null)
        {
            var emps = await _employeeRepository.GetPageAsync(pageNumber, pageSize, searchKey, condition);
            var employees = emps != null ? emps.Select(emp => new EmployeeListUnitDTO()
            {
                EmployeeId = emp.EmployeeId,
                TeamId = emp.Team?.TeamId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                TeamName = emp.Team?.Name,
                AbsenceBalance = emp.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Absence.ToString()).ResidualBalance,
                AssignmentBalance = emp.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Assignment.ToString()).ResidualBalance,
                PaidVacationBalance = emp.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Paid_vacation.ToString()).ResidualBalance,
                UnpaidVacationBalance = emp.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Unpaid_vacation.ToString()).ResidualBalance,
                SickLeaveBalance = emp.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance
            }).ToArray() : new EmployeeListUnitDTO[] {};

            return employees;
        }


        public async Task<EmployeeDTO> GetByEmailAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
            return _mapHelper.Map<Employee, EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetByIdWithTeamWithResidualsAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdWithTeamWithResidualsAsync(id);
            return _mapHelper.Map<Employee, EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetByEmailWithTeamWithResidualsAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailWithTeamWithResidualsAsync(email);
            return _mapHelper.Map<Employee, EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetByEmailWithTeamAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailWithTeamAsync(email);
            return _mapHelper.Map<Employee, EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetByIdAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            return _mapHelper.Map<Employee, EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetByIdWithResidualsAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdWithResidualsAsync(id);
            return _mapHelper.Map<Employee, EmployeeDTO>(employee);
        }

        public async Task<EmployeeDTO> GetByIdWithTeamAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdWithTeamAsync(id);
            return _mapHelper.NestedMap<Employee, EmployeeDTO, Team, TeamDTO>(employee);
        }

        public async Task RemoveFromTeamAsync(string[] membersIds, bool writeChanges = false)
        {
            var employees = await _employeeRepository.GetAsync(e => membersIds.Any(m => m == e.EmployeeId));

            foreach(var employee in employees)
            {
                employee.TeamId = null;
            }

            if (writeChanges)
            {
                await _employeeRepository.UpdateAsync();
            }           
        }

        public async Task AddToTeamAsync(string[] membersIds, string teamId, bool writeChanges = false)
        {
            var employees = await _employeeRepository.GetAsync(e => membersIds.Any(m => m == e.EmployeeId));

            foreach (var employee in employees)
            {
                employee.TeamId = teamId;
            }

            if (writeChanges)
            {
                await _employeeRepository.UpdateAsync();
            }        
        }

        public async Task UpdateRangeAsync(EmployeeDTO[] newEmployees, bool writeChanges = false)
        {
            var currentEmployees = await _employeeRepository.GetAsync(emp => newEmployees.Any(e=>emp.EmployeeId==e.EmployeeId));
            if (currentEmployees != null)
            {
                newEmployees.OrderBy(emp => emp.EmployeeId);
                currentEmployees.OrderBy(emp => emp.EmployeeId);
                for (int i = 0; i < currentEmployees.Length; i++)
                {
                    _mapHelper.MapChanges(newEmployees[i],currentEmployees[i]);
                }

                if (writeChanges)
                {
                    await _employeeRepository.UpdateAsync();
                }                
            }
        }

        public async Task UpdateAsync(EmployeeDTO newEmployee, bool writeChanges = false)
        {
            var currentEmployee = await _employeeRepository.GetByIdAsync(newEmployee.EmployeeId);
            if(currentEmployee != null)
            {
                currentEmployee.FirstName = newEmployee.FirstName;
                currentEmployee.LastName = newEmployee.LastName;
                currentEmployee.State = newEmployee.State;
                currentEmployee.BirthDate = newEmployee.BirthDate;
                currentEmployee.DismissalDate = newEmployee.DismissalDate;
                currentEmployee.HireDate = newEmployee.HireDate;
                currentEmployee.PersonalEmail = newEmployee.PersonalEmail;
                currentEmployee.WorkEmail = newEmployee.WorkEmail;
                currentEmployee.PostId = newEmployee.PostId;
                currentEmployee.PhoneNumber = newEmployee.PhoneNumber;
                foreach(var res in currentEmployee.EmployeeBalanceResiduals)
                {
                    foreach(var newRes in newEmployee.EmployeeBalanceResiduals)
                    {
                        if (res.Name == newRes.Name)
                        {
                            res.ResidualBalance = newRes.ResidualBalance;
                        }                    
                    }
                }

                if (writeChanges)
                {
                    await _employeeRepository.UpdateEmployeeAsync(currentEmployee, writeChanges);
                }              
            }         
        }

        public async Task CreateAsync(EmployeeDTO employee, bool writeChanges = false)
        {
            var employeeToAdd = _mapHelper.Map<EmployeeDTO, Employee>(employee);

            await _employeeRepository.CreateAsync(employeeToAdd, writeChanges);
        }
    }
}
