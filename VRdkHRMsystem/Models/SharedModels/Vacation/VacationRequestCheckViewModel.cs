using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.SharedModels
{
    public class VacationRequestCheckViewModel
    {
        public string VacationId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamleadFullName { get; set; }
        public string EmployeeId { get; set; }
        public string TeamName { get; set; }
        public string VacationType { get; set; }
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Comment { get; set; }
        public string ProccessedByName { get; set; }
        public string RequestStatus { get; set; }
        public string Post { get; set; }
    }
}
