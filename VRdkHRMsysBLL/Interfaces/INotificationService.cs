using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Notification;

namespace VRdkHRMsysBLL.Interfaces
{ 
    public interface INotificationService
    {
        Task CreateAsync(NotificationDTO notification);
        Task CreateRangeAsync(NotificationDTO[] notification);
    }
}