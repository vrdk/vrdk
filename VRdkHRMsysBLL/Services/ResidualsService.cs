using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.BalanceResiduals;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class ResidualsService : IResidualsService
    {
        private readonly IEmployeeBalanceResidualsRepository _residualsRepository;
        private readonly IMapHelper _mapHelper;

        public ResidualsService(IEmployeeBalanceResidualsRepository residualsRepository,
                                IMapHelper mapHelper)
        {
            _residualsRepository = residualsRepository;
            _mapHelper = mapHelper;
        }

        public async Task<BalanceResidualsDTO[]> GetAsync(Expression<Func<EmployeeBalanceResiduals, bool>> condition = null)
        {
            var residuals = await _residualsRepository.GetAsync(condition);
            return _mapHelper.MapCollection<EmployeeBalanceResiduals, BalanceResidualsDTO>(residuals);
        }

        public async Task<BalanceResidualsDTO> GetByEmployeeIdAsync(string id, string type)
        {
          var residual = await _residualsRepository.GetByEmployeeIdAsync(id,type);
          return _mapHelper.Map<EmployeeBalanceResiduals, BalanceResidualsDTO>(residual);
        }

        public async Task UpdateRangeAsync(BalanceResidualsDTO[] newResiduals)
        {
            var currentResiduals = await _residualsRepository.GetAsync(res => newResiduals.Any(r => res.ResidualId == r.ResidualId));            
            if (currentResiduals != null)
            {
                newResiduals.OrderBy(r => r.ResidualId);
                currentResiduals.OrderBy(r => r.ResidualId);
                for (int i = 0; i < currentResiduals.Length; i++)
                {
                    _mapHelper.MapChanges(newResiduals[i], currentResiduals[i]);
                }

                await _residualsRepository.UpdateAsync();
            }
        }

        public async Task CreateRangeAsync(BalanceResidualsDTO[] residuals)
        {
            var residualsToAdd = _mapHelper.MapCollection<BalanceResidualsDTO, EmployeeBalanceResiduals>(residuals);
            await _residualsRepository.CreateRangeAsync(residualsToAdd);
        }
        public async Task UpdateAsync(BalanceResidualsDTO newResidual)
        {
            var currentRequest = await _residualsRepository.GetByIdAsync(newResidual.ResidualId);
            if (currentRequest != null)
            {
                _mapHelper.MapChanges(newResidual, currentRequest);
                await _residualsRepository.UpdateAsync();
            }
        }
    }
}
