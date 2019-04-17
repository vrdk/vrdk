using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class AssignmentDTO
    {
        public string AssignmentId { get; set; }
        public string OrganisationId { get; set; }
        public string Name { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public DateTime CreateDate { get; set; }
        public AssignmentEmployeeDTO[] AssignmentEmployee { get; set; }
    }
}
