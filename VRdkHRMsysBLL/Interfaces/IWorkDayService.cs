using System;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IWorkDayService
    {
        Task CreateAsync(WorkDayDTO workDay, bool writeChanges = false);
        Task<WorkDayDTO> GetByIdAsync(string id);
        Task DeleteAsync(string id, bool writeChanges = false);
        Task UpdateAsync(WorkDayDTO workDay, bool writeChanges = false);
        Task<WorkDayDTO> GetByDateAsync(DateTime date, string employeeId);
    }
}