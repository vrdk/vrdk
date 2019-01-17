namespace VRdkHRMsystem.Models.SharedModels.Employee
{
    public class EmployeeViewModel
    {
        public string EmployeeId { get; set; }
        public string TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string TeamName { get; set; }
        public int PaidVacationBalance { get; set; }
        public int UnpaidVacationBalance { get; set; }
        public int SickLeaveBalance { get; set; }
        public int AbsenceBalance { get; set; }
        public int AssignmentBalance { get; set; }
        public bool State { get; set; }
    }
}
