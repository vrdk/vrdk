using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Absence;
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

        public async Task<AbsenceDTO> GetByEmployeeIdAsync(string id)
        {
            var abs = await _absenceRepository.GetAsync(ab => ab.EmployeeId == id && ab.AbsenceDate.Date == DateTime.UtcNow.Date);
            return _mapHelper.Map<Absence, AbsenceDTO>(abs.First());
        }

        public async Task CreateAsync(AbsenceDTO absence)
        {
            var absenceToAdd = _mapHelper.Map<AbsenceDTO, Absence>(absence);
            await _absenceRepository.CreateAsync(absenceToAdd);
        }
    }
}
