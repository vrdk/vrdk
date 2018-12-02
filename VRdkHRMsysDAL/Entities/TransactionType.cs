using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class TransactionType
    {
        public TransactionType()
        {
            Transaction = new HashSet<Transaction>();
        }

        public string TransactionTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<Transaction> Transaction { get; set; }
    }
}
