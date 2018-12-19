using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IEmployeeBalanceResidualsRepository : IRepository<EmployeeBalanceResiduals>
    {
        Task<EmployeeBalanceResiduals> GetByEmployeeIdAsync(string id, string type);
        Task CreateRangeAsync(EmployeeBalanceResiduals[] entities);
    }
}
