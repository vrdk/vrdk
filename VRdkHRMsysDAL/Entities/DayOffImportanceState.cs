using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class DayOffImportanceState
    {
        public DayOffImportanceState()
        {
            DayOff = new HashSet<DayOff>();
        }

        public string DayOffImportanceStateId { get; set; }
        public string Name { get; set; }

        public ICollection<DayOff> DayOff { get; set; }
    }
}
