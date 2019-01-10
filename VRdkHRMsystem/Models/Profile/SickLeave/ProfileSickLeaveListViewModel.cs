namespace VRdkHRMsystem.Models.Profile.SickLeave
{
    public class ProfileSickLeaveListViewModel
    {
        public string EmployeeId { get; set; }

        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ProfileSickLeavesViewModel[] SickLeaves { get; set; }
    }
}
