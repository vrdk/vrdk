using System;
using System.Collections.Generic;
using System.Text;

namespace VRdkHRMsysBLL.DTOs.Transaction
{
    public class TransactionDTO
    {
        public string TransactionId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionTypeId { get; set; }
        public int Change { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }
    }
}
