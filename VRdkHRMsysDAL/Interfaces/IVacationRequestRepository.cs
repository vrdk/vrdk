using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IVacationRequestRepository : IRepository<VacationRequest>
    {
        Task<VacationRequest> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<VacationRequest[]> GetPageWithPendingPriorityAsync(int pageNumber, int pageSize, Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null);
        Task<VacationRequest[]> GetPageWithProccessingPriorityAsync(int pageNumber, int pageSize, Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null);
        Task<int> GetVacationsCountAsync(Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null);
        Task<VacationRequest[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
    }
}
