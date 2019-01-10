using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Assignment;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IAssignmentService
    {
        Task CreateAsync(AssignmentDTO assignment);
        Task<AssignmentDTO[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null);
        Task<AssignmentEmployeeDTO[]> GetByEmployeeIdAsync(string id);
        Task<AssignmentEmployeeDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
        Task<int> GetAssignmentsNumberAsync(string searchKey = null, Expression<Func<AssignmentEmployee, bool>> condition = null);
    }
}