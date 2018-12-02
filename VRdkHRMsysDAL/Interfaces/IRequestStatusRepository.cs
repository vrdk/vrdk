using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IRequestStatusRepository : IRepository<RequestStatus>
    {
        Task<RequestStatus> GetByNameAsync(string name);
    }
}
