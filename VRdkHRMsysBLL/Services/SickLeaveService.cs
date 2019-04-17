using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class SickLeaveService : ISickLeaveService
    {
        private const string emptyValue = "Нет";
        private readonly ISickLeaveRequestRepository _sickLeaveRepository;
        private readonly IMapHelper _mapHelper;

        public SickLeaveService(ISickLeaveRequestRepository sickLeaveRepository,
                                  IMapHelper mapHelper)
        {
            _sickLeaveRepository = sickLeaveRepository;
            _mapHelper = mapHelper;
        }

        public async Task<SickLeaveRequestDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0)
        {
            var requests = await _sickLeaveRepository.GetProfilePageAsync(pageSize, id, pageNumber);
            return _mapHelper.MapCollection<SickLeaveRequest, SickLeaveRequestDTO>(requests);
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

        public async Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithResidualsAsync(string id)
        {
            var request = await _sickLeaveRepository.GetByIdWithEmployeeWithResidualsAsync(id);
            return _mapHelper.Map<SickLeaveRequest, SickLeaveRequestDTO>(request);
        }

        public async Task<int> GetSickLeavesNumber(string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            return await _sickLeaveRepository.GetSickLeavesNumberAsync(searchKey, condition);
        }

        public async Task<SickLeaveViewDTO[]> GetPageAsync(int pageNumber, int pageSize,string priorityStatus, string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            var reqs = await _sickLeaveRepository.GetPageAsync(pageNumber, pageSize, priorityStatus, searchKey, condition);
            var requests = reqs != null ? reqs.Select(req => new SickLeaveViewDTO
            {
               EmployeeId = req.EmployeeId,
               SickLeaveId = req.SickLeaveId,
               CreateDate = req.CreateDate,
               CloseDate = req.CloseDate,
               EmployeeFullName = $"{req.Employee.FirstName} {req.Employee.LastName}",
               Duration = req.CloseDate == null ? (int)(DateTime.UtcNow.Date - req.CreateDate).TotalDays : req.Duration,
               RequestStatus = req.RequestStatus,
               TeamName = req.Employee.Team != null ? req.Employee.Team.Name : emptyValue
            }).ToArray() : new SickLeaveViewDTO[] { };

            return requests;
        }

        public async Task<SickLeaveRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            var request = await _sickLeaveRepository.GetByIdWithEmployeeWithTeamAsync(id);
            return _mapHelper.NestedMap<SickLeaveRequest, SickLeaveRequestDTO, Employee, EmployeeDTO, Team, TeamDTO>(request);
        }

        public async Task CreateAsync(SickLeaveRequestDTO SickLeave, bool writeChanges = false)
        {
            var entity = _mapHelper.Map<SickLeaveRequestDTO, SickLeaveRequest>(SickLeave);
            await _sickLeaveRepository.CreateAsync(entity,writeChanges);
        }

        public async Task UpdateAsync(SickLeaveRequestDTO newRequest, bool writeChanges = false)
        {
            var currentRequest = await _sickLeaveRepository.GetByIdAsync(newRequest.SickLeaveId);
            if (currentRequest != null)
            {
                currentRequest.CloseDate = newRequest.CloseDate;
                currentRequest.RequestStatus = newRequest.RequestStatus;
                currentRequest.Duration = newRequest.Duration;
                currentRequest.Comment = newRequest.Comment;
                currentRequest.ProccessedbyId = newRequest.ProccessedbyId;
                if(currentRequest.Employee != null && newRequest.Employee != null && currentRequest.Employee.EmployeeBalanceResiduals != null && newRequest.Employee.EmployeeBalanceResiduals != null)
                {
                    currentRequest.Employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance = newRequest.Employee.EmployeeBalanceResiduals.FirstOrDefault(r => r.Name == ResidualTypeEnum.Sick_leave.ToString()).ResidualBalance;
                }

                if (writeChanges)
                {
                    await _sickLeaveRepository.WriteChangesAsync();
                }              
            }
        }
    }
}
