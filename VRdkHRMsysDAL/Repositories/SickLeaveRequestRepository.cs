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
    public class SickLeaveRequestRepository : ISickLeaveRequestRepository
    {
        private readonly HRMSystemDbContext _context;

        public SickLeaveRequestRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task<SickLeaveRequest> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            return await _context.SickLeaveRequest.Include(req => req.Employee).ThenInclude(emp => emp.Team).FirstOrDefaultAsync(req => req.SickLeaveId.Equals(id));
        }

        public async Task<SickLeaveRequest[]> GetAsync(Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            return condition != null ? await _context.SickLeaveRequest.Where(condition).ToArrayAsync() : await _context.SickLeaveRequest.ToArrayAsync();
        }

        public async Task<SickLeaveRequest> GetByIdAsync(string id)
        {
            return await _context.SickLeaveRequest.FirstOrDefaultAsync(req => req.SickLeaveId.Equals(id));
        }

        public async Task CreateAsync(SickLeaveRequest entity)
        {
            _context.SickLeaveRequest.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SickLeaveRequest entity)
        {
            _context.SickLeaveRequest.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
