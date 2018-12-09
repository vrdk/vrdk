using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface ISickLeaveRequestRepository : IRepository<SickLeaveRequest>
    {
        Task<SickLeaveRequest> GetByIdWithEmployeeWithTeamAsync(string id);
    }
}
