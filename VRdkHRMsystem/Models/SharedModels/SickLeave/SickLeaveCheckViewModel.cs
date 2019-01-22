using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.SharedModels.SickLeave
{
    public class SickLeaveCheckViewModel
    {
        public string SickLeaveId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamleadFullName { get; set; }
        public string EmployeeId { get; set; }
        public string TeamName { get; set; }
        public string Comment { get; set; }
        public string Post { get; set; }
        public string Result { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public string ProccessedByName { get; set; }
        public string RequestStatus { get; set; }
        public string[] Files { get; set; }
    }
}
