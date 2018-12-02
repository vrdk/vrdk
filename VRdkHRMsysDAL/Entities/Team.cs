using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Team
    {
        public Team()
        {
            Employee = new HashSet<Employee>();
        }

        public string TeamId { get; set; }
        public string TeamleadId { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }

        public Organisation Organisation { get; set; }
        public ICollection<Employee> Employee { get; set; }
    }
}
