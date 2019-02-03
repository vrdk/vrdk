using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysDAL.Interfaces
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Task<Employee> GetByEmailAsync(string email);
        Task<Employee> GetByIdWithTeamAsync(string id);
        Task<Employee> GetByEmailWithTeamAsync(string email);
        Task<Employee> GetByIdWithResidualsAsync(string id);
        Task<Employee> GetByIdWithTeamWithResidualsAsync(string id);
        Task<Employee> GetByEmailWithTeamWithResidualsAsync(string email);
        Task<Employee[]> GetWithTeamAsync(Expression<Func<Employee, bool>> condition = null);
        Task<Employee[]> GetPageAsync(int pageSize, int pageNumber, string searchKey, Expression<Func<Employee, bool>> condition = null);
        Task<int> GetEmployeesCount(Expression<Func<Employee, bool>> condition = null, string searchKey = null);
        Task<Employee[]> GetForCalendarAsync(string teamId, string teamleadId, int month, int year, string personalId = null);
        Task UpdateAsync(Employee entity, bool writeChange);
    }
}
