using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Post
    {
        public Post()
        {
            Employee = new HashSet<Employee>();
        }

        public string PostId { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }

        public Organisation Organisation { get; set; }
        public ICollection<Employee> Employee { get; set; }
    }
}
