using System;
using System.ComponentModel.DataAnnotations;
using VRdkHRMsystem.Models.SharedModels;

namespace VRdkHRMsystem.Models.TeamleadViewModels
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
