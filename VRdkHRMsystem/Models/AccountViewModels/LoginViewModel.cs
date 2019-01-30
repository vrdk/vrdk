using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = " заполните")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = " заполните")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
