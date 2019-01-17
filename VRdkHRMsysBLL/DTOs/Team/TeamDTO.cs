using VRdkHRMsysBLL.DTOs.Employee;

namespace VRdkHRMsysBLL.DTOs.Team
{
    public class TeamDTO
    {
        public string TeamId { get; set; }
        public string TeamleadId { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }

        public EmployeeDTO[] Employees { get; set; }
        public EmployeeDTO Teamlead { get; set; }
    }
}
