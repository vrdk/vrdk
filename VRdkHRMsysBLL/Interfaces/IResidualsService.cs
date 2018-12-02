using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.BalanceResiduals;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IResidualsService
    {
        Task<BalanceResidualsDTO> GetByEmployeeIdAsync(string id, string type);
        Task UpdateAsync(BalanceResidualsDTO newResidual);
    }
}