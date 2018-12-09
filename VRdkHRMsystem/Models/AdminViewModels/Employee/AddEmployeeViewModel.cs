using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.AdminViewModels.Employee
{
    public class AddEmployeeViewModel
    {    
        public string OrganisationId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PostId { get; set; }
        public IEnumerable<SelectListItem> Posts { get; set; }
        [Required]
        public bool State { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DismissalDate { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Personal Email")]
        public string PersonalEmail { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Work Email")]
        public string WorkEmail { get; set; }
        [Required]
        public string Role { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        [Required]
        [Display(Name = "Paid vacation balance")]
        [Range(1, 100)]
        public int PaidVacationBalance { get; set; }
        [Required]
        [Display(Name = "Paid vacation balance")]
        [Range(1, 100)]
        public int UnpaidVacationBalance { get; set; }
        [Required]
        [Display(Name = "Paid vacation balance")]
        [Range(1, 100)]
        public int SickLeaveBalance { get; set; }
        [Required]
        [Display(Name = "Paid vacation balance")]
        [Range(1,100)]
        public int AbsenceBalance { get; set; }
    }
}
