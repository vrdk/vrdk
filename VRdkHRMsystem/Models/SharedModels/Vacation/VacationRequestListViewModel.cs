using VRdkHRMsystem.Models.SharedModels.Vacation;

namespace VRdkHRMsystem.Models.SharedModels.Vacation
{
    public class VacationRequestListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public VacationRequestViewModel[] Vacations { get; set; }
    }
}
