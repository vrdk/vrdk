using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task CreateRangeAsync(Notification[] entities);
        Task ChangeStateAsync(string id);
        Task<Notification[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Notification, bool>> condition = null, string searchKey = null);
        Task<int> GetNotificationsCountAsync(Expression<Func<Notification, bool>> condition = null, string searchKey = null);
    }
}
