using Microsoft.AspNetCore.Http;

namespace VRdkHRMsystem.Models.RequestViewModels.SickLeave
{
    public class UpdateSickLeaveViewModel
    {
        public string SickLeaveId{ get; set; }
        public string Comment { get; set; }
        public IFormFile[] UploadedFiles { get; set; }
        public string[] ExistingFiles{ get; set; }
    }
}
