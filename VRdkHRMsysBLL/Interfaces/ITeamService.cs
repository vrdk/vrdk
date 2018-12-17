using System.Threading.Tasks;
using VRdkHRMsysBLL.DTOs.Team;

namespace VRdkHRMsysBLL.Interfaces
{
    public interface ITeamService
    {
        Task CreateAsync(TeamDTO team);
    }
}