﻿using Microsoft.EntityFrameworkCore;
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
            return await _context.SickLeaveRequest.Where(req => req.EmployeeId == id).OrderBy(req=>req.RequestStatus == "Pending" ? 0 : 1).ThenByDescending(req => req.CreateDate).
                                                                                      Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync();
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
                                                                               || req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).CountAsync() :
                                      await _context.SickLeaveRequest.Where(req => $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                                               || req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).CountAsync();

        }

        public async Task<SickLeaveRequest[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null)
        {
            if (searchKey == null)
            {
                return condition != null ?
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                Where(condition).
                                                OrderBy(req => req.RequestStatus == "Pending" ? 0 : 1).ThenByDescending(req => req.CreateDate).
                                                Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync() :
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                OrderBy(req => req.RequestStatus == "Pending" ? 0 : 1).ThenByDescending(req => req.CreateDate).
                                                Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync();


            }

            return condition != null ?
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                Where(condition).
                                                Where(req=> $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                         || req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).
                                                         OrderBy(req => req.RequestStatus == "Pending" ? 0 : 1).ThenByDescending(req=>req.CreateDate).
                                                         Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync() :
                await _context.SickLeaveRequest.Include(r => r.Employee).
                                                ThenInclude(emp => emp.Team).
                                                Where(req => $"{req.Employee.FirstName} {req.Employee.LastName}".ToLower().Contains(searchKey.ToLower())
                                                         || req.Employee.Team != null && req.Employee.Team.Name.ToLower().Contains(searchKey.ToLower())).
                                                         OrderBy(req=>req.RequestStatus == "Pending" ? 0 : 1).ThenByDescending(req => req.CreateDate).
                                                         Skip(pageNumber * pageSize).Take(pageSize).AsNoTracking().ToArrayAsync();


        }

        public async Task<SickLeaveRequest> GetByIdWithEmployeeWithTeamAsync(string id)
        {
            return await _context.SickLeaveRequest.Include(req => req.Employee).ThenInclude(emp => emp.Team).ThenInclude(t=>t.Teamlead).FirstOrDefaultAsync(req => req.SickLeaveId.Equals(id));
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

        public async Task CreateAsync(SickLeaveRequest entity, bool writeChanges)
        {
            _context.SickLeaveRequest.Add(entity);

            if (writeChanges)
            {
                await WriteChangesAsync();
            }
        }

        public async Task DeleteAsync(SickLeaveRequest entity, bool writeChanges)
        {
            _context.SickLeaveRequest.Remove(entity);

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
