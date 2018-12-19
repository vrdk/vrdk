using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.SickLeave;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class SickLeaveService : ISickLeaveService
    {
        private readonly ISickLeaveRequestRepository _sickLeaveRepository;
        private readonly IMapHelper _mapHelper;

        public SickLeaveService(ISickLeaveRequestRepository sickLeaveRepository,
                                  IMapHelper mapHelper)
        {
            _sickLeaveRepository = sickLeaveRepository;
            _mapHelper = mapHelper;
        }

        public async Task<SickLeaveRequestDTO[]> GetAsync(Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            var requests = await _sickLeaveRepository.GetAsync(condition);
            return _mapHelper.MapCollection<SickLeaveRequest, SickLeaveRequestDTO>(requests);
        }

        public async Task<SickLeaveRequestDTO> GetByIdAsync(string id)
        {
            var request = await _sickLeaveRepository.GetByIdAsync(id);
            return _mapHelper.Map<SickLeaveRequest, SickLeaveRequestDTO>(request);
        }

        public async Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            var request = await _sickLeaveRepository.GetByIdWithEmployeeWithTeamAsync(id);
            return _mapHelper.NestedMap<SickLeaveRequest, SickLeaveRequestDTO, Employee, EmployeeDTO, Team, TeamDTO>(request);
        }

        public async Task CreateAsync(SickLeaveRequestDTO SickLeave)
        {
            var entity = _mapHelper.Map<SickLeaveRequestDTO, SickLeaveRequest>(SickLeave);
            await _sickLeaveRepository.CreateAsync(entity);
        }

        public async Task UpdateAsync(SickLeaveRequestDTO newRequest)
        {
            var currentRequest = await _sickLeaveRepository.GetByIdAsync(newRequest.SickLeaveId);
            if (currentRequest != null)
            {
                _mapHelper.MapChanges(newRequest, currentRequest);
                await _sickLeaveRepository.UpdateAsync();
            }
        }
    }
}
