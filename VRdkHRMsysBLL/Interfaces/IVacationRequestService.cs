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
        Task<int> GetVacationsNumberAsync(string searchKey = null, Expression < Func<VacationRequest, bool>> condition = null);
        Task<VacationRequestViewDTO[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, string searchKey = null, Expression<Func<VacationRequest, bool>> condition = null);
        Task<VacationRequestDTO[]> GetAsync(Expression<Func<VacationRequest, bool>> condition = null);
        Task<VacationRequestDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
        Task UpdateAsync(VacationRequestDTO newRequest);
        Task CreateAsync(VacationRequestDTO request);
    }
}