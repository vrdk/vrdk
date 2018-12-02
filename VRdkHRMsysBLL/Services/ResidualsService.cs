using System;
using System.Collections.Generic;
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

        public async Task<BalanceResidualsDTO> GetByEmployeeIdAsync(string id, string type)
        {
          var residual = await _residualsRepository.GetByEmployeeIdAsync(id,type);
          return _mapHelper.Map<EmployeeBalanceResiduals, BalanceResidualsDTO>(residual);
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
