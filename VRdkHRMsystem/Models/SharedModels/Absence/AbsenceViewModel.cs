using System;

namespace VRdkHRMsystem.Models.SharedModels.Absence
{
    public class AbsenceViewModel
    {
        public string EmployeeId { get; set; }
        public string FullEmployeeName { get; set; }
        public string TeamName { get; set; }
        public DateTime Date { get; set; }
    }
}
