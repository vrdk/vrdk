using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.AdminViewModels.Employee
{
    public class EditEmployeeViewModel
    {
        [HiddenInput]
        public string EmployeeId { get; set; }
        [HiddenInput]
        public string OrganisationId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PostId { get; set; }
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
    }
}
