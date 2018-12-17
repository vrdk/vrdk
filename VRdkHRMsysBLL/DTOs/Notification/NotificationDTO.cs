using System;

namespace VRdkHRMsysBLL.DTOs.Notification
{
    public class NotificationDTO
    {
        public string NotificationId { get; set; }
        public string EmployeeId { get; set; }
        public string OrganisationId { get; set; }
        public string Description { get; set; }
        public string NotificationType { get; set; }
        public bool IsChecked { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
