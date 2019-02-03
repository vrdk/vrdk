using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Team;
using VRdkHRMsysBLL.Interfaces;
using VRdkHRMsysDAL.Entities;
using VRdkHRMsysDAL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IMapHelper _mapHelper;

        public TeamService(ITeamRepository teamRepository,
                           IMapHelper mapHelper)
        {
            _teamRepository = teamRepository;
            _mapHelper = mapHelper;
        }

        public async Task<int> GetTeamsCountAsync(string searchKey = null, Expression<Func<Team, bool>> condition = null)
        {
            return await _teamRepository.GetTeamsCountAsync(searchKey, condition);
        }

        public async Task<TeamListUnitDTO[]> GetPageAsync(int pageNumber, int pageSize, string searchKey = null, Expression<Func<Team, bool>> condition = null)
        {
            var teams = await _teamRepository.GetPageAsync(pageNumber, pageSize, searchKey, condition);

            var teamsList = teams != null ? teams.Select(t => new TeamListUnitDTO
            {
                TeamId = t.TeamId,
                Name = t.Name,
                TeamleadName = $"{t.Teamlead.FirstName} {t.Teamlead.LastName}",
                MembersCount = t.Employees.Count()
            }).ToArray() : new TeamListUnitDTO[] { };

            return teamsList;
        }

        public async Task<TeamDTO[]> GetAsync(Expression<Func<Team, bool>> condition = null)
        {
            var teams = await _teamRepository.GetAsync(condition);
            return _mapHelper.MapCollection<Team, TeamDTO>(teams);
        }

        public async Task<TeamDTO> GetByIdAsync(string id)
        {
            var team = await _teamRepository.GetByIdAsync(id);
            return _mapHelper.Map<Team, TeamDTO>(team);
        }

        public async Task<TeamDTO> GetForCalendaAsync(string id)
        {
            var team = await _teamRepository.GetForCalendarAsync(id);
            return _mapHelper.Map<Team, TeamDTO>(team);
        }

        public async Task UpdateAsync(TeamDTO newTeam, bool writeChanges = false)
        {
            var currentTeam = await _teamRepository.GetByIdAsync(newTeam.TeamId);
            if (currentTeam != null)
            {
                currentTeam.TeamleadId = newTeam.TeamleadId;
                currentTeam.Name = newTeam.Name;
            }

            if (writeChanges)
            {
                await _teamRepository.WriteChangesAsync();
            }          
        }

        public async Task CreateAsync(TeamDTO team, bool writeChanges = false)
        {
            var teamToAdd = _mapHelper.Map<TeamDTO, Team>(team);
            await _teamRepository.CreateAsync(teamToAdd, writeChanges);
        }

        public async Task DeleteAsync(TeamDTO team, bool writeChanges = false)
        {
            var teamToRemove = _mapHelper.Map<TeamDTO, Team>(team);
            await _teamRepository.DeleteAsync(teamToRemove, writeChanges);
        }
    }
}
