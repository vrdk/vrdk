using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Absence;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IAbsenceService
    {
        Task<AbsenceDTO> GetByEmployeeIdAsync(string id);
        Task CreateAsync(AbsenceDTO absence);
    }
}