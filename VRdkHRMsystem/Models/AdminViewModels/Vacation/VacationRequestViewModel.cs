using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.Vacation.AdminViewModels
{
    public class VacationRequestViewModel
    {
        public string EmployeeId { get; set; }
        public string VacationId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamName { get; set; }
        public string VacationType { get; set; }
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string RequestStatus { get; set; }
    }
}
