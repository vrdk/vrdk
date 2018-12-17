using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapHelper _mapHelper;

        public NotificationService(INotificationRepository notificationRepository, IMapHelper mapHelper)
        {
            _notificationRepository = notificationRepository;
            _mapHelper = mapHelper;
        }

        public async Task CreateAsync(NotificationDTO notification)
        {
            var notificationToAdd = _mapHelper.Map<NotificationDTO, Notification>(notification);
            await _notificationRepository.CreateAsync(notificationToAdd);
        }

        public async Task CreateRangeAsync(NotificationDTO[] notification)
        {
            var notificationsToAdd = _mapHelper.MapCollection<NotificationDTO, Notification>(notification);
            await _notificationRepository.CreateRangeAsync(notificationsToAdd);
        }
    }
}
