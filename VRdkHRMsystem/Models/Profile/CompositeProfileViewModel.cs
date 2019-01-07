using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.Profile
{
    public class CompositeProfileViewModel
    {
        public ProfileViewModel ProfileModel { get; set; }
        public ProfileResidualsViewModel ResidualsModel { get; set; }
    }
}
