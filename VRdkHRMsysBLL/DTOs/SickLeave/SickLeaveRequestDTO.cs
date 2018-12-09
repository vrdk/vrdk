using System;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.SickLeave
{
    public class SickLeaveRequestDTO
    {
        public string SickLeaveId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionId { get; set; }
        public string RequestStatus { get; set; }
        public string Comment { get; set; }
        public string ProccessedbyId { get; set; }
        public int? Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public EmployeeDTO Employee { get; set; }
    }
}
