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
    public class VacationRequestRepository : IVacationRequestRepository
    {
        private readonly HRMSystemDbContext _context;

        public VacationRequestRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task<VacationRequest[]> GetAsync(Expression<Func<VacationRequest, bool>> condition = null)
        {
            return condition != null ? await _context.VacationRequest.Where(condition).ToArrayAsync() : await _context.VacationRequest.ToArrayAsync();          
        }

        public async Task<VacationRequest[]> GetWithEmployeeWithTeamAsync(Expression<Func<VacationRequest, bool>> condition = null)
        {
            return condition != null ? 
                await _context.VacationRequest.Include(r=>r.Employee).ThenInclude(emp=>emp.Team).Where(condition).OrderByDescending(req=>req.RequestStatus).ThenByDescending(req=>req.CreateDate).ToArrayAsync() 
              : await _context.VacationRequest.Include(r => r.Employee).ThenInclude(emp => emp.Team).OrderByDescending(req => req.RequestStatus).ThenByDescending(req => req.CreateDate).ToArrayAsync();
        }

        public async Task<VacationRequest> GetByIdAsync(string id)
        {
            return await _context.VacationRequest.FirstOrDefaultAsync(req => req.VacationId.Equals(id));
        }

        public async Task<VacationRequest> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            return await _context.VacationRequest.Include(req=>req.Employee).ThenInclude(emp=>emp.Team).FirstOrDefaultAsync(req => req.VacationId.Equals(id));
        }

        public async Task CreateAsync(VacationRequest entity)
        {
            _context.VacationRequest.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(VacationRequest entity)
        {
            _context.VacationRequest.Remove(entity);
            await _context.SaveChangesAsync();
        }      

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
