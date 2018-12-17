using System;
using System.Collections.Generic;
using System.Text;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.Assignment
{
    public class AssignmentDTO
    {
        public string AssignmentId { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }

        public AssignmentEmployeeDTO[] Employees { get; set; }
    }
}
