﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Notification;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{ 
    public interface INotificationService
    {
        Task CreateAsync(NotificationDTO notification);
        Task CreateRangeAsync(NotificationDTO[] notification);
        Task<NotificationDTO[]> GetAsync(Expression<Func<Notification, bool>> condition = null);
        Task<NotificationDTO[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Notification, bool>> condition = null, string searchKey = null);
        Task<NotificationDTO> GetByIdAsync(string notificationId);
        Task CheckNotificationAsync(string notificationId);
        Task<int> GetNotificationsNumber(Expression<Func<Notification, bool>> condition = null, string searchKey = null);
    }
}