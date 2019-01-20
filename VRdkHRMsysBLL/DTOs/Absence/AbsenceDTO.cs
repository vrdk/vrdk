using System;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.Absence
{
    public class AbsenceDTO
    {
        public string AbsenceId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime AbsenceDate { get; set; }
        public EmployeeDTO Employee { get; set; }
    }
}
