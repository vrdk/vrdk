using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.AdminViewModels
{
    public class VacationRequestProccessViewModel
    {
        public string VacationId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamleadFullName { get; set; }
        public string TeamName { get; set; }
        public string VacationType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Comment { get; set; }
        public string Result { get; set; }
        public string ProccessComment { get; set; }
    }
}
