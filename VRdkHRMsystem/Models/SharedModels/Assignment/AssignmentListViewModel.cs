namespace VRdkHRMsystem.Models.SharedModels
{
    public class AssignmentListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public AssignmentViewModel[] Assignments { get; set; }
    }
}
