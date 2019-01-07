using System;

namespace VRdkHRMsysBLL.DTOs.SickLeave
{
    public class SickLeaveViewDTO
    {
        public string SickLeaveId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamName { get; set; }
        public int? Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string RequestStatus { get; set; }
    }
}
