using System;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IWorkDayRepository : IRepository<WorkDay>
    {
        Task<WorkDay> GetByDateAsync(DateTime date, string employeeId);
    }
}
