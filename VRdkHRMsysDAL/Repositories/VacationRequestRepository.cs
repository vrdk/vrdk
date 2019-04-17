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

        public async Task<VacationRequest[]> GetProfilePageAsync(int pageSize , string id, int pageNumber = 0)
        {
            return await _context.VacationRequest.Where(req => req.EmployeeId == id).OrderByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task<int> GetVacationsCountAsync(Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null)
        {
            if (searchKey == null) {
                return condition != null ? await _context.VacationRequest.Where(condition).CountAsync() :
                                           await _context.VacationRequest.CountAsync();
            }

            return condition != null ? await _context.VacationRequest.Where(req => req.Employee.FirstName.ToLower().Contains(searchKey.ToLower())
                                                                                || req.Employee.LastName.ToLower().Contains(searchKey.ToLower())
                                                                                || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).CountAsync():
                                       await _context.VacationRequest.Where(condition).
                                                                      Where(req => req.Employee.FirstName.ToLower().Contains(searchKey.ToLower())
                                                                                || req.Employee.LastName.ToLower().Contains(searchKey.ToLower())
                                                                                || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).CountAsync();                                                                              
        }

        public async Task<VacationRequest[]> GetAsync(Expression<Func<VacationRequest, bool>> condition = null)
        {
            return condition != null ? await _context.VacationRequest.Where(condition).ToArrayAsync() : await _context.VacationRequest.ToArrayAsync();          
        }

        public async Task<VacationRequest[]> GetPageAsync(int pageNumber, int pageSize, string priorityStatus, Expression<Func<VacationRequest, bool>> condition = null, string searchKey = null)
        {
            if(searchKey == null)
            {
                return condition != null ?
                await _context.VacationRequest.Include(r => r.Employee).
                                               ThenInclude(emp => emp.Team).
                                               Include(req => req.Employee).
                                               ThenInclude(emp => emp.EmployeeBalanceResiduals).Where(condition).OrderBy(req => req.RequestStatus == priorityStatus ? 0 : 1).ThenByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync()
              : await _context.VacationRequest.Include(r => r.Employee).
                                               ThenInclude(emp => emp.Team).
                                               Include(req => req.Employee).
                                               ThenInclude(emp => emp.EmployeeBalanceResiduals).OrderBy(req => req.RequestStatus == priorityStatus ? 0 : 1).ThenByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
            }

            return condition != null ?
                await _context.VacationRequest.Include(r => r.Employee).
                                               ThenInclude(emp => emp.Team).
                                               Include(req => req.Employee).
                                               ThenInclude(emp => emp.EmployeeBalanceResiduals).
                                               Where(condition).
                                               Where(req => req.BeginDate.ToString().Contains(searchKey)
                                                        || req.EndDate.ToString().Contains(searchKey)
                                                        || $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                        || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).OrderBy(req => req.RequestStatus == priorityStatus ? 0 : 1).ThenByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync()
              : await _context.VacationRequest.Include(r => r.Employee).
                                               ThenInclude(emp => emp.Team).
                                               Include(req => req.Employee).
                                               ThenInclude(emp => emp.EmployeeBalanceResiduals).
                                               Where(req => req.BeginDate.ToString().Contains(searchKey)
                                                        || req.EndDate.ToString().Contains(searchKey)
                                                        || $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                        || (req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower()))).OrderBy(req => req.RequestStatus == priorityStatus ? 0 : 1).ThenByDescending(req => req.CreateDate).Skip(pageNumber * pageSize).Take(pageSize).ToArrayAsync();
        }

        public async Task<VacationRequest> GetByIdAsync(string id)
        {
            return await _context.VacationRequest.FirstOrDefaultAsync(req => req.VacationId.Equals(id));
        }

        public async Task<VacationRequest> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            return await _context.VacationRequest.Include(req=>req.Employee).ThenInclude(emp=>emp.Team).ThenInclude(t=>t.Teamlead).FirstOrDefaultAsync(req => req.VacationId.Equals(id));
        }

        public async Task CreateAsync(VacationRequest entity, bool writeChanges)
        {
            _context.VacationRequest.Add(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(VacationRequest entity, bool writeChanges)
        {
            _context.VacationRequest.Remove(entity);

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
