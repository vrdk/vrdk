using System;

namespace VRdkHRMsystem.Models.SharedModels
{
    public class SickLeaveRequestViewModel
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
