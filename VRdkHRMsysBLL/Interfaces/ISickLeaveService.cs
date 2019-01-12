using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ISickLeaveService
    {
        Task<SickLeaveViewDTO[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<int> GetSickLeavesNumber(string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<SickLeaveRequestDTO[]> GetAsync(Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithResidualsAsync(string id);
        Task<SickLeaveRequestDTO> GetByIdAsync(string id);
        Task CreateAsync(SickLeaveRequestDTO SickLeave);
        Task UpdateAsync(SickLeaveRequestDTO newRequest);
        Task<SickLeaveRequestDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
    }
}