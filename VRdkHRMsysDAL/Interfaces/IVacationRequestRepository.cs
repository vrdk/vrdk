using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IVacationRequestRepository : IRepository<VacationRequest>
    {
        Task<VacationRequest> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<VacationRequest[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null);
        Task<int> GetVacationsCount(Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null);
        Task<VacationRequest[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
    }
}
