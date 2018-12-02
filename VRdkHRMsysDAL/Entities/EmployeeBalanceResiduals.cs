using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class EmployeeBalanceResiduals
    {
        public string ResidualId { get; set; }
        public string EmployeeId { get; set; }
        public string Name{ get; set; }
        public int ResidualBalance { get; set; }

        public Employee Employee { get; set; }
    }
}
