using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.SickLeave;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ISickLeaveService
    {
        Task CreateAsync(SickLeaveDTO SickLeave);
    }
}