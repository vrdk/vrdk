using System;
using VRdkHRMsystem.Models.Profile;

namespace VRdkHRMsystem.Models.TeamleadViewModels
{
    public class TimeManagementListViewModel
    {
        public string EmployeeFullName { get; set; }
        public DateTime Date { get; set; }
        public TimeManagementListUnitViewModel[] Records { get; set; }
    }
}
