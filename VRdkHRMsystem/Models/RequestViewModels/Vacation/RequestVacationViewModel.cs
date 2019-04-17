using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.RequestViewModels
{
    public class RequestVacationViewModel
    {        
        public string EmployeeId { get; set; }
        [Required]
        public string VacationType { get; set; }
        public IEnumerable<SelectListItem> VacationTypes { get; set; }
        [Required(ErrorMessage = " заполните")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required(ErrorMessage =" заполните")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [StringLength(128, ErrorMessage = " слишком длинное сообщение")]
        public string Comment { get; set; }
        [Range(1, 365, ErrorMessage = " от 0 до 365")]
        public int Duration { get; set; }
        public int PaidVacationResiduals { get; set; }
        public int UnpaidVaccationResiduals { get; set; }
    }
}
