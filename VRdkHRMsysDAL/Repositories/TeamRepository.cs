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
       
        public async Task<Team> GetForCalendarAsync(string id)
        {
            return await _context.Team.Include(t => t.Teamlead).FirstOrDefaultAsync(t=>t.TeamId == id);
        }
        
        public async Task<int> GetTeamsCountAsync(string searchKey = null, Expression<Func<Team, bool>> condition = null)
        {
            if(searchKey == null)
            {
                return condition != null ? await _context.Team.Where(condition).CountAsync() :
                                           await _context.Team.CountAsync();
            }

            return condition != null ? await _context.Team.Where(condition).Where(t => t.Employees.Count.ToString() == searchKey
                                                                                    || $"{t.Teamlead.FirstName} {t.Teamlead.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                                    || t.Name.ToLower().Contains(searchKey.ToLower())).CountAsync() :
                                       await _context.Team.Where(t => t.Employees.Count.ToString() == searchKey
                                                                   || $"{t.Teamlead.FirstName} {t.Teamlead.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                   || t.Name.ToLower().Contains(searchKey.ToLower())).CountAsync();
        }

        public async Task<Team[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Team, bool>> condition = null)
        {
            if(searchKey == null)
            {
                return condition != null ? await _context.Team.Include(t=> t.Employees).Include(t => t.Teamlead).Where(condition).Skip(pageNumber*pageSize).Take(pageSize).ToArrayAsync() : 
                                           await _context.Team.Include(t => t.Employees).Include(t => t.Teamlead).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
            }

            return condition != null ? await _context.Team.Include(t => t.Employees).Include(t => t.Teamlead).Where(condition).
                                                                                                              Where(t=>t.Employees.Count.ToString() == searchKey
                                                                                                                   || $"{t.Teamlead.FirstName} {t.Teamlead.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                                                                   || t.Name.ToLower().Contains(searchKey.ToLower())).
                                                                                                                   Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                                       await _context.Team.Include(t => t.Employees).Include(t => t.Teamlead).Where(t => t.Employees.Count.ToString() == searchKey
                                                                                                                   || $"{t.Teamlead.FirstName} {t.Teamlead.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                                                                   || t.Name.ToLower().Contains(searchKey.ToLower())).
                                                                                                                   Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task<Team[]> GetAsync(Expression<Func<Team, bool>> condition = null)
        {
            return condition != null ? await _context.Team.Include(t=>t.Employees).Include(t=>t.Teamlead).Where(condition).ToArrayAsync() : await _context.Team.Include(t => t.Teamlead).ToArrayAsync();
        }

        public async Task<Team> GetByIdAsync(string id)
        {
            return await _context.Team.Include(t=>t.Employees).Include(t=>t.Teamlead).FirstOrDefaultAsync(team => team.TeamId.Equals(id));
        }

        public async Task WriteChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task CreateAsync(Team entity, bool writeChanges)
        {
            _context.Team.Add(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(Team entity, bool writeChanges)
        {
            _context.Remove(_context.Team.FirstOrDefault(t=>t.TeamId==entity.TeamId));

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }
    }
}
