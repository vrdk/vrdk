using System;

namespace VRdkHRMsystem.Models.RequestViewModels.DayOff
{
    public class RequestDayOffViewModel
    {
        public string EmployeeId { get; set; }
        public string DayOffState { get; set; }
        public string DayOffImportance { get; set; }
        public string Comment { get; set; }
        public string TeamId { get; set; }
        public DateTime DayOffDate { get; set; }
    }
}
