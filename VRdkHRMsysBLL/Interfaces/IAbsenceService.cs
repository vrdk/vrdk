using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Absence;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface IAbsenceService
    {
        Task<AbsenceDTO> GetTodayByEmployeeIdAsync(string id);
        Task CreateAsync(AbsenceDTO absence, bool writeChanges = false);
        Task<AbsenceListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Absence, bool>> condition = null);
        Task<int> GetAbsencesCountAsync(string searchKey = null, Expression<Func<Absence, bool>> condition = null);
    }
}