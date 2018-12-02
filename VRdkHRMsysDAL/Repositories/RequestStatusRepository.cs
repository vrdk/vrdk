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
    public class RequestStatusRepository : IRequestStatusRepository
    {
        private readonly HRMSystemDbContext _context;

        public RequestStatusRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(RequestStatus entity)
        {
            _context.RequestStatus.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(RequestStatus entity)
        {
            _context.RequestStatus.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<RequestStatus[]> GetAsync(Expression<Func<RequestStatus, bool>> condition = null)
        {
            return condition != null ? await _context.RequestStatus.Where(condition).ToArrayAsync() : await _context.RequestStatus.ToArrayAsync();
        }

        public async Task<RequestStatus> GetByIdAsync(string id)
        {
            return await _context.RequestStatus.FirstOrDefaultAsync(st => st.RequestStatusId.Equals(id));
        }

        public async Task<RequestStatus> GetByNameAsync(string name)
        {
            return await _context.RequestStatus.FirstOrDefaultAsync(st => st.Name.Equals(name));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
