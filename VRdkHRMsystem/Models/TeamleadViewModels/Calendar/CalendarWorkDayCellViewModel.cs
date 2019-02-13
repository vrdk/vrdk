using System;

namespace VRdkHRMsystem.Models.TeamleadViewModels.Calendar
{
    public class CalendarWorkDayCellViewModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamId { get; set; }
        public string DayOffImportance { get; set; }
        public DateTime Date { get; set; }
        public DateTime TimeFrom { get; set; }
        public DateTime TimeTo { get; set; }
        public string Role { get; set; }
    }
}
