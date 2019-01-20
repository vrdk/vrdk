using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.AdminViewModels.Assignment
{
    public class EditAssignmentViewModel
    {
        public string AssignmentId { get; set; }
        public string OrganisationId { get; set; }
        [Required(ErrorMessage = " заполните")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string Name { get; set; }
        [Required(ErrorMessage = " заполните")]
        [DataType(DataType.Date)]
        public DateTime BeginDate { get; set; }
        [Required(ErrorMessage = " заполните")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = " выберите сотрудников")]
        public string[] AssignmentMembers { get; set; }
        [Required(ErrorMessage = " заполните")]
        [Range(1, 365, ErrorMessage = "длительность не может быть отрицательной")]
        public int Duration { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
    }
}
