namespace VRdkHRMsystem.Models.SharedModels
{
    public class VacationRequestListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public VacationRequestViewModel[] Vacations { get; set; }
    }
}
