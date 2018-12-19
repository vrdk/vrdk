using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.Profile.Vacation
{
    public class ProfileVacationsViewModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string VacationType { get; set; }
        public string RequestStatus { get; set; }
    }
}
