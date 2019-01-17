using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Organisation
    {
        public Organisation()
        {
            Assignment = new HashSet<Assignment>();
            Employee = new HashSet<Employee>();
            Notification = new HashSet<Notification>();
            Post = new HashSet<Post>();
            Team = new HashSet<Team>();
        }

        public string OrganisationId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }

        public virtual ICollection<Assignment> Assignment { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<Notification> Notification { get; set; }
        public virtual ICollection<Post> Post { get; set; }
        public virtual ICollection<Team> Team { get; set; }
    }
}
