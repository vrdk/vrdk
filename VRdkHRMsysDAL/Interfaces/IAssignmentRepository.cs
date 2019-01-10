using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<Assignment[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null);
        Task<AssignmentEmployee[]> GetByEmployeeIdAsync(string id);
        Task<AssignmentEmployee[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
        Task<int> GetAssignmentsCountAsync(Expression<Func<AssignmentEmployee, bool>> condition = null, string searchKey = null);
    }
}
