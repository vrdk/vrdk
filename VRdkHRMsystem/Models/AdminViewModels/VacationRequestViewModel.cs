using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.AdminViewModels
{
    public class VacationRequestViewModel
    {
        public string EmployeeId { get; set; }
        public string VacationId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamName { get; set; }
        public string VacationType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string VacationState { get; set; }
    }
}
