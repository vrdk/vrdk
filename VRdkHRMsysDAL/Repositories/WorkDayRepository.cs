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
    public class WorkDayRepository : IWorkDayRepository
    {
        private readonly HRMSystemDbContext _context;

        public WorkDayRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(WorkDay entity, bool writeChanges)
        {
            _context.WorkDay.Add(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }
        }

        public async Task DeleteAsync(WorkDay entity, bool writeChanges)
        {
            _context.WorkDay.Remove(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }
        }

        public async Task<WorkDay[]> GetAsync(Expression<Func<WorkDay, bool>> condition = null)
        {
            return condition != null ? await _context.WorkDay.Where(condition).ToArrayAsync() : await _context.WorkDay.ToArrayAsync();
        }

        public async Task<WorkDay> GetByIdAsync(string id)
        {
            return await _context.WorkDay.Include(w=>w.Employee).FirstOrDefaultAsync(w => w.WorkDayId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
