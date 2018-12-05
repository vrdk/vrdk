using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.RequestViewModels.SickLeave
{
    public class RequestSickLeaveViewModel
    {
        public string EmployeeId { get; set; }
        public string Comment { get; set; }
        public IFormFile[] Files { get; set; }
    }
}
