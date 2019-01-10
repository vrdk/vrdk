using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace VRdkHRMsysBLL.Services
{
    public class VacationService : IVacationService
    {
        private const string emptyValue = "Нет";
        private readonly IVacationRequestRepository _vacationRepository;
        private readonly IMapHelper _mapHelper;

        public VacationService(IVacationRequestRepository vacationRepository,
                                      IMapHelper mapHelper)
        {
            _vacationRepository = vacationRepository;
            _mapHelper = mapHelper;
        }

        public async Task<VacationRequestDTO[]> GetProfilePageAsync(int pageSize,string id,int pageNumber = 0)
        {
            var requests = await _vacationRepository.GetProfilePageAsync(pageSize, id, pageNumber);
            return _mapHelper.MapCollection<VacationRequest, VacationRequestDTO>(requests);
        }

        public async Task<int> GetVacationsNumberAsync(string searchKey = null, Expression < Func<VacationRequest, bool>> condition = null)
        {
            return await _vacationRepository.GetVacationsCountAsync(condition, searchKey);
        }

        public async Task<VacationRequestDTO> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            var request = await _vacationRepository.GetByIdWithEmployeeWithTeamAsync(id);
            return _mapHelper.NestedMap<VacationRequest, VacationRequestDTO,Employee,EmployeeDTO,Team,TeamDTO>(request);
        }

        public async Task<VacationRequestDTO> GetByIdAsync(string id)
        {
            var request = await _vacationRepository.GetByIdAsync(id);
            return _mapHelper.Map<VacationRequest, VacationRequestDTO>(request);
        }

        public async Task<VacationRequestDTO[]> GetAsync(Expression<Func<VacationRequest, bool>> condition = null)
        {
            var reqs = await _vacationRepository.GetAsync(condition);
            return _mapHelper.MapCollection<VacationRequest, VacationRequestDTO>(reqs);
        }

        public async Task<VacationRequestViewDTO[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, string searchKey = null ,Expression<Func<VacationRequest, bool>> condition = null) 
        {
            var reqs = await _vacationRepository.GetPageAsync(pageNumber,pageSize,priorityStatus, condition, searchKey);
            var requests = reqs != null ? reqs.Select(r => new VacationRequestViewDTO()
            {
                EmployeeId = r.EmployeeId,
                VacationId = r.VacationId,
                EmployeeFullName = $"{r.Employee.FirstName} {r.Employee.LastName}",
                BeginDate = r.BeginDate,
                EndDate = r.EndDate,
                Duration = r.Duration,
                RequestStatus = r.RequestStatus,
                TeamName = r.Employee.Team != null ? r.Employee.Team.Name : emptyValue,
                VacationType = r.VacationType,
                Balance = r.Employee.EmployeeBalanceResiduals.FirstOrDefault(res=>res.Name == r.VacationType).ResidualBalance               
            }
            ).ToArray() : new VacationRequestViewDTO[] { };

            return requests;
        }

        public async Task CreateAsync(VacationRequestDTO request)
        {
            var vacationRequest = _mapHelper.Map<VacationRequestDTO, VacationRequest>(request);
            await _vacationRepository.CreateAsync(vacationRequest);
        }

        public async Task UpdateAsync(VacationRequestDTO newRequest)
        {
            var currentRequest = await _vacationRepository.GetByIdAsync(newRequest.VacationId);
            if (currentRequest != null)
            {
                _mapHelper.MapChanges(newRequest, currentRequest);
                await _vacationRepository.UpdateAsync();
            }
        }
    }
}
