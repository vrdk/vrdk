using System;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Absence
    {
        public string AbsenceId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime AbsenceDate { get; set; }

        public Employee Employee { get; set; }
    }
}
