using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class AbsenceDTO
    {
        public string AbsenceId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime AbsenceDate { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
