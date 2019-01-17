using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Employee
    {
        public Employee()
        {
            Absence = new HashSet<Absence>();
            Assignments = new HashSet<AssignmentEmployee>();
            DayOff = new HashSet<DayOff>();
            EmployeeBalanceResiduals = new HashSet<EmployeeBalanceResiduals>();
            SickLeaveRequest = new HashSet<SickLeaveRequest>();
            Transaction = new HashSet<Transaction>();
            TeamNavigation = new HashSet<Team>();
            Notification = new HashSet<Notification>();
            VacationRequest = new HashSet<VacationRequest>();
            WorkDay = new HashSet<WorkDay>();
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
        public ICollection<Absence> Absence { get; set; }
        public ICollection<AssignmentEmployee> Assignments { get; set; }
        public ICollection<DayOff> DayOff { get; set; }
        public virtual ICollection<Team> TeamNavigation { get; set; }
        public ICollection<Notification> Notification { get; set; }
        public ICollection<EmployeeBalanceResiduals> EmployeeBalanceResiduals { get; set; }
        public ICollection<SickLeaveRequest> SickLeaveRequest { get; set; }
        public ICollection<Transaction> Transaction { get; set; }
        public ICollection<VacationRequest> VacationRequest { get; set; }
        public ICollection<WorkDay> WorkDay { get; set; }
    }
}
