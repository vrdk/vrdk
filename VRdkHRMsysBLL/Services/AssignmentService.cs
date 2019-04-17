using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
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

        public async Task<int> GetProfileAssignmentsCountAsync(Expression<Func<AssignmentEmployee, bool>> condition = null)
        {
            return await _assignmentRepository.GetProfileAssignmentsCountAsync(condition);
        }

        public async Task<int> GetAssignmentsCountAsync(string searchKey = null, Expression<Func<Assignment, bool>> condition = null)
        {
            return await _assignmentRepository.GetAssignmentsCountAsync(searchKey, condition);
        }

        public async Task<AssignmentListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Assignment, bool>> condition = null, string searchKey = null)
        {
            var assigns = await _assignmentRepository.GetPageAsync(pageNumber, pageSize, condition, searchKey);
            var assignments = assigns != null ? assigns.Select(a => new AssignmentListUnitDTO
            {
                AssignmentId = a.AssignmentId,
                EmployeesCount = a.AssignmentEmployee.Count(),
                BeginDate = a.BeginDate,
                EndDate = a.EndDate,
                Duration = a.Duration,
                Name = a.Name
            }).ToArray() : new AssignmentListUnitDTO[] { };

            return assignments;
        }

        public async Task<AssignmentEmployeeDTO[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0)
        {
            var assignments = await _assignmentRepository.GetProfilePageAsync(pageSize, id, pageNumber);
            return _mapHelper.MapCollection<AssignmentEmployee, AssignmentEmployeeDTO>(assignments);
        }

        public async Task CreateAsync(AssignmentDTO assignment, bool writeChanges = false)
        {
            var assignmentToAdd = _mapHelper.Map<AssignmentDTO, Assignment>(assignment);
            assignmentToAdd.AssignmentEmployee = _mapHelper.MapCollection<AssignmentEmployeeDTO, AssignmentEmployee>(assignment.AssignmentEmployee);
            await _assignmentRepository.CreateAsync(assignmentToAdd, writeChanges);
        }

        public async Task<AssignmentDTO[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null)
        {
            var assignments = await _assignmentRepository.GetWithEmployeeAsync(condition);
            return _mapHelper.NestedMapCollection<Assignment, AssignmentDTO, AssignmentEmployee, AssignmentEmployeeDTO, Employee, EmployeeDTO>(assignments);
        }

        public async Task<AssignmentDTO> GetByIdWithEmployeesAsync(string id)
        {
            var assignment = await _assignmentRepository.GetByIdWithEmployeesAsync(id);
            return _mapHelper.Map<Assignment, AssignmentDTO>(assignment);
        }

        public async Task DeleteAsync(AssignmentDTO entity, bool writeChanges = false)
        {
           await _assignmentRepository.DeleteAsync(_mapHelper.Map<AssignmentDTO, Assignment>(entity), writeChanges);
        }


        public async Task AddToAssignmentAsync(string[] employeeIds, string assignmentId, bool writeChanges = false)
        {
            await _assignmentRepository.AddToAssignmentAsync(employeeIds, assignmentId, writeChanges);
        }

        public async Task RemoveFromAssignmentAsync(string[] employeeIds, string assignmentId, bool writeChanges = false)
        {
           await _assignmentRepository.RemoveFromAssignmentAsync(employeeIds, assignmentId, writeChanges);
        }

        public async Task Update(AssignmentDTO entity, bool writeChanges = false)
        {
            var assignment = await _assignmentRepository.GetByIdAsync(entity.AssignmentId);
            if(assignment != null)
            {
                assignment.Name = entity.Name;
                assignment.BeginDate = entity.BeginDate;
                assignment.EndDate = entity.EndDate;
                assignment.Duration = entity.Duration;
            }

            if (writeChanges)
            {
                await _assignmentRepository.WriteChangesAsync();
            }        
        }
    }
}
