using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage =" заполните")]
        [EmailAddress(ErrorMessage =" неверный e-mail")]
        public string Email { get; set; }
    }
}
