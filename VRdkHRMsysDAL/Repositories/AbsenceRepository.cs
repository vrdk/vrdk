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

        public async Task<int> GetAbsencesCountAsync(string searchKey = null, Expression<Func<Absence, bool>> condition = null)
        {
            if (searchKey == null)
            {
                return condition != null ? await _context.Absence.Where(condition).CountAsync() :
                                           await _context.Absence.CountAsync();

            }

            return condition != null ? await _context.Absence.Where(condition).Where(a => $"{a.Employee.FirstName} {a.Employee.LastName}".ToLower().Contains(searchKey.ToLower()) || a.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).CountAsync() :
                                      await _context.Absence.Where(a => $"{a.Employee.FirstName} {a.Employee.LastName}".ToLower().Contains(searchKey.ToLower()) || a.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).CountAsync();

        }

        public async Task<Absence[]> GetPageAsync(int pageNumber, int pageSize, Expression<Func<Absence, bool>> condition = null, string searchKey = null)
        {
            if (searchKey == null)
            {
                return condition != null ? await _context.Absence.Include(a => a.Employee).ThenInclude(e=>e.Team).Where(condition).OrderByDescending(a => a.AbsenceDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                                           await _context.Absence.Include(a => a.Employee).ThenInclude(e => e.Team).OrderByDescending(a => a.AbsenceDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
            }

            return condition != null ? await _context.Absence.Include(a => a.Employee).ThenInclude(e => e.Team).Where(condition).
                                                                 Where(a => $"{a.Employee.FirstName} {a.Employee.LastName}".ToLower().Contains(searchKey.ToLower()) || a.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).
                                                                 OrderByDescending(a => a.AbsenceDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                                       await _context.Absence.Include(a => a.Employee).ThenInclude(e => e.Team).Where(a => $"{a.Employee.FirstName} {a.Employee.LastName}".ToLower().Contains(searchKey.ToLower()) || a.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).
                                                                 OrderByDescending(a => a.AbsenceDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task CreateAsync(Absence entity, bool writeChanges)
        {
            _context.Absence.Add(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(Absence entity, bool writeChanges)
        {
            _context.Absence.Remove(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
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

        public async Task WriteChangesAsync()
        {
           await _context.SaveChangesAsync();
        }
    }
}
