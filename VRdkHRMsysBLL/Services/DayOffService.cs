using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.DayOff;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class DayOffService : IDayOffService
    {
        private readonly IDayOffRepository _dayOffRepository;
        private readonly IMapHelper _mapHelper;

        public DayOffService(IDayOffRepository dayOffRepository,
                              IMapHelper mapHelper)
        {
            _dayOffRepository = dayOffRepository;
            _mapHelper = mapHelper;
        }

        public async Task CreateAsync(DayOffDTO dayOff)
        {
            var dayOffToAdd = _mapHelper.Map<DayOffDTO, DayOff>(dayOff);
            await _dayOffRepository.CreateAsync(dayOffToAdd);
        }
    }
}
