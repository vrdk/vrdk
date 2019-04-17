using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class VacationRequestViewDTO
    {
        public string EmployeeId { get; set; }
        public string VacationId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamName { get; set; }
        public string VacationType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string RequestStatus { get; set; }
        public int Balance { get; set; }
    }
}
