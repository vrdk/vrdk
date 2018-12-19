using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.WorkDay;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class WorkDayService : IWorkDayService
    {
        private readonly IWorkDayRepository _workDayRepository;
        private readonly IMapHelper _mapHelper;

        public WorkDayService(IWorkDayRepository workDayRepository,
                              IMapHelper mapHelper)
        {
            _workDayRepository = workDayRepository;
            _mapHelper = mapHelper;
        }

        public async Task CreateAsync(WorkDayDTO workDay)
        {
            var workDayToAdd = _mapHelper.Map<WorkDayDTO, WorkDay>(workDay);
            await _workDayRepository.CreateAsync(workDayToAdd);
        }
    }
}
