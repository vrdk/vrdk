using System;

namespace VRdkHRMsysBLL.DTOs
{
    public class NotificationDTO
    {
        public string NotificationId { get; set; }
        public string EmployeeId { get; set; }
        public string OrganisationId { get; set; }
        public string Description { get; set; }
        public string NotificationRange { get; set; }
        public string NotificationType { get; set; }
        public string RelatedTeamId { get; set; }
        public DateTime RelatedDate { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
