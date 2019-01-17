namespace VRdkHRMsystem.Models.SharedModels.Team
{
    public class TeamListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public TeamViewModel[] Teams{ get; set; }
    }
}
