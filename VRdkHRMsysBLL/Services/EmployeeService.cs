using System;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
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

        public async Task<EmployeeDTO> GetByIdWithTeamAsync(string id)
        {
            var employee = await _employeeRepository.GetByIdWithTeamAsync(id);
            return _mapHelper.NestedMap<Employee, EmployeeDTO, Team, TeamDTO>(employee);
        }

        public async Task UpdateAsync(EmployeeDTO newEmployee)
        {
            var currentEmployee = await _employeeRepository.GetByIdAsync(newEmployee.EmployeeId);
            if(currentEmployee != null)
            {
                _mapHelper.MapChanges(newEmployee, currentEmployee);             
                await _employeeRepository.UpdateAsync();
            }         
        }

        public async Task CreateAsync(EmployeeDTO employee)
        {
            employee.EmployeeBalanceResiduals = new BalanceResidualsDTO[]
            {
                new BalanceResidualsDTO
                {
                    ResidualId = Guid.NewGuid().ToString(),
                    EmployeeId = employee.EmployeeId,
                    Name = "UnpaidVacation",
                    ResidualBalance = 28
                },
                 new BalanceResidualsDTO
                {
                    ResidualId = Guid.NewGuid().ToString(),
                    EmployeeId = employee.EmployeeId,
                    Name = "PaidVacation",
                    ResidualBalance = 28
                }
            };
            var employeeToAdd = _mapHelper.NestedMap<EmployeeDTO, Employee, BalanceResidualsDTO, EmployeeBalanceResiduals>(employee);
            await _employeeRepository.CreateAsync(employeeToAdd);
        }
    }
}
