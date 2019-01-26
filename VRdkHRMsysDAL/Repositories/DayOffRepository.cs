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

        public async Task CreateAsync(DayOff entity, bool writeChanges)
        {
            _context.DayOff.Add(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }
        }

        public async Task DeleteAsync(DayOff entity, bool writeChanges)
        {
            _context.DayOff.Remove(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }
        }

        public async Task<DayOff[]> GetAsync(Expression<Func<DayOff, bool>> condition = null)
        {
            return condition != null ? await _context.DayOff.Where(condition).ToArrayAsync() : await _context.DayOff.ToArrayAsync(); 
        }

        public async Task<DayOff> GetByDateAsync(DateTime date, string employeeId)
        {
            return await _context.DayOff.FirstOrDefaultAsync(d => d.DayOffDate == date && d.EmployeeId == employeeId);
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
