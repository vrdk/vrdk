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
    }
}