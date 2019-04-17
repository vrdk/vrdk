namespace VRdkHRMsystem.Models.SharedModels
{
    public class TeamProfileViewModel
    {
        public string TeamId { get; set; }
        public string Name { get; set; }
        public EmployeeViewModel Teamlead { get; set; }
        public EmployeeViewModel[] Employees { get; set; }
    }
}
