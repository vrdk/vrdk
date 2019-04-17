using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class TransactionDTO
    {
        public string TransactionId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionType { get; set; }
        public int Change { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
}
