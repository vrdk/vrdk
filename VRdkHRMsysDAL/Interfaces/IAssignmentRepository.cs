using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task<Assignment[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null);
    }
}
