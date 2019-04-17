using System;

namespace VRdkHRMsystem.Models.TeamleadViewModels
{
    public class SickLeaveRequestProccessViewModel
    {
        public string SickLeaveId { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamleadFullName { get; set; }
        public string TeamName { get; set; }
        public string Post { get; set; }
        public string Result { get; set; }
        public string Comment { get; set; }
        public int? Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string[] Files { get; set; }
    }
}
