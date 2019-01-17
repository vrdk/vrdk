using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysDAL.Entities;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ITeamService
    {
        Task CreateAsync(TeamDTO team);
        Task<TeamDTO> GetByIdAsync(string id);
        Task UpdateAsync(TeamDTO newTeam);
        Task<TeamListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Team, bool>> condition = null);
        Task<int> GetTeamsCountAsync(string searchKey = null, Expression<Func<Team, bool>> condition = null);
    }
}