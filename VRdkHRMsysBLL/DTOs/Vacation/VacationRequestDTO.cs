using System;
using System.Collections.Generic;
using System.Text;
using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.Vacation
{
    public class VacationRequestDTO
    {
        public string VacationId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionId { get; set; }
        public string RequestStatusId { get; set; }
        public string VacationTypeId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public string ProcessedbyId { get; set; }
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ProcessDate { get; set; }

        public EmployeeDTO Employee { get; set; }
    }
}
