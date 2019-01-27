using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IAbsenceRepository : IRepository<Absence>
    {
        Task<Absence[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Absence, bool>> condition = null, string searchKey = null);
        Task<int> GetAbsencesCountAsync(string searchKey = null, Expression<Func<Absence, bool>> condition = null);
    }
}
