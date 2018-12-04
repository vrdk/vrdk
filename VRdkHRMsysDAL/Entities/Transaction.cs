using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Transaction
    {
        public string TransactionId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionType { get; set; }
        public int Change { get; set; }
        public DateTime TransactionDate { get; set; }
        public string Description { get; set; }

        public Employee Employee { get; set; }
    }
}
