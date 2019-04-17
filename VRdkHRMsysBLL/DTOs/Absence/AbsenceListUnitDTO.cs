using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class AbsenceListUnitDTO
    {
        public string EmployeeId { get; set; }
        public string FullEmployeeName { get; set; }
        public string TeamName { get; set; }
        public DateTime Date { get; set; }
    }
}