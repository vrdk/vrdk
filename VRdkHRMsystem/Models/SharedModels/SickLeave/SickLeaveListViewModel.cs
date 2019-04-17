namespace VRdkHRMsystem.Models.SharedModels
{
    public class SickLeaveListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public SickLeaveRequestViewModel[] SickLeaves { get; set; }
    }
}
