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
    public class EmployeeBalanceResidualsRepository : IEmployeeBalanceResidualsRepository
    {
        private readonly HRMSystemDbContext _context;

        public EmployeeBalanceResidualsRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(EmployeeBalanceResiduals entity)
        {
            _context.EmployeeBalanceResiduals.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(EmployeeBalanceResiduals entity)
        {
            _context.EmployeeBalanceResiduals.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<EmployeeBalanceResiduals[]> GetAsync(Expression<Func<EmployeeBalanceResiduals, bool>> condition = null)
        {
            return condition != null ? await _context.EmployeeBalanceResiduals.Where(condition).ToArrayAsync() : await _context.EmployeeBalanceResiduals.ToArrayAsync();
        }

        public async Task<EmployeeBalanceResiduals> GetByIdAsync(string id)
        {
            return await _context.EmployeeBalanceResiduals.FirstOrDefaultAsync(ab => ab.ResidualId.Equals(id));
        }

        public async Task<EmployeeBalanceResiduals> GetByEmployeeIdAsync(string id, string type)
        {
            return await _context.EmployeeBalanceResiduals.FirstOrDefaultAsync(res=>res.EmployeeId.Equals(id) && res.Name.Equals(type));
        }

        public async Task CreateRangeAsync(EmployeeBalanceResiduals[] entities)
        {
            _context.AddRange(entities);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
