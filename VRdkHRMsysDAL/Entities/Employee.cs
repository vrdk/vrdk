using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Absences = new HashSet<Absence>();
            Assignments = new HashSet<AssignmentEmployee>();
            DayOffs = new HashSet<DayOff>();
            EmployeeBalanceResiduals = new HashSet<EmployeeBalanceResiduals>();
            SickLeaves = new HashSet<SickLeaveRequest>();
            Transaction = new HashSet<Transaction>();
            TeamNavigation = new HashSet<Team>();
            Notifications = new HashSet<Notification>();
            Vacations = new HashSet<VacationRequest>();
            WorkDays = new HashSet<WorkDay>();
        }

        public string EmployeeId { get; set; }
        public string OrganisationId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PostId { get; set; }
        public bool State { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? DismissalDate { get; set; }
        public string PersonalEmail { get; set; }
        public string WorkEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string TeamId { get; set; }

        public Organisation Organisation { get; set; }
        public Post Post { get; set; }
        public Team Team { get; set; }
        public ICollection<Absence> Absences { get; set; }
        public ICollection<AssignmentEmployee> Assignments { get; set; }
        public ICollection<DayOff> DayOffs { get; set; }
        public virtual ICollection<Team> TeamNavigation { get; set; }
        public ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TimeManagementRecord> TimeManagmentRecords { get; set; }
        public ICollection<EmployeeBalanceResiduals> EmployeeBalanceResiduals { get; set; }
        public ICollection<SickLeaveRequest> SickLeaves { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<VacationRequest> Vacations { get; set; }
        public ICollection<WorkDay> WorkDays { get; set; }
    }
}
