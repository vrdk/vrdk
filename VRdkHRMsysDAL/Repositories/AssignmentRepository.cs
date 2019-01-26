using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Contexts;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysDAL.Repositories
{
    public class AssignmentRepository : IAssignmentRepository
    {
        private readonly HRMSystemDbContext _context;

        public  AssignmentRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Assignment entity, bool writeChanges)
        {
            _context. Assignment.Add(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }       
        }

        public async Task DeleteAsync (Assignment entity, bool writeChanges)
        {
            _context.Assignment.Remove(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }           
        }

        public async Task<int> GetAssignmentsCountAsync(string searchKey = null, Expression<Func<Assignment, bool>> condition = null)
        {
            if(searchKey == null)
            {
                return condition != null ? await _context.Assignment.Where(condition).CountAsync() :
                                           await _context.Assignment.CountAsync();

            }

            return condition != null ? await _context.Assignment.Where(condition).Where(a=>a.Name.ToLower().Contains(searchKey.ToLower()) || a.AssignmentEmployee.Count.ToString() == searchKey).CountAsync() :
                                      await _context.Assignment.Where(a => a.Name.ToLower().Contains(searchKey.ToLower()) || a.AssignmentEmployee.Count.ToString() == searchKey).CountAsync();

        }

        public async Task<int> GetProfileAssignmentsCountAsync(Expression<Func<AssignmentEmployee, bool>> condition = null)
        {
            return condition != null ? await _context.AssignmentEmployee.Where(condition).CountAsync() :
                                       await _context.AssignmentEmployee.CountAsync();
                                      
           
        }

        public async Task<Assignment[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Assignment, bool>> condition = null, string searchKey = null)
        {
            if(searchKey == null)
            {
                return condition != null ? await _context.Assignment.Include(a => a.AssignmentEmployee).Where(condition).OrderByDescending(a=>a.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                                           await _context.Assignment.Include(a => a.AssignmentEmployee).OrderByDescending(a => a.CreateDate).Skip(pageNumber*pageSize).Take(pageSize).ToArrayAsync();
            }

            return condition != null ? await _context.Assignment.Include(a => a.AssignmentEmployee).Where(condition).
                                                                 Where(a=>a.AssignmentEmployee.Count().ToString().Contains(searchKey) || a.Name.ToLower().Contains(searchKey.ToLower())).
                                                                 OrderByDescending(a => a.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                                       await _context.Assignment.Include(a => a.AssignmentEmployee).Where(a => a.AssignmentEmployee.Count().ToString().Contains(searchKey) || a.Name.ToLower().Contains(searchKey.ToLower())).
                                                                 OrderByDescending(a => a.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task<AssignmentEmployee[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0)
        {
            return await _context.AssignmentEmployee.Include(ae => ae.Assignment).Where(a => a.EmployeeId == id).OrderByDescending(a => a.Assignment.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task< Assignment[]> GetAsync(Expression<Func<Assignment, bool>> condition = null)
        {
            return condition != null ? await _context.Assignment.Where(condition).ToArrayAsync() : await _context.Assignment.ToArrayAsync();
        }

        public async Task<Assignment[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null)
        {
            return condition != null ? await _context.Assignment.Include(a=>a.AssignmentEmployee).ThenInclude(ae=>ae.Employee).Where(condition).ToArrayAsync()
                                     : await _context.Assignment.Include(a => a.AssignmentEmployee).ThenInclude(ae => ae.Employee).ToArrayAsync();
        }

        public async Task<Assignment> GetByIdWithEmployeesAsync(string id)
        {
            return await _context.Assignment.Include(a => a.AssignmentEmployee).ThenInclude(ae => ae.Employee).ThenInclude(emp=>emp.EmployeeBalanceResiduals).FirstOrDefaultAsync(a=>a.AssignmentId == id);
        }

        public async Task< Assignment> GetByIdAsync(string id)
        {
            return await _context. Assignment.FirstOrDefaultAsync(a => a. AssignmentId == id);
        }

        public async Task AddToAssignmentAsync(string[] employeeIds, string assignmentId, bool writeChanges)
        {
            _context.AssignmentEmployee.AddRange(employeeIds.Select(id => new AssignmentEmployee
            {
                AssignmentId = assignmentId,
                EmployeeId = id,
                RowId = Guid.NewGuid().ToString()
            }));

            if (writeChanges)
            {
                await _context.SaveChangesAsync();
            }            
        }

        public async Task RemoveFromAssignmentAsync(string[] employeeIds, string assignmentId, bool writeChanges)
        {
            _context.AssignmentEmployee.RemoveRange(_context.AssignmentEmployee.Where(ae => employeeIds.Contains(ae.EmployeeId) && ae.AssignmentId == assignmentId));

            if (writeChanges)
            {
                await UpdateAsync();
            }           
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
