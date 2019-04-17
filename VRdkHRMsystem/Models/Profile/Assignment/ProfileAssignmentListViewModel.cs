namespace VRdkHRMsystem.Models.Profile
{
    public class ProfileAssignmentListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public ProfileAssignmentsViewModel[] Assignments { get; set; }
    }
}
