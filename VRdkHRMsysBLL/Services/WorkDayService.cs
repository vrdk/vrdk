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

        public async Task DeleteAsync(string id)
        {
            var workDay = await _workDayRepository.GetByIdAsync(id);
            if(workDay != null)
            {
                await _workDayRepository.DeleteAsync(workDay);
            }     
        }

        public async Task UpdateAsync(WorkDayDTO workDay)
        {
            var currentWorkDay = await _workDayRepository.GetByIdAsync(workDay.WorkDayId);
            if (currentWorkDay != null)
            {
                currentWorkDay.TimeFrom = workDay.TimeFrom;
                currentWorkDay.TimeTo = workDay.TimeTo;
            }

            await _workDayRepository.UpdateAsync();
        }

        public async Task<WorkDayDTO> GetByIdAsync(string id)
        {
            var dayOff = await _workDayRepository.GetByIdAsync(id);
            return _mapHelper.Map<WorkDay, WorkDayDTO>(dayOff);
        }

        public async Task CreateAsync(WorkDayDTO workDay)
        {
            var workDayToAdd = _mapHelper.Map<WorkDayDTO, WorkDay>(workDay);
            await _workDayRepository.CreateAsync(workDayToAdd);
        }
    }
}
