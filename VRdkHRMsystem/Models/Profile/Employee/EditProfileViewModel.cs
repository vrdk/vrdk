using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.Profile
{
    public class EditProfileViewModel
    {
        public string EmployeeId { get; set; }
        public string OrganisationId { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string LastName { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [EmailAddress(ErrorMessage = " неверный  e-mail")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string PersonalEmail { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [Remote(action: "ValidateEmail", controller: "RemoteValidation", AdditionalFields = "EmployeeId")]
        [EmailAddress(ErrorMessage = " неверный  e-mail")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string WorkEmail { get; set; }
    }
}
