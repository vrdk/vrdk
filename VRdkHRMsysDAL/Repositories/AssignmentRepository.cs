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

        public async Task CreateAsync (Assignment entity)
        {
            _context. Assignment.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync (Assignment entity)
        {
            _context. Assignment.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task< Assignment[]> GetAsync(Expression<Func< Assignment, bool>> condition = null)
        {
            return condition != null ? await _context.Assignment.Where(condition).ToArrayAsync() : await _context.Assignment.ToArrayAsync();
        }

        public async Task<Assignment[]> GetWithEmployeeAsync(Expression<Func<Assignment, bool>> condition = null)
        {
            return condition != null ? await _context.Assignment.Include(a=>a.Employees).ThenInclude(ae=>ae.Employee).Where(condition).ToArrayAsync()
                                     : await _context.Assignment.Include(a => a.Employees).ThenInclude(ae => ae.Employee).ToArrayAsync();
        }

        public async Task< Assignment> GetByIdAsync(string id)
        {
            return await _context. Assignment.FirstOrDefaultAsync(ab => ab. AssignmentId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
