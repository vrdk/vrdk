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

        public async Task CreateAsync(TeamDTO team)
        {
            var teamToAdd = _mapHelper.Map<TeamDTO, Team>(team);
            await _teamRepository.CreateAsync(teamToAdd);
        }
    }
}
