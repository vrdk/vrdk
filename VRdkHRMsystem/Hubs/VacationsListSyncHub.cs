using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace VRdkHRMsystem.Hubs
{
    [Authorize(Roles ="Administrator, Teamlead")]
    public class VacationsListSyncHub : Hub
    {
        public async Task AddToGroup(string groupName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task AddVacationRequestToList(string groupName, object vacationRequest)
        {
            await Clients.Group(groupName).SendAsync("AddVacationRequestToList", vacationRequest);
        }

        public async Task SyncVacationLists(string groupName,object vacationRequest)
        {
            await Clients.GroupExcept(groupName, Context.ConnectionId).SendAsync("VacationRequestProccessed", vacationRequest);
        }      
    }
}
