using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.SickLeave;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ISickLeaveService
    {
        Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<SickLeaveRequestDTO> GetByIdAsync(string id);
        Task CreateAsync(SickLeaveRequestDTO SickLeave);
        Task UpdateAsync(SickLeaveRequestDTO newRequest);
    }
}