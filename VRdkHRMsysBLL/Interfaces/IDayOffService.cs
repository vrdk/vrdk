using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.DayOff;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IDayOffService
    {
        Task CreateAsync(DayOffDTO dayOff);
    }
}