using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface ISickLeaveRequestRepository : IRepository<SickLeaveRequest>
    {
        Task<SickLeaveRequest> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<SickLeaveRequest[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<int> GetSickLeavesNumberAsync(string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null);
    }
}
