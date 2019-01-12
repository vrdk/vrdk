using Microsoft.AspNetCore.Http;
using System;

namespace VRdkHRMsystem.Models.RequestViewModels.SickLeave
{
    public class UpdateSickLeaveViewModel
    {
        public string SickLeaveId{ get; set; }
        public string Comment { get; set; }
        public DateTime CreateDate { get; set; }
        public IFormFile File{ get; set; }
        public string[] ExistingFiles{ get; set; }
    }
}
