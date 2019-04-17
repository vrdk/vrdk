using System;

namespace VRdkHRMsystem.Models.TeamleadViewModels
{
    public class AbsenceApproveViewModel
    {
        public string EmployeeId { get; set; }
        public string TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public string Role { get; set; }
    }
}
