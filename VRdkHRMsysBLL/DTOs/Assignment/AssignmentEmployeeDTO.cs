using System;
using System.Collections.Generic;
using System.Text;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.Assignment
{
    public class AssignmentEmployeeDTO
    {
        public string RowId { get; set; }
        public string EmployeeId { get; set; }
        public string AssignmentId { get; set; }

        public EmployeeDTO Employee { get; set; }
        public AssignmentDTO Assignment { get; set; }
    }
}
