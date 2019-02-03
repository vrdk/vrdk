using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface ITimeManagementRecordRepository : IRepository<TimeManagementRecord>
    {
        Task UpdateAsync(TimeManagementRecord entity, bool writeChanges);
    }
}
