using Microsoft.AspNetCore.Mvc.Rendering;
using VRdkHRMsysBLL.DTOs.Employee;
using VRdkHRMsysBLL.DTOs.Post;

namespace VRdkHRMsystem.Interfaces
{
    public interface IViewListMapper
    {
        SelectListItem[] CreateVacationTypesList();
        SelectListItem[] CreateOrganisationPostsList(PostDTO[] posts, string editUserPost = null);
        SelectListItem[] CreateRolesList(string editUserRole = null);
        SelectListItem[] CreateEmployeesList(EmployeeDTO[] employees);
        SelectListItem[] CreateStateList(bool userState);
    }
}