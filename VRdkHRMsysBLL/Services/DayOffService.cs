using System;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
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

        public async Task DeleteAsync(string id, bool writeChanges = false)
        {
            var dayOff = await _dayOffRepository.GetByIdAsync(id);
            if(dayOff != null)
            {
                await _dayOffRepository.DeleteAsync(dayOff, writeChanges);
            }     
        }

        public async Task UpdateAsync(DayOffDTO dayOff, bool writeChanges = false)
        {
            var currentdayOff = await _dayOffRepository.GetByIdAsync(dayOff.DayOffId);
            if (currentdayOff != null)
            {
                currentdayOff.Comment = dayOff.Comment;
                currentdayOff.DayOffImportance = dayOff.DayOffImportance;
                currentdayOff.DayOffState = dayOff.DayOffState;
                currentdayOff.ProcessDate = dayOff.ProcessDate;
            }

            if (writeChanges)
            {
                await _dayOffRepository.WriteChangesAsync();
            }         
        }

        public async Task<DayOffDTO> GetByDateAsync(DateTime date, string employeeId)
        {
            var dayOff = await _dayOffRepository.GetByDateAsync(date, employeeId);
            return _mapHelper.Map<DayOff, DayOffDTO>(dayOff);
        }

        public async Task<DayOffDTO> GetByIdAsync(string id)
        {
            var dayOff = await _dayOffRepository.GetByIdAsync(id);
            return _mapHelper.Map<DayOff, DayOffDTO>(dayOff);
        }

        public async Task CreateAsync(DayOffDTO dayOff, bool writeChanges = false)
        {
            var dayOffToAdd = _mapHelper.Map<DayOffDTO, DayOff>(dayOff);
            await _dayOffRepository.CreateAsync(dayOffToAdd, writeChanges);
        }
    }
}
