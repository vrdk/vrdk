using System;
using System.ComponentModel.DataAnnotations;
using VRdkHRMsystem.Models.SharedViewModels.Employee;

namespace VRdkHRMsystem.Models.AdminViewModels.Assignment
{
    public class AddAssignmentViewModel
    {
        public string OrganisationId { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public string[] AssignmentMembers { get; set; }
        public EmployeeViewModel[] Employees { get; set; }
    }
}
