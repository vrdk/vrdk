using System;

namespace VRdkHRMsystem.Models.SharedModels
{
    public class AssignmentViewModel
    {
        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public int EmployeesCount { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
    }   
}
