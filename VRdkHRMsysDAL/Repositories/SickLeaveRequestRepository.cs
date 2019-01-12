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
    public class SickLeaveRequestRepository : ISickLeaveRequestRepository
    {
        private readonly HRMSystemDbContext _context;

        public SickLeaveRequestRepository(HRMSystemDbContext context)
        {
            _context = context;
        }

        public async Task<SickLeaveRequest[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0)
        {
            return await _context.SickLeaveRequest.Where(req => req.EmployeeId == id).OrderByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task<int> GetSickLeavesNumberAsync(string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
           
            if (searchKey == null)
            {
                return condition != null ? await _context.SickLeaveRequest.Where(condition).CountAsync() :
                                            await _context.SickLeaveRequest.CountAsync();
            }

            return condition != null ? await _context.SickLeaveRequest.Where(condition).
                                                                       Where(req => $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                               || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).CountAsync() :
                                      await _context.SickLeaveRequest.Where(condition).
                                                                      Where(req => $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                               || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).CountAsync();
        }

        public async Task<SickLeaveRequest[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            if (searchKey == null)
            {
                return condition != null ?
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                Where(condition).
                                                OrderByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                OrderByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();


            }

            return condition != null ?
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                Where(condition).
                                                Where(req=> $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                         || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).
                                                OrderByDescending(req=>req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync() :
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                Where(req => $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                         || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).
                                                OrderByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();

        }

        public async Task<SickLeaveRequest> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            return await _context.SickLeaveRequest.Include(req => req.Employee).ThenInclude(emp => emp.Team).FirstOrDefaultAsync(req => req.SickLeaveId.Equals(id));
        }

        public async Task<SickLeaveRequest[]> GetAsync(Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            return condition != null ? await _context.SickLeaveRequest.Where(condition).ToArrayAsync() : await _context.SickLeaveRequest.ToArrayAsync();
        }

        public async Task<SickLeaveRequest> GetByIdWithEmployeeWithResidualsAsync(string id)
        {
            return await _context.SickLeaveRequest.Include(req=>req.Employee).ThenInclude(emp=>emp.EmployeeBalanceResiduals).FirstOrDefaultAsync(req => req.SickLeaveId.Equals(id));
        }

        public async Task<SickLeaveRequest> GetByIdAsync(string id)
        {
            return await _context.SickLeaveRequest.FirstOrDefaultAsync(req => req.SickLeaveId.Equals(id));
        }

        public async Task CreateAsync(SickLeaveRequest entity)
        {
            _context.SickLeaveRequest.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(SickLeaveRequest entity)
        {
            _context.SickLeaveRequest.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
