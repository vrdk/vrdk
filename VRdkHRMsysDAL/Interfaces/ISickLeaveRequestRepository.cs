﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface ISickLeaveRequestRepository : IRepository<SickLeaveRequest>
    {
        Task<SickLeaveRequest> GetByIdWithEmployeeWithTeamAsync(string id);
        Task<SickLeaveRequest[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<int> GetSickLeavesNumberAsync(string searchKey = null, Expression<Func<SickLeaveRequest, bool>> condition = null);
        Task<SickLeaveRequest[]> GetProfilePageAsync(int pageSize, string id, int pageNumber = 0);
        Task<SickLeaveRequest> GetByIdWithEmployeeWithResidualsAsync(string id);
    }
}
