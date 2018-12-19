using System;
using System.Collections.Generic;
using System.Text;

namespace VRdkHRMsysBLL.DTOs.Absence
{
    public class AbsenceDTO
    {
        public string AbsenceId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime AbsenceDate { get; set; }
    }
}
