﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Contexts;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysDAL.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly HRMSystemDbContext _context;

        public NotificationRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetNotificationsCountAsync(Expression<Func<Notification, bool>> condition = null, string searchKey = null)
        {
            return searchKey == null ? await _context.Notification.Where(condition).CountAsync() : 
                                       await _context.Notification.Where(condition).Where(note => note.NotificationDate.ToString().Contains(searchKey) || note.Description.Contains(searchKey)).CountAsync();
        }

        public async Task<Notification[]> GetPageAsync(int pageNumber,int pageSize, Expression<Func<Notification, bool>> condition = null, string searchKey = null)
        {
            if(searchKey == null)
            {
                return condition != null
                                 ? await _context.Notification.Where(condition).Skip(pageNumber * pageSize).Take(pageSize).OrderByDescending(note => note.NotificationDate).ToArrayAsync()
                                 : await _context.Notification.Where(condition).Skip(pageNumber * pageSize).Take(pageSize).OrderByDescending(note => note.NotificationDate).ToArrayAsync();
            }

            return condition != null
                                 ? await _context.Notification.Where(condition).Where(note=>note.Description.ToLower().Contains(searchKey.ToLower())).Skip(pageNumber * pageSize).Take(pageSize).OrderByDescending(note => note.NotificationDate).ToArrayAsync()
                                 : await _context.Notification.Where(condition).Where(note =>note.Description.ToLower().Contains(searchKey.ToLower())).Skip(pageNumber * pageSize).Take(pageSize).OrderByDescending(note => note.NotificationDate).ToArrayAsync();
        }

        public async Task<Notification[]> GetAsync(Expression<Func<Notification, bool>> condition = null)
        {
            return condition != null ? await _context.Notification.Where(condition).OrderByDescending(note=>note.NotificationDate).ToArrayAsync() : await _context.Notification.OrderByDescending(note => note.NotificationDate).ToArrayAsync();
        }

        public async Task ChangeStateAsync(string id)
        {
            var notification = await _context.Notification.FirstOrDefaultAsync(note => note.NotificationId == id);
            if(notification != null && notification.IsChecked != true)
            {
                notification.IsChecked = true;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Notification> GetByIdAsync(string id)
        {
            return await _context.Notification.FirstOrDefaultAsync(nt=> nt.NotificationId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Notification entity)
        {
            _context.Notification.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CreateRangeAsync(Notification[] entities)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Notification entity)
        {
            _context.Notification.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}
