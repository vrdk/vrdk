using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Assignment;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class AssignmentService : IAssignmentService
    {
        private readonly IAssignmentRepository _assignmentRepository;
        
        private readonly IMapHelper _mapHelper;

        public AssignmentService(IAssignmentRepository assignmentRepository,
                                 IMapHelper mapHelper)
        {
            _assignmentRepository = assignmentRepository;
            _mapHelper = mapHelper;
        }

        public async Task<int> GetAssignmentsNumberAsync(string searchKey = null, Expression<Func<AssignmentEmployee, bool>> condition = null)
        {
            return await _assignmentRepository.GetAssignmentsCountAsync(condition, searchKey);
        }

        public async Task<AssignmentEmployeeDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0)
        {
            var requests = await _assignmentRepository.GetProfilePageAsync(pageSize, id, pageNumber);
            return _mapHelper.MapCollection<AssignmentEmployee, AssignmentEmployeeDTO>(requests);
        }

        public async Task CreateAsync(AssignmentDTO assignment)
        {
            var assignmentToAdd = _mapHelper.Map<AssignmentDTO, Assignment>(assignment);
            assignmentToAdd.Employees = _mapHelper.MapCollection<AssignmentEmployeeDTO, AssignmentEmployee>(assignment.Employees);
            await _assignmentRepository.CreateAsync(assignmentToAdd);
        }

        public async Task<AssignmentDTO[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null)
        {
            var assignments = await _assignmentRepository.GetWithEmployeeAsync(condition);
            return _mapHelper.NestedMapCollection<Assignment, AssignmentDTO, AssignmentEmployee, AssignmentEmployeeDTO, Employee, EmployeeDTO>(assignments);
        }

        public async Task<AssignmentEmployeeDTO[]> GetByEmployeeIdAsync(string id)
        {
            var assignments = await _assignmentRepository.GetByEmployeeIdAsync(id);
            return _mapHelper.MapCollection<AssignmentEmployee, AssignmentEmployeeDTO>(assignments);
        }
    }
}
