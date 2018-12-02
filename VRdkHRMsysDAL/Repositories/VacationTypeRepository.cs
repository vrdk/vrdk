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
    public class VacationTypeRepository : IVacationTypeRepository
    {
        private readonly HRMSystemDbContext _context;

        public VacationTypeRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(VacationType entity)
        {
            _context.VacationType.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(VacationType entity)
        {
            _context.VacationType.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<VacationType[]> GetAsync(Expression<Func<VacationType, bool>> condition = null)
        {
            return condition != null ? await _context.VacationType.Where(condition).ToArrayAsync() : await _context.VacationType.ToArrayAsync();
        }

        public async Task<VacationType> GetByIdAsync(string id)
        {
            return await _context.VacationType.FirstOrDefaultAsync(ab => ab.VacationTypeId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
