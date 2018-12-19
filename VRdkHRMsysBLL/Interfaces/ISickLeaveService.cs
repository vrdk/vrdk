using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ISickLeaveService
    {
        Task<SickLeaveRequestDTO[]> GetAsync(Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<SickLeaveRequestDTO> GetByIdAsync(string id);
        Task CreateAsync(SickLeaveRequestDTO SickLeave);
        Task UpdateAsync(SickLeaveRequestDTO newRequest);
    }
}