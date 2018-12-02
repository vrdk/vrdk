using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IVacationRequestRepository : IRepository<VacationRequest>
    {
        Task<VacationRequest> GetByIdWithEmployeeWithTeamAsync(string id);
    }
}
