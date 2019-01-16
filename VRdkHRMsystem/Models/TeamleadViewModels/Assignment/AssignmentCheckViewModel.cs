using System;
using System.ComponentModel.DataAnnotations;
using VRdkHRMsystem.Models.SharedViewModels.Employee;

namespace VRdkHRMsystem.Models.TeamleadViewModels.Assignment
{
    public class AssignmentCheckViewModel
    {   
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public EmployeeAssignmentViewModel[] AssignmentMembers { get; set; }
    }
}
