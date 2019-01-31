using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRdkHRMsystem.Models.SharedViewModels.Employee;

namespace VRdkHRMsystem.Models.AdminViewModels.Team
{
    public class AddTeamViewModel
    {
        public string OrganisationId { get; set; }
        public string TeamId { get; set; }
        [Required(ErrorMessage = " заполните")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string Name { get; set; }
        public string TeamleadId { get; set; }
        public IEnumerable<SelectListItem> Teamleads { get; set; }
        [Required(ErrorMessage = " выберите сотрудников")]
        public string[] TeamMembers { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
    }
}
