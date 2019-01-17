using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Contexts;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysDAL.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSystemDbContext _context;

        public EmployeeRepository(HRMSystemDbContext context)
        {
            _context = context;
        }                                                                                                              

        public async Task<Employee[]> GetAsync(Expression<Func<Employee, bool>> condition = null)
        {
            return condition != null ? await _context.Employee.Where(condition).ToArrayAsync() : await _context.Employee.ToArrayAsync();
        }

        public async Task<Employee[]> GetWithTeamAsync(Expression<Func<Employee, bool>> condition = null)
        {
            return condition != null ? await _context.Employee.Where(condition).Include(emp => emp.Team).ToArrayAsync() : await _context.Employee.Include(emp => emp.Team).ToArrayAsync();
        }

        public async Task<int> GetEmployeesCount(Expression<Func<Employee, bool>> condition = null, string searchKey = null)
        {
            return searchKey == null ? await _context.Employee.Where(condition).CountAsync() :
                                       await _context.Employee.Where(condition).Where(emp => emp.Team.Name.ToLower().Contains(searchKey.ToLower())
                                                                                || $"{emp.FirstName} {emp.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                                || emp.EmployeeBalanceResiduals.Any(res => res.ResidualBalance.ToString().Contains(searchKey))).CountAsync();
        }

        public async Task<Employee[]> GetPageAsync(int pageNumber, int pageSize, string searchKey, Expression<Func<Employee, bool>> condition = null)
        {
            if(searchKey == null)
            {
                return condition != null ? await _context.Employee.Where(condition).Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).OrderBy(emp=>emp.Team.Name).ToArrayAsync() :
                                           await _context.Employee.Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).OrderBy(emp => emp.Team.Name).ToArrayAsync();
            }
                return condition != null ? await _context.Employee.Where(condition).Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).
                                                                   Where(emp=>emp.Team.Name.ToLower().Contains(searchKey.ToLower())
                                                                   || $"{emp.FirstName} {emp.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                   || emp.EmployeeBalanceResiduals.Any(res=>res.ResidualBalance.ToString().Contains(searchKey))
                                                                   || emp.TeamNavigation.Any(t => t.Name.ToLower().Contains(searchKey.ToLower()))).OrderBy(emp => emp.Team.Name).Skip(pageNumber*pageSize).Take(pageSize).ToArrayAsync() :
                                           await _context.Employee.Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).
                                                                   Where(emp => emp.Team.Name.ToLower().Contains(searchKey.ToLower())
                                                                   || $"{emp.FirstName} {emp.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                   || emp.EmployeeBalanceResiduals.Any(res => res.ResidualBalance.ToString().Contains(searchKey))
                                                                   || emp.TeamNavigation.Any(t => t.Name.ToLower().Contains(searchKey.ToLower()))).
                                                                   OrderBy(emp => emp.Team.Name).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            return await _context.Employee.FirstOrDefaultAsync(em => em.EmployeeId == id);
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            return await _context.Employee.FirstOrDefaultAsync(em => em.WorkEmail == email);
        }

        public async Task<Employee> GetByIdWithResidualsAsync(string id)
        {
            return await _context.Employee.Include(x => x.EmployeeBalanceResiduals).FirstOrDefaultAsync(em => em.EmployeeId == id);
        }

        public async Task<Employee> GetByIdWithTeamWithResidualsAsync(string id)
        {
            return await _context.Employee.Include(x => x.EmployeeBalanceResiduals).Include(x => x.Team).FirstOrDefaultAsync(em => em.EmployeeId == id);
        }

        public async Task<Employee> GetByEmailWithTeamWithResidualsAsync(string email)
        {
            return await _context.Employee.Include(x => x.EmployeeBalanceResiduals).Include(x => x.Team).FirstOrDefaultAsync(em => em.WorkEmail == email);
        }

        public async Task<Employee> GetByEmailWithTeamAsync(string email)
        {
            return await _context.Employee.Include(x => x.Team).FirstOrDefaultAsync(em => em.WorkEmail == email);
        }

        public async Task<Employee> GetByIdWithTeamAsync(string id)
        {
            return await _context.Employee.Include(x => x.Team).FirstOrDefaultAsync(em => em.EmployeeId == id);
        }

        public async Task CreateAsync(Employee entity)
        {
            _context.Employee.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Employee entity)
        {
            _context.Employee.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity)
        {
            _context.Employee.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
