using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ITimeManagementService
    {
        Task CreateAsync(TimeManagementRecordDTO record, bool writeChanges = false);
        Task DeleteAsync(TimeManagementRecordDTO record, bool writeChanges = false);
        Task<TimeManagementRecordDTO[]> GetAsync(Expression<Func<TimeManagementRecord, bool>> condition);
        Task<TimeManagementRecordDTO> GetByIdAsync(string id);
        Task UpdateAsync(TimeManagementRecordDTO newRecord, bool writeChanges = false);
    }
}