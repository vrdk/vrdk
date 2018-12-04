using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.RequestViewModels
{
    public class RequestVacationViewModel
    {        
        public string EmployeeId { get; set; }
        [Required]
        public string VacationType { get; set; }
        public IEnumerable<SelectListItem> VacationTypes { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [StringLength(128)]
        public string Comment { get; set; }
        [Required]
        public int Duration { get; set; }
    }
}
