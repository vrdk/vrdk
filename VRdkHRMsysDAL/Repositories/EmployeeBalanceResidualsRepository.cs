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

        public async Task CreateAsync(EmployeeBalanceResiduals entity, bool writeChanges)
        {
            _context.EmployeeBalanceResiduals.Add(entity);
            
            if(writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(EmployeeBalanceResiduals entity, bool writeChanges)
        {
            _context.EmployeeBalanceResiduals.Remove(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
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

        public async Task CreateRangeAsync(EmployeeBalanceResiduals[] entities, bool writeChanges)
        {
            _context.AddRange(entities);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }
        public async Task WriteChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
