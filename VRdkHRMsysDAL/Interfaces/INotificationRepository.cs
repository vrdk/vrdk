using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface INotificationRepository : IRepository<Notification>
    {
        Task CreateRangeAsync(Notification[] entities);
    }
}
