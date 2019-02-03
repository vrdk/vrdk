using System;
using VRdkHRMsystem.Models.Profile.TimeManagement;

namespace VRdkHRMsystem.Models.TeamleadViewModels.TimeManagement
{
    public class TimeManagementListViewModel
    {
        public string EmployeeFullName { get; set; }
        public DateTime Date { get; set; }
        public TimeManagementListUnitViewModel[] Records { get; set; }
    }
}
