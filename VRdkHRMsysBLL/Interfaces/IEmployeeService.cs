using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> GetByIdAsync(string id);
        Task<EmployeeDTO> GetByEmailAsync(string email);
        Task<EmployeeDTO> GetByIdWithTeamAsync(string id);
        Task<EmployeeDTO> GetByEmailWithTeamAsync(string email);
        Task<EmployeeDTO> GetByIdWithResidualsAsync(string id);
        Task<EmployeeDTO[]> GetAsync(Expression<Func<Employee, bool>> condition = null);
        Task<EmployeeDTO[]> GetWithTeam(Expression<Func<Employee, bool>> condition = null);
        Task CreateAsync(EmployeeDTO employee);
        Task UpdateAsync(EmployeeDTO employee);
        Task UpdateRangeAsync(EmployeeDTO[] newEmployees);
    }
}