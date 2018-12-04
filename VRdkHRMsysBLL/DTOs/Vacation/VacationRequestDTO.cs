using System;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.Vacation
{
    public class VacationRequestDTO
    {
        public string VacationId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionId { get; set; }
        public string RequestStatus { get; set; }
        public string VacationType { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public string ProccessedbyId { get; set; }
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ProccessDate { get; set; }

        public EmployeeDTO Employee { get; set; }
    }
}
