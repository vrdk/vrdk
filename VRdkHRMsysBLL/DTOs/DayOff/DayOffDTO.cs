using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class DayOffDTO
    {
        public string DayOffId { get; set; }
        public string EmployeeId { get; set; }
        public string DayOffState { get; set; }
        public string Comment { get; set; }
        public string DayOffImportance { get; set; }
        public DateTime DayOffDate { get; set; }
        public DateTime? ProcessDate { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
