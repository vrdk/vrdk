using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Assignment;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IAssignmentService
    {
        Task Update(AssignmentDTO entity);
        Task DeleteAsync(AssignmentDTO entity);
        Task CreateAsync(AssignmentDTO assignment);
        Task AddToAssignmentAsync(string[] employeeIds, string assignmentId);
        Task RemoveFromAssignmentAsync(string[] employeeIds, string assignmentId);
        Task<AssignmentDTO> GetByIdWithEmployeesAsync(string id);
        Task<AssignmentDTO[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null);
        Task<int> GetAssignmentsCountAsync(string searchKey = null, Expression<Func<Assignment, bool>> condition = null);
        Task<AssignmentEmployeeDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
        Task<int> GetProfileAssignmentsCountAsync(Expression<Func<AssignmentEmployee, bool>> condition = null);
        Task<AssignmentListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Assignment, bool>> condition = null, string searchKey = null);
    }
}