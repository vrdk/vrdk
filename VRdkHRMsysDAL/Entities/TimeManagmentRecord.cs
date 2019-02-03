using System;

namespace VRdkHRMsysDAL.Entities
{
    public partial class TimeManagementRecord
    {
        public string TimeManagementRecordId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ProccessDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public string Description { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
