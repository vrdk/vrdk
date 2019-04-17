using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IResidualsService
    {
        Task<BalanceResidualsDTO[]> GetAsync(Expression<Func<EmployeeBalanceResiduals, bool>> condition = null);
        Task<BalanceResidualsDTO> GetByEmployeeIdAsync(string id, string type);
        Task UpdateAsync(BalanceResidualsDTO newResidual, bool writeChanges = false);
        Task UpdateRangeAsync(BalanceResidualsDTO[] newResidual, bool writeChanges = false);
        Task CreateRangeAsync(BalanceResidualsDTO[] residuals, bool writeChanges = false);
    }
}