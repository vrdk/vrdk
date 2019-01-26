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
    public class AbsenceRepository : IAbsenceRepository
    {
        private readonly HRMSystemDbContext _context;

        public AbsenceRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Absence entity, bool writeChanges)
        {
            _context.Absence.Add(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }
        }

        public async Task DeleteAsync(Absence entity, bool writeChanges)
        {
            _context.Absence.Remove(entity);

            if (writeChanges)
            {
                await UpdateAsync();
            }
        }

        public async Task<Absence[]> GetAsync(Expression<Func<Absence, bool>> condition = null)
        {
            return condition != null ? await _context.Absence.Include(a=>a.Employee).Where(condition).ToArrayAsync() : await _context.Absence.Include(a => a.Employee).ToArrayAsync();
        }

        public async Task<Absence> GetByIdAsync(string id)
        {
            return await _context.Absence.FirstOrDefaultAsync(ab => ab.AbsenceId.Equals(id));
        }

        public async Task UpdateAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
