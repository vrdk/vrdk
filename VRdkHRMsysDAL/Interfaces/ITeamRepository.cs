using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Team, bool>> condition = null);
        Task<Team> GetForCalendarAsync(string id);
        Task<int> GetTeamsCountAsync(string searchKey = null, Expression<Func<Team, bool>> condition = null);
    }
}
