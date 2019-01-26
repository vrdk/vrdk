using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Absence;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IAbsenceService
    {
        Task<AbsenceDTO> GetTodayByEmployeeIdAsync(string id);
        Task CreateAsync(AbsenceDTO absence, bool writeChanges = false);
    }
}