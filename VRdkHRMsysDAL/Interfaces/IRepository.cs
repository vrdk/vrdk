﻿using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IRepository<T>
    {
        Task<T[]> GetAsync(Expression<Func<T, bool>> condition = null);
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync();
    }
}
