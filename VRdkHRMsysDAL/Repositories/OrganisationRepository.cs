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
    public class OrganisationRepository : IOrganisationRepository
    {
        private readonly HRMSystemDbContext _context;

        public OrganisationRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Organisation entity)
        {
            _context.Organisation.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Organisation entity)
        {
            _context.Organisation.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Organisation[]> GetAsync(Expression<Func<Organisation, bool>> condition = null)
        {
            return condition != null ? await _context.Organisation.Where(condition).ToArrayAsync() : await _context.Organisation.ToArrayAsync();
        }

        public async Task<Organisation> GetByIdAsync(string id)
        {
            return await _context.Organisation.FirstOrDefaultAsync(ab => ab.OrganisationId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
