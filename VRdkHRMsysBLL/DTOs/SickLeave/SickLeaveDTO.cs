using System;
using System.Collections.Generic;
using System.Text;

namespace VRdkHRMsysBLL.DTOs.SickLeave
{
    public class SickLeaveDTO
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
    }
}
