using System;

namespace VRdkHRMsystem.Models.SharedModels
{
    public class EmployeeProfileViewModel
    {
        public string EmployeeId { get; set; }
        public string OrganisationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Post { get; set; }
        public bool State { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public string Role { get; set; }
        public int PaidVacationBalance { get; set; }
        public int UnpaidVacationBalance { get; set; }
        public int SickLeaveBalance { get; set; }
        public int AbsenceBalance { get; set; }
        public int AssignmentBalance { get; set; }
    }
}
