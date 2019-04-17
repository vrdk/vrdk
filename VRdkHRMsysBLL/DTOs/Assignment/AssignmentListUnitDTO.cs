using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class AssignmentListUnitDTO
    {
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public int EmployeesCount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
    }
}
