using VRdkHRMsystem.Models.Profile;

namespace VRdkHRMsystem.Models.RequestViewModels
{
    public class CompositeRequestVacationViewModel
    {
        public ProfileViewModel ProfileModel { get; set; }
        public RequestVacationViewModel RequestVacationModel { get; set; }
    }
}
