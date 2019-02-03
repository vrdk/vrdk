using System;

namespace VRdkHRMsystem.Models.Profile.TimeManagement
{
    public class TimeManagementProccessViewModel
    {
        public string EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeManagementRecordViewModel[] Records { get; set; }
    }
}
