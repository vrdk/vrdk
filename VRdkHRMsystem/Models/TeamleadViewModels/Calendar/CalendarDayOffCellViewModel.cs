using System;

namespace VRdkHRMsystem.Models.TeamleadViewModels.Calendar
{
    public class CalendarDayOffCellViewModel
    {
        public string EmployeeId { get; set; }
        public string DayOffImportance { get; set; }
        public string DayOffState { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string TeamId { get; set; }
        public DateTime Date { get; set; }
        public string Role { get; set; }
    }
}
