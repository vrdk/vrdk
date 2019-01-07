namespace VRdkHRMsystem.Models.Profile
{
    public class ProfileResidualsViewModel
    {
        public string EmployeeId { get; set; }
        public int PaidVacationBalance { get; set; }
        public int UnpaidVacationBalance { get; set; }
        public int SickLeaveBalance { get; set; }
        public int AbsenceBalance { get; set; }
        public int AssignmentBalance { get; set; }
    }
}
