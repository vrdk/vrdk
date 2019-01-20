using Microsoft.AspNetCore.Mvc.Rendering;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Post;
using VRdkHRMsysBLL.DTOs.Team;

namespace VRdkHRMsystem.Interfaces
{
    public interface IViewListMapper
    {
        SelectListItem[] CreateVacationTypesList();
        SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts, string editUserPost = null);
        SelectListItem[] CreateRolesList(string editUserRole = null);
        SelectListItem[] CreateEmployeesList(EmployeeDTO[] employees);
        SelectListItem[] CreateStateList(string userState = null);
        SelectListItem[] CreateSelectedEmployeesList(EmployeeDTO[] employees, string[] selectedEmployees);
        SelectListItem[] CreateTeamList(TeamDTO[] teams, string currentId);
    }
}