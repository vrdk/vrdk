using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Models.Profile
{
    public class ProfileViewModel
    {
        public string EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Post { get; set; }
        public bool State { get; set; }
        public string PhoneNumber { get; set; }
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DismissalDate { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public string Team { get; set; }
        public string Teamlead { get; set; }
    }
}
