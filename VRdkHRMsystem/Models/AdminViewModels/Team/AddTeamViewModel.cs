﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VRdkHRMsystem.Models.SharedViewModels.Employee;

namespace VRdkHRMsystem.Models.AdminViewModels.Team
{
    public class AddTeamViewModel
    {
        public string OrganisationId { get; set; }
        public string TeamId { get; set; }
        public string Name { get; set; }
        [Required(ErrorMessage = " необходино заполнить")]
        [MaxLength(50, ErrorMessage = " до 50 символов")]
        public string TeamleadId { get; set; }
        public IEnumerable<SelectListItem> Teamleads { get; set; }
        public string[] TeamMembers { get; set; }
        public IEnumerable<SelectListItem> Employees { get; set; }
    }
}
