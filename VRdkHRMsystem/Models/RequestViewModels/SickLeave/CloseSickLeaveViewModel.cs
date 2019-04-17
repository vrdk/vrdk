using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
namespace VRdkHRMsystem.Models.RequestViewModels
{
    public class CloseSickLeaveViewModel
    {
        public string SickLeaveId { get; set; }
        public string Comment { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreateDate { get; set; }
        public string[] Files { get; set; }
        public IFormFile File {get;set;}
    }
}
