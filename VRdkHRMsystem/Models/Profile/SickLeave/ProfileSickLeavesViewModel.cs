using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.Profile.SickLeave
{
    public class ProfileSickLeavesViewModel
    {
        public string SickLeaveId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string RequestStatus { get; set; }
        public int? Duration { get; set; }
    }
}
