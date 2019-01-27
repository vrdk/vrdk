using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Globalization;
using VRdkHRMsystem.Models.SharedModels.Employee;

namespace VRdkHRMsystem.Models.SharedModels.Calendar
{
    public class CalendarViewModel
    {
        public string TeamId { get; set; }
        public IEnumerable<SelectListItem> Teams { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public CultureInfo Culture { get; set; }
        public CalendarEmployeeViewModel[] Employees { get; set; }
        public string TeamName { get; set; }
        public string MainMemberId { get; set; }
        public string Role { get; set; }
    }
}
