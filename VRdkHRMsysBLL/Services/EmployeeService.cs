using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.BalanceResiduals;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Team;
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

        public async Task<EmployeeDTO[]> GetWithTeam(Expression<Func<Employee, bool>> condition = null)
        {
            var employees = await _employeeRepository.GetWithTeamAsync(condition);
            return _mapHelper.MapCollection<Employee, EmployeeDTO>(employees);
        }

        public async Task<EmployeeDTO> GetByEmailAsync(string email)
        {
            var employee = await _employeeRepository.GetByEmailAsync(email);
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
        public async Task UpdateRangeAsync(EmployeeDTO[] newEmployees)
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

                await _employeeRepository.UpdateAsync();
            }
        }
        public async Task UpdateAsync(EmployeeDTO newEmployee)
        {
            var currentEmployee = await _employeeRepository.GetByIdWithResidualsAsync(newEmployee.EmployeeId);
            if(currentEmployee != null)
            {
                _mapHelper.MapChanges(newEmployee, currentEmployee);    
                await _employeeRepository.UpdateAsync();
            }         
        }

        public async Task CreateAsync(EmployeeDTO employee)
        {
            var employeeToAdd = _mapHelper.NestedMap<EmployeeDTO, Employee, BalanceResidualsDTO, EmployeeBalanceResiduals>(employee);
            await _employeeRepository.CreateAsync(employeeToAdd);
        }
    }
}
