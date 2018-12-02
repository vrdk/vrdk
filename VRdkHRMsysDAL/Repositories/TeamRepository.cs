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
    public class TeamRepository : ITeamRepository
    {
        private readonly HRMSystemDbContext _context;

        public TeamRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Team entity)
        {
            _context.Team.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Team entity)
        {
            _context.Team.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<Team[]> GetAsync(Expression<Func<Team, bool>> condition = null)
        {
            return condition != null ? await _context.Team.Where(condition).ToArrayAsync() : await _context.Team.ToArrayAsync();
        }

        public async Task<Team> GetByIdAsync(string id)
        {
            return await _context.Team.FirstOrDefaultAsync(ab => ab.TeamId.Equals(id));
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
