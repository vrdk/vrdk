using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class AssignmentEmployee
    {
        public string RowId { get; set; }
        public string EmployeeId { get; set; }
        public string AssignmentId { get; set; }

        public Assignment Assignment { get; set; }
        public Employee Employee { get; set; }
    }
}
