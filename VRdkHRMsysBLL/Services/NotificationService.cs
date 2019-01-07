using System;
using System.Linq.Expressions;
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

        public async Task CheckNotificationAsync(string notificationId)
        {
            await _notificationRepository.ChangeStateAsync(notificationId);
        }

        public async Task<NotificationDTO> GetByIdAsync(string notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);
            return _mapHelper.Map<Notification, NotificationDTO>(notification);
        }

        public async Task<NotificationDTO[]> GetPageAsync(int pageNumber,int pageSize, Expression<Func<Notification, bool>> condition = null, string searchKey = null)
        {
            var notifications = await _notificationRepository.GetPageAsync(pageNumber,pageSize, condition, searchKey ?? null);
            return _mapHelper.MapCollection<Notification, NotificationDTO>(notifications);
        }
        public async Task<NotificationDTO[]> GetAsync(Expression<Func<Notification, bool>> condition = null)
        {
            var notifications = await _notificationRepository.GetAsync(condition);
            return _mapHelper.MapCollection<Notification, NotificationDTO>(notifications);
        }

        public async Task<int> GetNotificationsNumber(Expression<Func<Notification, bool>> condition = null,string searchKey = null)
        {
            return await _notificationRepository.GetNotificationsCountAsync(condition, searchKey);
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
