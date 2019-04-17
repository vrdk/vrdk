using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ITeamService
    {
        Task CreateAsync(TeamDTO team, bool writeChanges = false);
        Task<TeamDTO> GetByIdAsync(string id);
        Task UpdateAsync(TeamDTO newTeam, bool writeChanges = false);
        Task<TeamDTO[]> GetAsync(Expression<Func<Team, bool>> condition = null);
        Task<TeamListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Team, bool>> condition = null);
        Task<TeamDTO> GetForCalendaAsync(string id);
        Task<int> GetTeamsCountAsync(string searchKey = null, Expression<Func<Team, bool>> condition = null);
        Task DeleteAsync(TeamDTO team, bool writeChanges = false);
    }
}