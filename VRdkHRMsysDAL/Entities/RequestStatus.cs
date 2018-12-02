using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class RequestStatus
    {
        public RequestStatus()
        {
            SickLeaveRequest = new HashSet<SickLeaveRequest>();
            VacationRequest = new HashSet<VacationRequest>();
        }

        public string RequestStatusId { get; set; }
        public string Name { get; set; }

        public ICollection<SickLeaveRequest> SickLeaveRequest { get; set; }
        public ICollection<VacationRequest> VacationRequest { get; set; }
    }
}
