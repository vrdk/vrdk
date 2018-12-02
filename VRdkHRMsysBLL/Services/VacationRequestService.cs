using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.BalanceResiduals;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.StatusType;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.DTOs.Vacation;
using VRdkHRMsysBLL.Enums;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class VacationRequestService : IVacationRequestService
    {
        private readonly IVacationRequestRepository _vacationRepository;
        private readonly IVacationTypeRepository _vacationTypeRepository;
        private readonly IRequestStatusRepository _requestStatusRepository;
        private readonly IMapHelper _mapHelper;

        public VacationRequestService(IVacationRequestRepository vacationRepository,
                                      IVacationTypeRepository vacationTypes,
                                      IRequestStatusRepository requestStatusRepository,
                                      IMapHelper mapHelper)
        {
            _vacationRepository = vacationRepository;
            _vacationTypeRepository = vacationTypes;
            _requestStatusRepository = requestStatusRepository;
            _mapHelper = mapHelper;
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

        public async Task<VacationRequestDTO[]> GetProccessingVacationRequestsAsync(string organisationId, string teamId)
        {
            var reqs =  await _vacationRepository.GetAsync(req
                                                         => (req.Employee.TeamId == teamId
                                                          || req.RequestStatus.Name.Equals(RequestStatusEnum.Proccessing.ToString()))
                                                          && req.Employee.Organisation.Equals(organisationId));
            return _mapHelper.MapCollection<VacationRequest, VacationRequestDTO>(reqs);
        }

        public async Task<VacationTypeDTO[]> GetVacationTypesAsync()
        {
            var types = await _vacationTypeRepository.GetAsync();
            return _mapHelper.MapCollection<VacationType, VacationTypeDTO>(types);
        }

        public async Task<RequestStatusDTO[]> GetRequestStatusesAsync()
        {
            var statuses = await _requestStatusRepository.GetAsync();
            return _mapHelper.MapCollection<RequestStatus, RequestStatusDTO>(statuses);
        }

        public async Task<RequestStatusDTO> GetRequestStatusByNameAsync(string name)
        {
            var status = await _requestStatusRepository.GetByNameAsync(name);
            return _mapHelper.Map<RequestStatus, RequestStatusDTO>(status);
        }

        public async Task CreateVacationRequestAsync(VacationRequestDTO request)
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
