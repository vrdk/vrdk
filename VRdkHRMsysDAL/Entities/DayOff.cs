﻿using System;

namespace VRdkHRMsysDAL.Entities
{
    public partial class DayOff
    {
        public string DayOffId { get; set; }
        public string EmployeeId { get; set; }
        public string DayOffState { get; set; }
        public string DayOffImportance{ get; set; }
        public string Comment { get; set; }
        public DateTime DayOffDate { get; set; }
        public DateTime? ProcessDate { get; set; }
        public Employee Employee { get; set; }
    }
}
