using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class VacationRequest
    {
        public string VacationId { get; set; }
        public string EmployeeId { get; set; }
        public string TransactionId { get; set; }
        public string RequestStatusId { get; set; }
        public string VacationTypeId { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Comment { get; set; }
        public string ProccessedbyId { get; set; }
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? ProccessDate { get; set; }

        public Employee Employee { get; set; }
        public RequestStatus RequestStatus { get; set; }
        public VacationType VacationType { get; set; }
    }
}
