using System;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.TimeManagement
{
    public class TimeManagementRecordDTO
    {

        public string TimeManagementRecordId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ProccessDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string Description { get; set; }

        public virtual EmployeeDTO Employee { get; set; }
    }
}
