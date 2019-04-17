using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.RequestViewModels
{
    public class RequestDayOffViewModel
    {
        public string EmployeeId { get; set; }
        public string DayOffState { get; set; }
        public string DayOffImportance { get; set; }
        [MaxLength(100, ErrorMessage = "до 100 символов")]
        public string Comment { get; set; }
        public string TeamId { get; set; }
        public DateTime DayOffDate { get; set; }
    }
}
