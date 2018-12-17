using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Organisation
    {
        public Organisation()
        {
            Employee = new HashSet<Employee>();
            Post = new HashSet<Post>();
            Notification = new HashSet<Notification>();
            Team = new HashSet<Team>();
        }

        public string OrganisationId { get; set; }
        public string Name { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<Employee> Employee { get; set; }
        public ICollection<Notification> Notification { get; set; }
        public ICollection<Post> Post { get; set; }
        public ICollection<Team> Team { get; set; }
    }
}
