using System;
using System.Collections.Generic;
using System.Text;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.DayOff
{
    public class DayOffDTO
    {
        public string DayOffId { get; set; }
        public string EmployeeId { get; set; }
        public string DayOffState { get; set; }
        public string DayOffImportance { get; set; }
        public DateTime DayOffDate { get; set; }
        public DateTime? ProcessDate { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
