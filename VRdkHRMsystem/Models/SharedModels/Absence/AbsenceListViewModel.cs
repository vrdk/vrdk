namespace VRdkHRMsystem.Models.SharedModels.Absence
{
    public class AbsenceListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public AbsenceViewModel[] Absences { get; set; }
    }
}
