using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Team
    {
        public Team()
        {
            Employees = new HashSet<Employee>();
        }

        public string TeamId { get; set; }
        public string TeamleadId { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }

        public Organisation Organisation { get; set; }
        public ICollection<Employee> Employees { get; set; }
        public Employee Teamlead { get; set; }
    }
}
