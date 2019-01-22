using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.DayOff;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IDayOffService
    {
        Task CreateAsync(DayOffDTO dayOff);
        Task<DayOffDTO> GetByIdAsync(string id);
        Task DeleteAsync(string id);
    }
}