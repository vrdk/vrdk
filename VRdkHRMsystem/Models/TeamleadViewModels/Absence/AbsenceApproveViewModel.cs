using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.TeamleadViewModels.Absence
{
    public class AbsenceApproveViewModel
    {
        public string EmployeeId { get; set; }
        public string TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
    }
}
