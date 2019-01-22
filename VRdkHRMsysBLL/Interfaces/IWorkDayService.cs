using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.WorkDay;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IWorkDayService
    {
        Task CreateAsync(WorkDayDTO workDay);
        Task<WorkDayDTO> GetByIdAsync(string id);
        Task DeleteAsync(string id);
        Task UpdateAsync(WorkDayDTO workDay);
    }
}