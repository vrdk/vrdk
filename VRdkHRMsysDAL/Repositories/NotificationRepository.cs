using Microsoft.EntityFrameworkCore;
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

        public async Task<Notification[]> GetAsync(Expression<Func<Notification, bool>> condition = null)
        {
            return condition != null ? await _context.Notification.Where(condition).ToArrayAsync() : await _context.Notification.ToArrayAsync();
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
