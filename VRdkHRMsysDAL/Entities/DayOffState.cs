using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class DayOffState
    {
        public DayOffState()
        {
            DayOff = new HashSet<DayOff>();
        }

        public string DayOffStateId { get; set; }
        public string Name { get; set; }

        public ICollection<DayOff> DayOff { get; set; }
    }
}
