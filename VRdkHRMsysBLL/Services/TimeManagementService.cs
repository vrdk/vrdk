using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.TimeManagement;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class TimeManagementService : ITimeManagementService
    {
        private readonly ITimeManagementRecordRepository _timeManagementRepository;
        private readonly IMapHelper _mapHelper;

        public TimeManagementService(ITimeManagementRecordRepository timeManagementRepository,
                                  IMapHelper mapHelper)
        {
            _timeManagementRepository = timeManagementRepository;
            _mapHelper = mapHelper;
        }

        public async Task<TimeManagementRecordDTO[]> GetAsync(Expression<Func<TimeManagementRecord, bool>> condition)
        {
            var records = await _timeManagementRepository.GetAsync(condition);
            return _mapHelper.MapCollection<TimeManagementRecord, TimeManagementRecordDTO>(records);
        }

        public async Task<TimeManagementRecordDTO> GetByIdAsync(string id)
        {
            var record = await _timeManagementRepository.GetByIdAsync(id);
            return _mapHelper.Map<TimeManagementRecord, TimeManagementRecordDTO>(record);
        }

        public async Task UpdateAsync(TimeManagementRecordDTO newRecord, bool writeChanges = false)
        {
            var currentRecord = await _timeManagementRepository.GetByIdAsync(newRecord.TimeManagementRecordId);
            if(currentRecord != null)
            {
                currentRecord.TimeFrom = newRecord.TimeFrom;
                currentRecord.TimeTo = newRecord.TimeTo;
                currentRecord.Description = newRecord.Description;
                currentRecord.ProccessDate = DateTime.UtcNow;

                if (writeChanges)
                {
                  await  _timeManagementRepository.UpdateAsync(currentRecord, writeChanges);
                }
            }
        }

        public async Task CreateAsync(TimeManagementRecordDTO record, bool writeChanges = false)
        {
            var recordToAdd = _mapHelper.Map<TimeManagementRecordDTO, TimeManagementRecord>(record);
            await _timeManagementRepository.CreateAsync(recordToAdd, writeChanges);
        }

        public async Task DeleteAsync(TimeManagementRecordDTO record, bool writeChanges = false)
        {
            var recordToRemove = _mapHelper.Map<TimeManagementRecordDTO, TimeManagementRecord>(record);
            await _timeManagementRepository.DeleteAsync(recordToRemove, writeChanges);
        }
    }
}
