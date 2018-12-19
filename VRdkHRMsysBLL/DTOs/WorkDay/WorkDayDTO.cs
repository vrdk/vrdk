using System;
using System.Collections.Generic;
using System.Text;

namespace VRdkHRMsysBLL.DTOs.WorkDay
{
    public class WorkDayDTO
    {
        public string WorkDayId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime WorkDayDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
        public DateTime ProcessDate { get; set; }
    }
}
