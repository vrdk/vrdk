using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VRdkHRMsystem.Models.AdminViewModels.Team
{
    public class EditTeamViewModel
    {
        public string TeamId { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string Name { get; set; }
        public string TeamleadId { get; set; }
        public IEnumerable<SelectListItem> Teamleads { get; set; }
        public string[] TeamMembers { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
    }
}
