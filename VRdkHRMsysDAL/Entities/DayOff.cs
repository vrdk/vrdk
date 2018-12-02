using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class DayOff
    {
        public string DayOffId { get; set; }
        public string EmployeeId { get; set; }
        public string DayOffStateId { get; set; }
        public string DayOffImportanceStateId { get; set; }
        public DateTime DayOffDate { get; set; }
        public DateTime? ProcessDate { get; set; }

        public DayOffImportanceState DayOffImportanceState { get; set; }
        public DayOffState DayOffState { get; set; }
        public Employee Employee { get; set; }
    }
}
