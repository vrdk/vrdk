﻿using System;
using System.Collections.Generic;

namespace VRdkHRMsysDAL.Entities
{
    public partial class Notification
    {
        public string NotificationId { get; set; }
        public string EmployeeId { get; set; }
        public string OrganisationId { get; set; }
        public string Description { get; set; }
        public string NotificationType { get; set; }
        public string NotificationRange { get; set; }
        public string RelatedTeamId { get; set; }
        public DateTime RelatedDate{ get; set; }
        public DateTime NotificationDate { get; set; }

        public Employee Employee { get; set; }
        public Organisation Organisation { get; set; }
    }
}
