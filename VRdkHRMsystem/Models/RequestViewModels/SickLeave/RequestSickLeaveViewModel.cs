using Microsoft.AspNetCore.Http;

namespace VRdkHRMsystem.Models.RequestViewModels.SickLeave
{
    public class RequestSickLeaveViewModel
    {
        public string EmployeeId { get; set; }
        public string Comment { get; set; }
        public IFormFile[] Files { get; set; }
    }
}
