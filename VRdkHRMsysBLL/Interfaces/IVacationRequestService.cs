using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.StatusType;
using VRdkHRMsysBLL.DTOs.Vacation;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IVacationRequestService
    {
        Task<VacationRequestDTO> GetByIdAsync(string id);
        Task CreateVacationRequestAsync(VacationRequestDTO request);
        Task<VacationRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<VacationRequestDTO[]> GetProccessingVacationRequestsAsync(string organisationId, string teamId);       
        Task<VacationTypeDTO[]> GetVacationTypesAsync();
        Task<RequestStatusDTO> GetRequestStatusByNameAsync(string name);
        Task<RequestStatusDTO[]> GetRequestStatusesAsync();
        Task UpdateAsync(VacationRequestDTO newRequest);
    }
}