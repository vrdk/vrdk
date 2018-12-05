using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class SickLeaveService : ISickLeaveService
    {
        private readonly ISickLeaveRequestRepository _SickLeaveRepository;
        private readonly IMapHelper _mapHelper;

        public SickLeaveService(ISickLeaveRequestRepository SickLeaveRepository,
                                  IMapHelper mapHelper)
        {
            _SickLeaveRepository = SickLeaveRepository;
            _mapHelper = mapHelper;
        }

        public async Task CreateAsync(SickLeaveDTO SickLeave)
        {
            var entity = _mapHelper.Map<SickLeaveDTO, SickLeaveRequest>(SickLeave);
            await _SickLeaveRepository.CreateAsync(entity);
        }
    }
}
