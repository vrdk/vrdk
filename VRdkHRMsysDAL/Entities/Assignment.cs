﻿using System;
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

        public ICollection<AssignmentEmployee> AssignmentEmployee { get; set; }
    }
}
