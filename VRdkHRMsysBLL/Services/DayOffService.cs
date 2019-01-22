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

        public async Task DeleteAsync(string id)
        {
            var dayOff = await _dayOffRepository.GetByIdAsync(id);
            if(dayOff != null)
            {
                await _dayOffRepository.DeleteAsync(dayOff);
            }     
        }

        public async Task<DayOffDTO> GetByIdAsync(string id)
        {
            var dayOff = await _dayOffRepository.GetByIdAsync(id);
            return _mapHelper.Map<DayOff, DayOffDTO>(dayOff);
        }

        public async Task CreateAsync(DayOffDTO dayOff)
        {
            var dayOffToAdd = _mapHelper.Map<DayOffDTO, DayOff>(dayOff);
            await _dayOffRepository.CreateAsync(dayOffToAdd);
        }
    }
}
