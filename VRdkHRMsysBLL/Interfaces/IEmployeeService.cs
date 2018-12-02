using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeDTO> GetByIdAsync(string id);
        Task<EmployeeDTO> GetByEmailAsync(string email);
        Task<EmployeeDTO> GetByIdWithTeamAsync(string id);
        Task<EmployeeDTO> GetByEmailWithTeamAsync(string email);
        Task CreateAsync(EmployeeDTO employee);
        Task UpdateAsync(EmployeeDTO employee);   
    }
}