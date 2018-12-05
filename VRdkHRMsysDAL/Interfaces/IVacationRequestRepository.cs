using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IVacationRequestRepository : IRepository<VacationRequest>
    {
        Task<VacationRequest> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<VacationRequest[]> GetWithEmployeeWithTeamAsync(Expression<Func<VacationRequest, bool>> condition = null);
    }
}
