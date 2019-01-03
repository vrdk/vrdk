using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRdkHRMsystem.Models.Profile;

namespace VRdkHRMsystem.Models.RequestViewModels.SickLeave
{
    public class CompositeRequestSickLeaveViewModel
    {
        public ProfileViewModel ProfileModel { get; set; }
        public RequestSickLeaveViewModel RequestSickLeaveModel { get; set; }
    }
}
