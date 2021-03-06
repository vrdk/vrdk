﻿using System;

namespace VRdkHRMsystem.Models.SharedModels
{
    public class CalendarEmployeeViewModel
    {
        public string EmployeeId { get; set; }
        public string TeamId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public CalendarVacationViewModel[] Vacations { get; set; }
        public CalendarSickleaveViewModel[] SickLeaves { get; set; }
        public CalendarAssignmentEmployeeViewModel[] Assignments { get; set; }
        public CalendarAbsenceViewModel[] Absences { get; set; }
        public CalendarWorkDayViewModel[] WorkDays { get; set; }
        public CalendarDayOffViewModel[] DayOffs { get; set; }
    }

    public class CalendarVacationViewModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }       
    }

    public class CalendarSickleaveViewModel
    {
        public DateTime CreateDate { get; set; }
        public DateTime CloseDate { get; set; }
    }

    public class CalendarAssignmentEmployeeViewModel
    {
        public CalendarAssignmentViewModel Assignment{ get; set; }
    }

    public class CalendarAssignmentViewModel
    {
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class CalendarAbsenceViewModel
    {
        public DateTime AbsenceDate { get; set; }
    }

    public class CalendarDayOffViewModel
    {
        public string DayOffId { get; set; }
        public DateTime DayOffDate { get; set; }
        public string DayOffState { get; set; }
        public string DayOffImportance { get; set; }
        public string Comment { get; set; }
    }

    public class CalendarWorkDayViewModel
    {
        public string WorkDayId { get; set; }
        public DateTime WorkDayDate { get; set; }
        public TimeSpan TimeFrom { get; set; }
        public TimeSpan TimeTo { get; set; }
    }
}
