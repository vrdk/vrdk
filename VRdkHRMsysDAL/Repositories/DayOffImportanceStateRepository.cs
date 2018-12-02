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
    public class DayOffImportanceStateRepository : IDayOffImportanceStateRepository
    {
        private readonly HRMSystemDbContext _context;

        public DayOffImportanceStateRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(DayOffImportanceState entity)
        {
            _context.DayOffImportanceState.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DayOffImportanceState entity)
        {
            _context.DayOffImportanceState.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<DayOffImportanceState[]> GetAsync(Expression<Func<DayOffImportanceState, bool>> condition = null)
        {
            return condition != null ? await _context.DayOffImportanceState.Where(condition).ToArrayAsync() : await _context.DayOffImportanceState.ToArrayAsync();
        }

        public async Task<DayOffImportanceState> GetByIdAsync(string id)
        {
            return await _context.DayOffImportanceState.FirstOrDefaultAsync(ab => ab.DayOffImportanceStateId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
