namespace VRdkHRMsystem.Models.SharedModels.Employee
{
    public class EmployeeListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public EmployeeViewModel[] Employees { get; set; }
    }
}
