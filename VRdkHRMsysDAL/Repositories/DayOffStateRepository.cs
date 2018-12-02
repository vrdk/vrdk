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
    public class DayOffStateRepository : IDayOffStateRepository
    {
        private readonly HRMSystemDbContext _context;

        public DayOffStateRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(DayOffState entity)
        {
            _context.DayOffState.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DayOffState entity)
        {
            _context.DayOffState.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<DayOffState[]> GetAsync(Expression<Func<DayOffState, bool>> condition = null)
        {
            return condition != null ? await _context.DayOffState.Where(condition).ToArrayAsync() : await _context.DayOffState.ToArrayAsync();
        }

        public async Task<DayOffState> GetByIdAsync(string id)
        {
            return await _context.DayOffState.FirstOrDefaultAsync(ab => ab.DayOffStateId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
