namespace VRdkHRMsystem.Models.Profile
{
    public class NotificationListViewModel
    {
        public int Count { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public NotificationViewModel[] Notifications { get; set; }
    }
}
