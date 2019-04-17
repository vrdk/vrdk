using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class EmployeeDTO
    {
        public string EmployeeId { get; set; }
        public string TeamId { get; set; }
        public string OrganisationId { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostId { get; set; }
        public bool State { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public TeamDTO Team { get; set; }
        public BalanceResidualsDTO[] EmployeeBalanceResiduals { get; set; }
        public VacationRequestDTO[] Vacations { get; set; }
        public SickLeaveRequestDTO[] SickLeaves { get; set; }
        public AbsenceDTO[] Absences { get; set; }
        public AssignmentEmployeeDTO[] Assignments { get; set; }
        public DayOffDTO[] DayOffs { get; set; }
        public WorkDayDTO[] WorkDays { get; set; }
    }
}
