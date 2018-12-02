using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class VacationType
    {
        public VacationType()
        {
            VacationRequest = new HashSet<VacationRequest>();
        }

        public string VacationTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<VacationRequest> VacationRequest { get; set; }
    }
}
