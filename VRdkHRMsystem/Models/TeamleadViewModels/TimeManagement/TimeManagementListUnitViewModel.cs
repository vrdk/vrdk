using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.Profile
{
    public class TimeManagementListUnitViewModel
    {
        public string TimeManagementRecordId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime RecordDate { get; set; }
        public DateTime ProccessDate { get; set; }
        [Required]
        public TimeSpan TimeFrom { get; set; }
        [Required]
        public TimeSpan TimeTo { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
