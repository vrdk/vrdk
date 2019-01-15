using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IAssignmentRepository : IRepository<Assignment>
    {
        Task AddToAssignmentAsync(string[] employeeIds, string assignmentId);
        Task<Assignment[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null);
        Task RemoveFromAssignmentAsync(string[] employeeIds, string assignmentId);
        Task<Assignment> GetByIdWithEmployeesAsync(string id);
        Task<int> GetAssignmentsCountAsync(string searchKey = null, Expression<Func<Assignment, bool>> condition = null);
        Task<AssignmentEmployee[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
        Task<int> GetProfileAssignmentsCountAsync(Expression<Func<AssignmentEmployee, bool>> condition = null);
        Task<Assignment[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Assignment, bool>> condition = null, string searchKey = null);
    }
}
