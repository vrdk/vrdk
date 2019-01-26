using System;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IDayOffRepository : IRepository<DayOff>
    {
        Task<DayOff> GetByDateAsync(DateTime date, string employeeId);
    }
}
