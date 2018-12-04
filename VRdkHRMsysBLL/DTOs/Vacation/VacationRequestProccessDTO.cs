using System;

namespace VRdkHRMsysBLL.DTOs.Vacation
{
    public class VacationRequestProccessDTO
    {
        public string EmployeeId { get; set; }
        public string VacationId { get; set; }
        public string EmployeeFullName { get; set; }
        public string TeamName { get; set; }
        public string VacationType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDenied { get; set; }
    }
}
