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
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly HRMSystemDbContext _context;

        public EmployeeRepository(HRMSystemDbContext context)
        {
            _context = context;  
        }

        public async Task<Employee[]> GetForCalendarAsync(string teamId, string teamleadId, int month, int year, string personalId = null)
        {
            return await _context.Employee.Where(e => e.TeamId == teamId || e.EmployeeId == teamleadId).OrderBy(e=> personalId == null ? e.EmployeeId == teamleadId? 0 : 1 : e.EmployeeId == personalId ? 0 : 1).Select(e => new Employee
            {
                EmployeeId = e.EmployeeId,
                FirstName = e.FirstName,
                LastName = e.LastName,
                TeamId = e.TeamId,
                Vacations = e.Vacations.Where(v=>((v.BeginDate.Month == month && v.BeginDate.Year == year) || (v.EndDate.Month == month && v.EndDate.Year == year))  && v.RequestStatus == "Approved").ToList(),
                SickLeaves = e.SickLeaves.Where(s=>((s.CreateDate.Month == month && s.CreateDate.Year == year && s.CloseDate == null) || (s.CloseDate != null && s.CloseDate.Value.Month == month && s.CloseDate.Value.Year == year) || (s.CloseDate == null && s.CloseDate.Value.Month != month && s.CloseDate.Value.Year != year)) && (s.RequestStatus == "Approved" || s.RequestStatus == "Closed")).ToList(),
                Assignments = e.Assignments.Where(a=>(a.Assignment.BeginDate.Month == month && a.Assignment.BeginDate.Year == year) || (a.Assignment.EndDate.Month == month && a.Assignment.EndDate.Year == year) ).Select(a=>new AssignmentEmployee
                {
                    Assignment = a.Assignment
                }).ToList(),
                Absences = e.Absences.Where(a=>a.AbsenceDate.Month == month && a.AbsenceDate.Year == year).ToList(),
                WorkDays = e.WorkDays.Where(w=>w.WorkDayDate.Month == month && w.WorkDayDate.Year == year).ToList(),
                DayOffs = e.DayOffs.Where(d=>d.DayOffDate.Month == month && d.DayOffDate.Year == year).ToList()              
            }).AsNoTracking().ToArrayAsync();
        }

        public async Task<Employee[]> GetAsync(Expression<Func<Employee, bool>> condition = null)
        {
            return condition != null ? await _context.Employee.Where(condition).ToArrayAsync() : await _context.Employee.ToArrayAsync();
        }

        public async Task<Employee[]> GetWithTeamAsync(Expression<Func<Employee, bool>> condition = null)
        {
            return condition != null ? await _context.Employee.Where(condition).Include(emp => emp.Team).ToArrayAsync() : await _context.Employee.Include(emp => emp.Team).ToArrayAsync();
        }

        public async Task<int> GetEmployeesCount(Expression<Func<Employee, bool>> condition, string searchKey = null)
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
                return condition != null ? await _context.Employee.Where(condition).Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).
                                                                   OrderBy(emp=>emp.Team.Name).Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync() :
                                           await _context.Employee.Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).
                                                                   OrderBy(emp => emp.Team.Name).Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync();
            }
                return condition != null ? await _context.Employee.Where(condition).Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).
                                                                   Where(emp=>emp.Team.Name.ToLower().Contains(searchKey.ToLower())
                                                                   || $"{emp.FirstName} {emp.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                   || emp.EmployeeBalanceResiduals.Any(res=>res.ResidualBalance.ToString().Contains(searchKey))
                                                                   || emp.TeamNavigation.Any(t => t.Name.ToLower().Contains(searchKey.ToLower()))).
                                                                   OrderBy(emp => emp.Team.Name).Skip(pageNumber*pageSize).Take(pageSize).AsNoTracking().ToArrayAsync() :
                                           await _context.Employee.Include(emp => emp.Team).Include(emp => emp.EmployeeBalanceResiduals).
                                                                   Where(emp => emp.Team.Name.ToLower().Contains(searchKey.ToLower())
                                                                   || $"{emp.FirstName} {emp.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                   || emp.EmployeeBalanceResiduals.Any(res => res.ResidualBalance.ToString().Contains(searchKey))
                                                                   || emp.TeamNavigation.Any(t => t.Name.ToLower().Contains(searchKey.ToLower()))).
                                                                   OrderBy(emp => emp.Team.Name).Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync();
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

        public async Task CreateAsync(Employee entity, bool writeChanges)
        {
            _context.Employee.Add(entity);

            if(writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(Employee entity, bool writeChanges)
        {
            _context.Employee.Remove(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task WriteChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee entity, bool writeChanges)
        {
            _context.Employee.Update(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }
    }
}
