using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VRdkHRMsystem.Models.Profile;

namespace VRdkHRMsystem.Models.RequestViewModels.Vacation
{
    public class CompositeRequestVacationViewModel
    {
        public ProfileViewModel ProfileModel { get; set; }
        public RequestVacationViewModel RequestVacationModel { get; set; }
    }
}
