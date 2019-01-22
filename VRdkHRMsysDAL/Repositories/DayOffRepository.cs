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
    public class DayOffRepository : IDayOffRepository
    {
        private readonly HRMSystemDbContext _context;

        public DayOffRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(DayOff entity)
        {
            _context.DayOff.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(DayOff entity)
        {
            _context.DayOff.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<DayOff[]> GetAsync(Expression<Func<DayOff, bool>> condition = null)
        {
            return condition != null ? await _context.DayOff.Where(condition).ToArrayAsync() : await _context.DayOff.ToArrayAsync(); 
        }

        public async Task<DayOff> GetByIdAsync(string id)
        {
            return await _context.DayOff.Include(w => w.Employee).FirstOrDefaultAsync(d=> d.DayOffId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
