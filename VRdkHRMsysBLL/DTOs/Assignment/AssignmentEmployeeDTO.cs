namespace VRdkHRMsysBLL.DTOs
{
    public class AssignmentEmployeeDTO
    {
        public string RowId { get; set; }
        public string EmployeeId { get; set; }
        public string AssignmentId { get; set; }

        public EmployeeDTO Employee { get; set; }
        public AssignmentDTO Assignment { get; set; }
    }
}
