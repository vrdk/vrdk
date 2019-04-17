using VRdkHRMsystem.Models.Profile;

namespace VRdkHRMsystem.Models.RequestViewModels
{
    public class CompositeRequestSickLeaveViewModel
    {
        public ProfileViewModel ProfileModel { get; set; }
        public RequestSickLeaveViewModel RequestSickLeaveModel { get; set; }
    }
}
