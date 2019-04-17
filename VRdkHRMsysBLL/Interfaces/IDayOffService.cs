using System;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IDayOffService
    {
        Task CreateAsync(DayOffDTO dayOff, bool writeChanges = false);
        Task<DayOffDTO> GetByIdAsync(string id);
        Task DeleteAsync(string id, bool writeChanges = false);
        Task UpdateAsync(DayOffDTO dayOff, bool writeChanges = false);
        Task<DayOffDTO> GetByDateAsync(DateTime date, string employeeId);
    }
}