﻿using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
        Task<Employee> GetByIdWithTeamAsync(string id);
        Task<Employee> GetByEmailWithTeamAsync(string email);
        Task<Employee> GetByIdWithResidualsAsync(string id);
    }
}
