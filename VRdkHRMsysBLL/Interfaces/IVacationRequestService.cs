using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IVacationService
    {
        Task<VacationRequestDTO> GetByIdAsync(string id);      
        Task<VacationRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<VacationRequestViewDTO[]> GetForProccessAsync(Expression<Func<VacationRequest, bool>> condition = null);
        Task<VacationRequestDTO[]> GetAsync(Expression<Func<VacationRequest, bool>> condition = null);
        Task UpdateAsync(VacationRequestDTO newRequest);
        Task CreateAsync(VacationRequestDTO request);
    }
}