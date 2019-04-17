using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class AbsenceService : IAbsenceService
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly IMapHelper _mapHelper;

        public AbsenceService(IAbsenceRepository absenceRepository, IMapHelper mapHelper)
        {
            _absenceRepository = absenceRepository;
            _mapHelper = mapHelper;
        }

        public async Task<AbsenceDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0)
        {
            var requests = await _absenceRepository.GetProfilePageAsync(pageSize, id, pageNumber);
            return _mapHelper.MapCollection<Absence, AbsenceDTO>(requests);
        }

        public async Task<int> GetAbsencesCountAsync(string searchKey = null, Expression<Func<Absence, bool>> condition = null)
        {
            return await _absenceRepository.GetAbsencesCountAsync(searchKey, condition);
        }

        public async Task<AbsenceListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Absence, bool>> condition = null)
        {
            var assigns = await _absenceRepository.GetPageAsync(pageNumber, pageSize, condition, searchKey);
            var assignments = assigns != null ? assigns.Select(a => new AbsenceListUnitDTO
            {
                EmployeeId = a.EmployeeId,
                FullEmployeeName = $"{a.Employee.FirstName} {a.Employee.LastName}",
                TeamName = a.Employee.Team?.Name,
                Date = a.AbsenceDate
            }).ToArray() : new AbsenceListUnitDTO[] { };

            return assignments;
        }

        public async Task<AbsenceDTO> GetTodayByEmployeeIdAsync(string id)
        {
            var abs = await _absenceRepository.GetAsync(ab => ab.EmployeeId == id && ab.AbsenceDate.Date == DateTime.UtcNow.Date);
            return abs.Count() == 0 ? null : new AbsenceDTO();
        }

        public async Task CreateAsync(AbsenceDTO absence, bool writeChanges = false)
        {
            var absenceToAdd = _mapHelper.Map<AbsenceDTO, Absence>(absence);
            await _absenceRepository.CreateAsync(absenceToAdd, writeChanges);
        }
    }
}
