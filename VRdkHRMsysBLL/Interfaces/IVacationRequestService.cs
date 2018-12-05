using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Vacation;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IVacationService
    {
        Task<VacationRequestDTO> GetByIdAsync(string id);      
        Task<VacationRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<VacationRequestViewDTO[]> GetProccessingVacationRequestsAsync(string organisationId, string teamId);       
        Task UpdateAsync(VacationRequestDTO newRequest);
        Task CreateAsync(VacationRequestDTO request);
    }
}