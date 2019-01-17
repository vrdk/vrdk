using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsystem.Models.SharedModels.Employee;

namespace VRdkHRMsystem.Models.SharedModels.Team
{
    public class TeamProfileViewModel
    {
        public string TeamId { get; set; }
        public string Name { get; set; }
        public EmployeeViewModel Teamlead { get; set; }
        public EmployeeViewModel[] Employees { get; set; }
    }
}
