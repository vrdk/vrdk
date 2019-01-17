using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Assignment
    {
        public Assignment()
        {
            AssignmentEmployee = new HashSet<AssignmentEmployee>();
        }

        public string AssignmentId { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string OrganisationId { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Organisation Organisation { get; set; }
        public virtual ICollection<AssignmentEmployee> AssignmentEmployee { get; set; }
    }
}
