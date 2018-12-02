using System;
using System.Collections.Generic;
using System.Text;

namespace VRdkHRMsysBLL.DTOs.BalanceResiduals
{
    public class BalanceResidualsDTO
    {
        public string ResidualId { get; set; }
        public string EmployeeId { get; set; }
        public string Name { get; set; }
        public int ResidualBalance { get; set; }
    }
}
