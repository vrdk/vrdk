using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.SharedModels.SickLeave
{
    public class SickLeaveListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public SickLeaveRequestViewModel[] SickLeaves { get; set; }
    }
}
