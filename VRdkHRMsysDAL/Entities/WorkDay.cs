using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class WorkDay
    {
        public string WorkDayId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime WorkDayDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public DateTime ProcessDate { get; set; }

        public Employee Employee { get; set; }
    }
}
