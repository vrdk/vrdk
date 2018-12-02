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

        public async Task CreateAsync(Absence entity)
        {
            _context.Absence.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Absence entity)
        {
            _context.Absence.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Absence[]> GetAsync(Expression<Func<Absence, bool>> condition = null)
        {
            return condition != null ? await _context.Absence.Where(condition).ToArrayAsync() : await _context.Absence.ToArrayAsync();
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
