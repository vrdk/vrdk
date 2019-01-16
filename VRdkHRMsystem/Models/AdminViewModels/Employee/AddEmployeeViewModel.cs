using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.AdminViewModels.Employee
{
    public class AddEmployeeViewModel
    {    
        public string OrganisationId { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        public string LastName { get; set; }
        [Required]
        public string PostId { get; set; }
        public IEnumerable<SelectListItem> Posts { get; set; }
        [Required]
        public bool State { get; set; }
        public IEnumerable<SelectListItem> States { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DismissalDate { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [EmailAddress]
        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [EmailAddress]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }
        [Required]
        public string Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [Range(0,100)]
        public int PaidVacationBalance { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [Range(0,100)]
        public int UnpaidVacationBalance { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [Range(0,100)]
        public int SickLeaveBalance { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [Display(Name = "Absence balance")]
        [Range(0,100)]
        public int AbsenceBalance { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [Display(Name = "Assignment balance")]
        [Range(0,100)]
        public int AssignmentBalance { get; set; }
        public IFormFile Photo { get; set; }
    }
}
